using System.ComponentModel.DataAnnotations;

public class Log
{
    [Key]
    public int iDlOG { get; set; }
    public string CodigoRobo { get; set; }
    public string UsuarioRobo { get; set; }
    public DateTime DateLog { get; set; }
    public string Etapa { get; set; }
    public string InformacaoLog { get; set; }
    public int IdProdutoAPI { get; set; }
<<<<<<< HEAD
}
=======
}
>>>>>>> 2276b13a3ed9efde2241f5677f88e97e0ffee933
