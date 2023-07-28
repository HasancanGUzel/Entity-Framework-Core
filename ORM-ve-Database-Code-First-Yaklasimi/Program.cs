namespace ORM_ve_Database_Code_First_Yaklasimi;
class Program
{
    static void Main(string[] args)
    {
        #region ORM Nedir
		
		//*Yazılım uygulamarında veriler fiziksel olarak veritabanlarında tutulmaktadır.
		//*şimdi örnek olarak adonet galiba onun veritabanı var
		
		#region Kod içerisinde SQL yazmanın dezavantajları (avantajları yoktur)
			
			// using System.Data.SqlClient
			// await using SqlConnection connection = new ("Server=localhost,1433;Database=Northwind;User Id=sa;Password=123");
			// await connection.OpenAsync();
			
			// SqlCommand command = new("Select * from Employees",connection);
			// SqlDataReader dr= await command.ExecuteReaderAsync();
			// while(await dr.ReadAsync())
			// {
			// 	Console.WriteLine($"{dr["FirstName"]} {dr["LastName"]}");
			// }
			// await connection.CloseAsync();
			
			//*Görüldüğü üzere kod içeriisnde Sql sorguları yazılmış
			//*Kodun içerisinde SQL ile cümlelerin olması kodun kirlenemsine sebep olur ve bunu kullanacak olan başka bir kişinin SQL hakkında bilgisi olması gerekmektedir.
			//*yani SQL ile yazılmış kodu oracle kullanan birine verdiniz ondan değişiklik yapmasını istediniz ama hem SQL bilmiyor hemde Select * from Employees yazmışıs kullanacak kişi bunun içinde sütunları bilmiyor
			
			
			
			//!KOD İÇERİİSNDE SQL YAZMANIN DEZAVANTAJLARI NELERDİR!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			
			//*Kodun kirlenmesini sağlar
			//*Geliştirme ve bbakım maliyeti yüksek kod inşasına sebep olur
			//*Veritabanı bağımlılığı yaratır.
			//*Kompleks sorgular manuel bir şekilde oluşturulması gerekir.
			//*Geliştirici açısından SQL sorumluluğu beklenir.
			//*Veritabanı sorgulama neticesinde gelen datalar manuel olarak dönüştürülür.Sorgu sürecinden tablo,kolon vs. gibi bağımlılıklar olduğu gibi gelen dalatalarda da aynı bağımlıkılar szö konusu olcakatır.
			//*Veritabanında olan değişiklilere uygun bir şekilde kodunda tekrar review edilmesi gerekir. Misal bir kolon adı değiştiğinde ya da bir kolonun hernangi bir kuralı (constrain,vlidation vs.)değiştiğinde bu durumdan kodun haberda olması şçin bilinçli bir review gerekmketedir.
			//*kodu aşırı derecede veritabanı seviyesine indiger.Bu durum tüm gelişmelerin veritabanıyla uyumlu bir şekilde seyretmesi zorundalığı doğuru.
			//*Geliştirlen yazılımın sürecinde tüm veritabanı işlemleri o anli kullaınlan programlama dilşi ve OOP nin nimetlerinen istifade etmeksizin icra edilir.
			//*Gün gelir veritabanını değiştirme durumunda SQL kodlarını yeni veritabanına göre Refactoring etmeniz gerekecektir.
			
			
		
		#endregion
		
		//*yukarıdaki değelendirmelri yaptık bu problemden nasıl kurtulabilriiz (ORM ile) ORM NEDİR!!!!!!!!
		//*yAZILIM VE VERİTABANI ARASINDAKİ BAĞLANTI ÜZERİNDEN SORGULAR EŞLİĞİNDE VERİ TRANSFERİ OOP  nimetlerinden istifade ederek sağlayabileceği ve böylece kodun da geliştiricininde Sql e bağlılığı olmaskızın hızlı ve kolayca operasyonları gerçekleştirebilceği bir yaklalşım ortaya konmuştur.
		//*Bu yaklaşımın adı ORM dir
		//*ORM= Object Relational Mapping yani(Nesne İlişkisel Eşleme)
		//*Geliştirlen yazılım içerisibnde OOP yapısına uygun olmayan ,katı ve komplkes veritabanı sorguları yerine veritabanı objelerinin bir OOP nesnesi gibi düşünülerek yazlım tarafından kullanılabilmesi olarak sağlayan bir yaklaşımdır. 
		//*Bu yaklaşıma göre veritaban,tablolar ve veriler yazılım tarafında birer nesneye karşılık gelmektedirler.Böylece tüm veritabanı süreçlerini OOP kavramlarıyla rahatlıkla yönetebilir ve kodu SQL den arındıorabiliriz.
		
		//!ORM AVANTAJLARI !!!!!!!!!!!!!!!!!!
		//*Veritabanı bağımsızlığı sağlar.
		//*Kullanılan veritabanına göre uygun sorgu oluşturu.
		//*OOP nimetlerinden faydalanarak SQL mantığı işlenmesini sağlar.
		//*Geliştiricinin kullanılan veritabanına dair SQL yeteneklerinin olması beklenmez
		//*Sorgular otomatik generate edilceğinden dolayı kodu SQL bağımlılığından soyutluyor.
		
		
		
		
		
		
	#endregion
	
	    #region EF Core (Entity Framework Core) Nedir
	
		#region Açıklamalar 
			
			//* EF Core , ORM yaklaşımını benimsemiş bir araçtır.
			//*Amacı kod içerisinde OOP nimetlerinden istifad ederek SQL sorguları oluşturmamızı sağlanmaktır.
			//*Kod içeriisnde ihtiyaca bianen geliştirlmiş olan tekrarlı SQL sorgucuklarından bizleri kurtarmaktadır.
			//*Code First ve Database First yaklaşımları eşliğinde veritabanı ile yazılım aratsındakş koordinasyonu sağlamaktadır. 
			
			/*
				Kod üzerinden;
					. Veritabanı ve tablo,
					. Constraint,
					. Sequence,
					. İlişkili sorgular,
					. View,
					. Stored Procedure,
					. Function
					. Temporal Table
				gibi veritabanı nesneleri oluşturmamızı ve kullanmamızı sağlamaktadır.
				
				Query için LINQ sorguları desteklenmektedir.	
			*/
			
			//*ORM olarak neden EF Core seçilmelidir.
			//*Ef Core her ne kadar hızlı ve performanslı bir yapıya sahip olsa da piyasadaki en hızlı ORM aracıdır diyemeyiz.
			//*Misal olarak; minimal özelliklere sahip olan Dapper,Raw(ham) sorgular kullanıldığından dolayı kelimenin tam anlamıyla EF Core dan çok daha hızlıdır.
			//*lakin her bir güncellemesinde performansının arttığı gözlemlenen EF Core un ise bir çok özelliği mevcuttur.
			
			/*
				*EF Core migration yapılanmalarını vs ilerde göreceğiz bunun için belirli araçlar yüklememmiz gerekmektedir.
				
			   YÜKLENECEK ARAÇLAR
				 
				 1) .NET Core command-line interface(CLI)tools
					   dotnet-ef ile başlayan CLI komutlarını ilgili PC de aktif olarak kullanabilmemizi sağlayan tooldur.
					   
					BU KOMUTU KURABİLMEK İÇİN consola veya windows poewershelle yaz  ;  poewershelle yazdığımız zaman genel olarak kuruyor sadece proje içine değil
						dotnet tool install --global dotnet-ef	
						
					EĞER BU TOOL KURULU İSE BUNU GÜNCELLEMEK İÇİN;	consola veya windows poewershelle yaz
						dotnet tool update --global dotnet-ef	
						
					Bu durumu CHECK etmek istiyorsak 	consola veya windows poewershelle yaz
						dotnet ef  yazmak yeterli
						
					Bu toolu yükledikten sonra belirli bir projede kullanbilmek için ilgili projede şu paketin de yüklü olması gerekmketedir.
						Microsoft.EntityFrameworkCore.Design	
							
						
				 2) Package Manager Console(PMC)tools
					Visual studio da Package Manager Console üzerinden talimatlar vermemizi sağlayan bir tooldur.
					Yüklenecek paket;
						Microsoft.EntityFrameworkCore.Tools
						 
			
			*/
			

		#endregion
		
		#region Database First ve Code First Yaklaşımları
			 //* Bu yaklaşımlar şöyşe olabilir
			 //*Mesela hazır olan bir projeye geldin ve bunun veritabanı hazır bunun üzerinde EF Core kullanmak var
			 //* Bir de projeye sıfırdan başlıyorsun ve bunda EF Core kullanmak var.
			 
			 //*Yani uzun zamandır devam eden  ve veritabanı doğal olarak mevcut olan bir projeye katıldığımızı varsaylım.
			 //*Ve bu veritabanı üzerinden EF Core ORM aracı ile işlemler yapmamızı bekliyorlar 
			 //*Böyle bir durumda projede veritabanının var olmasından dolayı büyük ihtimalle DATABSE FİRST yaklaşımı tercih etmemiz gerekecektir
			 
			 
			 //*Lakin veritabanı daha inşa edilmemiş bir projeye katılım gösteriyor olsaydınız bu durumda da ya DATABASE FİRST ya da CODE FİRST yaklaşımından birini tercih etmemiz gerekecektir.
			 
			 #region DATABASE FİRST
				//*Var olan veritabanını tersine mühendislikl ile analiz edip otomatik olarak kod kısmında modelleyen bir yaklaşımdır.
				//*yani şöyle ki hedef veritabanının belirli talimatlar aracılığıyla otomatik olarak kod kısmında OOP nimetleri eşliğinde modellnmesidir.
				 
				 /*
					!AVANTAJLARI:
						*Hazır veritabanlarını hızlı bir şekilde modelleyebilmemizi sağlar.
						*Veritabanında süreçte olan değişiklikleri de hızlıca koda aktarmamızı sağlar
						*SQL Server,Oracle,PostgreSQL vs. gibi EF Core tarafından desteklenen tüm veritabanlarında kullanılabilir.
						*Veritabanından bağımsız olarak tüm modellemeyi OOP nimetlerri karşılığında sağlamaktadır.
						
						
					!DEZAVANTAJLARI:
						*Kod veritabanına göre şekkilleneceği/modelleneceği için yönetim veritabanı tarafından sağlanır. Haliyle veritabanı bilgisi gerektiri.
						*Değişiklikler veritabanı kısmında olacağı için geliştirici tarafından sürekli bir kontrol/güncellme davranışı sergilenmelidir.
					
					
				 */
				 
				 /*
					!Database First yaklaşımı hangi durumlarda tercih edilir:
						
						-Önceden oluşturulmuş hali hazırda veritabanı var olan uygulamalrda
						-Uzun süreli devam eden uygulamalrda(özellikle devlet gibi köklü kurumsal projelein veritabanlarını modellerken),
						-Veritabanı yönetimine, geliştirme süreçlerine ve tasarımına dair herhangi bir kararın geliştiriciler tarafından verilemdiği durumlarda tercih edilmelidir. Yani veritabanı kısmını başka bir bölüm yapıyorsa sen karışmıyorsan gibi düşün

				 */
				 
				 
			 
			 
			 #endregion
		
			#region CODE FİRST
				//*EF Core ile çalışma yapılacak olan veritabanı önceden oluşturulmamış ise bu veritabanıı kod kısmında modelleyerek ardından bu model uygun veritabanını sunucuda oluştuıran(migration) yaklaşımıdır.
				
				//*bu yaklaşımda veritabanı önce kodlar tasarlanır , sonra veritabanı sunucuna gönderilerek veritabanı oluşturulur (bizim shoppapi geliştiriken yaptığımız gibi düşün)
				
				//*Database First yaklaşımının tam tersi davranışı gerektirir.
				
				
				
				/*
					!AVANTAJLARI::
						*-Kod üzerinden veritabanını modellememizi sağlar.
						*-Veritabanına dokunmaksızın kod üzerinden gerekli düzenlemeleri ve güncellemeleri hızlıca yapabilmemizi sağlar.
						*-SQL Server,Oracle , PostgreSQL vs gibi EF Core un desteklediği tüm veritabanlarında kulalnılabilir.
						*-Veritabanından bağımsız olardak tüm modellemeyi OOP nimetleri karşılğında sağlamaktadır.
						*-Koddaki ihtiyaca dönük veritabanı şekilleneceği / modellebneceği ve nihai oalrak bu modele göre inşa için yönetim geiştirici tarafından sağlanmaktadır.Haliyle herhangi bir veritabanı bilgisine gerek duymamaktadır.
						*-Değişiklikler kod kımsında yapılacağı için geliştirici tarafından sürekli bir kontrol/ güncelleme davranışı sergilenmeyecektir.
						*-Veritabanı modeli kod üzerinde yapıldığı için istenilen sunucuda anında ilgili modeldeki veritabanı elde edilebilir.
						
				    
                    !DEZAVANTAJLARI::
						*Üretilecek veritabanının tasarımı ve stratejisi geliştirici sorumluluğundadır.
				
				*/
				
				/*
					!Code First yaklaşımı hangi durumlarda tercih edilir:
						
						*-Veritabanı bilgisine ihtiyaç duyulmayan,
						*-Veritabanı tasarımının kod üzerinde yapılarak , geliştirici tarafından sorumluluğunun üstlenileceği,
						*-Veritabanı yönetiminin kod üzerinden sağlanacağı durumlarda tercih edilebilir.

				 */


			#endregion		
			 
			 //*Bu iki yaklaşımın kullanım alanlarının radikal olarak ayrdılığı tek nokta ilgili veritabanının önceden oluşturulmuş olup olmamasıdır. Eğer önceden oluştşrulmuş ise Database First yaklaşımı elzemdir.
			 
			 //*Yok eğer önceden oluşturulmamış ise Code First yaklaşımını tercih edebileceğiniz gibi önce sunucuda veritabanını manuel tasarlayıp sonrasında da Database First yaklaşımıyla kod inşa edebiliriz.
			 
			 
			 
			
			
		#endregion
	
		#region Yapısal Olarak EF Core Aktörleri
		
			#region DbContext
				//* EF Core da veritabanını temsizl edecek olan sınıf DbContext olarak nitelenediirlmektedir.
				//* Veritabanında karşılık gelecek olan veritabanı adını yazıyoruz ve bunu EntityFrameworkCore miras verdiyiroyuz. aşşağıdaki gibi;
				
				 /*NorthwindContext:DbContext	
				 {
					 içinde de tablo adlarımız olucak
					 yani entity ler buraya DbSet olarak eklenmelidir.
				 }		

				*/	

				/*
					!DbContext nesnesinin sorumlu olduğu faliyetler
					
						-Konfigürasyon:
							Veritabanı bağlantısı,model yapılanmaları ve veritabanı nesnesi ile tablo nesneleri arasındaki ilişkileri sağlar.
						
						-Sorgulama:
							Sorgulama operasyonlarını yürütür.Kod tarafında gerçekleştirilen sorgulama adımlarını SQL sorgusuna dönüştürü ve veritabanına gönderiri.
							
						-Change Tracking:
							Sorgulama neticesinde elde edilen veriler üzerindeki değişiklikleri takip eder.
							
						-Veri kalıcılığı
							Verilerin kayıt edilmesi, güncellenmesi ve silinmesi operasyonlarını gerçekleştirir.
														
				*/		
			
			#endregion
			
			#region ENtity
				
				//*Tablo nesnesi -Entity
				//*EF Core	da tabloları temsil edecek sınıflar Entity olarak nitelendirilmektedir.
				//*Yer yüzündeki herhangi bir olguyu/nesneyi/objeyi modelleyen sınıfa Entity(varlık)denmektedir.
				//*EF Core açısından baktığımızda entity, bir veritabanı tablosunu modelleyen sınıftır.
				//*Veritabanı tablo adı çoğul olur lakin o tabloyu modelleyen entity sınıfının adı tekil olur.
				
				
				

			#endregion	

				
			 
			 
       #endregion
	   
	    #region DATABASE FİRST YAKLAŞIMINI PRATİKTE İNCELEYELİM
			//*Tersine mühendislik, bir sunucudaki veritabanının iskelesini kod kısmında oluşturma sürecidir.
			//*Bu süreci PACKAGE MANAGER CONSOLE(PMC) ya da DOTNET CLI aracılığyla yapabiliriz.
			
			#region Package Manager Console(PMC) ile tersine mühendislik
				//* Scaffold-DbContext 'Connection String' Microsoft.EntityFrameworkCore.[Provider]  //* bu kodu yazdığımız zaman veritabanındaki tüm veritabanı tabloları gelecek
				//*'Connection String' yukarıda bu kısma hangi veritabanında çalışacaksak ona uygun yazmamız lazım bunun içinde internete Connection String yaazdığımız zaman kullanmak istediğimiz veritabanını seçerek oradan Connection String i kopyalayabiliriz. Ben herzamanki gibi SQL kullandığım için bildiğim bağlantı cümleciği var.
				
				//*Microsoft.EntityFrameworkCore.[Provider] provider kısmı ise SqlServer vs.
				
				
				//*PMC ile veritabanını modelleyebilmek için aşşağıdaki kütüphaneleri projeye yüklememiz gerekmektedir.
				//*Microsoft.EntityFrameworkCore.Tools
				//*Database Provider(Örn; Microsoft.EntityFrameworkCore.SqlServer)
				
				
				//* Scaffold-DbContext 'Connection String' Microsoft.EntityFrameworkCore.[Provider] -Tables Table1, Table2 // böyle yaparak da çalışmak istediğimz tabloları getirebiliriz.
				
				
				//*DbContext adını belirtmek
				//*Scaffold ile moellenen veritabanı için oluşturulacak context nesnesi adını veritabanından alacaktır.Buradan context adını değiştirebiliriz.
				//* Scaffold-DbContext 'Connection String' Microsoft.EntityFrameworkCore.[Provider] -Context Contexteverilecekisim
				
			
			
			#endregion
			
			#region DOTNET CLI ile tersine mühendislik (bu benim her zaman kullanıdğım)
				// dotnet ef dbcontext scaffold 'Connection String' Microsoft.EntityFrameworkCore.[Provider]  //* bu kodu yazdığımız zaman veritabanındaki tüm veritabanı tabloları gelecek
				
				//*DOTNET CLI ile veritabanını modelleyebilmek için aşşağıdaki kütüphaneleri projeye yüklememiz gerekmektedir.
				//*Microsoft.EntityFrameworkCore.Design
				//*Database Provider(örn; Microsoft.EntityFrameworkCore.SqlServer)
				
				//*Microsoft.EntityFrameworkCore.[Provider] provider kısmı ise SqlServer vs.
				
				
				//* dotnet ef dbcontext scaffold 'Connection String' Microsoft.EntityFrameworkCore.[Provider] --table Table1 --table Table2 // böyle yaparak da çalışmak istediğimz tabloları getirebiliriz.
				
				
				//*DbContext adını belirtmek
				//*Scaffold ile moellenen veritabanı için oluşturulacak context nesnesi adını veritabanından alacaktır.Buradan context adını değiştirebiliriz.
			    //* dotnet ef dbcontext scaffold 'Connection String' Microsoft.EntityFrameworkCore.[Provider] --context Contexteverilecekisim
				
			
			#region
		
		#endregion
	   
	#endregion
	
			
			#region PATH ve NAMESPACE Belirtme
			
			//!PATH:::
				//Entity ler ve DbContext sınıfı, default olarak direkt projenin kök dizinine modellenir ve projenin varsayılan namesapacini kullanırlar.Eğer ki bunlara müdahale etmek istiyorsanız aşşağıdaki gibi talimat verebilirisniz yani bunları context i contex klasörününe entity leri entity klasörüne almak istiyorsak;
				
				
				//Package Manager Console(PMC) için
				// Scaffold-DbContext 'Connection String' Microsoft.EntityFrameworkCore.[Provider] -ContextDir Data -OutputDir Contexts //context classı Contexts kalasörü içinde olsun dedik
				
				//DOTNET CLI için
				// dotnet ef dbcontext scaffold 'Connection String' Microsoft.EntityFrameworkCore.[Provider] --context-dir Data --output-dir Entitys //Entity lerimiz de Entitys klasörü içinde olsun dedik
				
			//!NAMESPACE:::
			
				//Package Manager Console(PMC) için
				// Scaffold-DbContext 'Connection String' Microsoft.EntityFrameworkCore.[Provider] -Namespace YourNamespace -ContextNamespace YourNameSpace //BİRİNCİ NAMESPACE ENTİTY LERİN NAMESPACEİ 2.CİNAME SPCAE DE Context imizin namepsaci
				
				//DOTNET CLI için
				// dotnet ef dbcontext scaffold 'Connection String' Microsoft.EntityFrameworkCore.[Provider] --namespace YourNameSpace --context-namespace YourNameSpace //BİRİNCİ NAMESPACE ENTİTY LERİN NAMESPACEİ 2.CİNAME SPCAE DE Context imizin namepsaci
				
				
				
				
			
			
			#endregion
	
	        #region Model Güncelleme (FORCE) Kavramı
				//*bzi scalffoldlar veritabanını kod kısmına eklerken sıfırdan projede olmadan ekliyoruz
				//*eğer projeye önceden o veritabanını scafoldla kurmuş olsaydık tekrar scaffold yaptığımız zaman hata verirdi. 
				//*yani veritabanında güncelleme oldu bunu projeye aktarmak istiyoruz tekrar scaffold yapmamız gerek ama izin vermez 
				//*bunun için  ya projedeki classları klasörleri slip tekrar scaffold yapıcaz   ya da;
				//*FORCE kavramını kullanırız force kavramı ezerek scaffold kavramını tekrar kullanmamızı sağlar
				
				
				//Package Manager Console(PMC) için
				// Scaffold-DbContext 'Connection String' Microsoft.EntityFrameworkCore.[Provider] -Force 
				
				//DOTNET CLI için
				// dotnet ef dbcontext scaffold 'Connection String' Microsoft.EntityFrameworkCore.[Provider] --force
			
			#endregion
			
			
			#region Modellerin  Özelleştirilmesi
				//*Database first yaklaşımında veritabanı nesneleri otomatik olarak oluşmaktadır.Bazen biz bu otomatize olan süreçte ekstra olarak manuel olarak entity ler veya context nesnesinde özelleştirme yapmak isteriz.
				//*Ama bu durumda biz veya başkası veritabanında yapılan değişiklikler neticesinde Force komutunu kullanarak tüm değişiklikleri kod kısmına yansıtabilir bu durumda bizim yaptığımız değişikliklerin ezilme riskin söz konusu olur.
				//*Bu tar özellştirme durumlarında bizzat model sınıflarını kullanmaktansa bunların partial class ları üzerinde çalışmak en doğrusudur.
				
			
			
			#endregion

	    #endregion	

		#region CODE FİRST YAKLAŞIMINI
			  //migration ve migrate kavramlarını kullanıcaz
			  //migration kod da yazdığımız veritabanı sorgularını veritabanı diline çevirir.
			  //migrate ise göç anlamına gelir bu migration olmuş verileri database veritabanına göç ettirir gönderir.
			  
			  //Migration oluşturmak için temelde EF Core aktörleri olan DbContext ve Entity classlarını oluşturmak gerekir.Bunları oluşturduktan sonra migration  Package Manager Console(PMC) ce Dotnet CLI olmak üzere 2 türlü talimatla verilebilir.
			  
			//!----------------- migration oluşturma  -------------------------------
			
			  // PACKAGE MANAGER CONSOLE DA MİGRATİON OLUŞTURMA
			  // add-migration [migration name]
			  
			  //DOTNET CLI MİGRATİON OLUŞTURMA
			  //dotnet ef migrations add [migration name]
			  
			  
			  
			//!----------------- migration ları migrate (UP)etme  -------------------------------
			
			  // PACKAGE MANAGER CONSOLE DA MİGRATİON OLUŞTURMA
			  // update-database
			  
			  //DOTNET CLI MİGRATİON OLUŞTURMA
			  //dotnet ef database update 
			  
			//!------- --------migration ları migrate (DOWN)etme eski migratina gitme  geri dönmek istediğimiz migrationun ismini veriyoruz.--------------------
			
			  // PACKAGE MANAGER CONSOLE DA MİGRATİON OLUŞTURMA
			  // update-database [migration name]
			  
			  //DOTNET CLI MİGRATİON OLUŞTURMA
			  //dotnet ef migrations update [migration name]  
			  
			  
          //!----------------- migrationun path yani klasör ismini belirlememizi  -------------------------------
			  
			  // PACKAGE MANAGER CONSOLE DA MİGRATİON PATH BELİRLEME
			  // add-migration [migration name] -OutputDir [path]
			  
			  //DOTNET CLI MİGRATİON PATH BELİRLEME
			  //dotnet ef migrations add [migration name] --output-dir [path]
			  
          //!----------------- migrationun silme  -------------------------------
		  
			   // PACKAGE MANAGER CONSOLE DA MİGRATİON SİLME
			  // remove-migration
			  
			  //DOTNET CLI MİGRATİON SİLME
			  //dotnet ef migrations remove
			  
			  
			  
		//!----------------- migrationun listeleme  -------------------------------
		  
			   // PACKAGE MANAGER CONSOLE DA MİGRATİON LİSTELEME
			  // get-migration
			  
			  //DOTNET CLI MİGRATİON LİSTELEME
			  //dotnet ef migrations list
			  
			  
			  
			  
			//!------KOD ÜZERİNDEN DE MİGRATE YAPABİLİYORUZ (RUNTİME DA) VERİTABANINI MİGRATE EDEBİLİRİZ(var olan migration ları runtime da gönderiyoruz.)

				// AppDbContext context= new();
				// await context.Database.MigrateAsync();
				
				//context den bir nesne(instance) üretiyoruz. Database property si ile MigrateAsync aracılığyla veritabanına gönderiyoruz.
			  
			  
			  
			  
			  
			  
			  
			  
		
		#endregion

		#region Entity Tanımlama Kuralları
			//*EF CORE, her tablonun default olarak bir primary key kolonu olması gerektiğini kabul eder.
			//*Haliyle, bu kolonu temsil eden bir proprty tanımlamadığımız taktirde hata verecektir.
			
		#endregion

#endregion


    }
}
