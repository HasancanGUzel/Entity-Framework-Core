using Microsoft.EntityFrameworkCore;

namespace Constraints;
class Program
{
      static ApplicationDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
       #region Primary Key Constraint

        //*Bir kolonu PK constraint ile birincil anahtar yapmak istiyorsak eğer bunun için name convention'dan istifade edebiliriz. Id, ID, EntityNameId, EntityNameID şeklinde tanımlanan tüm propertyler default olarak EF Core tarafından pk constraint olacak şekilde generate edilirler.
        //*Eğer ki, farklı bir property'e PK özelliğini atamak istiyorsan burada HasKey Fluent API'ı yahut Key attribute'u ile bu bildirimi iradeli bir şekilde yapmak zorundasın.

        #region HasKey Fonksiyonu
            //*bunları önceki derslerde görmüştük primary key yapmak için fluent apida kullanılıyotr
        #endregion
        #region Key Attribute'u
            //*bunları önceki derslerde görmüştük primary key yapmak için  entitde kullanılıyor
        #endregion
        #region Alternate Keys - HasAlternateKey
            //*Bir entity içerisinde PK'e ek olarak her entity instance'ı için alternatif bir benzersiz tanımlayıcı işlevine sahip olan bir key'dir.
        #endregion
        #region Composite Alternate Key
            //*2 veya daha fazla  kolonu uniqr key yapmak için kullanılır.
            //*Composite şöyle demek; a ve b kolonu olsun
            /*
                a       b
                a       c
                a       d
                a       e

            *Şimdi burada uniqe composite  olması demek a kolonu tekrar ediyorsa b kolonu tekrar edemez
            *2. satırda a tekrar etti c yazdık 3 .satırda a yine tekrar ederken c yi tekrar yazamayız d yazdık böyle 

            */
        #endregion

        #region HasName Fonksiyonu İle Primary Key Constraint'e İsim Verme
            //*Primary key constraini veritabanında keys klasörünün altında isminin ne olmasını istiyorsan yazabilirsin
        #endregion
        #endregion

        #region Foreign Key Constraint

            #region HasForeignKey Fonksiyonu
                //*foreign key kolonun hangidi olmasını istityorsak bunu fluent apida verebiliriz
            #endregion
            #region ForeignKey Attribute'u
                //*foreign key kolonun hangidi olmasını istityorsak bunu entity de  verebiliriz

            #endregion
            #region Composite Foreign Key
                //*alternate key ile aynı 
            #endregion

            #region Shadow Property Üzerinden Foreign Key
               //*şimdi biz blogId foregin key sildik yazmadığımızı varsayalım(yazmazsak zaten ef core otomatik foreig key atıyor neyse) ve shadow forein key atamak isteyelim burada kolon oluşturuoyurz ve aşşağıda onu foreign key olarak atıyoruz. fluent apida açıkladım aynı yazı ama ordan belki daha iyi anlarsın.
            #endregion

            #region HasConstraintName Fonksiyonu İle foreign Key Constraint'e İsim Verme
                //*hasname ile aynı sadece foreign key constarine isim vermek için kullanıyoruz.
            #endregion
       
        #endregion

        #region Unique Constraint

            #region HasIndex - IsUnique Fonksiyonları
                //*fluent apida tanımlıyoruz
                //*tanımladığımız kolonu uniqe hale getiriyoruz
            #endregion

            #region Index, IsUnique Attribute'ları
                //*class seviyesinde uygulanmak için tasarlanmış yani url üstünde değilde blog class ının üstünde tanımlıyoruz.
                //*tanımladığımız kolonu uniqe hale getiriyoruz
            #endregion

            #region Alternate Key
                //*fluent apida  alternate key i ile de tanımlayabiliriz.
            #endregion
        
        #endregion

        #region Check Constratint

            #region HasCheckConstraint
                //*herhangi bir kolondaki veriyi check etmemiz sağlayan belirli şartlara göre değenlendirip ona göre kabul eden/etmemizi sağlayan bir kısıtlayıcıdı
            #endregion
        
        #endregion
    }
}


//[Index(nameof(Blog.Url), IsUnique = true)] //*url mizin uniqe olmasını istiyoruz bunun için kullanıyoruz
class Blog
{
    public int Id { get; set; }
    public string BlogName { get; set; }
    public string Url { get; set; }

    public ICollection<Post> Posts { get; set; }
}
class Post
{
    public int Id { get; set; }
    //public int BlogId { get; set; }
    public string Title { get; set; }
    public string BlogUrl { get; set; }
    public int A { get; set; } //* bu kolonlar check constarint çin oluşturuldu örneğin bir post girilirken A değeri 10 un altındaysa veya 2 kolonla örnek vericek olursak A değeri B den küçükse değer girilmesin diye şartlar oluşturuyorsak kullanırız.
    public int B { get; set; }

    public Blog Blog { get; set; }
}


class ApplicationDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        #region Primary key constraint
            //modelBuilder.Entity<Blog>()
            //    .HasKey(b => b.Id);
            
            //modelBuilder.Entity<Blog>()
            //    .HasKey(b => b.Id)
            //    .HasName("ornek"); //* primary key constraint ismini ornek olarak değiştirdik

            //modelBuilder.Entity<Blog>()
            //    .HasAlternateKey(b => b.Url ); //*uniqe key oluşturuyoruz (alternate key)

            //modelBuilder.Entity<Blog>()
            //    .HasAlternateKey(b => new{b.Url, b.BlogName }); //*uniqe key oluşturuyoruz (alternate key) ama composite alternate key
        #endregion
        
        #region Foreign Key constraint
            //modelBuilder.Entity<Blog>()
            //    .HasMany(b => b.Posts)
            //    .WithOne(b => b.Blog)
            //    .HasForeignKey(p=>p.BlogId); //*foreign key tanımlama
        
            //modelBuilder.Entity<Blog>()
            //    .HasMany(b => b.Posts)
            //    .WithOne(b => b.Blog)
            //    .HasForeignKey(p=>new{p.BlogId, p.BlogUrl }); //*composite foreign key


            //modelBuilder.Entity<Blog>()
            //    .Property<int>("BlogForeignKeyId"); //*şimdi biz blogId foregin key sildik yazmadığımızı varsayalım(yazmazsak zaten ef core otomatik foreig key atıyor neyse) ve shadow forein key atamak isteyelim burada kolon oluşturuoyurz ve aşşağıda onu foreign key olarak atıyoruz

            //modelBuilder.Entity<Blog>()
            //    .HasMany(b => b.Posts)
            //    .WithOne(b => b.Blog)
            //    .HasForeignKey("BlogForeignKeyId")
            //    .HasConstraintName("ornekforeignkey"); //*foreign key constrainine isim vermek için kullanıyoruz.

        #endregion


        
        //modelBuilder.Entity<Blog>()
        //    .HasIndex(b => b.Url)
        //    .IsUnique(); //*ffluent api da url i böyle uniqe hale getirebiliriz.

        //modelBuilder.Entity<Blog>()
        //    .HasAlternateKey(b => b.Url); //*url in benzersiz uniqe olmasını sağlıyoruz.

        modelBuilder.Entity<Post>()
            .HasCheckConstraint("a_b_check_const", "[A] > [B]"); //*A kolonu B kolonundan büyük olduğu zaman Post girilsin dedik A B den küçük veya eşitse post a değer girilmesin dedik.
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            optionsBuilder.UseSqlite("Data Source=ApplicationDb");
    }
}