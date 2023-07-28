using Microsoft.EntityFrameworkCore;



namespace Veri_Guncelleme;

class Program
{
    static void Main(string[] args)
    {
        #region Veri Nasıl Güncellenir

            // ETicaretContext context = new ETicaretContext();
            // Urun urun = context.Urunler.FirstOrDefault(u=>u.Id==3);
            // urun.UrunAdi="H ÜRÜNÜ";
            // urun.Fiyat=999;
            // context.SaveChangesAsync();
            
            
        #endregion

        #region ChangeTracker Nedir
            //Change Tracker ,context üzerinden gelen verilerin takibinden sorumlu bir mekanızmadır. Bu takip mekanızması sayesinde context üzerinden gelen verilerle ilgili işlemler yahut delete sorgularının oluşturulacağı anlaşılır.
        #endregion

        #region Takip edilemeyen nesneler nasıl güncellenir
        //Bu nesne context üzerinden gelmedi ve ChangeTracker da devrede değil peki bunu nasıl güncelleriz
        // ETicaretContext context=new ETicaretContext();
        //     Urun urun = new Urun()
        //     {
        //         Id=3,
        //         UrunAdi="yeni ürün",
        //         Fiyat=123
        //     };
            #region Update Fonskyironu
                //ChangeTracker mekanızması taafından takip edilmeyen nesnelerin güncellenmesi için Update Fonksiyonu kullanılır
                //Update fonskyionunu kullanabilmek için kesinlikle ilgili nesnede Id değeri verilmedlidir.

                // context.Urunler.Update(urun); // context oluşturudk ve bu contex e gidiyoruz Urunlere git Update işlemi var diyoruz ve urun nesnesii update et diyoruz.
            // context.SaveChangesAsync();
            #endregion
            
        #endregion
    
        #region EntityState Nedir??

            //Bir entity instancenın durumunu ifae eden bir referanstır.

            // ETicaretContext context = new ETicaretContext();
            // Urun u= new Urun();
            // System.Console.WriteLine(context.Entry(u).State);
        #endregion

        #region Ef Core açısından bir verinin güncelenmesi gerektiğini nasıl anlaşılıyor.
            // ETicaretContext context = new ETicaretContext();
            // Urun urun= context.Urunler.FirstOrDefault(u=>u.Id==3);

            // System.Console.WriteLine(context.Entry(urun).State);// değişiklik olmadığı için Unchanged olarak gözüküyor

            // urun.UrunAdi="Hilmi"; // değişiklik yaptık
            // System.Console.WriteLine(context.Entry(urun).State);//durumuna baktık Modified oldu

            // context.SaveChangesAsync();//veritabanına kaydettik
            // System.Console.WriteLine(context.Entry(urun).State);//tekrar durumuna baktık burdada Unchanged oldu 

        #endregion
    
        #region Birden fazla veri güncellenirken nelere dikkat edilmelidir.
            ETicaretContext context = new ETicaretContext();
            var urunler= context.Urunler.ToList();
            foreach (var urun in urunler)
            {
                urun.UrunAdi+="*";
                // context.SaveChangesAsync();//burada savechanges yaparsak baya maliyetli olur. burada kullanmak yerine foreach içinde işlmler yapılsın her urunun sonuna * eklnesin sonra savechanges yap
                
            }
            context.SaveChangesAsync();// burada yap maliyet azalsın

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


