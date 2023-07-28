using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LazyLoading;
class Program
{
        static ApplicationDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk
    static void Main(string[] args)
    {
        #region Lazy Loading Nedir?
        //*Navigation property'ler üzerinde bir işlem yapılmaya çalışıldığı taktirde ilgili propertynin/ye temsil ettiği/karşılık gelen tabloya özel bir sorgu oluşturulup execute edilmesini ve verilerin yüklenmesini sağlayan bir yaklaşımdır.
        #endregion

        //var employee = await context.Employees.FindAsync(2);
        //Console.WriteLine(employee.Region.Name);//*şimdi burada biz ilk önce employee lardan id si 2 olanı buldk getirdik ama order ve regionları boş bu satır da da employee içindeki region ın name ini yazdırmaya çalıştık hata verdi çünkü region boştu 
        //*peki lazy loadin neye yarar biz employee dan ne zamanki regionu çağırırz ozaman veritabanına gder sql sorgusunu oluşturur execute edildikten sonra veriler getirilir sonra name yazdırabiliriz.

        //!Proxiyi ekledikden sonra
        //*Proxie yi ekledikten sonra cw da yaptığımız sorgu çalışacak hata vermez 
        //*yani ilk satırda region ve order gelmez cw da employee içinde regionu çağırınca veritabanına gider vegerekli bilgileri alır gelir
        //*Birde ilk satırda region ve order gelmez dedim ama şimdi biz debug koyarak çalışırsak ve ilk satırda employee içine baktığımız zaman hangi bilgiler gelmiş diye lazy loading çalıştırmış oluruz ve tam baktığımız zaman order ve region bilgisini getirmiş olur 

        #region Prox'lerle Lazy Loading
        //Microsoft.EntityFrameworkCore.Proxies //* bunu projemize kurmamız lazım ve onmodelcreting de eklememiz lazım

            #region Property'lerin virtual Olması
            //*Eğer ki proxler üzerinden lazy loading operasyonu gerçekleştiriyorsanız Navigtation Propertylerin virtual ile işaretlenmiş olması gerekmektedir. Aksi taktirde patlama meydana gelecektir.
            #endregion

        #endregion

        #region Proxy Olmaksızın Lazy Loading
        //*Prox'ler tüm platformlarda desteklenmeyebilir. Böyle bir durumda manuel bir şekilde lazy loading'i uygulamak mecburiyetinde kalabiliriz.

        //*Manuel yapılan Lazy Loading operasyonlarında Navigation Proeprtylerin virtual ile işaretlenmesine gerek yoktur!
        //*Biz proxi tabanlı yapmıştık bir önceki örnekte ve onmodelcretaing de eklemiştik eğer prox olmadan manuel olarak yapacaksak  eklediğimiz yere parantez içine .UseLazyLoadingProxies(false) yazmamız lazım

            #region ILazyLoader Interface'i İle Lazy Loading
            //Microsoft.EntityFrameworkCore.Abstractions //*bu kütüphaneyi kurmamız lazım
            //var employee = await context.Employees.FindAsync(2);
            #endregion
            #region Delegate İle Lazy Loading
            //var employee = await context.Employees.FindAsync(2);
            #endregion

        #endregion

        #region N+1 Problemi
        //var region = await context.Regions.FindAsync(1);
        //foreach (var employee in region.Employees)
        //{
        //    var orders = employee.Orders;
        //    foreach (var order in orders)
        //    {
        //        Console.WriteLine(order.OrderDate);
        //    }
        //} //*ayni şimdi buarad region oluişturdu region içimn employee oluştrudu tamam sıkıntı yok ama döngü içine girdiğimizm zaman her bir employee için order oluşturuyor sürekli sorgu çalışıyor bu sıkıntı maliyeti fazlalaştırır
        #endregion
    }
}

//*Lazy Loading, kullanım açısından oldukça maliyetli ve performans düşürücü bir etkiye sahip yöntemdir. O yüzden kullanırken mümkün mertebe dikkatli olmalı ve özellikle navigation propertylerin döngüsel tetiklenme durumlarında lazy loading'i tercih etmemeye odaklanmalıyız. Aksi taktirde her bir tetiklemeye karşılık aynı sorguları üretip execute edecektir. Bu durumu N+1 Problemi olarak nitelendirmekteyiz.
//*Mümkün mertebe, ilişkisel verileri eklerken Lazy Loading kullanmamaya özen göstermeliyiz.


#region Proxy İle Lazy Loading
public class Employee
{
    public int Id { get; set; }
    public int RegionId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Salary { get; set; }
    public virtual List<Order> Orders { get; set; }//*virtual ile işarteliyoruz proxi ile lazy loading için şart
    public virtual Region Region { get; set; } //*virtual ile işarteliyoruz proxi ile lazy loading için şart
}
public class Region
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }//*virtual ile işarteliyoruz proxi ile lazy loading için şart
}
public class Order
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public virtual Employee Employee { get; set; }//*virtual ile işarteliyoruz proxi ile lazy loading için şart
}
#endregion
#region ILazyLoader Interface'i İle Lazy Loading
// public class Employee
// {
//    ILazyLoader _lazyLoader;//*referans oluşturduk
//    Region _region;//*regionla ilgili referans oluştuduk
//    List<Order> _order;//*kendim yaptım
//    public Employee() { } //*employeenin normal constructır ı
//    public Employee(ILazyLoader lazyLoader)
//        => _lazyLoader = lazyLoader; ////*oluştuduğumuz referansa oluştuduğumuz parametreyi atıyoruz.

