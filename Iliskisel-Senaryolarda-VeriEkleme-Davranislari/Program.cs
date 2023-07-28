using Microsoft.EntityFrameworkCore;

namespace Iliskisel_Senaryolarda_VeriEkleme_Davranislari;
class Program
{
      static ApplicationDbContext context = new(); // her regionda tekrar context tanımlamamak için global olarak oluşturduk

    static void Main(string[] args)
    {
        #region One to One İlişkisel Senaryolarda Veri Ekleme

            //1. yöntemde Pirncipal entity üzerinden ekleme işlemi gerçekleştiryorsak dependent entity vermek zorunda değiliz ama

            //2. yöndetemde olduğu gibi dependet enttiy üzerinden bir veri ekleme gerçekleştiriyorsak Principal entity zorunlu olarak verilmelidir.
            #region 1.Yöntem-> Principal Entity Üzerinden Dependent Entity Verisi Ekleme

                // Person person = new();
                // person.Name = "Hasan";
                // person.Address=new(){PersonAddress="asdasdad/asdadsad"};
                // context.Add(person);
                // context.SaveChanges();
            #endregion
                //eğerki principla entity üzerinden ekleme gerçekleştiriliyosa dependent entity nesnesi verilemk zorunda değildir amma velakin dependent entity üzerinden ekleme işlemi gerçekleşt,iriliyosa eğer burada principla entitynin nesnesine ihtiyacımız vardı ve zorunlu olarak verilmelidir.
            #region 2.Yöntem-> Dependet Entity Üzerinden Principal Entity Verisi Ekleme
                // Address address= new()
                // {
                //     PersonAddress="ASDASDAS/ASDASDAS",
                //     Person = new(){Name="ahmet"}
                // };
                // context.Add(address);
                // context.SaveChanges();
            #endregion
        #endregion    
       
        #region One to Many İlişkisel Senaryolarda Veri Ekleme
             
             #region 1.Yöntem-> Principal Entity Üzerinden Dependent Entity Verisi Ekleme

                #region Nesne Refransı Üzerinden Ekleme
                //oop de gördüğümüz gibi referans null ise bu referasn üzerinden  bir membera ulaştığımız zaman null refernas hatası  verecek biz bunun için blog nesnesi üzerinden post a erişirken kesinklikle null olmadığından emin olmamız lazım işte bunun içinde  blog entitisi içerisinde constructor içinde post nesnesi üretiyoruz.
                    // Blog blog = new(){Name="hasan.com.blog"};
                    // blog.Posts.Add(new(){Title="Post 1"});
                    // blog.Posts.Add(new(){Title="Post 2"});
                    // blog.Posts.Add(new(){Title="Post 3"});
                    
                    
                    // context.Add(blog);
                    // context.SaveChanges();
                #endregion

                #region Object Initilializer Üzerinden Ekleme
                    // Blog blog2 = new() // bunun için construcotra gerek yok olmadan da olur
                    // {
                    //     Name="A Blog",
                    //     Posts = new List<Post>()
                    //     {
                    //         new(){Title = "Post4"}, 
                    //         new(){Title="Post5"}
                    //     }
                    // };
                    //   context.Add(blog2);
                    // context.SaveChanges();
                #endregion

                
            #endregion
               
            #region 2.Yöntem-> Dependet Entity Üzerinden Principal Entity Verisi Ekleme
            //şimdi bire çok  ilişkilerde dependet enttiy den başlamanın bir mantığı yok çünkü;
            //dependent den başaldığımız zaman bir blog entitsine sadece bir post ekleyebiliyoruz bununda bire çok ilşki için mantığı olmuyor bire bire ilişki gibi oluyor
                // Post post= new () 
                // {
                //     Title="Post 6",
                //     Blog = new(){Name="b blog"}
                // };
                // context.Add(post);
                // context.SaveChanges();
            #endregion

            #region 3.Yöntem-> Foreign Key Kolonu Üzerinden Veri Ekleme 

                //1. ve 2. yöntemler hiç olmayan verilerin ilişkisel olarak ejklenmesini sağlarken bu 3. yöntem önceden eklenmiş olan bir principal entity verisiyle yeni dependent entitylerin ilişkisel olarak eşleştirilmesini sağlamaktadır.
            
                // Post post = new()
                // {
                //     BlogId=1, // şimdi burada dependet entity ile veri eklemeye çalıştığımız zaman blog entitisi de mecbur olmalıydı  bizde burada yeni blog oluşturmak yerine var olan blog üzerine blogId si sayesinde post ekleme işlemi yaptık
                //     Title = "pOST7"
                // };

                // context.Add(post);
                // context.SaveChanges();
            #endregion

            


        #endregion     
    
        #region Many to Many İlişkisel Senaryolarda Veri Ekleme    

                #region 1. yöntem
                    //çoka çok ilşkisi eğer ki default convension üzerinden tasarlanmışsa kullanılan bir yönetnmedir.

                    // Book book = new()
                    // {
                    //     BookName="A kitabı",
                    //     Authors= new HashSet<Author>()
                    //     {
                    //         new(){AuthorName="Hilmi"},
                    //         new(){AuthorName="Ayşe"},
                    //         new(){AuthorName="Fatma"},
                    //     }
                    // };
                    // context.Add(book);
                    // context.SaveChanges();

                #endregion

                #region 2.YÖntem
                     //çoka çok ilşkisi eğer ki  fluent Api üzerinden tasarlanmışsa kullanılan bir yönetnmedir.

                    //şimdi burada author oluşturduk mustafa adında bu yazarı hem 1 ıd li kitapla eşleştirdik hemde yeni bir kitap oluşturup onunla eşleştirdik
                    Author author= new()
                    {
                        AuthorName="Mustafa",
                        Books=new HashSet<BookAuthor>()
                        {
                            new(){BookId=1},
                            new(){Book=new(){BookName="b kitap"}}
                        }
                    };                                  



                #endregion


        #endregion    
    
    
    }

