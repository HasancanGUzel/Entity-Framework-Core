namespace Kalitimsal_Durumlarda_EFCORE_Davranislari;
class Program
{
    static void Main(string[] args)
    {
        #region Kalıtımsal Durumlardan Kastedilen Nedir ?
            //*Entitylerin kendileri aarsında mirası na denir.
            //*Kendileri arasında kalıtım veriyorlarsa nasıl davranış sergiliyorlar bunu inceleyeceğiz.

        #endregion
    }
}
//*Bunları tek tek daha ayrıntılı inceleyeceğiz burada kısa bir giriş yapıyoruz.
#region Table Per Hierarchy (TPH)

    /*
        *Şimdi aşşağıda  bir hiyerarşi var yani person employee ve customer a miras vermiş employee da technician a miras vermiş
        *Hiyeraşi başına tek bir tablo oluşturuarak o tablo üzerinden operasyonları verisel işlemleri yüyürtmeye denir.
        *Kalıtımsal hiyerarşideki tüm entityler için tek bir tablo oluşturuluyor ve bunlar arasındaki ayrım için bir  !DISCRIMINATOR kolonu kullanmaktadır.
        *Böyle bir tablo oluşacak

    Persons
        Person ->     Id
        Person ->     Name
        Person ->     Surname
                      Discriminator
        Customer ->   CompanyName
        Employee  ->  Department
        Technician -> Branch
    */
    // class Person
    // {
    //     public int Id { get; set; }
    //     public string? Name { get; set; }
    //     public string? Surname { get; set; }
    // }
    // class Employee:Person
    // {
    //     public string? Department { get; set; }
    // }
    // class Customer:Person
    // {
    //     public string? CompanyName { get; set; }
    // }
    // class Technician:Employee
    // {
    //     public string? Branch { get; set; }
    // }
#endregion

#region Table Per Type (TPT)
    //*HER TÜR BAŞINA BİR TABLO DAVRANIŞI DIR.
    //*Kaltımsal hiyerarşide her sınıfa karşılık tablo oluşturmakta ve oluşturulan bir üst sınıfla birebir ilişki sağlamaktadır.
    /*
        yani şöyle;
        *Person tablosu Employee ve Customer ile bire bir ilişki ile bağlı ve employee tablosuda technician ile bire bir lişkiyle bağlı
!        ----Person tablosu----     | 
            -Id                     |
            -Name                   |
            -Surname                |
!       -----Employee Tablosu--- >- | >--|
            -Id                     |    |
            -Department             |    |
!       ----Customer Tablosu---- >- |    |
            -Id                          |
            -CompanyName                 |
!       -----Technicians Tablosu     >-- |
            -Id
            -Branch            
    */

    //  class Person
    // {
    //     public int Id { get; set; }
    //     public string? Name { get; set; }
    //     public string? Surname { get; set; }
    // }
    // class Employee:Person
    // {
    //     public string? Department { get; set; }
    // }
    // class Customer:Person
    // {
    //     public string? CompanyName { get; set; }
    // }
    // class Technician:Employee
    // {
    //     public string? Branch { get; set; }
    // }
#endregion

#region Table Per Concrete Type (TPC)
    //*ef core 7 ile beraber geldi
    //*Somut tür başına tablo demektir.
    //*Kalıtımsal hiyeraşideki sadece concrete sınıflara karşılık birer tablo oluşurmkta ve abstract yapılanmaların memberlarını bu tablolara eklemektedir.
    /*
!               Technician          Customer            Employee
!NO IDENTİTY    -Id                 -Id                 -Id  
                -Name               -Name               -Name
                -Surname            -Surname            -Surname
                -Department         -CompanyName        -Department
                -Branch
! ID KOLONLARI IDENTİTY DEĞİL ARDIŞIK ŞEKİLDE ARTAN DEĞİL.
*şimdi biz bir technician eklersek id si 1 olacak customera birşey eklerse 1 den başlamaz 2 olarak devam eder employee birşey eklersek 3 den devam eder. Tekrar technician a birşey eklersek ıd 4 den devam eder.
   */
    class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
    class Employee:Person
    {
        public string? Department { get; set; }
    }
    class Customer:Person
    {
        public string? CompanyName { get; set; }
    }
    class Technician:Employee
    {
        public string? Branch { get; set; }
    }
#endregion