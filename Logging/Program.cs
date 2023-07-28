using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logging;
class Program
{
    static ApplicationDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
        var datas =  context.Persons.ToList();
        #region Neden Loglama Yaparız?
        //*Çalışan bir sistemin runtime'da nasıl davranış gerçekleştirdiğini gözlemleyebilmek için log mekanizmalarından istifade ederiz.
        #endregion
        #region Neleri Loglarız?
        //*Yapılan sorguların çalışma süreçlerindeki davranışları.
        //*Gerekirse hassas veriler üzerinde de loglama işlemleri gerçekleştiriyoruz.
        #endregion
        #region Basit Olarak Loglama Nasıl Yapılır?
        //*Minumum yapılandırma gerektirmesi.
        //*Herhangi bir nuget paketine ihtiyaç duyulmaksızın loglamanın yapılabilmesi.

        #region Debug Penceresine Log Nasıl Atılır?
        //*onconfiguring de yaptık
        #endregion
        #region Bir Dosyaya Log Nasıl Atılır?
        //*Normalde console yahut debug pencerelerine atılan loglar pek takip edilebilir nitelikte olmamaktadır.
        //*Logları kalıcı hale getirmek istediğimiz durumlarda en basit halyile bu logları harici bir dosyaya atmak isteyebiliriz.
        #endregion

        #endregion
        #region Hassas Verilerin Loglanması - EnableSensitiveDataLogging
        //*Default olarak EF Core log mesajlarında herhangi bir verinin değerini içermemektedir. Bunun nedeni, gizlilik teşkil edebilecek verilerin loglama sürecinde yanlışlıklada olsa açığa çıkmamasıdır. 
        //*Bazen alınan hatalarda verinin değerini bilmek hatayı debug edebilmek için oldukça yardımcı olabilmektedir. Bu durumda hassas verilerinde loglamasını sağlayabiliriz.
        //*  .EnableSensitiveDataLogging() kullanılır
        #endregion
        #region Exception Ayrıntısını Loglama - EnableDetailedErrors
             //.EnableDetailedErrors();
        #endregion
        #region Log Levels
        //*EF Core default olarak Debug sevisinin üstündeki(debug dahil) tüm davranıuşları loglar.
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
    StreamWriter _log = new("logs.txt", append: true);//*dosyaya logu atıyoruz.uygulamanın debug doyasına kurucak bu logs.txt yi
    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            optionsBuilder.UseSqlite("Data Source=ApplicationDb");

        //optionsBuilder.LogTo(Console.WriteLine);//*basit log
        //optionsBuilder.LogTo(message => Debug.WriteLine(message));//*debug log

        //optionsBuilder.LogTo(async message => await _log.WriteLineAsync(message);//*dosyaya logu atmak için
        //optionsBuilder.LogTo(message => _log.WriteLine(message));//*dosyaya logu atmak için

          optionsBuilder.LogTo(async message => await _log.WriteLineAsync(message))
            .EnableSensitiveDataLogging();//*hassas verilerin loglanması

            optionsBuilder.LogTo(async message => await _log.WriteLineAsync(message))
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();//*excepitpn ayrıntısını loglama
            
        optionsBuilder.LogTo(async message => await _log.WriteLineAsync(message),LogLevel.Information)//*log level
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }

    public override void Dispose()
    {
        base.Dispose();
        _log.Dispose();//*dosyaya logu atmak için log u dipose ediyoruz
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _log.DisposeAsync();//*dosyaya logu atmak için log u dipose ediyoruz
    }
}
