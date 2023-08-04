
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace Global_Query_Filters;
class Program
{
    static ApplicationDbContext context = new();
    static void Main(string[] args)
    {
       #region Global Query Filters Nedir?
        //*Bir entity'e özel uygulama seviyesinde genel/ön kabullü şartlar oluşturmamızı ve böylece verileri global bir şekilde filtrelememeizi sağlayan bir özelliktir.
        //*Böylece belirtilen entity üzerinden yapılan tm sorgulamalarda ekstradan bir şart ifadesine gerek kalmaksızın filtreleri otomatik uygulayarak hızlıca sorgulama yapmamızı sağlamaktadır.

        //*Genellikle uygulama bazında aktif(IsActive) gibi verilerle çalışıldığı durumlarda,
        //*MultiTenancy uygulamalarda TenantId tanımlarken vs. kullanılabilri.
        #endregion
        #region Global Query Filters Nasıl Uygulanır?
        //await context.Persons.Where(p => p.IsActive).ToListAsync();//*şimdi burada biz personlardan isactive true olanalrı yani aktif olanları listeledik peki bunu personları çağırırken sürekli yapmaktansa onmodelcreating de tanımlarız ordan kullanırız where kullanmaya gerek yok ama bizim personları çağırırken sadece aktif olanları istiyor olursak kullanmamız gerek
        //await context.Persons.ToListAsync();//*aşşağıda yağtığımız configurasyon sayesinde biz personları listelediğimiz halde personlar da aktif olanlar gelecek
        //await context.Persons.FirstOrDefaultAsync(p => p.Name.Contains("a") || p.PersonId == 3);
        #endregion
        #region Navigation Property Üzerinde Global Query Filters Kullanımı
        //var p1 = await context.Persons
        //    .AsNoTracking()
        //    .Include(p => p.Orders)
        //    .Where(p => p.Orders.Count > 0)
        //    .ToListAsync();

        //var p2 = await context.Persons.AsNoTracking().ToListAsync();
        //Console.WriteLine();
        #endregion
        #region Global Query Filters Nasıl Ignore Edilir? - IgnoreQueryFilters
        //var person1 = await context.Persons.ToListAsync();
        //var person2 = await context.Persons.IgnoreQueryFilters().ToListAsync();//*tanımlamış olduğumuz global filtersı iptal etmiş oluyoruz.

        //Console.WriteLine();
        #endregion
        #region Dikkat Edilmesi Gereken Husus!
        //Global Query Filter uygulanmış bir kolona farkında olmaksızın şart uygulanabilmektedir. Bu duruma dikkat edilmelidir.

         context.Persons.Where(p => p.IsActive).ToList();
        #endregion

    }
}

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public List<Order> Orders { get; set; }
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

        modelBuilder.Entity<Person>().HasQueryFilter(p => p.IsActive);//*global olarak kesinlikle bu uygulama üzerinde personlar sorgulanırken bu şartı her sorguya ekle demiş oluyoruz
        //modelBuilder.Entity<Person>().HasQueryFilter(p => p.Orders.Count > 0);
    }

    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ApplicationDb");
    }
}
