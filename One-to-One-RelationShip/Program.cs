using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace One_to_One_RelationShip;
class Program
{

      static ESirketDbContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static  void Main(string[] args)
    {
            
            var calisanlar =  context.Calisanlar.ToList();
            

    }

            #region Default Convention
            //Her iki entity'de Navigation Property ile birbirlerini tekil olarak referans ederek fiziksel bir ilişkinin olacağı ifade edilir.
            //One to One ilişki türünde, dependent entity'nin hangisi olduğunu default olarak belirleyebilmek pek kolay değildir. Bu durumda fiziksel olarak bir foreign key'e karşılık property/kolon tanımlayarak çözüm getirebiliyoruz.
            //Böylece foreign key'e karşılık property tanımlayarak lüzumsuz bir kolon oluşturmuş oluyoruz.

            // class Calisan //principal
            // {
            //    public int Id { get; set; }
            //    public string Adi { get; set; }

            //    public CalisanAdresi CalisanAdresi { get; set; }
            // }
            // class CalisanAdresi //dependent
            // {
            //    public int Id { get; set; }
            //    public int CalisanId { get; set; } //foreign key
            //    public string Adres { get; set; }

            //    public Calisan Calisan { get; set; }
            // }
            #endregion
           
            #region Data Annotations
            //Navigation Property'ler tanımlanmalıdır.
            //Foreign kolonunun ismi default convention'ın dışında bir kolon olacaksa eğer ForeignKey attribute ile bunu bildirebiliriz.
            //Foreign Key kolonu oluşturulmak zorunda değildir. 
            //1'e 1 ilişkide ekstradan foreign key kolonuna ihtiyaç olmayacağından dolayı dependent entity'deki id kolonunun hem foreign key hem de primary key olarak kullanmayı tercih ediyoruz ve bu duruma özen gösterilidir diyoruz.
            // class Calisan
            // {
            //    public int Id { get; set; }
            //    public string Adi { get; set; }

            //    public CalisanAdresi CalisanAdresi { get; set; }
            // }
            // class CalisanAdresi
            // {
            //    [Key, ForeignKey(nameof(Calisannnn))] // buraya parantez içine aşşağıdaki navigation property nin adını veriyoruz ,hem primary key sin hemde foreign key sin dedik
            //    public int Id { get; set; }

            // //      Bunun yerine maliyeti azaltmak için yukaırdaki gibi yapabiliriz
            // //    [ForeignKey(nameof(Calisannnn))] // buraya parantez içine aşşağıdaki navigation property nin adını veriyoruz
            // //    public int C { get; set; }  //public int CalisanId { get; set; }


            //    public string Adres { get; set; }

            //    public Calisan Calisannnn { get; set; }
            // }
            #endregion

            #region Fluent API
            //Navigation Proeprtyler tanımlanmalı!
            //Fleunt API yönteminde entity'ler arasındaki ilişki context sınıfı içerisinde OnModelCreating fonksiyonun override edilerek metotlar aracılığıyla tasarlanması gerekmektedir. Yani tüm sorumluluk bu fonksiyon içerisindeki çalışmalardadır.
            class Calisan
            {
                public int Id { get; set; }
                public string Adi { get; set; }

                public CalisanAdresi CalisanAdresi { get; set; }
            }
            class CalisanAdresi
            {
                public int Id { get; set; }
                public string Adres { get; set; }

                public Calisan Calisan { get; set; }
            }
            #endregion

                class ESirketDbContext : DbContext
            {
                public DbSet<Calisan> Calisanlar { get; set; }
                public DbSet<CalisanAdresi> CalisanAdresleri { get; set; }
                protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                {
                    optionsBuilder.UseSqlite("Data Source=ESirketDB");
                }
                // Model'ların(entity) veritabanında generate edilecek yapıları bu fonksiyonda içerisinde konfigüre edilir
                protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    modelBuilder.Entity<CalisanAdresi>() // çalışanadresi içindeki Id yi primary kolonunu bildiriyoruz. aynı zamanda aşşağıda ise ıd kolonunun foreign key olduğunu da belirtttik.
                        .HasKey(c => c.Id);

                    modelBuilder.Entity<Calisan>() // hasOne ile bire bir ilişki kurcağız , çalışan entitysindeki  calişanadresi navigation ni belirtiyoruruz.sonra withOne ile de birber bir çünkü calisanadresi içindeki calisan  navigationunu belirtiyoruz ve hasforeignkey ile de foreign keyi mizi belirtiyoruz.
                        .HasOne(c => c.CalisanAdresi)
                        .WithOne(c => c.Calisan)
                        .HasForeignKey<CalisanAdresi>(c => c.Id);
                }
            }



}

