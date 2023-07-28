using Microsoft.EntityFrameworkCore;

namespace Change_TRacker_Derinlemesine_Inceleme;
class Program
{

    static ETicaretContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static async void Main(string[] args)
    {
        #region Change Tracker Nedir
            //Context nesnesi üzerinden gelen tüm nesneler/veriler otomatik olarak bir takip mekanizması tarafından izlenirler işte bu takip mekanıznasına changetarcker denir.Change tracker ile nesneler üzerindeki değişiklikler ya da işlemler takip edilerek netice itirabiyle bu işlemler fıtratna uygun sql sorgucukları generate edilir.İşte bu işleme de Change tracking denir.
        #endregion

        #region Change Tracker Propertysi
            //takip edilen nesnelere erişebilmemizi sağlayan ve gerektiği taktirde işlemler gerçekleştirmemizi sağlayan bir property dir.
            //Context sınıfının base classı olan DbContext sınıfının bir memberıdır.

            // var urunler = await context.Urunler.ToListAsync();
            // urunler[6].Fiyat=123;//update
            // context.Urunler.Remove(urunler[7]);//delete
            // urunler[8].UrunAdi="adasda";//Update

            // var datas= context.ChangeTracker.Entries();//chnage tracker ile  elimizde olan verilerin entries durumlarına bakıyoruz modified(unchanged, deleted, update vs)

            // System.Console.WriteLine();
        #endregion

        #region DetecChanges Metodu
            //Ef core, context nesnesi tarafından izlenen tüm nesnelerdeki değişiklikleri change tracker sayesinden takip edebilmekte ve nesnelerde olan verisel değişiklikler yakalanarak bunların anlık görüntülerini(snapshot)'ini oluşturabilir.
            //Yapılan değişikliklerin veritabanına gönderilmeden önce algılandığından emin olmak gerekir.SaveChanges fonksiyonu çağrıldığı anda nesneler Ef Core tarafından otomatik kontrol edilirler.
            //Ancak yapılan operasyonlarda güncel tracking verilerinden emin olabilmek çin değişikliklerin algılanmasıı opsiyonel olarak gerçekleştirmek isteyebiliriz. İşste bunun için DetecChanges fonksiyonu kullanabiliriz ve her ne kadar Ef Core değişiklikleri otomatik algılıyor olsa da siz yine de iradenizle ktroler zorlayabilirsiniz.

            // var urun = await context.Urunler.FirstOrDefaultAsync(u=>u.Id==3);
            // urun.Fiyat = 123;

            // context.ChangeTracker.DetectChanges();
            // await context.SaveChangesAsync();

        #endregion
        
        #region AutoDetectChangesEnabled Property'si
            //ilgili metotlar(savechanges,entries) tarafından detectChanges metodunun otomatik olarak tetiklenmesinin konfigürasyonunu yapmamızı sağlayan property dir.
            //SaveChanges fonksiyonu tetiklendiğinde DetectChanges metodunu içerisinde default olarak çağırılmaktadır.Bu durumda DetectChanges fonksiyonunun kullanımını irademizle yönetmek ve maliyet peformasn optimizasyonun yapmak istediğimiz durumlarda AutoDetectChangesEnabled özelliğini kapatabiliriz.
        #endregion

        #region Entries Metodu
            //Context teki Entry metodunun koleksiyonel versiyonudur.
            //Change Tracker mekanizması tarafından izlenen her entity neznesinin bilgisini EntityEntry türünden elde etmemizi sağlar ve belirli işlemler yapabilmemize olanak tanır.
            //Entries metodu DetectChanges metodunu tetikler.Bu durumda tıpkı savechange da olduğı gibi bir maliyettir.
            //Buradaki maliyetten kaçınmak için AutoDeytectChanges özelliğine false değeri verilebilir.

            // var urunler =await context.Urunler.ToListAsync(); 
            // urunler.FirstOrDefault(u=>u.Id==7).Fiyat=123;//Update
            // context.Urunler.Remove(urunler.FirstOrDefault(u=>u.Id==8));//delete
            // urunler.FirstOrDefault(u=>u.Id==9).UrunAdi="adasda";//Update

            // context.ChangeTracker.Entries().ToList().ForEach(e=>
            // {
            //     if(e.State==EntityState.Unchanged)
            //     {
            //             //yani ilgili statein entitystaei Unchanged ise bunu yap
            //     }
            //     else if(e.State==EntityState.Deleted)
            //     {
            //             //yani ilgili statein entitystaei deleted  ise bunu yap dedik
                    
            //     }
            // });

        #endregion

        #region AcceptAllChanges Metodu
            //SaveChanges( ) veya SaveChanges(true) tetiklendiğinde Ef Core  herşeyin yolunda olduğunu varsayarak track ettiği verilerin takibini keser yeni değişikliklerin takip edilmesini bekler.Bmyle bir durumda beklenmeyen bir durum/olası bşr hata söz konusu olur sa eğer Ef Core takip ettiği nesneleri bırakacağı için bir düzeltme mevzu bahis olamayacaktır.

            //Haliyle bu durumda devreye savechanges(false) ve AcceptAllChanges metotları girecektir.
            //SaveChanges(false) ef core gerekli veritabanı komutlarını yürütmesini söyle ancak gerektiğinde yeniden oynatılabilmesi için değişiklikleri beklemeye / nesneleri takip etmeye cdevame der..Taa ki AcceptAllchnges metdouu irademizle çağırna kadar.

            //SaveChanges(false) ile işlemin başarılı olduğundan emin olursanız AceeptAllChanges metodu ile nesnelerden takibi kesebilirsiniz. 

            // var urunler =await context.Urunler.ToListAsync(); 
            // urunler.FirstOrDefault(u=>u.Id==7).Fiyat=123;//Update
            // context.Urunler.Remove(urunler.FirstOrDefault(u=>u.Id==8));//delete
            // urunler.FirstOrDefault(u=>u.Id==9).UrunAdi="adasda";//Update

            // await context.SaveChangesAsync(false);
            // context.ChangeTracker.AcceptAllChanges();

        #endregion

        #region HasChanges Metodu
            //Takip edilen nesneler arasından değişiklik yapılanların olup olmadığının bilgisini veririR.
            //Arka planda DetectChanges metodunu tetikler.
            // var result=context.ChangeTracker.HasChanges(); 
        #endregion

        #region Entity States
            //Entity nesnelerinin durumlarını ifade eder.
            #region Detached
                //Nesnenin change tracker mekanızması tarafından takip edilmediğini ifade eder.
                // Urun urun = new();
                // System.Console.WriteLine(context.Entry(urun).State);

            #endregion

            #region Added
            //Veritababnına eklenecek nesneyi ifade eder.Added yenüz veritabanına işlenmeyen veriyi iafede eder.Savechanges fonksiyonu çağrğıldığıjndan insert sorgusu oluşturuklacağı anlamına gelir.

                // Urun urun = new(){Fiyat=123,UrunAdi="asddas"};
                // System.Console.WriteLine(context.Entry(urun).State);
                // await context.Urunler.AddAsync(urun);
                // System.Console.WriteLine(context.Entry(urun).State);
                // await context.SaveChangesAsync();
                // urun.Fiyat=321;
                // System.Console.WriteLine(context.Entry(urun).State);
                // await context.SaveChangesAsync();

                

            #endregion
       
            #region Unchanged
                //veritabanından sorgulandığından beri nesne üzerinde herhangi bir değişiklşik yapılmadığını ifade eder. Sorgu neticesinde elde edilen tüm nesneler başlanğıçta bu state değerindedir.
                // var urunler = await context.Urunler.ToListAsync();

                // var data = context.ChangeTracker.Entries();
                // System.Console.WriteLine();
            #endregion

            #region Modified
                //Nesne üzerinde değişiklşik yani güncelleme yapıldığını iafde eder. SvaeCahngaes dfonkyionu çağrııldığında Uğpdate sorgusu oluşturulacağı anlamanına gelir.

                // var urun = await context.Urunler.FirstOrDefaultAsync(u=>u.Id==3);
                // System.Console.WriteLine(context.Entry(urun).State);
                // urun.UrunAdi="asdasdas";
                // System.Console.WriteLine(context.Entry(urun).State);
                // await context.SaveChangesAsync();
                // System.Console.WriteLine(context.Entry(urun).State);  
            #endregion

            #region Deleted 
                //Nesnenin silidniğini iafde eder.SaveChanges fonksiyonu çağrııldığında delete sorgusu oluşturulacağı anlamnına gelir.
                // var urun = await context.Urunler.FirstOrDefaultAsync(u=>u.Id==4);
                // context.Urunler.Remove(urun);
                // System.Console.WriteLine(context.Entry(urun).State);
                // await context.SaveChangesAsync();
            #endregion
       
            #region Context nesnesi üzerinden Change Tracker
                // var urun = await context.Urunler.FirstOrDefaultAsync(u=>u.Id==55);
                // urun.Fiyat=123;
                // urun.UrunAdi="Silgi"; // modified uğdate işlemi yaptık ama daha kaydetmedik şimdi biz 55 id li ürünün uğdate yapmadan önceki bilgilerini isteseydik bunun için OriginalValues Property kullanabiliriz.
                #region Entry Metodu
                    #region OriginalValues Propertysi
                        // var fiyat = context.Entry(urun).OriginalValues.GetValue<float>(nameof(Urun.Fiyat));
                        // var urunAdi = context.Entry(urun).OriginalValues.GetValue<string>(nameof(Urun.UrunAdi));
                    #endregion

                    #region CurrentValues Propertysi de nesnenin o anki değerini verir
                        // var urunadi=context.Entry(urun).CurrentValues.GetValue<string>(nameof(Urun.UrunAdi));
                    #endregion

                    #region GetDatabaseValue propertsi
                    //veritabanındaki verinin güncel halini getiriyor.Orignalvalues gibi
                        // var _urun= await context.Entry(urun).GetDatabaseValuesAsync();
                    #endregion
                #endregion
            #endregion   
       
            #region Change Tracker ın Intercaptor olarak kullanılması
                //satır 197 de başlıyor
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


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken=default)
    {
        var entries=ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            if (entry.State==EntityState.Added)
            {
                
            }
        }
        return base.SaveChangesAsync(cancellationToken);
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

