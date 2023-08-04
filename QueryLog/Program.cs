using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace QueryLog;
class Program
{
          static ApplicationDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
        #region Query Log Nedir?
        //*LINQ soprguları neticesinde generate edilen sorguları izleyebilmek ve olası teknik hataları ayıklayabilmek amacıyla query log mekanizmasından istifade etmekteyiz.
        #endregion
        #region Nasıl Konfigüre Edilir?
        //Microsoft.Extensions.Logging.Console   //*yüklenmeli
         context.Persons.ToList();

         context.Persons
            .Include(p => p.Orders)
            .Where(p => p.Name.Contains("a"))
            .Select(p => new { p.Name, p.PersonId })
            .ToList();
        #endregion
        #region Filtreleme Nasıl Yapılır?

        #endregion
   }
}

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }

    public ICollection<Order> Orders { get; set; }
}
public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }

    public Person Person { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }
    //  readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());//*log lama basit haliyle
    readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder
    .AddFilter((category, level) =>
    {
        return category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information;
    })
    .AddConsole()); //*burada da filter ile filtreleem yapıyoruz.
    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ApplicationDb");
        optionsBuilder.UseLoggerFactory(loggerFactory);
    }
}
