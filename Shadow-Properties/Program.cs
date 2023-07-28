using Microsoft.EntityFrameworkCore;

namespace Shadow_Properties;
class Program
{
      static ApplicationDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
        #region Shadow Properties - Gölge Özellikler
        //Entity sınıflarında fiziksel olarak tanımlanmayan/modellenmeyen ancak EF Core tarafından ilgili entity için var olan/var olduğu kabul edilen property'lerdir.
        //Tabloda gösterilmesini istemediğimiz/lüzumlu görmediğimiz/entity instance'ı üzerinde işlem yapmayacağımız kolonlar için shadow propertyler kullanılabilir.
        //Shadow property'lerin değerleri ve stateleri Change Tracker tarafından kontrol edilir.
        #endregion
 
        #region Foreign Key - Shadow Properties
        //*İlişkisel senaryolarda foreign key property'sini tanımlamadığımız halde EF Core tarafından dependent entity'e eklenmektedir. İşte bu shadow property'dir.

        // var blogs =  context.Blogs.Include(b => b.Posts)
        //    .ToList();
        // Console.WriteLine();
        #endregion

        #region Shadow Property Oluşturma
        //*Bir entity üzerinde shadow property oluşturmak istiyorsanız eğer Fluent API'ı kullanmanız gerekmektedir.
            //    modelBuilder.Entity<Blog>()
            //        .Property<DateTime>("CreatedDate");
        #endregion

        #region Shadow Property'e Erişim Sağlama
        #region ChangeTracker İle Erişim
        //* Shadow property'e erişim sağlayabilmek için Change Tracker'dan istifade edilebilir.

        // var blog =  context.Blogs.First();
        // //* blog. dediğimiz zaman CreatedDate gelmiyor bunun için aşşağıdaki adımları yapıyoruz.  
        // var createDate = context.Entry(blog).Property("CreatedDate");
        // Console.WriteLine(createDate.CurrentValue); //* createdDate şuandaki mevcut değeri
        // Console.WriteLine(createDate.OriginalValue);

        // createDate.CurrentValue = DateTime.Now; //*CreatedDate propertmize şimdiki zamanı atıyoruz.
        //  context.SaveChanges();
         #endregion

        #region EF.Property İle Erişim
        //*Özellikle LINQ sorgularında Shadow Propery'lerine erişim için EF.Property static yapılanmasını kullanabiliriz.
        var blogs =  context.Blogs.OrderBy(b => EF.Property<DateTime>(b, "CreatedDate")).ToList();

        var blogs2 =  context.Blogs.Where(b => EF.Property<DateTime>(b, "CreatedDate").Year > 2020).ToList();
        Console.WriteLine();
        #endregion
        #endregion
    }
}


class Blog
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Post> Posts { get; set; }
}

class Post
{
    //*Biz önceden bilmeden shadow property kullanmıştık
    //*Şöyle bizim blog entitysi principal iken Post entitysi dependent entitydir.
    //*dependent entityde 1 tane foreign key olması lazım ama biz onu yazmasakta arka planda EfCore otomatik oluşturuyordu işte bu Shadow property di.
    public int Id { get; set; }
    public string Title { get; set; }
    public bool lastUpdated { get; set; }

    public Blog Blog { get; set; }
}

class ApplicationDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
         optionsBuilder.UseSqlite("Data Source=ApplicationDb");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //*Burada blog entitysinde gözükmesini istemediğimzi bir kolonu burada generic propert<> ile bir kolon eklemiş oluyoruz ve bu shadow property oluyor.
        modelBuilder.Entity<Blog>()
            .Property<DateTime>("CreatedDate");

        base.OnModelCreating(modelBuilder);
    }
}
