using Microsoft.EntityFrameworkCore;

namespace Indexes;
class Program
{
          static ApplicationDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
            #region Index Nedir?
            //*Index, bir sütuna dayalı sorgulamaları daha verimli ve performanslı hale getirmek için kullanılan yapıdır.
            #endregion
            #region Index'leme Nasıl Yapılır?
                //*PK, FK ve AK olan kolonlar otomatik olarak indexlenir. 

                #region Index Attribute'u
                    //*class üzerine yazdığımız indexleme yöntemi
                #endregion
                #region HasIndex Metodu
                    //*fluent api da yaptığımız yöntem
                #endregion

            #endregion
            #region Composite Index
            //context.Employees.Where(e => e.Name == "" || e.Surname == ""); //*böyle bir sorgu yazacaksan ve verimli olmasını istiyorsak composite indexleme yapabiliriz.

            //*ama sadece name üzerinde veya surname üzerinde bir işlem yapacaksak comosite indexleme bir anlam ifade etmez
            //*ama attribute olarak composite olarak name ve surname olarak özel indexleme yapsarsak sadece name üzerinde işlem yapasark name üzerine yaptığımız indexleme devreye girer. surname üzerinde işlem yaparsam surname üzerine yaptığımız indexleme devreye girer composite işlem yapacaksam composite indexleme devreye girer.
            #endregion
            #region Birden Fazla Index Tanımlama
                    //*yukarıda açıkladım zaten o bakımdan birden fazla index tanımlarız.
            #endregion
            #region Index Uniqueness
                //*aşşağıda açıkladım
            #endregion

            #region Index Sort Order - Sıralama Düzeni (EF Core 7.0)
                    //*default olarak asscending dir.
                #region AllDescending - Attribute
                //*Tüm indexlemelerde descending davranışının bütünsel olarak konfigürasyonunu sağlar.
                #endregion
                #region IsDescending - Attribute
                //*Indexleme sürecindeki her bir kolona göre sıralama davranışını hususi ayarlamak istiyorsak kullanılır.
                #endregion
                #region IsDescending Metodu

                #endregion

            #endregion

            #region Index Name
                //*index e isim vermek için
            #endregion
            #region Index Filter
                //*attribute olarak kullanamıyoruz aşşağıdaki metod sayesinde fluent api olarak kullaanbiliriz
            #region HasFilter Metodu
                    //*fluent api olarak kullanabiliriz.
            #endregion
            #endregion
            #region Included Columns
                //*flueht apida açıklamasını yaptım zaten sadece flunet api da kullanılıyor attribute yok
            #region IncludeProperties Metodu

            #endregion
            #endregion  
  }
}

//[Index(nameof(Name))] //*attribute olarak indexleme yaptık
//[Index(nameof(Surname))]
//[Index(nameof(Name), nameof(Surname))]//*composite olarak indexleme
//[Index(nameof(Name), IsUnique = true)] //*Name propertysini indexle ve unique hale getir dedik mükerrer kayıt giremeyeceğiz
//[Index(nameof(Name), AllDescending = true)] //*bu  descending yapıyor
//[Index(nameof(Name), nameof(Surname), AllDescending = true)] //*bu da descending yapıyor ama 2 kolonuda ben sadece 1 nin descending olmasını istiyorsam aşşağıdaki gibi yapabilirim
//[Index(nameof(Name), nameof(Surname), IsDescending = new[] { true, false })] //*name i descending surname i asscending olarak yarlmaış oluyoruz.
//[Index(nameof(Name), Name = "name_index")] //*index e isim verdik
class Employee
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Salary { get; set; }
}

class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Employee>()
        //.HasIndex(x => x.Name); //*fluent api olarak indexleme yaptık attribute daki 1 sıradakiyle aynı
        //.HasIndex(x => new { x.Name, x.Surname });//*composite olarak indexlme
        //.HasIndex(nameof(Employee.Name), nameof(Employee.Surname));//*composite olarak indexleme yukarıkiyle aynı
        //.HasIndex(x => x.Name) //* name i hem ındexledik ve unique olmasını sağladık
        //.IsUnique();

        //modelBuilder.Entity<Employee>()
        //    .HasIndex(x => x.Name)
        //    .IsDescending(); //*name kolonunu descending index oluşturduk

        //modelBuilder.Entity<Employee>()
        //    .HasIndex(x => new { x.Name, x.Surname })
        //    .IsDescending(true, false); //*attribute da yaptığımız gibi name descending surname asscending index olarak oluşacak

        //modelBuilder.Entity<Employee>()
        //    .HasIndex(x => x.Name)
        //    .HasDatabaseName("name_index"); //*index i isimlendirdik

        //modelBuilder.Entity<Employee>()
        //    .HasIndex(x => x.Name)
        //    .HasFilter("[NAME] IS NOT NULL");//*name i is not null olanları index ledik

        // modelBuilder.Entity<Employee>()
        //     .HasIndex(x => new { x.Name, x.Surname })
        //     .IncludeProperties(x => x.Salary); //*şimdi biz indexleme yaparken employees enttişisndeki name ve surname yi indexledik biz bir sorgu yazdığımız zaman  örneğin
            /*
                context.Employees.Where.......Select(x=>new{
                    x.name,
                    x.surname
                    x.salary
                })
                *burada name ve surname için index yazmıştık performasn lı çalıştı ama salary için bir indexleme yapmamıştık ve bunun için ana tabloya gidiyor bunun için ef core diyorki select sürecinde eklenebilecek propertyleride index tablosuan ekleyebilirisn diyor bu kodumuz bu işe yarıyor.
            */
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            optionsBuilder.UseSqlite("Data Source=ApplicationDb");
    }
}
