using Microsoft.EntityFrameworkCore;

namespace Backign_Fields_Ozelligi;
class Program
{
      static BackingFieldDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
        var person = context.Persons.Find(1);
        Person person2 = new()
        {
           Name = "Person 101",
           Department = "Department 101"
        };

         context.Persons.Add(person2);
         context.SaveChanges();

        
    }
}


    #region Backing Fields
    //* Tablo içerisindeki kolonları, entity class'ları içerisinde property'ler ile değil field'larla temsil etmemizi sağlayan bir özelliktir.
    // class Person
    // {
    //    public int Id { get; set; }
    //    public string name;
    //    public string Name { get => name.Substring(0, 3); set => name = value.Substring(0, 3); }
    //    public string Department { get; set; } //*şimdi biz burada  property yerine fieldlarla temsil ettik yani veritabanından gelen veya veritabanına gönderşlen veriyi kapsülleme yapma imkanımız var. eğerki bizim get ve set içinde yaptığımız substring işlemi olmaysadı(sadece substirng ile ilgili değil) yukarıdaki ilk satır da find operatörüyle yaptığımız yer de field yani name null dönecekti
    // }
    #endregion

    #region BackingField Attributes
    // class Person
    // {
    //    public int Id { get; set; }
    //    public string name;
    //    [BackingField(nameof(name))] //*burada da yukarıdaki gibi null dönmesin diye açıklamasını yukarıda yaptım
    //    public string Name { get; set; }
    //    public string Department { get; set; }
    // }
    #endregion

    #region HasField Fluent API
    //* Fleunt API'da HasField metodu BackingField özelliğine karşılık gelmektedir.
    class Person
    {
       public int Id { get; set; }
       public string name;
       public string Name { get; set; }
       public string Department { get; set; }
    }
    #endregion

    #region Field And Property Access
    //EF Core sorgulama sürecinde entity içerisindeki propertyleri ya da field'ları kullanıp kullanmayacağının davranışını bizlere belirtmektedir.

    //EF Core, hiçbir ayarlama yoksa varsayılan olarak propertyler üzerinden verileri işler, eğer ki backing field bildiriliyorsa field üzerinden işler yok eğer backing field bildirildiği halde davranış belirtiliyorsa ne belirtilmişse ona göre işlemeyi devam ettirir.

    //UsePropertyAccessMode üzerinden davranış modellemesi gerçekleştirilebilir.
    #endregion

    #region Field-Only Properties
    //Entitylerde değerleri almak için property'ler yerine metotların kullanıldığı veya belirli alanların hiç gösterilmemesi gerektiği durumlarda(örneğin primary key kolonu) kullanabilir.
    // class Person
    // {
    //     public int Id { get; set; }
    //     public string name;
    //     public string Department { get; set; }

    //     public string GetName()
    //         => name;
    //     public string SetName(string value)
    //         => this.name = value;
    // }
    #endregion

    class BackingFieldDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlite("Data Source=BackingFieldDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Person>()
            //    .Property(p => p.Name)
            //    .HasField(nameof(Person.name))
            //    .UsePropertyAccessMode(PropertyAccessMode.PreferProperty);

            //* Field : Veri erişim süreçlerinde sadece field'ların kullanılmasını söyler. Eğer field'ın kullanılamayacağı durum söz konusu olursa bir exception fırlatır.
            //* FieldDuringConstruction : Veri erişim süreçlerinde ilgili entityden bir nesne oluşturulma sürecinde field'ların kullanılmasını söyler.,
            //* Property : Veri erişim sürecinde sadece propertynin kullanılmasını söyler. Eğer property'nin kullanılamayacağı durum söz konusuysa (read-only, write-only) bir exception fırlatır.
            //* PreferField,
            //* PreferFieldDuringConstruction,
            //* PreferProperty

            // modelBuilder.Entity<Person>()
            //     .Property(nameof(Person.name));
        }
    }
