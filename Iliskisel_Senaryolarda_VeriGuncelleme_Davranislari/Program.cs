using Microsoft.EntityFrameworkCore;

namespace Iliskisel_Senaryolarda_VeriGuncelleme_Davranislari;
class Program
{
    //* herhnagi bir satırı güncelleme değil
    //*iki ilişkisel tablo arasındaki verilerin ilişkilerinin güncellemesi üzerinde duruyoruz.

    // !!!!! genel olarak asenkron yani ASNC olarak kullan 
      static ApplicationDbContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
        #region One to One İlişkisel Senaryolarda Veri Güncelleme
            #region Saving
            // Person person = new()
            // {
            // Name = "Gençay",
            // Address = new()
            // {
            //     PersonAddress = "Yenimahalle/ANKARA"
            // }
            // };

            // Person person2 = new()
            // {
            // Name = "Hilmi"
            // };

            // context.Add(person);
            // context.Add(person2);
            // context.SaveChanges();
            #endregion

            #region 1. Durum | Esas tablodaki veriye bağımlı veriyi değiştirme
            // Person? person =  context.Persons
            //    .Include(p => p.Address)
            //    .FirstOrDefault(p => p.Id == 1); // personlardan ıd 1 olanı ve ınclude ile de addreslere erişerek ıd si 1 olan personla birlikte adresinide getiriyoruz.

            // context.Addresses.Remove(person.Address); //burada ise yukarıdan gelen veri sayesinde elimizde personun adres bilgisini bulunduruyoruz artık  context üzerinden Addresses tablosuna erişerek elimizde bulunan adresi siliyoruz
            // person.Address = new()//burada ise sildiğimiz adresin yerinede yeni adresi ekliyoruz 
            // {
            //    PersonAddress = "Yeni adres"
            // };

            //  context.SaveChanges();
            #endregion
            #region 2. Durum | Bağımlı verinin ilişkisel olduğu ana veriyi güncelleme
            // Address? address =  context.Addresses.Find(1);
            // address.Id = 2;
            //  context.SaveChanges(); // bu kod bloğu hata verir çünkü  adresin içindeki ıd kolonu bir keye karşılık geliyor bu yüzden silinemiyor bunun için öncellikle  bağımlı olan dependent veriyi silin savechanges çağrıın sonra ilgili veriyi oluşturdukdan sonra principal veri ile ilişkilendir.

            // Address? address =  context.Addresses.Find(2); 
            // context.Addresses.Remove(address);
            //  context.SaveChanges();//yukarıda olmamıştı bizde burada yukarıda açıkladığım gibi yapttık ıd ye karşılık gelen addresi bulduk bunu sildik ve kaydettik

            // // Person? person = context.Persons.Find(2);//sonra ıd si 2 olan personu bulduk getirdik ve bu person nun bilgilerini elimizde addres bilgisi vardı yukarıda onun person bilgisine attık sonra bu addres bilgisinide addresses tablosuna gönderdik
            // // address.Person = person;
            // // context.Addresses.Add(address);

            // address.Person = new() //yukarıdaki satır yerine burayıda kullanarak addresimize yeni bir person ekleyebiliriz
            // {
            //    Name = "Rıfkı"
            // };
            //  context.Addresses.Add(address);

            //  context.SaveChanges();
            #endregion
        #endregion

        #region One to Many İlişkisel Senaryolarda Veri Güncelleme
        #region Saving
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
        //await context.SaveChangesAsync();
        #endregion

        #region 1. Durum | Esas tablodaki veriye bağımlı verileri değiştirme
        // Blog? blog =  context.Blogs
        //    .Include(b => b.Posts)
        //    .FirstOrDefault(b => b.Id == 1); //ıd si 1 olan blog un bilgileri postlar ile birlikte getirdik.

        // Post? silinecekPost = blog.Posts.FirstOrDefault(p => p.Id == 2); 
        // blog.Posts.Remove(silinecekPost);// id si 1 olan blog un postları elimizde bunlar arasından post ıd si 2 olanı silinecekpost değişkenine atıyoruz. ve  siliyoruz

        // blog.Posts.Add(new() { Title = "4. Post" });
        // blog.Posts.Add(new() { Title = "5. Post" });//sonra ıd si 1 olan blog elimizde hala onadan post lara erişerek yeni 4. ve 5. postları ekliyoruz ve bunlar ıd 1 olan blog ile eşleşiyor
        // //birde bundan önceki projde gördüğümüz post lar boş gelmemesi için entity de constructor tanımlamıştık çünkü ozaman include yapmıyorduk ama burada biz blog un post larını getirdiğimiz için onu yapmasakta olur.

