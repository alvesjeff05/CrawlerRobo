using System;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CrowlerRobo.Email;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using static RoboContext;

namespace CrowlerRobo
{
    class Program
    {
        //Lista para armazenar produtos já verificados
        static List<Produto> produtosVerificados = new List<Produto>();

        static void Main(string[] args)
        {
            //Definir o intervalo de tempo para 5 minutos (300.000 milissegundos)
            int intervalo = 300000;

            //Criar um temporizador que dispara a cada 5 minutos
            Timer timer = new Timer(VerificarNovoProduto, null, 0, intervalo);

            //Manter a aplicaçãp rodando
            while (true)
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }
        static async void VerificarNovoProduto(object state)
        {
            string username = "11164448";
            string password = "60-dayfreetrial";
            string url = "http://regymatrix-001-site1.ktempurl.com/api/v1/produto/getall";

            try
            {
                //Criar um objeto HttpClient
                using (HttpClient client = new HttpClient())
                {
                    //Adicionar as credenciais de autenticação básica
                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    //Fazer a requisição GET à API
                    HttpResponseMessage response = await client.GetAsync(url);

                    //Verificar se a requisição foi bem-sucedida (código de status 200)
                    if (response.IsSuccessStatusCode)
                    {
                        //Ler o conteúdo da resposta como uma string
                        string responseData = await response.Content.ReadAsStringAsync();

                        //Processar os dados da resposta
                        List<Produto> novosProdutos = ObterNovosProdutos(responseData);
                        foreach (Produto produto in novosProdutos)
                        {
                            if (!produtosVerificados.Exists(p => p.Id == produto.Id))
                            {
                                // Se é um novo produto, faça algo com ele
                                Console.WriteLine($"Novo produto encontrado: ID {produto.Id}, Nome: {produto.Nome}");
                                // Adicionar o produto à lista de produtos verificados
                                produtosVerificados.Add(produto);

                                // Registra um log no banco de dados apenas se o produto for novo
                                if (!ProdutoJaRegistrado(produto.Id))
                                {
                                    RegistrarLog("2805", "jeffalves", DateTime.Now, "ConsultaAPI - Verificar Produto", "Sucesso", produto.Id);

                                    MercadoLivreScraper mercadoLivreScraper = new MercadoLivreScraper();
                                    var mercado = mercadoLivreScraper.ObterPreco(produto.Nome, produto.Id);

                                    MagazineLuizaScraper magazineLuizaScraper = new MagazineLuizaScraper();
                                    var magazine = magazineLuizaScraper.ObterPreco(produto.Nome, produto.Id);

                                    Compare compare = new Compare();
                                    string compareResult = compare.VerificarValor(mercado, magazine, produto.Nome);

                                    RegistrarLog("2805", "jeffalves", DateTime.Now, "BenchMarking", "Sucesso", produto.Id);

                                    VerificarEmail.EnviarEmail(produto.Nome, mercado, produto.Nome, magazine, compareResult, produto.Nome);

                                    RegistrarLog("2805", "jeffalves", DateTime.Now, "Envio de email", "Sucesso", produto.Id);

                                }
                            }
                        }
                    }   
                    else
                    {
                        //Imprimir mensagem de erro caso a requisição falhe
                        Console.WriteLine($"Erro: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                //Imprimir mensagem de erro caso ocorra uma exceção
                Console.WriteLine($"Erro ao fazer requisição: {ex.Message}");
            }
        }
        //Método para processar os dados de resposta e obter produtos
        static List<Produto> ObterNovosProdutos(string responseData)
        {
            //Desserializar os dados da resposta para uma lista de produtos
            List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(responseData);
            return produtos;
        }

        // Método para verificar se o produto já foi registrado no banco de dados
        static bool ProdutoJaRegistrado(int idProduto)
        {
            using (var context = new RoboContext())
            {
                return context.LOGROBO.Any(log => log.IdProdutoAPI == idProduto && log.CodigoRobo == "2805");
            }
        }

        //Método para registrar um log no banco de dados
        static void RegistrarLog(string CodRob, string UsuRob, DateTime DateLog, string Processo, string InfLog, int IdProd)
        {
            using (var context = new RoboContext())
            {
                var log = new Log
                {
                    CodigoRobo = CodRob,
                    UsuarioRobo = UsuRob,
                    DateLog = DateLog,
                    Etapa = Processo,
                    InformacaoLog = InfLog,
                    IdProdutoAPI = IdProd
                };
                context.LOGROBO.Add(log);
                context.SaveChanges();
            }
        }
    }
}
