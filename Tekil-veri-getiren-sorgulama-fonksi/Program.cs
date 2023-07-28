using Microsoft.EntityFrameworkCore;

namespace Tekil_veri_getiren_sorgulama_fonksi;
class Program
{
    static ETicaretContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk
 
    static async void Main(string[] args)
    {
        #region Tekil Veri Getiren Sorgulama Fonksyionları
            
             //yazdığımız sorgu sonucunda sadece tek bir kayıt döndüren fonksişyonlara Single veya SingleOrDefault fonksiyonlar denir arasındaki farklar aşşağıda açıklanacak
            #region SingleAsync
               //eğer ki sorgu neticesinde 1 den fazla veri geliyorsa veya hiç gelmiyorsa her iki durmda da  exception(hata) fırlatır

            //var urun =await context.Urunler.SingleAsync(u=>u.Id==55);// tek bir veri getiriyor. çalışıyor

            // var urun =await context.Urunler.SingleAsync(u=>u.Id==55555); // herhangi bir kayıt gelmiyor ve hata alıyoruz.
            
            // var urun =await context.Urunler.SingleAsync(u=>u.Id>55); // birden fazla veri geliyor ama yine hata alıyoruz.
            #endregion

            #region SingleOrDefaultAsync
                // sorgu neticesinde birden fazla veri geliyorsa exception fırlatır hiç veri gelmiyorsa null döner

                // var urun =await context.Urunler.SingleOrDefaultAsync(u=>u.Id==55);//tek bir kayıt getiriyor çalışıyor

                //  var urun =await context.Urunler.SingleOrDefaultAsync(u=>u.Id==55555); // hiç bir kayıt gelmiyor fakat null dönüyor.

                //   var urun =await context.Urunler.SingleOrDefaultAsync(u=>u.Id > 55); // birden fazla veri geliyor ama hata alıyoruz.
            #endregion

            //yapılan sorguda tek bir verinin gelmesi amaçlanıyorsa Firsdt ya da FirstOrDefault fonskyionları kullanılır.
            #region FirstAsync
                //Sorgu neticesinde elde edilen verilerden ilkini getiri. Eğerki hiç veri gelmiyorsa hata fırlatır.

                // var urun =await context.Urunler.FirstAsync(u=>u.Id==55);//tek kayıt döndürüyoruz.

                // var urun =await context.Urunler.FirstAsync(u=>u.Id==55555); // hiç veri gelmiyor hata verir.

                // var urun =await context.Urunler.FirstAsync(u=>u.Id > 55); // 55 ıd den den büyük ne kadar  sorgu varsa getiri ama içinden ilk baştakini getiri yani 56 yı hata vermez.
            #endregion

            #region FirstOrDefaultAsync
                //Sorgu neticesinde elde edilen verilerden ilkinni getiri eğer ki hiç veri gelmiyorsa default olarak null veri döndürür.

                // var urun =await context.Urunler.FirstAsync(u=>u.Id==55);//tek kayıt döndürüyoruz.

                // var urun =await context.Urunler.FirstAsync(u=>u.Id==55555); // hiç veri gelmiyor null döner.

                // var urun =await context.Urunler.FirstAsync(u=>u.Id > 55); // 55 ıd den den büyük ne kadar  sorgu varsa getiri ama içinden ilk baştakini getiri yani 56 yı hata vermez.
            #endregion

            #region SingleAsync , SingleOrDefaultAsync, FirstAsync, FİrstOrDefaultAsync karşılaştırması
                //single ve first arasındaki fark şimdi;
                //biz veritabanına mail kaydediyoruz ve bu maillerin uniqe olduğunu biliyoruz ve hepsi farklı şimdi mailler le iş yaptığımızda tekrarlı mail olmadığını biliyorsak single kullanabiliriz mantıklı olur
                // ama tekrarlı veriler olduğunu düşünüyorsak bun da da first kullanılır yukarıdaki örnekte  id si 55 den büyük olanlar mesela single da hata veriri ama first de hata vermiyor çünkü onlar arasından ilk olanı alır yani 56 id liyi
            #endregion

            #region FindAsync
            //find fonksiyonu primary key kolonuna özel hızlı bir şekilde sorgulama yapmamızı sağlayan bir fonskyiondur.

            //    Urun urun = await context.Urunler.FirstOrDefaultAsync(u=>u.Id==55);

            //    Urun urun = await context.Urunler.FindAsync(55);//bu  yukarıdakiyle aynı ama daha kısa ve hızlı
                #region Composite primary key durumu
                // UrunParca u = await context.UrunParca.FindAsync(2,5);
                    
                #endregion
            #endregion

            #region FindAsync ile SingleAsync, SingleOrDefaultAsync,FirstAsync,FirstOrDefaultAsync Fonksiyonlarının karşılaştıılması
            //FindAsync  
            //Sorgulama sürecince önce context içerisni kontrol edip kaydı bulamadığı taktirde sorguyu veritabanına gönderir.
            //Yalnızca primary key alanlarını sorgulayabilir.
            //Kayıt bulunamazsa null döndürür.

            //Diğerleri
            //Sorguyu her zaman veritabanına gönderir.
            //Tüm kolonları where cümleciği eşliğinde sorgulayabilir.
            //SingleAsync ve FirstAsync exception fırlatır.
            //SingleOrDefaultAsync ve FirsOrDefaultAsync ise null döner
                
            #endregion

            #region LastAsync
                //First ile aynı ama farkı first toplu gelen sorguda  ilkini alırken last ise sonuncuyu alır.
                //Eğerki hiç veri gelmiyorsa hata fırlatır.
                //LastAsync kullanırken OrdeBy dan(ascending veya descending olması öenmli ddeğil) sonra kullanılır farkı bu

                // var urun= await context.Urunler.OrderBy(u=>u.UrunAdi).LastAsync(u=>u.Id>55);
            #endregion

            #region LastOrDefaultAsync
                //First ile aynı ama farkı first toplu gelen sorguda  ilkini alırken last ise sonuncuyu alır.
                //Eğerki hiç veri gelmiyorsa null döner.
                //LastAsync kullanırken OrdeBy dan(ascending veya descending olması öenmli ddeğil) sonra kullanılır farkı bu

                // var urun= await context.Urunler.OrderBy(u=>u.UrunAdi).LastOrDefaultAsync(u=>u.Id>55);
            #endregion








        #endregion
    }
}


public class ETicaretContext:DbContext
{
    public DbSet<Urun> Urunler{get;set;}
    public DbSet<Parca> Parcalar{get;set;}
    public DbSet<UrunParca> UrunParca{get;set;}
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ETicaretDb");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<UrunParca>().HasKey(up=>new{up.UrunId,up.ParcaId});
    }

    
}
public class Urun
{
    public int Id { get; set; }
    public string UrunAdi { get; set; }
    public float Fiyat { get; set; }
    public ICollection<Parca> Parcalar{get;set;}
}
public class Parca
{
    public int Id { get; set; }
    public string ParcaAdi { get; set; }
}

public class UrunParca
{
    public int UrunId { get; set; }
    public int ParcaId { get; set; }
    public Urun Urun{get;set;}
    public Parca Parca { get; set; }


}
