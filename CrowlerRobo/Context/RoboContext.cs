using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

public class RoboContext : DbContext
{
    public DbSet<Log> Logs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"DataSource=SQL9001.site4now.net; Initial Catalog=db_aa5b20_apialmoxarifado; User id=db_aa5b20_apialmoxarifado_admin; Password=master@123;");
    }
}