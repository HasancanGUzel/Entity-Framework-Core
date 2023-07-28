using Microsoft.EntityFrameworkCore;

namespace Sequences;
class Program
{
        static ApplicationDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
            #region Sequence Nedir?
            //*Veritabanında benzersiz ve ardışık sayısal değerler üreten veritabanı nesnesidir.
            //*Sequence herhangi bir tablonun özelliği değildir. Veritabanı nesnesidir. Birden fazla tablo tarafından kullanılabilir.
            #endregion
            #region Sequence Tanımlama
            //*Sequence'ler üzerinden değer oluştururken veritabanına özgü çalışma yapılması zaruridir. SQL Server'a özel yazılan Sequence tanımı misal olarak Oracle için hata verebilir.
            //*Fluent apida tanımlanır

                #region HasSequence
                    //*bu kodu kullanarak sequence tanımlanır
                #endregion
                #region HasDefaultValueSql
                        //*buda öncelerden gördüğümüz sql sorgusu yazmamıza yarıyordu.
                #endregion

            #endregion

            //await context.Employees.AddAsync(new() { Name = "Gençay", Surname = "Yıldız", Salary = 1000 });
            //await context.Employees.AddAsync(new() { Name = "Mustafa", Surname = "Yıldız", Salary = 1000 });
            //await context.Employees.AddAsync(new() { Name = "Tuaip", Surname = "Yıldız", Salary = 1000 });

            //await context.Customers.AddAsync(new() { Name = "Muiddin" });
            //await context.SaveChangesAsync();

            //*şimdi buarada veri eklerken customer ve employee olarak 2 tablomuz var ama tek bir sequence de çalışıyoruz o yüzden customer a veri ekledik ve ıd sini 1 olarak atarken employee de 3 tane veri ekledik ama ıd 1 den değilde 2,3,4 diye devam etti. tekrar veri eklemey çalışsam ıd 5 den devam edecek

            #region Sequence Yapılandırması

            #region StartsAt
                //*sequencenin kaçtan başlayacağını bildiriyoruz.
            #endregion
            #region IncrementsBy
                //*Sequence nin kaçar kaçar artacağını bilditriyoruz.
            #endregion
            #endregion
            #region Sequence İle Identity Farkı
            //*Sequence bir veritabanı nesnesiyken, Identity ise tabloların özellikleridir.
            //*Yani Sequence herhangi bir tabloya bağımlı değildir. 
            //*Identity bir sonraki değeri diskten alırken Sequence ise RAM'den alır. Bu yüzden önemli ölçüde Identity'e nazaran daha hızlı, performanslı ve az maliyetlidir.
            #endregion
    }
}

class Employee
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Salary { get; set; }
}
class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence("EC_Sequence");//*sequence tanımlamasını yaptık
           

        // modelBuilder.HasSequence("EC_Sequence")
        //     .StartsAt(100) //*sequencenin 100 den başlasın dedik
        //     .IncrementsBy(5); //* ve 5 er 5er artsın dedik bunları tanımladan yaparsak default olarak 1 den  başlar 1 er 1 er devam eder.
            

        modelBuilder.Entity<Employee>()
            .Property(e => e.Id)
            .HasDefaultValueSql("NEXT VALUE FOR EC_Sequence"); //* buarad employee entitisnin içinde ıd kolonunun değerini adı EC_Sequence olan sequencedan bir sonraki değeri getir demiş oluyoruz ıd nin identity sini kaldırıyoruz onun yerine sequence gellmiş oluyor.

        modelBuilder.Entity<Customer>()
            .Property(c => c.Id)
            .HasDefaultValueSql("NEXT VALUE FOR EC_Sequence"); //*customer da da yukarıdaki açıklama gibi
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            optionsBuilder.UseSqlite("Data Source=ApplicationDb");
    }
}
