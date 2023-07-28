using Microsoft.EntityFrameworkCore;

namespace Iliskisel_Senaryolarda_VeriSilme_Cascade_Delete_Davranislari;
class Program
{

      static ApplicationDbContext context = new(); //* her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
             #region One to One İlişkisel Senaryolarda Veri Silme
             //* person tablosundaki ıd si 1 olan personun adresini silmek istiyoruz bunu ilişkisel senaryo olarak yapıyoruz bunu direk de silebiliridk
            // Person? person =  context.Persons
            //    .Include(p => p.Address)
            //    .FirstOrDefault(p => p.Id == 1);//*person tablosuna gidip ıd si 1 olan personu getir ama getirirken de include ile addresinide getir dedik

            // if (person != null) //* yukarıdaki sorgu sonucu person içinde ki değer boş değilse eğer personun adresini sil dedik
            //    context.Addresses.Remove(person.Address);
            //  context.SaveChanges();
            #endregion

            #region One to Many İlişkisel Senaryolarda Veri Silme
            //* ıd si 1 olan blog umuzun içindeki 2 ıd li postu silicez
            // Blog? blog =  context.Blogs
            //    .Include(b => b.Posts)
            //    .FirstOrDefault(b => b.Id == 1); //* ıd si 1 olan blog u ve onun ile ilişkili post ları getirdik
            // Post? post = blog.Posts.FirstOrDefault(p => p.Id == 2); //* yukarıdaki sorgu sonucu elimizde olan blog bilgisi sayesinde buradan blog ile ilişkili olan post lardan ıd si 2 olan postu getirdik ve post değişkenine attık.

            // context.Posts.Remove(post); //*  sonra da bu postu sildik
            //  context.SaveChanges();
            #endregion

            #region Many to Many İlişkisel Senaryolarda Veri Silme
            //* ıd si 1 olan kitabın 2 ıd li yazarını silicez ama sadece cros table dan silicez author dan da silersek yazarın diğer kitaplarıda olacak sıkıntı olur
            // Book? book =  context.Books
            //    .Include(b => b.Authors)
            //    .FirstOrDefault(b => b.Id == 1); //* ıd si 1 olan kitabı bu kitapla ilişkisi olan yazarlar geldi

            // Author? author = book.Authors.FirstOrDefault(a => a.Id == 2);//*yukarıdaki satır sonucun da book lardan yazarlara eriştik ve ıd si 2olan yazarı author değişkenine attık
             // context.Authors.Remove(author); //!Yazarı silmeye kalkar!!!
            // book.Authors.Remove(author);//* burada kitabın içinden yazarı siliyoruz yani cross table daki bulduğumuz id ye denk geleni sildi yukarıdaki gibi değil yukarıda direk yazar tablosundan ,elde ettiğimiz yazarı siliyorduk
            //  context.SaveChanges();
            #endregion

               //! Biz şuana kadar principal tableda ki verinin dependent table daki ilişkisel verileri arasından birini silmeye çalışırken neler olduğunu görmüştük 
               //! peki principal table daki bir veriyi silmeye çalışırsak ne olur? Cascade Delete ile inceliyoruz

            #region Cascade Delete
            //* Bu davranış modelleri Fluent API ile konfigüre edilebilmektedir.
            #region Cascade
            //*Esas tablodan silinen veriyle karşı/bağımlı tabloda bulunan ilişkili verilerin silinmesini sağlar.

            // Blog? blog = context.Blogs.Find(1);
            // context.Blogs.Remove(blog);
            // context.SaveChanges(); //* şimdi burada ıd si 1 olan blog u ve onun ilişkisi olan post larda silinmiş oldu
            #endregion

            #region SetNull
            //*Esas tablodan silinen veriyle karşı/bağımlı tabloda bulunan ilişkili verilere null değerin atanmasını sağlar.

            //!One to One senaryolarda eğer ki Foreign key ve Primary key kolonları aynı ise o zaman SetNull davranuışını KULLANAMAYIZ!

            // Blog? blog = context.Blogs.Find(1);
            // context.Blogs.Remove(blog);
            // context.SaveChanges();

            #endregion

