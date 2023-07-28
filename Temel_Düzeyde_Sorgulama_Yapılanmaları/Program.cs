
using Microsoft.EntityFrameworkCore;

namespace Temel_Düzeyde_Sorgulama_Yapılanmaları;
class Program
{
    static ETicaretContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk
    static async void Main(string[] args)
    {
        #region Temel Basit bir sorgulama Nasıl yapılır
            #region Method Syntax
            // List<Urun>urunler =await context.Urunler.ToListAsync(); // veya
            // var urunler =await context.Urunler.ToListAsync();
            
        #endregion

        #region Query Syntax

            // var urunler2=await (from herhangisim in context.Urunler select herhangisim).ToListAsync();
        #endregion
        
        #endregion
        
        #region Sorguyu Execute Etmek için ne Yapmamız Gerekmektedir.
           
            #region ToListAsync

                #region Method Syntax
                    // var urunler = await context.Urunler.ToListAsync();
                    
                #endregion

                #region Query Syntax
                    //  var urunler2 = await (from urun in context.Urunler select urun).ToListAsync();
                #endregion
            
            #endregion
           
               // sorguyu exxecute etmek için ToList de kullanabiliriz veya foreach kullanarak da getirebiliriz.
               //bunun için bir query sorgusu yazarız ve bunu döngüde execute edebiliriz.
            #region Foreach

                // var urunler =from urun in context.Urunler select urun;
                // foreach (Urun urun in urunler)
                // {
                //     System.Console.WriteLine(urun.UrunAdi);
                // }
            #endregion

            #region Deferred Execution(Ertelenmiş Yapılanma)
            
                   // IQueryable çalışmalarında ilgili kod yazıldığı noktada tetiklenmez/çalıştırılmaz yani ilgili kod yazıldığı noktada yazıldıüğı noktada sorguyu generate etmez. Nerede eder?? çalıştırıldığı ;/exevute edildiği noktada tetiklenir! İşste bu duruma ertelenmiş çalışma denir

                    #region UrunId5 den büyük olanalrı getir
                        
                        // int urunId=5;
                        // var urunler = from urun in context.Urunler where urun.Id>urunId select urun; // şimdi burada  Query sorgusu yazdık ve urun.Id si urunId yani 5 den büük olanları getir dedik
                        // foreach (Urun  urun in urunler)//execute ettik ve urun.Id si urunId(5) den büyük olanları getirdi
                        // {
                        //     System.Console.WriteLine(urun.UrunAdi); 
                        // }    


                    #endregion

                    #region Urun Id yi sonradan değiştirme
                        // int urunId=5;
                        // var urunler = from urun in context.Urunler where urun.Id>urunId select urun; // şimdi burada  Query sorgusu yazdık ve urun.Id si urunId yani 5 den büük olanları getir dedik
                        // //peki biz foreach da execute etmeden önce urunId değerini değiştirisek ne olur yani; 
                        // urunId=200; // execute etmeden önce urunId değerini 200 yaptık 
                        // // normalde yukarıdaki query sorgusu çaılıştığı zaman urunId 5 olması lazım ve execute ettiğimzdede urunId si 5 den büyük olanları getirmesi gerektiğini düşünürüz amaburada öyle olmamış olacak ve urunId si 200 den büyük olanları getirecek   
                        // foreach (Urun  urun in urunler)//execute ettik ve urun.Id si urunId(200) den büyük olanları getirdi
                        // {
                        //     System.Console.WriteLine(urun.UrunAdi); 
                        // }    

                        //bunun mantığı;;;;
                        //urunId ye 5 değerini verdik
                        //sonra query sorgusu yazdık ama çalıştırmadık yani sorgunun içinde urunId nin değerini daha bilmiyor
                        //sonra biz urunId ye 200 değerini verdik
                        //ve foreach da execute ettiğimiz zaman sorgu yeni çalıştığı için urunId nin son değerini alarak sorguyu oluşturuyor ve buna ertelenmiş çalışma deniyor.
                    #endregion
                
                  
                
            #endregion
       
        #endregion
       
        #region IQueryable ve IEnumerable Nedir Basit Olarak

            var urunler = from urun in context.Urunler select urun; // IQueryable yani sorgulama aşaması yani elimizde şuan veri yok sorguyu execute edersek veritabanına gidip verileri getirirp tutması IEnumerable olur.
             var urunler2 =await (from urun in context.Urunler select urun).ToListAsync(); // buda IEnumerable oluyor sorgu çalışıp veritabanından verileri getirdi.

          
            #region IQueryable
                //sorguya karşılık gelir
                //EF Core üzerinden yağpılmış olan sorgunun execute edilmemiş halini ifade eder.

            #endregion
           
            #region IEnumerable
                //Sorgunun çalıştırılıp/execute edilip verilerin in memorye yüklenmiş halini ifade eder.
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