        //  context.SaveChanges();
        #endregion
        #region 2. Durum | Bağımlı verilerin ilişkisel olduğu ana veriyi güncelleme
        // Post? post =  context.Posts.Find(4); //4 ıd li postmuzu buluyoruz
        // post.Blog = new() //burada da yeni bir blog oluşturuyoruz 2.blog adında ve bu bloga a yukarıda post a attığımız 4 id li postu bu yeni ürettiğimiz bloga kaydediyoruz
        // {
        //    Name = "2. Blog"
        // };
        //  context.SaveChangesAsync();


        // Post? post =  context.Posts.Find(5); // eğerki yukarıdaki kod yerine 5 ıd li post u 2 ıdli bloga atacağız diyelim 2 ıdli blog uda daha yeni yukarıda oluşturduk
        // Blog? blog =  context.Blogs.Find(2); //burada 2 ıd li blogu buluyoruz
        // post.Blog = blog; // ve 5 ıd li postun içinde blog ıd sine de 2 ıdli blog u veriyoruz
        //  context.SaveChanges();
        #endregion
        #endregion

        #region Many to Many İlişkisel Senaryolarda Veri Güncelleme
        #region Saving
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

        #region 1. Örnek
        //* 1.kitabı 3.yaazarla ilişkilendirmek istiyoruz

        //* Book? book =  context.Books.Find(1);//kitaplardan  id si 1 olanı bulup book a attık
        //* Author? author =  context.Authors.Find(3); //yazarlardan da ıd si 3 olanı bulup author içine attık 
        //* book.Authors.Add(author);//sonra kitap bilgisini tutan book içindeki Author kolonuna yukarıdaki satırda id si 3 olan yazarı tuttuğumuz author değişkenini gönderiyoruz.

         context.SaveChanges();
        #endregion
        #region 2. Örnek
        //* burada 3 ıdli yazarın  bütün kitaplarla ilişkisi var (zaten 3 kitap var elimizde) bizde yazar sadece 1 ıdli kitapla ilişkisi olsun diğer 2 kitapla ilişkisini kesmek istiyoruz.
        //*İnclude kullandığımız yerde find kullanılamıyor
        // Author? author =  context.Authors
        //    .Include(a => a.Books)
        //    .FirstOrDefault(a => a.Id == 3); //* ıd si 3 olan yazarın ilişkisi olan kitaplar la birlikte getiriyoruz.

        // foreach (var book in author.Books) //* yukarıdaki satırda elimizde kitap ve author bilgileri var bizde foreach ile yazarın ilişkisi olduğu kitaplar üzerinde dönücez ve kitap ıd si 1 e eşit olmayanları silecek
        // {
        //    if (book.Id != 1)
        //        author.Books.Remove(book);
        // }

        //  context.SaveChanges();
        #endregion
        #region 3. Örnek

        //* ıd si 2 olan kitabın  ıd si 1 olan yazrla ilişkisini kesip ıd si 3 olan yazarla ilişkilendir ve yeni bir tane yazar ekle
        Book? book =  context.Books
           .Include(b => b.Authors)
           .FirstOrDefault(b => b.Id == 2); //*şimdi buradaıd si 2 olan kitabın yazarıyla birlikte bilgileri geldi

        Author silinecekYazar = book.Authors.FirstOrDefault(a => a.Id == 1);
        book.Authors.Remove(silinecekYazar); //* ıd si 2 olan kitabın 1 ıd li yazarla ilişkisini kesicez bunun için silinecek olan yazar bilgisini yukarıdaki satırda bulup silinecekYazar değişkenine atıyoruz

        Author author =  context.Authors.Find(3); //* sonra ıd si 3 olan yazarı tablodan çekiyoruz.
        book.Authors.Add(author);//* ıd si 3 olan yazarı ıd si 2 olan kitap(book ilk yaptığımız işlem) ile ilişkilendiriyoruz.
        book.Authors.Add(new() { AuthorName = "4. Yazar" });//* ve son olarak da yeni bir yazar ekliyoruz ve  id si 2 olan kitaba bu yazar ile ilişkilendiriyoruz.

         context.SaveChanges();
        #endregion
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
            public int BlogId { get; set; }
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
                    .HasForeignKey<Address>(a => a.Id);
            }
        }

 
