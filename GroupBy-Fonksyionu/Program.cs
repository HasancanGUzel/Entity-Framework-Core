using Microsoft.EntityFrameworkCore;

namespace GroupBy_Fonksyionu;
class Program
{
    static ETicaretContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static async void Main(string[] args)
    {
        #region Group By Fonksyionu
        //gurplama yapmamaız sağlayan fonksiyondur.

        #region Method Syntax
            //  var datas =await context.Urunler.GroupBy(u=>u.Fiyat).Select(group=>new
            //  {
            //     Count=group.Count(),
            //     Fiyat = group.Key
            //  }).ToListAsync();
        #endregion

        #region Query Syntax
            var datas = await (from urun in context.Urunler
                        group urun by urun.Fiyat
                        into groupsonuc
                        select new
                        {
                            Fiyat = groupsonuc.Key,
                            Count= groupsonuc.Count()
                        }).ToListAsync();
        #endregion
            
        #endregion

        #region Foreach Fonksiyonu
            //Bir sogrulama fonksşyonu değidlri.
            //sorgulama netişcesinde elde edilen koleskiyonel veriler üzerinde iterasyonlar olarak dönememizi ve teker teker verileri eklde edip işlemler yapabilmememiz sağlyana bir fonjsşyondur.Foreach döngüsünü metod halidir.
            foreach (var item in datas)
            {

            }
            datas.ForEach(x=>
            {

            });
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
