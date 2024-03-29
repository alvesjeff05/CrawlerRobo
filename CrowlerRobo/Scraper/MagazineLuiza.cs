﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


public class MagazineLuizaScraper
{
    public string ObterPreco(string descricaoProduto, int idProduto)
    {
        try
        {
            // Inicializa o ChromeDriver
            using (IWebDriver driver = new ChromeDriver())
            {
                // Abre a página
                driver.Navigate().GoToUrl($"https://www.magazineluiza.com.br/busca/{descricaoProduto}");

                // Aguarda um tempo fixo para permitir que a página seja carregada (você pode ajustar conforme necessário)
                System.Threading.Thread.Sleep(5000);

                // Encontra o elemento que possui o atributo data-testid
                IWebElement priceElement = driver.FindElement(By.CssSelector("[data-testid='price-value']"));

                // Verifica se o elemento foi encontrado
                if (priceElement != null)
                {
                    // Obtém o preço do primeiro produto
                    string secondProductPrice = priceElement.Text;

                    // Registra o log com o ID do produto
                    RegistrarLog("2805", "jeffalves", DateTime.Now, "WebScraping - Magazine Luiza", "Sucesso", idProduto);

                    // Retorna o preço
                    return secondProductPrice;
                }
                else
                {
                    Console.WriteLine("Preço não encontrado.");

                    // Registra o log com o ID do produto
                    RegistrarLog("2805", "jeffalves", DateTime.Now, "WebScraping - Magazine Luiza", "Preço não encontrado", idProduto);

                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

            // Registra o log com o ID do produto
            RegistrarLog("2805", "jeffalves", DateTime.Now, "Web Scraping - Magazine Luiza", $"Erro: {ex.Message}", idProduto);

            return null;
        }
    }

    private static void RegistrarLog(string CodigoRobo, string UsuarioRobo, DateTime DateLog, string Processo, string InfLog, int IdProd)
    {

        using (var context = new RoboContext())
        {
            var log = new Log
            {
                CodigoRobo = CodigoRobo,
                UsuarioRobo = UsuarioRobo,
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