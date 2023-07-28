
using Microsoft.EntityFrameworkCore;
namespace IliskiselKavramlar;
class Program
{
    static void Main(string[] args)
    {
        #region RelationShips(İlişkiler)Terimleri
            // 2 tabomuz var bunlardan biri Çalışanlar diğeride Departmanlar tablomuz
            //departmansız çalışan olmaz çalışan departman tablosuna bağımlı yani.
            /*
                    çalışanalr tablosu
                    -Id 
                    -calisanAdi
                    -DepartmanId (foreign key)
                    public Departman Departman (buda navgation property) yani bizim bir tane departmanımız olabilirken bu departmnaında birden fazla çalışanı olabilir anlamnında bire çok bir ilişki var

                    Departmanlar tablosu
                    -Id (principal Key)
                    -departmanAdi
                    public ICollection<Calisan>Calisanlar (buda navgation property)
                    

            */   
            #region Principal Entity(Asıl Entity)
                //Knedi başına var olabilen tebloyu modelleyen entitye denir.

                //Departmanlar tablosunu modelleyen 'Departman' entity sidir.
            #endregion

             #region Dependent Entity(Bağımlı Entity)
                //Kendi başına var olamayan bir başka tabloya bağımlı(ilişkisel olarak bağımlı) olarak modelleyen entitye denir.

                //çalışanlar tablosunu modelleyen 'Çalışan' entitysidir.
             #endregion   

              #region Foreign Key
                //Principal Enttiy ile Dependent Entity arasındaki ilişkiyi sağlayan key dir.
                //Dependent entity de tanımlanır.
                //Pricncipal enttiy deki principal keyi tutar.
             #endregion   
              #region Principal Key
                //Principal entity deki Id nin kendisidir.
                //principal entity nin kimmliği olan kolonu ifade eden propertydir.
             #endregion   
              #region Navigation Property Nedir.
                //İlişkisel tablolar araındaki fiziksel erişimi entity class ları üzerinden sağlayan propertylerdir.
                //Bir property nin navigation propertsi olabilmesi için kesinlikle entity türünden olması gerekiyor.
                //Navigation propertyler entitylerdeki tanımlamalarına göre n'e n  veya 1'e n şeklinde ilişkili türleerini ifade etmektedirler.
             #endregion  
             
             #region İlişki türleri

                #region One To One (bire bir) 1-1
                    //Çalışan ile adresi aarsındaki ilişki,
                    //karı koca arasındaki ilşki
                #endregion

                #region One to Many (bire çok) 1-n
                    //Bizim çalışan ve depamrtndaki örneğimiz 1-n örneğidir.
                    //anne ve çocukları aarsındaki ilişki 
                #endregion

                #region Many to Many (çoka çok) n-n
                    //çalışanlar ile projeler arasındaki ilşşki
                    //kardeşler arasındaki ilşki
                #endregion



             #endregion
              
              #region Entity Framework Core da ilişki yapılandırma Yöntemleri

                #region Default Conventions
                    //Vrassayılan entity kurallarını kullanarak yapılan ilişki yapılandırma yönetmeleridir.
                    //navigation propertyleri kullanarak ilişki şablonlarını çıkarmaktadır.
                #endregion

                #region Data Annotations Attributes
                    //entity nin niteliklerine gör eince ayarlar yapmamızı sağlayan attributes lardır.  [Key], [Foreign Key]
                #endregion

                #region Fluent API
                //entity modellerinde ilişkileri yapılandırırken daha detaylı çalşmamızı sağlayan yönetemdir.
                    #region HasOne
                        //ilgili entity nin ilişkisel entity "bire bir" veya "bire çok" olacak şekilde ilişkisini yapılandırmaya başlayan metotdur.
                    #endregion
                    #region HasMany
                        //ilgili entity nin ilişkisel entitye "çoka bir" veya "çoka çok" olacak şekilde ilişkisini yapılandırmaya başlayan metotdur.
                    #endregion
                    #region WithOne
                        //hasone veya hasmany den sonra "bire bir" ya da "çoka bir" olacak şekilde ilişki yapılandırmasını tamamlayan metotdur.
                    #endregion
                    #region WithMany
                        //HasOne veya HasHany den sonra "bire çok" ya da "çoka çok" olacak şekilde ilişki yapılandırmasını tamamlayan metotdur.
                    #endregion

                #endregion
              
              
              #endregion  


        #endregion
    }
    class Calisan
            {
                public int Id { get; set; }
                public string CalisanAdi { get; set; }
                public int DepartmanId { get; set; } //(foreign key)
                public Departman Departman { get; set; } // (buda navgation property) yani bizim bir tane departmanımız olabilirken bu departmnaında birden fazla çalışanı olabilir anlamnında bire çok bir ilişki var

            }

            class Departman
            {
                public int Id { get; set; } // (principal Key)
                public string DepartmanAdi { get; set; }
                public ICollection<Calisan> Calisanlar { get; set; } // (buda navgation property)
                // public Calisan Calisan { get; set; }// eğer yukarıdaki satır olmasaydı ve böyle kullansaydık bu one to one 1-1 ilişki olacaktı
            }

             
}


