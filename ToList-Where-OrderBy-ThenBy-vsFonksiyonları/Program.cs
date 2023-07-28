using Microsoft.EntityFrameworkCore;

namespace ToList_Where_OrderBy_ThenBy_vsFonksiyonları;


class Program
{
    static ETicaretContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk
    static async void Main(string[] args)
    {
        #region Coğul veri getiren sorgulama fonksiyonları
            
            #region ToListAsync
                //Üretilen sorguyu execute etmemmizi sağlayan fonksiyondur.
                #region Method Syntax

                    // var urunler = context.Urunler.ToListAsync();
                
                #endregion

                #region Query Syntax
                    // var urunler=(from urun in context.Urunler select urun).ToListAsync();// VEYA AŞŞAĞIDAKİ GİBİ
                    
                    // var urunler=from urun in context.Urunler select urun;
                    // var datas= await urunler.ToListAsync();
                #endregion
                
            #endregion
            
            #region Where

                //oluşturuklan sorguya where şartı eklememizi sağlayan fonksiyondur.

                #region Method Syntax
                //  var urunler = await context.Urunler.Where(u=>u.Id>500).ToListAsync();  // ıd si 500 den büyük olan sorgular
                    
                #endregion

                #region Query Syntax
                    // var urunler = from urun in context.Urunler
                    //                where urun.Id>500 && urun.UrunAdi.EndsWith("7")
                    //                select urun; // ID Sİ    500 DEN BÜYÜK VE SONU 7 İLE BİTEN SORGULŞARI GETİRDİ.
                    //  var data = await urunler.ToListAsync();               
                #endregion
            #endregion

            #region OrderBy
            //sorrgu üzerinde sıralama yapmamızı sağlayan bir fonksiyondur.(Default olarak Ascending olarak sıralama yapar istersek orderby dedikden sonra da Ascending yazabiliriz ama gerek yok)
                
                #region Method Syntax
                // var urunler = context.Urunler.Where(u=>u.Id>500 || u.UrunAdi.EndsWith("2")).OrderBy(u=>u.UrunAdi);
                    
                #endregion

                #region Query Syntax
                // var urunler2 = from urun in context.Urunler
                //                 where urun.Id>500 || urun.UrunAdi.StartsWith("2")
                //                 orderby urun.UrunAdi
                //                 select urun;

                #endregion
            #endregion

            #region ThenBy
            //order by üzerinden yapılan sıralama işlmenini farklı kolonlarada uygulamamızı sağlayan bşr fonskyiondur.(Ascending)
            // var urunler = context.Urunler.Where(u=>u.Id>500 || u.UrunAdi.EndsWith("2")).OrderBy(u=>u.UrunAdi).ThenBy(u=>u.Fiyat).ThenBy(u=>u.Id); // şimdi buarada normlade orderby ile urun adina göre sıralama yaptık tamam ama ürün adından aynı olan varsa thenby ile dedikki bunların arasında da fiyayta göre sıralama yap fiyata göre aynı olan varsa da ıd ye göre sırala dedik
            // await urunler.ToListAsync();
                
            #endregion

            #region OrderByDescending
             //sıralama işleminde descending olarak sırlama yapmamızı sağlayan bir fonskyiondur.

             #region Method Syntax
            //  var urunler = await context.Urunler.OrderByDescending(u=>u.Fiyat).ToListAsync();
                
             #endregion

             #region Query Syntax
            //  var urunler =await (from urun in context.Urunler
            //                 orderby urun.UrunAdi descending
            //                 select urun).ToListAsync();

             #endregion
                
            #endregion

            #region ThenByDescending
                 //order by descending üzerinden yapılan sıralama işlmenini farklı kolonlarada uygulamamızı sağlayan bşr fonskyiondur.
                //  var urunler =await context.Urunler.OrderByDescending(u=>u.Id).ThenByDescending(u=> u.Fiyat).ThenBy(u=>u.UrunAdi).ToListAsync(); // şimdi burada order bydescending ile id yi büyükten küçüğe sırakladık sonra aynı  ıd den varsa bu sefer onları fiyata göre descending olarak sıraladık fiyattanda aynı olan varsa urun adi nı ascending thenby olarak sıraladık.
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