        #region One to One İlişkisel Senaryolarda Veri Ekleme
            
            // class Person
            // {
            //     public int Id { get; set; }
            //     public string Name { get; set; }
            //     public Address Address { get; set; }
            // }

            // class Address
            // {
            //     public int Id { get; set; }
            //     public string PersonAddress { get; set; }
            //     public Person Person { get; set; }
            // }

            // class ApplicationDbContext:DbContext
            // {
            //     public DbSet<Person> Persons { get; set; }
            //     public DbSet<Address> Addresses { get; set; }
            //     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            //     {
            //         optionsBuilder.UseSqlite("Data Source=ApplicationDb");
            //     }

            //     protected override void OnModelCreating(ModelBuilder modelBuilder)
            //      {
           
            //       modelBuilder.Entity<Address>() 
            //     .HasOne(a => a.Person)
            //     .WithOne(p => p.Address)
            //     .HasForeignKey<Address>(a =>a.Id );
            //        }

            // }

       #endregion
         
        #region One to Many İlişkisel Senaryolarda Veri Ekleme
            
            // class Blog
            // {
            //     public Blog()
            //     {
            //         //kullanımı yukarıda açıkladım
            //         //burada hashset yerine list,  arraylist kullanabilirz
            //         Posts = new HashSet<Post>();
            //     }
            //     public int Id { get; set; }
            //     public string Name { get; set; }
            //     public ICollection<Post> Posts { get; set; }
            // }

            // class Post
            // {
            //     public int Id { get; set; }
            //     public int BlogId { get; set; } // bu foreign key kolonunu tanımlamasaydık 1. ve 2. yöntemler çalışırdı ama 3.yöntem için ekledik
            //     public string Title { get; set; }
            //     public Blog Blog { get; set; }
            // }   

            // class ApplicationDbContext:DbContext
            // {
            //     public DbSet<Blog> Blogs { get; set; }
            //     public DbSet<Post> Posts { get; set; }
            //     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            //     {
            //         optionsBuilder.UseSqlite("Data Source=ApplicationDb");
            //     } 

            // }

         #endregion   

        #region Many to Many İlişkisel Senaryolarda Veri Ekleme

            #region 1.Yöntem için
            //      class Book
            // {
            //     public Book()
            //     {
            //         Authors= new HashSet<Author>();
            //     }
            //     public int Id { get; set; }
            //     public string BookName { get; set; }
            //     public ICollection<Author> Authors { get; set; }
            // }


            

            // class Author
            // {
            //     public Author()
            //     {
            //         Books=new HashSet<Book>();
            //     }
            //     public int Id { get; set; }
            //     public string AuthorName { get; set; }
            //     public ICollection<Book>Books { get; set; }
            // }


            //  class ApplicationDbContext:DbContext
            // {
            //     public DbSet<Book> Books { get; set; }
            //     public DbSet<Author> Authors { get; set; }
            //     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            //     {
            //         optionsBuilder.UseSqlite("Data Source=ApplicationDb");
            //     }
            // }

            #endregion


            #region 2.Yöntem için
            class Book
            {
                public Book()
                {
                    Authors= new HashSet<BookAuthor>();
                }
                public int Id { get; set; }
                public string BookName { get; set; }
                public ICollection<BookAuthor> Authors { get; set; }
            }


            class BookAuthor
            {
                public int BookId { get; set; }
                public int AuthorId { get; set; }
                public Book Book { get; set; }
                public Author Author { get; set; }
            }

            class Author
            {
                public Author()
                {
                    Books=new HashSet<BookAuthor>();
                }
                public int Id { get; set; }
                public string AuthorName { get; set; }
                public ICollection<BookAuthor>Books { get; set; }
            }


             class ApplicationDbContext:DbContext
            {
                public DbSet<Book> Books { get; set; }
                public DbSet<Author> Authors { get; set; }
                protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                {
                    optionsBuilder.UseSqlite("Data Source=ApplicationDb");
                }

                protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    modelBuilder.Entity<BookAuthor>()
                    .HasKey(ba=>new{ba.AuthorId,ba.BookId});

                    modelBuilder.Entity<BookAuthor>()
                    .HasOne(ba => ba.Book)
                    .WithMany(b=>b.Authors)
                    .HasForeignKey(ba=>ba.BookId);

                    modelBuilder.Entity<BookAuthor>()
                    .HasOne(ba => ba.Author)
                    .WithMany(b=>b.Books)
                    .HasForeignKey(ba=>ba.AuthorId);

                }
            }
            #endregion
          



            



        #endregion    


}
