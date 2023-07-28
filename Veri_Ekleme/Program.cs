using Microsoft.EntityFrameworkCore;

namespace Veri_Ekleme;
class Program
{
    static void Main(string[] args)
    {
        #region Veri Ekleme
            // ETicaretContext context = new ETicaretContext();
            // Urun urun=new()
            // {
            //     UrunAdi="A ürünü",
            //     Fiyat=1000
            // };

            // context.AddAsync(urun);
            // // context.Urunler.AddAsync(urun);  böyle veya yukarıdaki gibi ekleme yapılabilir.
            // context.SaveChangesAsync(); //saveChanges; insert, update  ve delete sorgularını oluşturup bir transaction eşliğinde veritabanına gönderip execute eden fonksiyondur.Eğerki oluşturulan sorgulardan biri başarısız olursa tüm işlemleri geri alır.
       
        #endregion    
        

        #region EF CORE Açısından Bir verinin Eklenmesi Gereketiği Nasıl Anlaşılıyor?
            // ETicaretContext context = new ETicaretContext();
            // Urun urun=new()
            // {
            //     UrunAdi="A ürünü",
            //     Fiyat=1000
            // };
            // Console.WriteLine(context.Entry(urun).State); //Daha ekleme işlemini yapmadan state durumuna baktık (Detached)

            // context.AddAsync(urun);// ürünü ekledik ve sonra durumuna baktık

            // Console.WriteLine(context.Entry(urun).State);// durumu(Added) eklenmeye hazır ekle, eklendi gibi (silmeden Deleted, güncellemede Modified   olucak)

            // context.SaveChangesAsync(); // ve eklenen ürünü veritabanına kaydetme işleminden sonra durumuna baktık

            // Console.WriteLine(context.Entry(urun).State);// durumu (unchanged) veritabanına eklendikten sonra hiçbir değişiklik yapılmamış alamanına geliyor
            
           



        #endregion 

       #region Birden fazla veri eklenirken nelere dikkat edilmedilidir.

        #region SaveChanges
            //SaveChanges fonskyionu her tetiklendiğnde bir transaction oluşturacağından dolayı Ef Core ile yapılan her bir işleme özel kullanmaktan kaçınmalıyız!
       //Çünkü her işleme özel transaction veritabanı açısından ekstradan maliyet demektir.
       //O yüzden mümkün mertebe tüm işlemlerimi tek bir transaction  eşliğinde veritabanına gönderebilmek için savechangesı aşağıdaki gibi tek seferde kullanmak hem maliyet hem de yönetilebilirlik açısıdnan katkıda bulunmuş olacaktır.

            // ETicaretContext context = new ETicaretContext();
            // Urun urun1=new()
            // {
            //     UrunAdi="C ürünü",
            //     Fiyat=1000
            // };
            // Urun urun2=new()
            // {
            //     UrunAdi=" D ürünü",
            //     Fiyat=1000
            // };
            // Urun urun3=new()
            // {
            //     UrunAdi="E ürünü",
            //     Fiyat=1000
            // };

            // context.AddAsync(urun1);
            // // context.SaveChangesAsync();//burada her ürünü ekleme silme güncelleme vs da savchanges yapmak yerine işlmeleri yapıktan sonra en sonda 1 tane savchanges çağırmak yeterli

            // context.AddAsync(urun2);
            // // context.SaveChangesAsync();

            // context.AddAsync(urun3);
            // context.SaveChangesAsync();
        #endregion
        #region AddRange
             ETicaretContext context = new ETicaretContext();
            Urun urun1=new()
            {
                UrunAdi="C ürünü",
                Fiyat=1000
            };
            Urun urun2=new()
            {
                UrunAdi=" D ürünü",
                Fiyat=1000
            };
            Urun urun3=new()
            {
                UrunAdi="E ürünü",
                Fiyat=1000
            };

           context.Urunler.AddRangeAsync(urun1,urun2,urun3);
            context.SaveChangesAsync();
        #endregion
            
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
