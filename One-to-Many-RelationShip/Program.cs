using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace One_to_Many_RelationShip;
class Program
{
      static ESirketDbContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk


    static void Main(string[] args)
    {
        
         var calisanlar =  context.Calisanlar.ToList();
    }

            #region Default Convention
            //Navigation Proeprtyler tanımlanmalı!
        //Default convention yönteminde bire çok ilişkiyi kurarken foreign key kolonuna karşılık gelen bir property tanımlamak mecburiyetinde değilidiz. Eğer tanımlamazsak EF Core bunu kendisi tamamlayacak yok eğer tanımlarsak, tanımladığımızı baz alacaktır.
        // class Calisan //Dependent Entity
        // {
        //    public int Id { get; set; }
        //    public int DepartmanId { get; set; } // bunu burada koymayabiliriz otomatik olarak kendisi koyuyor
        //    public string Adi { get; set; }

        //    public Departman Departman { get; set; }
        // }
        // class Departman
        // {
        //    public int Id { get; set; }
        //    public string DepartmanAdi { get; set; }

        //    public ICollection<Calisan> Calisanlar { get; set; }

        //}
        #endregion
        #region Data Annotations
        //Navigation Proeprtyler tanımlanmalı!
        //Default convention yönteminde foreign key kolonuna karşılık gelen property'i tanımladığımızda bu property ismi temel geleneksel entity tanımlama kurallarına uymuyorsa eğer Data Annotations'lar ile müdahalede bulunabiliriz."
        // class Calisan //Dependent Entity
        // {
        //    public int Id { get; set; }
        //    [ForeignKey(nameof(Departman))]
        //    public int DId { get; set; }

        //    public string Adi { get; set; }

        //    public Departman Departman { get; set; }
        // }
        // class Departman
        // {
        //    public int Id { get; set; }
        //    public string DepartmanAdi { get; set; }

        //    public ICollection<Calisan> Calisanlar { get; set; }

        //}
        #endregion
        #region Fluent API
        //Navigation Proeprtyler tanımlanmalı!
        class Calisan //Dependent Entity
        {
            public int Id { get; set; }
            public int DId { get; set; }
            public string Adi { get; set; }

            public Departman Departman { get; set; }
        }
        class Departman
        {
            public int Id { get; set; }
            public string DepartmanAdi { get; set; }

            public ICollection<Calisan> Calisanlar { get; set; }

        }
        #endregion


        class ESirketDbContext : DbContext
        {
            public DbSet<Calisan> Calisanlar { get; set; }
            public DbSet<Departman> Departmanlar { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
               optionsBuilder.UseSqlite("Data Source=ESirketDB");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Calisan>()// hasOne ile "bire"  ilişki kurcağız , çalışan entitysindeki  departman navigation ni belirtiyoruruz.sonra withMany ile de "çok"  çünkü departman  içindeki calisanlar  navigationunu belirtiyoruz ve hasforeignkey ile de foreign keyi mizi belirtiyoruz. 
                    .HasOne(c => c.Departman)
                    .WithMany(d => d.Calisanlar)
                    .HasForeignKey(c => c.DId);;//eğer çalışan entity si içerisinde  public int DId { get; set; } prop unu tnımlamasaydım burada foreign key tanımlamamam gerek yoktu otomatik olarak tanımlayacaktı
            }
        }
}
