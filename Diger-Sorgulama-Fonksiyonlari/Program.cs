
using Microsoft.EntityFrameworkCore;

namespace Diger_Sorgulama_Fonksiyonlari;
class Program
{
    static ETicaretContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static async void Main(string[] args)
    {
        #region CountAsync

            //Oluşturulan sorgunun execute edilmesi neticesinde kaç adet satırın elde edileceğini sayısal olarak(int) bizlere bildiren fonskyiondur
            
            // var urunler= (await context.Urunler.ToListAsync()).Count(); // böyle veriyi çekip sonra count hesaplar

            // var urunler = context.Urunler.CountAsync(); // burada ise direk count hesaplar
            // var urunler = context.Urunler.LongCountAsync(u=>u.Fiyat > 5000); 

        #endregion

        #region LongCountAsync
            //Oluşturulan sorgunun execute edilmesi neticesinde kaç adet satırın elde edileceğini sayısal olarak(long) bizlere bildiren fonskyiondur
            // var urunler = context.Urunler.LongCountAsync(); 
            // var urunler = context.Urunler.LongCountAsync(u=>u.Fiyat > 5000); 

        #endregion

        #region AnyAsync (varmı yokmu )
            //sorgu neticesinde verinin gelip gelmediğini  bool türünde dönen fonskyiondur. Birşey dönüyorsa 1 dönemüyrsa 0 döndür.
            // var urunler = await context.Urunler.AnyAsync();
            // var urunler = await context.Urunler.Where(u=>u.UrunAdi.Contains("1")).AnyAsync();//veya
            // var urunler = await context.Urunler.AnyAsync(u=>u.UrunAdi.Contains("1"));//yukarıdakiyle aynı
        #endregion

        #region MaxAsync
        //verilen kolondaki max değeri getiri.
            // var fiyat = await context.Urunler.MaxAsync(u=>u.Fiyat);
        #endregion

        #region MinAsync
        //verilen kolondaki max değeri getirir.
        //  var fiyat = await context.Urunler.MinAsync(u=>u.Fiyat);
        #endregion

        #region Distinct
            //sorguda mükerrer kayıtlar vsrsa bunları tekilleştiren bir işleve sahip fonksiyondur.
            // var urunler = await context.Urunler.Distinct().ToListAsync(); //IQerayble dönüyor o yüzden tolist çağırdık bizim yukarıdaki fonskiynlarımız biri long biri int biri bool dönüyordu to list çağırmadık o yüzden
        #endregion

        #region AllAsync
            // Bir sorgu neticesinde gelen verilerin verilen şarta uyup uymadığını kontrol etymektedir.Eğer ki veriler şarta uyuyorsa true, uymuyorsa false döndürecektir.
            // var m= context.Urunler.AllAsync(u=>u.Fiyat>5000); // bool dönüyor toListAsync kullanmadık.
        #endregion  

        #region SumAsync
        //vermiş olduğumuz sayıosal kolonun toplamını alır..
        // var fiyatToplam =await  context.Urunler.SumAsync(u=>u.Fiyat);
        #endregion

        #region AverageAsync
            //Vermiş olduğumuz sayılsa propert nin aritmetik ortalamasını alır.
            // var aritmetikOrtalama = await context.Urunler.AverageAsync(u=>u.Fiyat);
        #endregion

        #region ContainsAsync
             //Like sorgusu oluşturmamız sağlar. %...% böyle like sorgusu ama where olmadan olmaz 
            //  var urunler =await  context.Urunler.Where(u=>u.UrunAdi.Contains("7")).ToListAsync();
        #endregion

        #region StartsWith
        //like sorgusu oluşturmamızı sağlar ama  ...% bu tarsa where olmazsa olamz
            //  var urunler =await  context.Urunler.Where(u=>u.UrunAdi.StartsWith("7")).ToListAsync();
            
        #endregion

        #region EndsWith
        //like sorgusu oluşturmamızı sağlar ama  %... bu tarsa where olmazsa olamz
            //  var urunler =await  context.Urunler.Where(u=>u.UrunAdi.EndsWith("7")).ToListAsync();
            
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
