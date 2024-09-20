using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooKing.Apartments.Infra.Migrations
{
    /// <inheritdoc />
    public partial class populateSearchField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE A
                SET A.SearchField = A.Name + '-' + A.Description + '-' + A.Address_City + '-' + A.Address_State + '-' + (
                    SELECT STRING_AGG(AM.Name, '-') 
                    FROM [Apartment].[AmenityApartment] AA
                    INNER JOIN [Apartment].[Amenity] AM ON AA.AmenitiesId = AM.Id
                    WHERE AA.ApartmentId = A.Id
                )
                FROM [Apartment].[Apartment] A;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
