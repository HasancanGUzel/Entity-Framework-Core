using Microsoft.EntityFrameworkCore;

namespace Table_Per_Type_TPT_;
class Program
{
      static ApplicationDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
        #region Table Per Type (TPT) Nedir?
        //*Entitylerin aralarında kalıtımsal ilişkiye sahip olduğu durumlarda her bir türe/entitye/tip/referans karşılık bir tablo generate eden davranıştır.
        //*Her generate edilen bu tablolar hiyerarşik düzlemde kendi aralarında birebir ilişkiye sahiptir.
        #endregion
        #region TPT Nasıl Uygulanır?
        //*TPT'yi uygulayabilmek için öncelikle entitylerin kendi aralarında olması gereken mantıkta inşa edilmesi gerekmektedir.
        //*Entityler DbSet olarak bildirilmelidir.
        //*Hiyerarşik olarak aralarında kalıtımsal ilişki olan tüm entityler OnModelCreating fonksiyonunda ToTable metodu ile konfigüre edilmelidir. Böylece EF Core kalıtımsal ilişki olan bu tablolar arasında TPT davranışının olduğunu anlayacaktır.
        #endregion
        #region TPT'de Veri Ekleme
        //Technician technician = new() { Name = "Şuayip", Surname = "Yıldız", Department = "Yazılım", Branch = "Kodlama" };
        //await context.Technicians.AddAsync(technician); //* şimdi burada her entity ayrı tablo ama birbirlerine bağlılar burada eklediğimiz veri de name ve surname person tablosuna department employee branch da technician tablosuna gitmiş oldu ve hepsinin ıd side aynı  oldu o ıd ile bağlıalr birbirlerine

        //Customer customer = new() { Name = "Hilmi", Surname = "Celayir", CompanyName = "Çaykur" };
        //await context.Customers.AddAsync(customer);
        //await context.SaveChangesAsync(); //* burada da name ve surname person companyname de customer tablosuna eklendi ve sırada hangi ıd varsa o ıd ile ilişkilendirildiler
        #endregion
        #region TPT'de Veri Silme
        //Employee? silinecek = await context.Employees.FindAsync(3);
        //context.Employees.Remove(silinecek);
        //await context.SaveChangesAsync(); //*employee da ıd si 3 olanı sil dedik ama bu ıd person technicians de de bağlantılı bunlarda da bulup silecek

        //Person? silinecekPerson = await context.Persons.FindAsync(1);
        //context.Persons.Remove(silinecekPerson);
        //await context.SaveChangesAsync(); //*burada da personlarda ıd si 1 olanı sil dedik ama bu 1 ıd si employee ve technician da da vardı oradan da silmiş oldu
        #endregion
        #region TPT'de Veri Güncelleme
        //Technician technician = await context.Technicians.FindAsync(2);
        //technician.Name = "Mehmet";
        //await context.SaveChangesAsync();//*burada  technicianda ıd si 2 olanı güncelle dedik ve name kımsını persondan ıd si 1 olanı buldu ve güncelledi
        #endregion
        #region TPT'de Veri Sorgulama
        //Employee employee = new() { Name = "Fatih", Surname = "Yavuz", Department = "ABC" };
        //await context.Employees.AddAsync(employee);
        //await context.SaveChangesAsync();

        //var technicians = await context.Technicians.ToListAsync();
        //var employees = await context.Employees.ToListAsync();

        //Console.WriteLine();
        #endregion
    }
}

abstract class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}
class Employee : Person
{
    public string? Department { get; set; }
}
class Customer : Person
{
    public string? CompanyName { get; set; }
}
class Technician : Employee
{
    public string? Branch { get; set; }
}

class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Technician> Technicians { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Person>().ToTable("Persons");
        // modelBuilder.Entity<Employee>().ToTable("Employees");
        // modelBuilder.Entity<Customer>().ToTable("Customers");
        // modelBuilder.Entity<Technician>().ToTable("Technicians");
        modelBuilder.Entity<Person>().UseTptMappingStrategy(); //* efcore7 yeniliklerinde bunuda kullanabiliriz
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            optionsBuilder.UseSqlite("Data Source=ApplicationDb");
    }
}