            #region Restrict
            //*Esas tablodan herhangi bir veri silinmeye çalışıldığında o veriye karşılık dependent table'da ilişkisel veri/ler varsa eğer bu sefer bu silme işlemini engellemesini sağlar.

            //  Blog? blog =  context.Blogs.Find(2);
            // context.Blogs.Remove(blog);
            //  context.SaveChanges();
            #endregion

           
            #endregion

            #region Saving Data
            //Person person = new()
            //{
            //    Name = "Gençay",
            //    Address = new()
            //    {
            //        PersonAddress = "Yenimahalle/ANKARA"
            //    }
            //};

            //Person person2 = new()
            //{
            //    Name = "Hilmi"
            //};

            //await context.AddAsync(person);
            //await context.AddAsync(person2);

            //Blog blog = new()
            //{
            //    Name = "Gencayyildiz.com Blog",
            //    Posts = new List<Post>
            //    {
            //        new(){ Title = "1. Post" },
            //        new(){ Title = "2. Post" },
            //        new(){ Title = "3. Post" },
            //    }
            //};

            //await context.Blogs.AddAsync(blog);

            //Book book1 = new() { BookName = "1. Kitap" };
            //Book book2 = new() { BookName = "2. Kitap" };
            //Book book3 = new() { BookName = "3. Kitap" };

            //Author author1 = new() { AuthorName = "1. Yazar" };
            //Author author2 = new() { AuthorName = "2. Yazar" };
            //Author author3 = new() { AuthorName = "3. Yazar" };

            //book1.Authors.Add(author1);
            //book1.Authors.Add(author2);

            //book2.Authors.Add(author1);
            //book2.Authors.Add(author2);
            //book2.Authors.Add(author3);

            //book3.Authors.Add(author3);

            //await context.AddAsync(book1);
            //await context.AddAsync(book2);
            //await context.AddAsync(book3);
            //await context.SaveChangesAsync();
        #endregion
    }
}

    class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Address Address { get; set; }
    }
    class Address
    {
        public int Id { get; set; }
        public string PersonAddress { get; set; }

        public Person Person { get; set; }
    }
    class Blog
    {
        public Blog()
        {
            Posts = new HashSet<Post>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
    class Post
    {
        public int Id { get; set; }
        public int? BlogId { get; set; }//! bunu fluent api da setnull kullanacağım zaman hata vermesin diye koydum çünkü eğerki ben blog u sildiğim zaman post daki foreign key e null vermemiz gerekicek o yüzden ? koyduk
        public string Title { get; set; }

        public Blog Blog { get; set; }
    }
    class Book
    {
        public Book()
        {
            Authors = new HashSet<Author>();
        }
        public int Id { get; set; }
        public string BookName { get; set; }

        public ICollection<Author> Authors { get; set; }
    }
    class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }
        public int Id { get; set; }
        public string AuthorName { get; set; }

        public ICollection<Book> Books { get; set; }
    }


    class ApplicationDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             optionsBuilder.UseSqlite("Data Source=ApplicationDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasOne(a => a.Person)
                .WithOne(p => p.Address)
                .HasForeignKey<Address>(a => a.Id)
                .OnDelete(DeleteBehavior.SetNull); //! one to one senaryolarda foreign key ve primary key aynı kolon ise bunu setnull yapamayız unutma
                

            modelBuilder.Entity<Post>() //!şimdi burada eğerki post entitsindeki foreign key e ? yani null olabilir demezsem hata verir post entisinde açıkladım
                .HasOne(p => p.Blog)
                .WithMany(b => b.Posts)
                .IsRequired(false) //! bunuda setnull kullandığım için ilgili foreign key kolonu illaki  required(gerekli,zorunlu) olmak zorunda değil diyoruz
                .OnDelete(DeleteBehavior.SetNull); //* burada cascade delet i nasıl yapacaksa yani cascade,setnull,restrict hangisini kullanacaksan buraya yazman lazım.


                 //! Çoka çok ilişkide silme kavramı her daim cascade üzerine kuruludur.Yani çoka çok ilişkide bir kitap sildiğimizde cros table da bunun karşılığında null yaz demiyoruz direk siliyoruz.Zaten entityframeworkcore buna izin vermiyor yazamıyoruz yani
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books);
        }
    }