//    public int Id { get; set; }
//    public int RegionId { get; set; }
//    public string? Name { get; set; }
//    public string? Surname { get; set; }
//    public int Salary { get; set; }
//    public List<Order> Orders  //*ekdnim ekledim
//    {
//         get => _lazyLoader.Load(this, ref _order);
//         set => _order = value;
//     }

//    public Region Region
//    {
//        get => _lazyLoader.Load(this, ref _region);//*_lazyloader .Load ile lazyloading işlemini gerçekleştirdi diyoruz this ilede bu entity üzerinde demek  neye karşı yapacağız onu da ref parametresi ile yukarıda oluştuduğumuz_region referansıyla
//        set => _region = value;
//    }
// }
// public class Region
// {
//    ILazyLoader _lazyLoader; //*yukarıda yaptığımızın aynısı region için yapıyoruz.
//    ICollection<Employee> _employees;//*navigation propertynin türü ne ise aynı türden olması lazım yani aşşağıda ICollection burada da  aynı olması lazım
//    public Region() { }
//    public Region(ILazyLoader lazyLoader)
//        => _lazyLoader = lazyLoader;
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public ICollection<Employee> Employees
//    {
//        get => _lazyLoader.Load(this, ref _employees);
//        set => _employees = value;
//    }
// }
// public class Order
// {
//     ILazyLoader _lazyLoader;//*referans oluşturduk
//    Employee _employee;//*Employee ilgili referans oluştuduk
//    public Order() { } //*order ın normal constructır ı
//    public Order(ILazyLoader lazyLoader)
//        => _lazyLoader = lazyLoader; ////*oluştuduğumuz referansa oluştuduğumuz parametreyi atıyoruz.

//    public int Id { get; set; }
//    public int EmployeeId { get; set; }
//    public DateTime OrderDate { get; set; }
//    public Employee Employee { 
//     get => _lazyLoader.Load(this, ref _employee);//*_lazyloader .Load ile lazyloading işlemini gerçekleştirdi diyoruz this ilede bu entity üzerinde demek  neye karşı yapacağız onu da ref parametresi ile yukarıda oluştuduğumuz _employee referansıyla
//        set => _employee = value; }
// }

#endregion
#region Delegate İle Lazy Loading
// public class Employee
// {
//    Action<object, string> _lazyLoader; //*Ilazyloader dan farkı delegate oluşturuyoruz o kadar
//    Region _region;
//    public Employee() { }
//    public Employee(Action<object, string> lazyLoader)
//        => _lazyLoader = lazyLoader;
//    public int Id { get; set; }
//    public int RegionId { get; set; }
//    public string? Name { get; set; }
//    public string? Surname { get; set; }
//    public int Salary { get; set; }
//    public List<Order> Orders { get; set; }
//    public Region Region
//    {
//        get => _lazyLoader.Load(this, ref _region); //* delegate yönteminde load kullanamıyoruz normalde bunun için aşşağıda Load ile ilgili tanımlama yapıyoruz.
//        set => _region = value;
//    }
// }
// public class Region
// {
//    Action<object, string> _lazyLoader;
//    ICollection<Employee> _employees;
//    public Region() { }
//    public Region(Action<object, string> lazyLoader)
//        => _lazyLoader = lazyLoader;
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public ICollection<Employee> Employees
//    {
//        get => _lazyLoader.Load(this, ref _employees);
//        set => _employees = value;
//    }
// }
// public class Order
// {
//    public int Id { get; set; }
//    public int EmployeeId { get; set; }
//    public DateTime OrderDate { get; set; }
//    public Employee Employee { get; set; }
// }

// static class LazyLoadingExtension
// {
//    public static TRelated Load<TRelated>(this Action<object, string> loader, object entity, ref TRelated navigation, [CallerMemberName] string navigationName = null)
//    {
//        loader.Invoke(entity, navigationName);
//        return navigation;
//    }
// }
//*burada action delegate ine karşılık extension yapacağımız için this Action diyerek loader ismini verdik
//*loaderi tetiklemek istiyoruz parametre veya invoke ile de tetikleyebiliriz. invoke bizden 2 tane parametre istiyor bunlar dan birincisi entity istiyor 2.si ise  entity içerisinde layzloading e tabi tutulan navigationproperty hangisiyse onu vericez bunları nereden alıcaz.
//*entity bilgisini Load metodunun parametresinden object entity dediğimiz yerden
//*navigation property alabilmem içinde ilk önce türünü almam lazım onuda yukarıdaki örneklerde ref diyerek alıyorduk buradad da aynı parametreden alıcaz ama gerneric yapılanmayla ref TRelated navigation diyerek türünü alıyorum
//*son olarak da navigation property nin ismini alıyoruz.
#endregion

class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Region> Regions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlite("Data Source=ApplicationDb");

        //optionsBuilder.UseLazyLoadingProxies(); //*proxies burada çağırmamız lazım veya yukaırda veritabanı bağlantısından öncede tanımlayabiliriz. 
    }
}
