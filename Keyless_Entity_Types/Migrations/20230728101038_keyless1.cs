using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keyless_Entity_Types.Migrations
{
    /// <inheritdoc />
    public partial class keyless1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                 migrationBuilder.Sql($@"  
                    CREATE VIEW vw_PersonOrderCount
                    AS
	                    SELECT p.Name, COUNT(*) Count FROM Persons p
	                    JOIN Orders o 
		                    ON p.PersonId = o.PersonId
	                    GROUP By p.Name
                ");//*view oluşturduk
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"DROP VIEW vw_PersonOrderCount");
        }
    }
}
