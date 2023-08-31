namespace Ef_Core_Efficient_Querying;
class Program
{
    static ApplicationDbContext context = new();
    static void Main(string[] args)
    {
        #region EF Core Select Sorgularını Güçlendirme Teknikleri

            #region IQueryable - IEnumerable Farkı

            //*IQueryable, bu arayüz üzerinde yapılan işlemler direkt generate edilecek olan sorguya yansıtılacaktır.
            //*IEnumerable, bu arayüz üzerinde yapılan işlemler temel sorgu neticesinde gelen ve in-memorye yüklenen instance'lar üzerinde gerçekleştirilir. Yani sorguya yansıtılmaz.

            //*yani IQueryable da sorguyu yazdığımız zaman  select *from persons Where ...... diye yazarız sorguda ne istediğimizi söyleriz
            //*ama IEnumerable da Select *from persons   yazarız hepsi gelir biz inmemroy de istediğimizi alırız.

            //*IQueryable ile yapılan sorgulama çalışmalarında sql sorguyu hedef verileri elde edecek şekilde generate edilecekken, IEnumerable ile yapılan sorgulama çalışmalarında sql daha geniş verileri getirebilecek şekilde execute edilerek hedef veriler in-memory'de ayıklanır.

            //*IQueryable hedef verileri getirirken, hedef verilerden daha fazlasını getirip in-memory'de ayıklar.

            //*IQueryable ve IEnumarable davranışsal olarak aralarında farklar barindırsalarda her ikiside Deferred Execution(gecikmlei çalıştırma) davranışı sergiler.
            //*Yani her iki arayüz üzerinden de oluşturulan işlemi execute edebilmek için .Tolist gibi tetikleyici fonkjsiyonları yahut foreach gibi tetikleyici işlemleri gerçekleştirmemiz gerekemktedir.

            #region IQueryable
            //var persons = await context.Persons.Where(p => p.Name.Contains("a"))
            //                             .Take(3)
            //                             .ToListAsync();


            //var persons = await context.Persons.Where(p => p.Name.Contains("a"))
            //                             .Where(p => p.PersonId > 3)
            //                             .Take(3)
            //                             .Skip(3)
            //                             .ToListAsync();

            #endregion
            #region IEnumerable
            //var persons = context.Persons.Where(p => p.Name.Contains("a")) //*buraya kadar IQueryable olmuş oluyor yani Select * from persons Where Name LIKE '%a%' lıke sorgusu name içinde a geçenleri getirdik ama bu bizim tamamlanmış sorgumuz daha değil ve buradan sonra IEnumarable a geçiş yapıyoruz yani şuan elimizde atıyorum 10 veri var ama tamamlanmış veriler değil daha sonra ise bu elimdeki 10 veriden 3 ünü al diyorum.
            //                             .AsEnumerable()
            //                             .Take(3)
            //                             .ToList();
            #endregion

            #region AsQueryable

            #endregion
            #region AsEnumerable

            #endregion
            #endregion

            #region Yalnızca İhtiyaç Olan Kolonları Listeleyin - Select
            //var persons = await context.Persons.Select(p => new
            //{
            //    p.Name
            //}).ToListAsync();
            #endregion

            #region Result'ı Limitleyin - Take
            //await context.Persons.Take(50).ToListAsync();
            #endregion

            #region Join Sorgularında Eager Loading Sürecinde Verileri Filtreleyin
            //var persons = await context.Persons.Include(p => p.Orders
            //                                                  .Where(o => o.OrderId % 2 == 0)
            //                                                  .OrderByDescending(o => o.OrderId)
            //                                                  .Take(4))
            //    .ToListAsync();

            //foreach (var person in persons)
            //{
            //    var orders = person.Orders.Where(o => o.CreatedDate.Year == 2022);
            //}

            #endregion

            #region Şartlara Bağlı Join Yapılacaksa Eğer Explicit Loading Kullanın
            //var person = await context.Persons.Include(p => p.Orders).FirstOrDefaultAsync(p => p.PersonId == 1);
            //var person = await context.Persons.FirstOrDefaultAsync(p => p.PersonId == 1);

            //if (person.Name == "Ayşe")
            //{
            //    //Order'larını getir...
            //    await context.Entry(person).Collection(p => p.Orders).LoadAsync();
            //}
            #endregion

            #region Lazy Loading Kullanırken Dikkatli Olun!
            #region Riskli Durum
            //var persons = await context.Persons.ToListAsync();

            //foreach (var person in persons)
            //{
            //    foreach (var order in person.Orders)
            //    {
            //        Console.WriteLine($"{person.Name} - {order.OrderId}");
            //    }
            //    Console.WriteLine("***********");
            //}
            #endregion
            #region İdeal Durum
            //var persons = await context.Persons.Select(p => new { p.Name, p.Orders }).ToListAsync();

            //foreach (var person in persons)
            //{
            //    foreach (var order in person.Orders)
            //    {
            //        Console.WriteLine($"{person.Name} - {order.OrderId}");
            //    }
            //    Console.WriteLine("***********");
            //}
            #endregion
            #endregion

            #region İhtiyaç Noktalarında Ham SQL Kullanın - FromSql

            #endregion

            #region Asenkron Fonksiyonları Tercih Edin

            #endregion

            #endregion
    }
}

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }

    public virtual Person Person { get; set; }
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
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
           UseSqlite("Data Source=ApplicationDB")
            .UseLazyLoadingProxies();
    }

}
