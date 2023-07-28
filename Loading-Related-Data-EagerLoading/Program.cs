using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Loading_Related_Data_EagerLoading;
class Program
{
    static ApplicationDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
       #region Loading Related Data

        #region Eager Loading
        //*Eager loading, generate edilen bir sorguya ilişkisel verilerin parça parça eklenmesini sağlayan ve bunu yaparken iradeli/istekli bir şekilde yapmamızı sağlayan bir yöntemdir.
        //*EAGER LOADİNG, arkaplanda üretilen sorguya join uygular (sorguya göre left veya right vs değişir)

        #region Include
        //*Eager loading operasyonunu yapmamızı sağlayan bir fonksiyondur.
        //*Yani üretilen bir sorguya diğer ilişkisel tabloların dahil edilmesini sağlayan bir işleve sahiptir..

        //var employees = await context.Employees.Include("Orders").ToListAsync();
        
        //var employees = await context.Employees
        //    .Include(e => e.Region)
        //    .Include(e=>e.Orders)
        //     .ToListAsync();


        //var employees = await context.Employees
        //    .Include(e => e.Region)
        //    .Where(e => e.Orders.Count > 2) //*bunların yeri önemli değil aşşağıda olabilir bir yukaırda da olabilir.
        //    .Include(e => e.Orders)
        //    .ToListAsync();

        #endregion
        #region ThenInclude
        //*ThenInclude, üretilen sorguda Include edilen tabloların ilişkili olduğu diğer tablolarıda sorguya ekleyebilmek için kullanılan bir fonksiyondur. 
        //*Eğer ki, üretilen sorguya include edilen navigatiobn property koleksionel bir propertyse işte o zaman bu property üzerinden diğer ilişkisel tabloya erişim gösterilememektedir. Böyle bir durumda koleksiyonel propertylerin türlerine erişip, o tür ile ilişkili diğer tablolarıda sorguya eklememizi sağlayan fonksiyondur.

        //var orders = await context.Orders //*orderlardan employee git onları getir birde employee dan regionları getir dedik
        //    .Include(o => o.Employee) //*bu satırı kullansakta olur kullanmakasata çünkü aşşağıdaki satır hem employee hemde regionları getiriyor.))
        //    .Include(o => o.Employee.Region)
        //    .ToListAsync();  //* burada theninclude kullanmadık ama aşşağıdaki kodda kullandık kullanmak zorunda kaldık çünkü burada tekil yani ilişkisel tablolarda derece farkketmeksizin gittik yani order dan employee tek(orderin içinde employee tek tutyoruz) olduğu için geçtik ve employee den de regiona tek(employee içinde region tek) o yüzden sıkıntı yok birde aşşağıya bak

        //var regions = await context.Regions
        //    .Include(r => r.Employees)
        //        .ThenInclude(e => e.Orders)
        //    .ToListAsync(); //*burada ise theninclude kullanmak zorunda kaldık çünkü regiondan employee geçtik ama tek değil ve bu yüzden employe içindeki order lara ulaşamadık theninclude kullandık ve sonra ulaştık

        #endregion
        #region Filtered Include
        //*Sorgulama süreçlerinde Include yaparken sonuçlar üzerinde filtreleme ve sıralama gerçekleştirebilmemiz isağlayan bir özleliktir.

        //var regions = await context.Regions
        //    .Include(r => r.Employees.Where(e => e.Name.Contains("a")).OrderByDescending(e => e.Surname))
        //    .ToListAsync();

        //*Desktelenen fonksiyon : Where, OrderBy, OrderByDescending, ThenBy, ThenByDescending, Skip, Take

        //*Change Tracker'ın aktif olduğu durumlarda Include ewdilmiş sorgular üzerindeki filtreleme sonuçları beklenmeyen olabilir. Bu durum, daha önce sorgulanmş ve Change Tracker tarafından takip edilmiş veriler arasında filtrenin gereksinimi dışında kalan veriler için söz konusu olacaktır. Bundan dolayı sağlıklı bir filtred include operasyonu için change tracker'ın kullanılmadığı sorguları tercih etmeyi düşünebilirsiniz.

        #endregion
        #region Eager Loading İçin Kritik Bir Bilgi
        //*EF Core, önceden üretilmiş ve execute edilerek verileri belleğe alınmış olan sorguların verileri, sonraki sorgularda KULLANIR!

        //var orders = await context.Orders.ToListAsync(); //*burada sadece ordeerları sorguldık

        //var employees = await context.Employees.ToListAsync(); //*burada ise sadece employee sorguladık ama employee ile birlikte order larda geldi ama nasıl
        //*yukarıda yazdığımız bilgi sayesinde orderları çekmiştik ve bellekte duruyordu ve employee ları da çekince bellek de duran orderlarla employee ları ilişkilendirdi ilişkisi olanları employee larda gösterdi. 

        #endregion
        #region AutoInclude - EF Core 6
        //*Uygulama seviyesinde bir entitye karşılık yapılan tüm sorgulamalarda "kesinlikle" bir tabloya Include işlemi gerçekleştirlecekse eğer bunu her bir sorgu için tek tek yapmaktansa merkezi bir hale getirmemizi sağlayan özelliktir.

        //var employees = await context.Employees.ToListAsync(); //*fluent apida ekledik ve employee çağırdığımız her zaman regionlar da geliyor.
        #endregion
        #region IgnoreAutoIncludes
        //*AutoInclude konfigürasyonunu sorgu seviyesinde pasifize edebilmek için kullandığımız fonksiyondur.

        //var employees = await context.Employees.IgnoreAutoIncludes().ToListAsync(); //* bu sorguda autoinclude yani employee ları çağırırken regionlar gelmesin istiyorsak  regionların işimize yaramadığı durumlarda bu sorgu için regionları getirmiyoruz onuda IgnoreAutoIncludes sadece bu sorgu için işe yarar.
        #endregion
        #region Birbirlerinden Türetilmiş Entity'ler Arasında Include

            #region Cast Operatörü İle Include
            var persons1 =  context.Persons.Include(p => ((Employee)p).Orders).ToList();
            #endregion
            #region as Operatörü İle Include
            var persons2 =  context.Persons.Include(p => (p as Employee).Orders).ToList();
            #endregion
            #region 2. Overload İle Include
            var persons3 =  context.Persons.Include("Orders").ToList();
            #endregion
            #endregion
            Console.WriteLine();
        #endregion


        #region Explicit Loading
                //*Bir sonraki derste inceleyeceğiz
        #region Collection Fonksiyonu

        #endregion
        #region Reference Fonksiyonu

        #endregion
        #endregion

        #region Lazy Loading
                //*Bir sonraki derste inceleyeceğiz
        #region N + 1 Problemi

        #endregion
        #endregion
        #endregion

    }
}

public class Person
{
    public int Id { get; set; }

}
public class Employee : Person
{
    //public int Id { get; set; }
    public int RegionId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Salary { get; set; }

    public List<Order> Orders { get; set; }
    public Region Region { get; set; }
}
public class Region
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Employee> Employees { get; set; }
}
public class Order
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }

    public Employee Employee { get; set; }
}


class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Region> Regions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<Employee>()
            .Navigation(e => e.Region)
            .AutoInclude();//*otomatik include yaptık her employee çağırdığımızda regionlarıyla birlikte geliyor
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            optionsBuilder.UseSqlite("Data Source=ApplicationDb");
    }
}
