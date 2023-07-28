
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace Generated_Values_Yapilandirmasi;
class Program
{
      static ApplicationDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
            #region Generated Value Nedir?
            //EF Core'da üretilen değerlerle ilgili çeşitli modellerin ayrıntılarını yapılandırmamızı sağlayan bir konfigürasyondur.
            #endregion

            #region Default Values

            //EF Core herhangi bir tablonun herhangi bir kolonuna yazılım tarafından bir değer gönderilmediği taktirde bu kolona hangi değerin(default value) üretilip yazdırılacağını belirleyen yapılanmalardır.

                    #region HasDefaultValue
                    //Static veri veriyor.
                    #endregion

                    #region HasDefaultValueSql
            //SQL cümleciği
            #endregion

            #endregion

            #region Computed Columns

                #region HasComputedColumnSql
                //Tablo içerisindeki kolonlar üzerinde yapılan aritmatik işlemler neticesinde üretilen kolondur.
                #endregion

            #endregion

            #region Value Generation

                #region Primary Keys
                //Herhangi bir tablodaki satırları kimlik vari şekilde tanımlayan, tekil(unique) olan sütun veya sütunlardır.
                #endregion

                #region Identity
                //Identity, yalnızca otomatik olarak artan bir sütundur. Bir sütun, PK olmaksızın identity olarak tanımlanabilir. Bir tablo içerisinde identity kolonu sadece tek bir tane olarak tanımlanabilir.
                #endregion

                //Bu iki özellik genellikle birlikte kullanılmaktadırlar. O yüzden EF Core PK olan bir kolonu otomatik olarak Identity olacak şekilde yapılandırmaktadır. Ancak böyle olması için bir gereklilik yoktur!

                #region DatabaseGenerated

                    #region DatabaseGeneratedOption.None - ValueGeneratedNever
                    //Bir kolonda değer üretilmeyecekse eğer None ile işaretliyoruz.
                    //EF Core'un default olarak PK kolonlarda getirdiği Identity özelliğini kaldırmak istiyorsak eğer None'ı kullanabiliriz.
                    #endregion

                    #region DatabaseGeneratedOption.Identity - ValueGeneratedOnAdd
                    //Herhangi bir kolona Identity özelliğini vermemizi sağlayan bir konfigürasyondur.

                        #region Sayısal Türlerde
                        //Eğer ki Identity özelliği bir tabloda sayısal olan bir kolonda kullanılacaksa o durumda ilgili tablodaki pk olan kolondan özellikle/iradlei bir şekilde identity özelliğinin kaldırılması gerekmektedir.(None)
                        #endregion

                        #region Sayısal Olmayan Türlerde
                        //Eğer ki Identity özelliği bir tabloda sayısal olmaan bir kolonda kullaınacaksa o durumda ilgili talbodaki pk olan kolondan iradeli bir şekilde identity özelliğinin kaldırılmasına gerek yoktur. 
                        #endregion

                    #endregion

                    #region DatabaseGeneratedOption.Computed - ValueGeneratedOnAddOrUpdate
                    //EF Core üzerinde bir kolon Computed column ise ister Computed olarak belirleyebilirsiniz isterseniz de belirmeden kullanmaya devam edebilirsiniz.
                    #endregion

                 #endregion

            #endregion
            
            Person p = new()
            {
                Name = "Gençay",
                Surname = "Yıldız",

                Premium = 10,
                TotalGain = 110
            };
            context.Persons.Add(p);
             context.SaveChanges();
    }
}



class Person
{
    //[DatabaseGenerated(DatabaseGeneratedOption.None)] //* personId kolonumuzu bir bir artmasın dedik çünküz biz aşşağıda personcode u(int olanı)  ıdentity yaptık  ama persondı nin artık kayıt yaparken ıd sini kendim belirlemem lazım ve tekrar eden veri olmaması lazım farklı değer girmem lazım yoksa hata verir. aşşağıda personcode kolonu int değilde farklı türdeyse bu satırı tanımlamay gerek yok otomatik olarak ıdentity kaldırıyor.
    public int PersonId { get; set; } 
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Premium { get; set; }
    public int Salary { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)] //*compudet kolon olduğunu bilditiyoruz
    public int TotalGain { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //*artık ıdentity sensin dedik ama bizim primary key kolonu yani PersonId kolonumuz da primary key olduğu için identity var bunun için PersonId kolonuna none özelliğini vermemiz gerek ama int yerine guid olan personcode da kullanmış olsaydık yukarıda persondı yi none yapmamıza gerek yok
    public Guid PersonCode { get; set; }
    //   public int PersonCode { get; set; }
}

class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .Property(p => p.Salary)
            //.HasDefaultValue(100);
            .HasDefaultValueSql("FLOOR(RAND() * 1000)");

        modelBuilder.Entity<Person>()
            .Property(p => p.TotalGain)
            .HasComputedColumnSql("([Salary] + [Premium]) * 10")
            .ValueGeneratedOnAddOrUpdate(); //* burada ekstradan compudet column olduğunu belirtiyoruz bi artısı yok


        

        modelBuilder.Entity<Person>()
            .Property(p => p.PersonId)
            .ValueGeneratedNever(); //* personId nin ıdnetitysini fluent Api da kaldırmak için kullanıyoruz

        modelBuilder.Entity<Person>()
            .Property(p => p.PersonCode)
            .HasDefaultValueSql("NEWID()"); //*bizim ıdentity kullandıpımız kolon sayısal tür değilse eğer (yukarıda guid) bu fluent api yapmazsak hata alırız. değer girilmedi diye ama bunu yaparsak yeni bir ıd atamış oluyor

        modelBuilder.Entity<Person>()
            .Property(p => p.PersonCode)
            .ValueGeneratedOnAdd(); //* person codun ıdentity olmasını istiyorsak fluent api da böyle tanımlayabiliriz.
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseSqlite("Data Source=ApplicationDb");
    }
}
