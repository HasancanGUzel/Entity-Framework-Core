using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Owned_Entities_Table_Splitting;
class Program
{
    static ApplicationDbContext context = new();

    static void Main(string[] args)
    {
        
        #region Owned Entity Types Nedir?
        //*EF Core entity sınıflarını parçalayarak, propertylerini kümesel olarak farklı sınıflarda barındırmamıza ve tüm bu sınıfları ilgili entity'de birlkeştirip bütünsel olarak çalışmamıza izin vermektedir.
        //*Böylece bir entity, sahip olunan(owned) birden fazla alt sınıfın birleşmesiyle meydana gelebilmektedir.
        #endregion
        #region Owned Entity Types'ı Neden Kullanırız?
        //*https://www.gencayyildiz.com/blog/wp-content/uploads/2020/12/Entity-Framework-Core-Owned-Entities-and-Table-Splitting.png

        //*Domain Driven Design(DDD) yaklaşımında Value Object'lere karşılık olarak Owned Entity Types'lar kullanılır!
        #endregion
        #region Owned Entity Types Nasıl Uygulanır?
        //*Normal bir entity'de farklı sınıfların referans edilmesi primary key vs. gibi hatalara sebebiyet verecektir. Çünkü direkt bir sınfıın referans olarak alınması ef core tarafından ilişkisel bir tasarım olarak algılanır. Bizlerin entity ieçrisindeki propertyleri kümesel olarak barındıran sınıfları o entity'nin bir parçası olduğunu bildirmemiz özellikle gerekmektedir.

        #region OwnsOne Metodu 
            //*onmodelcreating de yaptık 
        #endregion
        #region Owned Attribute'u
            //* owned Entity typsin  üstüne Owned attributnu yazarak ilişkisel tasarım olduğunu belirtiyoruz
        #endregion
        #region IEntityTypeConfiguration<T> Arayüzü
            //*aşşağıda yaptık
        #endregion

        #region OwnsMany Metodu
        //*OwnsMany metodu, entity'nin farklı özelliklerine başka bir sınıftan ICollection türünde Navigation Property aracılığıyla ilişkisel olarak erişebilmemizi sağlayan bir işleve sahiptir.
        //*Normalde Has ilişki olarak kurulabilecek bu ilişkinin temel farkı, Has ilişkisi DbSet property'si gerektirirken, OwnsMany metodu ise DbSet'e ihtiyaç duymaksızın gerçekleştirmemizi sağlamaktadır.

        //var d = await context.Employees.ToListAsync();
        //Console.WriteLine();
        #endregion
        #endregion
        #region İç İçe Owned Entity Types
        //*işlemedik
        #endregion
        #region Sınırlılıklar
            //*Herhangi bir owned entity type için dbset propertyisine ihtiyaç yoktur.
            //*OnmodelCreating fonksiyonunda entity<T> ile Owned Entity Tupe türübde bir sınıf üzerinde herhangi bir konfigürasyon gerçekleştirilemez!
            //*Owned entity typeların kaltımsal hiyerarşi desteği yoktur.
        #endregion
    }
}

class Employee
{
    public int Id { get; set; }
    //public string Name { get; set; }
    //public string MiddleName { get; set; }
    //public string LastName { get; set; }
    //public string StreetAddress { get; set; }
    //public string Location { get; set; }
    public bool IsActive { get; set; }

    public EmployeeName EmployeeName { get; set; }
    public Address Adress { get; set; }

    public ICollection<Order> Orders { get; set; }
}
class Order
{
    public string OrderDate { get; set; }
    public int Price { get; set; }
}
//[Owned]
class EmployeeName
{
    public string Name { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public EmployeBilmemneName EmployeBilmemneName { get; set; }
}

class EmployeBilmemneName
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
}
//[Owned]
class Address
{
    public string StreetAddress { get; set; }
    public string Location { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region OwnsOne
        //modelBuilder.Entity<Employee>().OwnsOne(e => e.EmployeeName, builder =>
        //{
        //    builder.Property(e => e.Name).HasColumnName("Name");
        //});
        //modelBuilder.Entity<Employee>().OwnsOne(e => e.Adress);
        //*Employee entitisi üzerinden OwsnOne çağırıyoruz ve EmployeeName ve Adress in Owns entity types olduğunu belirtiyoruz.
        #endregion
        #region OwnsMany
        modelBuilder.Entity<Employee>().OwnsMany(e => e.Orders, builder =>
        {
            builder.WithOwner().HasForeignKey("OwnedEmployeeId");
            builder.Property<int>("Id");
            builder.HasKey("Id");
        });
        //*yani şöyle ben order entitysi tanımlamış olsaydım Order entitysi içerisinde ıd si ve Employee bilgisini tutmak zorundaydım ve Db set olarak belirtlemiliydim
        //*ama  biz Order owned Entity types tanımladık ve onu Db set olarak belirtmedik ama Employee içerisinde ICollection olarak tutuk ve Order içinde Employee tutmadık,Id si nide yazmadık owned entity types olduğu için
        //*Orderlara direk erişim gösteremeyiz normal entity olmadıkları için
        #endregion
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());//*burada tanımlamayı yaptık
    }
    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ApplicationDB");
    }
}

class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.OwnsOne(e => e.EmployeeName, builder =>
        {
            builder.Property(e => e.Name).HasColumnName("Name");
        });
        builder.OwnsOne(e => e.Adress);
        //*OwnsOne Onmodelcreatinde yaptığımız gibi burada da yapıyoruz ve EmployeeConfiguration da onmodeCreatinde tanımlamamız gerek
    }
}
