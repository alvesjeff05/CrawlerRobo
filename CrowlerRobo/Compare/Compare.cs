using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Compare
{
    public string VerificarValor(string firstProductPrice, string secondProductPrice, string nomeProduto)
    {
      
        char[] charsRemove = {'$', 'R', ' '};
 
        double firstPrice = Convert.ToDouble(firstProductPrice.Trim(charsRemove));
        double secondPrice = Convert.ToDouble(secondProductPrice.Trim(charsRemove));
        string linkMercado = $"https://mercadolivre.com.br/{nomeProduto}".Replace(' ', '+');
        string linkLuiza = $"https://www.magazineluiza.com.br/{nomeProduto}".Replace(' ', '+');

        if (firstPrice < secondPrice) 
        {
            return ("O produto no Mercado Livre está mais em conta - " + linkMercado);
        }
        else if (firstPrice > secondPrice)
        {
            return ("O produto no Magazine Luiza está mais em conta - " + linkLuiza);
        }
        else
        {
            return("Os preços são iguais.");
        }
    }
}