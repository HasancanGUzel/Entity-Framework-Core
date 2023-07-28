using Microsoft.EntityFrameworkCore;

namespace Sorgu_Sonucu_Donusum_Fonskyionlari;
class Program
{
    static ETicaretContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static async void Main(string[] args)
    {
        //Bu fonskyionlar ile sorgu neticesinde elde edilen verilerin istediğimz doğrultusunda farklı türlerde projeksiyon edebiliyoruz.

        #region ToDictionaryAsync
        //sorgu neticesinde gelecek olan veriyi bir dictioanry olarak elde etmek / tutumak / karşılamak istiyorsak eğer kullanılır
            // Dictionary<string,float> urunler =await context.Urunler.ToDictionaryAsync(u=>u.UrunAdi,u=>u.Fiyat);//veya
            // var urunler1 =await context.Urunler.ToDictionaryAsync(u=>u.UrunAdi,u=>u.Fiyat); // aynısı
            

            // tolist ile aynı amaca hizmet etmektedir. Yani, oluşturulan sorguyu execute edip neticesini alrılar.
            //ToList: Gelen sorgu neticesini entity türünde bir koleksiyona(List<TEntity>) dönüştürmektedir.
            //ToDictionary: Gelen sorgu neticesini Dictionary türünde bir koleksiyona dönüştürecektir.
        #endregion

        #region ToArrayAsync
            //Oluşturulan sorguyu dizi olarak elde eder.
            //Tolist ile muadil amaca hhizmet eder. yani sorguyu execute eder lakin gelen sonucu entity dizi olarak  elde eder.

            //  Urun[] urunler= await context.Urunler.ToArrayAsync(); // veya
            //  var urunler1= await context.Urunler.ToArrayAsync(); //aynısı
        #endregion

        #region Select
        //select fonksiyonunun işlevsel olarak birden fazla davranışı söz konusudur.
        //select sadece ilgili tabloya yani urun tablosuna etki eder
        // 1.) select fonksyionu generate edilecek sorgunun çekilecek kolonlarını sağlamaktadır.

            // var urunler = await context.Urunler.Select(u=>new Urun
            // {
            //     Id=u.Id,
            //     Fiyat=u.Fiyat
            // }) .ToListAsync();

         // 2.) Select fonksyionu gelen verileri farklı türlerd karşılamamaız sağlar. T,anonim 

            //  var urunler = await context.Urunler.Select(u=>new  //new dedikten sonra Urun demedik böylece anonim olmuş oldu
            // {
            //     Id=u.Id,
            //     Fiyat=u.Fiyat
            // }) .ToListAsync();
            
        #endregion
    
        #region SelectMany
        //select many ise select gibi ilgili tabloya etki eder ama yanında başka tabloyada etki eder yani parcalar tablosuna çünkü urun içinde ICollection ile parcaları tutyoruz
            //Select ile aynı amaca hizmet eder lakin ilişkisel tablolar neticesinden gelen koleksiyonel verileri de tekilleştirip projeksiyon etmemeizi sağlar.
            //   var urunler = await context.Urunler.Include(u=>u.Parcalar).SelectMany(u=>u.Parcalar,(u,p)=>new
            //   {
            //     u.Id,
            //     u.Fiyat,
            //     p.ParcaAdi
            //   }).ToListAsync();
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
