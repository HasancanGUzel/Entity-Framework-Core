using Microsoft.EntityFrameworkCore;

namespace Veri_Silme;
class Program
{
    static async void Main(string[] args)
    {
        #region Veri nasıl silinir

            // ETicaretContext context = new ETicaretContext();
            // Urun urun=await context.Urunler.FirstOrDefaultAsync(u=>u.Id==5);
            // context.Urunler.Remove(urun);
            // await context.SaveChangesAsync();

        #endregion
        
        #region Silme işleminde ChangeTrackerın Rolü
            //Change Tracker ,context üzerinden gelen verilerin takibinden sorumlu bir mekanızmadır. Bu takip mekanızması sayesinde context üzerinden gelen verilerle ilgili işlemler yahut delete sorgularının oluşturulacağı anlaşılır.


            
        #endregion

        #region Takip Edilmeyen Nesneler Nasıl Silinir
            // ETicaretContext context=new();
            // Urun u = new()
            // {
            //     Id=2
            // };

            // context.Urunler.Remove(u);
            // await context.SaveChangesAsync();
        #endregion
    
        #region EntityState ile Silme İşlemi
            // ETicaretContext context=new();
            // Urun u =new(){Id=1};
            // context.Entry(u).State=EntityState.Deleted;
            // await context.SaveChangesAsync();

        #endregion

        #region Birden fazla Veri Silinirken Nelere Dikkat Edilmelidir.

            //SaveChange verimli kullanraka
        #endregion

        #region RemoveRange
            // ETicaretContext context = new();
            // List<Urun>urunler=await context.Urunler.Where(u=>u.Id>=7 && u.Id <=9).ToListAsync(); // ıd si 7 ve 9 arasında olan verileri listeleyip Urun türünde List şeklinde tutuyoruz ve bunu RemoveRange ile siliyoruz.
            // context.Urunler.RemoveRange(urunler);
            // await context.SaveChangesAsync();
            
        #endregion


    
    }
}


public class ETicaretContext:DbContext
{
    public DbSet<Urun> Urunler{get;set;}
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ETicaretDb");
    }
    
}
public class Urun
{
    public int Id { get; set; }
    public string UrunAdi { get; set; }
    public float Fiyat { get; set; }
}
