using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace View_Olusturma_Kullanma.Migrations
{
    /// <inheritdoc />
    public partial class view2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.Sql($@"
                        CREATE VIEW vm_PersonOrders
                        AS
	                        SELECT TOP 100 p.Name, COUNT(*) [Count] FROM Persons p
	                        INNER JOIN Orders o
		                        ON p.PersonId = o.PersonId
	                        GROUP By p.Name
	                        ORDER By [Count] DESC
                        ");
                        //*normalde view içerisinde order by kullanılmaz sql dede kullanılmaz efcore için geçerli değil yani
                        //*bunun için de TOP, OFFSET ya da FOR ile çalışma yazpmalısın diyor bizde TOP keywordünü kullanıyoruz
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.Sql($@"DROP VIEW vm_PersonOrders");
             //*terminalden view1 migrationuna geri dönersem view2  deki Down metodu çalışır ve view silinir.
        }
    }
}
