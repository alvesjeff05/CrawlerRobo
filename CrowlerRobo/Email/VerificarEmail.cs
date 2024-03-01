//Enviar email com o resultado da comparação
using System.Net;
using System.Net.Mail;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;


namespace CrowlerRobo.Email;

public class VerificarEmail
{

     internal static void EnviarEmail(string nomeProdutoMercadoLivre, string precoProdutoMercadoLivre, string nomeProdutoMagazineLuiza, string precoProdutoMagazineLuiza, string VerificarValor, string nomeProduto)
        {
            // Configurações do servidor SMTP do Hotmail
            string smtpServer = "smtp-mail.outlook.com"; // Servidor SMTP do Hotmail
            int porta = 587; // Porta SMTP do Hotmail para TLS/STARTTLS
            string remetente = "willijefferson.crawler@hotmail.com"; // Seu endereço de e-mail do Hotmail
            string senha = "robocrawler!"; // Sua senha do Hotmail

            // Configurar cliente SMTP
            using (SmtpClient client = new SmtpClient(smtpServer, porta))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(remetente, senha);
                client.EnableSsl = true; // Habilitar SSL/TLS

                // Construir mensagem de e-mail
                MailMessage mensagem = new MailMessage(remetente, "willijefferson.crawler@hotmail.com")
                {
                    Subject = "Resultado da comparação de preços",
                    Body = $"Mercado Livre\n" +
                           $"Produto: {nomeProduto}\n" +
                           $"Preço: R$ {precoProdutoMercadoLivre}\n\n" +
                           $"Magazine Luiza\n" +
                           $"Produto: {nomeProduto}\n" +
                           $"Preço: R$ {precoProdutoMagazineLuiza}\n\n" +
                           $"Melhor compra: {VerificarValor}"
                };

                // Enviar e-mail
                client.Send(mensagem);

                Console.WriteLine(nomeProdutoMercadoLivre);
                Console.WriteLine(precoProdutoMercadoLivre);
                Console.WriteLine(nomeProdutoMagazineLuiza);
                Console.WriteLine(precoProdutoMagazineLuiza);
                
            }
        }

}