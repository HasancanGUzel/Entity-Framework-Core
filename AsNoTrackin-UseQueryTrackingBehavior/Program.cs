using Microsoft.EntityFrameworkCore;

namespace AsNoTrackin_UseQueryTrackingBehavior;
class Program
{
    static ETicaretContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
        #region AsNoTracking Metodu
            //Context üzerinden gelen tüm datalar Change Tracker mekanızması tarafından takip edilmektedir.

            //Change tracker takip ettiği nesnelerin sayısıylsa doğru oranytılı olacak şekilde bir maliyete sahiptir. O yzüden üzerinde işlem yapılmayacak verilerin takip edilmesi bizlere lüzumsuz yere maliyet çıkaracaktır.

            //AsNotRacking metodu context üzerinden sorgu neticesinde gelecek olan verilerin  Change Tracker tarafından takip edilmesini engeller.

            //AsNoTrackin metodu ile ChangeTrackerın ihtiyaç olmayan verilerdeki maliyetini törpülemiş oluruz.

            //AsNoTracking fonksiyonu ile yapılan sorgulamalrda verileri elde edbilir bu verileri istenilen noktalarda kullanabilir lakin veriler üzerinde herangi bir değişiklik / update işlemi yapamayız.

            // var kullanicilar = context.Kullanicilar.AsNoTracking().ToList(); // kullanıcıları çek ama instanclarını takip edilmesine müsade etme yani takip etme
            // foreach (var kullanici in kullanicilar)
            // {
            //     System.Console.WriteLine(kullanici.Adi);
            // }
        #endregion

            //Change tracker aktif ise;
            //Change tracker mekanizması sayesinde yinelenen datalar aynı instanceları kullanırlar yani;
            //şimdi bizim kullanıcılar ve roller tablomuz var
            //ve 3 kullanıcımız olsun a , b , c kullanıcıları 2 de rolümüz olsun user ve admin
            // a kullanıcısı user rolünü atayalım
            // b ye de admin rolünü atyalım.
            // c ye de admin rolünü atadığımız zaman ayrı bir admin rolü oluşturulmayacak mükerrer olarak oluşturulmayacak aynı admin rolünü alacak 

            //eğer biz bağlantıyı koparırsak yani AsNoTracking mekanızmasını kullanırsak her yinelenen data için ayrı bir instance kullanır.
            //yani yukarıdaki gibi b ve c aynı admin instanc ını kullanamaz ayrı ayrı admin rolünü kullanır.

            //Bunun için AsNoTrackingWithIdentityResolution metodunu kullanabiliriz.


        #region AsNoTrackingWithIdentityResolution Metodu
            //Change tracker mekanzıması yinelenen verileri tekil instance olarak getirir. Buradan ekstradan bir performans kazanımı söz konusudur.

            //Bizler yaptığımız sorgularda takip mekanzıamsının AsNoTracking möetodu ile maliyetini kırmaks isterken bazen maliyete sebebiyet verebilriiz.(Özellikle ilişkisel tabloları sorgularken bu duruma dikkat etmemiz gerekiyor.)

            //AsNoTracking ile elde edilen veriler takip edilemeyecğinden dolayı yinelenen verilerin ayrı instanclarda olmasına sebebiyet veriyoruzç.Çünkü change tracker mekanızması takip ettiği nesneden bellekte varsa eğer aynı nesneden birdaha oluşturma gereği duymaksızın o nesneye ayrı noktalardaki ihtiyacı aynı instance üzerinden gidermektedir.


            //Böyle bir durumda hem takip mekanzıamsının maliyetini ortdan kaldırmak hem de yinelenen dataları tek bir instance üzerinden karşılamak için AsNoTrackingWithIdentityResolution fonksiyonunu kullanabiliriz. 

            // var kitaplar = context.Kitaplar.Include(k=>k.Yazarlar).AsNoTrackingWithIdentityResolution().ToList();

            //AsNoTrackingWithIdentityResolution fonksiyonu AsnoTracking fonksiyonuna nazaran görece yavaştır/maliyetlidir. Lakin ChangeTracker a nazaran daha performanslı ve az maliyetlidir.
        #endregion

        #region AsTracking Metodu
            //oluşturulan sorgunun takip edilmesini istiyorsak bunu kullanabiliriz. AsNoTracking in tam tersidir.

            //context üzeerinden gelen dataların change tracker tarafından takip edilmesini iradeli bir şekilde ifade etmemizi sağlayan fonksiyondur.

            //neden kullanılır?

            //Bir sonraki inceleyeceğimiz UseQueryTrackingBehavior metodunun davranışı gereği uygulama seviyesinde ChangeTracker ın default olarak devre de olup olmamasını ayarlıyor olacağız.Eğerki default olarak pasif hale getrişlirse böyle durumda takip mekanızmasının ihtiyaç olduğu sorgularda AsTracking fonksiyonunu kullanabilir ve böylece takip meknaızmasını iradeli bir şekilde devereye sokmulş oluruz.

            // var kitaplar = context.Kitaplar.AsTracking().ToList(); 
        #endregion

        #region UseQueryTRackingBehavior Metodu
            //EfCore / uygulama seviyesinden ilgili contexten gelen verlerin üzerinde changetracker mekanızmasının davranışı temel seviyede belirlememizi sağlayan fonksiyondur.Yani konfigürasyon fonksiyonudur.
        #endregion
    }
}


public class ETicaretContext:DbContext
{
    public DbSet<Kullanici> Kullanicilar{get;set;}
    public DbSet<Rol> Roller{get;set;}
    public DbSet<Kitap> Kitaplar{get;set;}
    public DbSet<Yazar> Yazarlar{get;set;}
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ETicaretDb");

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);//default hali TRackall dır. 
        //context üzerinden gelen tüm datalar artık default olarak takip edilemeyecek ama biz takip etmek istiyorsak o noktalarda AsTracking metodunu kullanacağız
    }
    
}
public class Kullanici
{
    public int Id { get; set; }
    public string Adi { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<Rol> Roller { get; set; }
}
public class Rol
{
    public int Id { get; set; }
    public string RolAdi { get; set; }
    public ICollection<Kullanici> Kullanicilar { get; set; }
}
public class Kitap
{
    public Kitap() => Console.WriteLine("Kitap nesnesi oluşturuldu."); // constructıor
    public int Id { get; set; }
    public string KitapAdi { get; set; }
    public int SayfaSayisi { get; set; }

    public ICollection<Yazar> Yazarlar { get; set; }
}
public class Yazar
{
    public Yazar() => Console.WriteLine("Yazar nesnesi oluşturuldu."); //constructıor
    public int Id { get; set; }
    public string YazarAdi { get; set; }

    public ICollection<Kitap> Kitaplar { get; set; }
}
