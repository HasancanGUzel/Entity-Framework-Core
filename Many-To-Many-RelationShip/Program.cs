using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Many_To_Many_RelationShip;
class Program
{

      static EKitapDbContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }



    #region Default Convention
//İki entity arasındaki ilişkiyi navigation propertyler üzerinden çoğul olarak kurmalıyız. (ICollection, List)
//Default Convention'da cross table'ı manuel oluşturmak zorunda değiliz. EF Core tasarıma uygun bir şekilde cross table'ı kendisi otomatik basacak ve generate edecektir.
//Ve oluşturulan cross table'ın içerisinde composite primary key'i de otomatik oluşturmuş olacaktır.
// class Kitap
// {
//    public int Id { get; set; }
//    public string KitapAdi { get; set; }

//    public List<Yazar> Yazarlar { get; set; } // burada cross table kurmamaıza gerek yoktu o yüzden kitapların içinde çoka çok olduğunu belirtmek için Yazarları list vs. şeklinde tutuyoruz.
// }
// class Yazar
// {
//    public int Id { get; set; }
//    public string YazarAdi { get; set; }

//    public List<Kitap> Kitaplar { get; set; }// burada cross table kurmamaıza gerek yoktu o yüzden yazarların içinde çoka çok olduğunu belirtmek için Kitapları list vs. şeklinde tutuyoruz.
// }
#endregion
#region Data Annotations
//Cross table manuel olarak oluşturulmak zorundadır.
//DbSet olarak eklenmesine lüzum yok, 
//Entity'lerde oluşturduğumuz cross table entity si ile bire çok bir ilişki kurulmalı.
//Cross Table'da composite primary key'i data annotation(Attributes)lar ile manuel kuramıyoruz. Bunun için de Fluent API'da çalışma yaopmamız gerekiyor.
//Cross table'a karşılık bir entity modeli oluşturuyorsak eğer bunu context sınıfı içerisinde DbSet property'si olarka bildirmek mecburiyetinde değiliz!
// class Kitap
// {
//    public int Id { get; set; }
//    public string KitapAdi { get; set; }

//    public ICollection<KitapYazar> Yazarlar { get; set; } // burada artık default conersion gibi kitap ve yazar entitimizi tutmaya gerek yok onun yerine kendimizin oluşturudğu Corss table ı yazıyoruz ve bunuda list şeklinde tutuyoruz
// }
// //Cross Table
// class KitapYazar
// {
//    [ForeignKey(nameof(Kitap))]
//    public int KId { get; set; }    //public int KitapId { get; set; }  //şimdi biz burada yandaki şekilde değilde KId  ve YId şeklinde kullanırsak ve onmodelcreating içinde de KId ve YId şeklinde kullanırsak bu Id lerin kitaplar ve yazarlar tablosundan olduğunu anlamayacak ve bu oluşturduğumuzun haricinde KitapId ve YazarId foreign key kolonlarını oluşturacak bunun için ya burada yaptığımız gibi propların başına foreign key ibaresini koyacağuıız ya da proplara tabloların adında isim vereceğiz(KitapId,YazarId) ve onmodelcrteaitnde de o isimleri kullanacğız(KitapId,YazarId)

//    [ForeignKey(nameof(Yazar))]
//    public int YId { get; set; }    //public int YazarId { get; set; }

//    public Kitap Kitap { get; set; }
//    public Yazar Yazar { get; set; }
// }
// class Yazar
// {
//    public int Id { get; set; }
//    public string YazarAdi { get; set; }

//    public ICollection<KitapYazar> Kitaplar { get; set; } // burada artık default conersion gibi kitap ve yazar entitimizi tutmaya gerek yok onun yerine kendimizin oluşturudğu Corss table ı yazıyoruz ve bunuda list şeklinde tutuyoruz
// }
#endregion
#region Fluent API
//Cross table manuel oluşturulmalı
//DbSet olarak eklenmesine lüzum yok, 
//Composite Primary Key Haskey metodu ile kurulmalı!
class Kitap
{
    public int Id { get; set; }
    public string KitapAdi { get; set; }

    public ICollection<KitapYazar> Yazarlar { get; set; } // burada artık default conersion gibi kitap ve yazar entitimizi tutmaya gerek yok onun yerine kendimizin oluşturudğu Corss table ı yazıyoruz ve bunuda list şeklinde tutuyoruz
}
//Cross Table
class KitapYazar
{
    public int KitapId { get; set; }
    public int YazarId { get; set; }

    public Kitap Kitap { get; set; }
    public Yazar Yazar { get; set; }
}
class Yazar
{
    public int Id { get; set; }
    public string YazarAdi { get; set; }

    public ICollection<KitapYazar> Kitaplar { get; set; } // burada artık default conersion gibi kitap ve yazar entitimizi tutmaya gerek yok onun yerine kendimizin oluşturudğu Corss table ı yazıyoruz ve bunuda list şeklinde tutuyoruz
}
#endregion

class EKitapDbContext : DbContext
{
    public DbSet<Kitap> Kitaplar { get; set; }
    public DbSet<Yazar> Yazarlar { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseSqlite("Data Source=EKitapDB");
    }


    #region Data Annotations OnModelCreating
             //Data Annotations 3 .açıklamaya bianen bunu yapıyoruz bunu yapmadan olmaz
                // protected override void OnModelCreating(ModelBuilder modelBuilder)
                // {
                // modelBuilder.Entity<KitapYazar>() // entity den kitapyazara geldik.HasKey ile kitap yazardaki KId ve YId compoiste primary key olduğunu belirtiyoruz.
                //     .HasKey(ky => new { ky.KId, ky.YId });
                // }

                //Data Annotations KitapYazardaki açıklamam istinaden yazddım
                // protected override void OnModelCreating(ModelBuilder modelBuilder)
                // {
                //    modelBuilder.Entity<KitapYazar>() // entity den kitapyazara geldik.HasKey ile kitap yazardaki KitapId ve YazarId compoiste primary key olduğunu belirtiyoruz.
                //        .HasKey(ky => new { ky.KitapId, ky.YazarId });
                // }
    #endregion
    
    #region FluentApi
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KitapYazar>() // entity den kitapyazara geldik.HasKey ile kiatp yazaraki KitapId ve YazarId compoiste primary key olduğunu belirtiyoruz.
                .HasKey(ky => new { ky.KitapId, ky.YazarId });

            modelBuilder.Entity<KitapYazar>() // entity den kitapyazara geldik.HasOne ile bir kısmı Kitap olarak belirtiyoruz çok kısmı ise yazarları belirtiyoruz. foreign key de kitapId
                .HasOne(ky => ky.Kitap)
                .WithMany(k => k.Yazarlar)
                .HasForeignKey(ky => ky.KitapId);

            modelBuilder.Entity<KitapYazar>()//burada da tam tersi  entity den kitapyazara geldik.HasOne ile bir kısmı Yazar olarak belirtiyoruz çok kısmı ise kitapları belirtiyoruz. foreign key de YazarId
                .HasOne(ky => ky.Yazar)
                .WithMany(y => y.Kitaplar)
                .HasForeignKey(ky => ky.YazarId);
         }
    #endregion  



    
}
}
