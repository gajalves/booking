using Microsoft.EntityFrameworkCore.Migrations;

namespace BooKing.Apartments.Infra.Migrations
{
    /// <inheritdoc />
    public partial class SeedApartmentsAndAmenities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert data into Amenity table
            migrationBuilder.Sql(@"
                INSERT INTO [Apartment].[Amenity] (Id, Name) VALUES 
                ('1b06cfeb-2f17-4e5b-b5e4-7f37a63c6121', 'Pool'),
                ('e2f77dd6-80fa-4ef4-bcfc-89d2125ddf50', 'WiFi'),
                ('2a8bb93d-2b14-4d2b-9c38-12569bf93fba', 'Gym'),
                ('3b7c991e-8d4b-4f1e-97c2-7b2f29bc72b3', 'Parking'),
                ('4c9a8a9e-3a6d-4e1a-a8b3-9c6a9db7d7f1', 'Air Conditioning');
            ");

            // Insert data into Apartment table
            migrationBuilder.Sql(@"
                INSERT INTO [Apartment].[Apartment] (Id, CleaningFee, Description, ImagePath, LastBookedOnUtc, Name, OwnerId, Price, Address_Country, Address_State, Address_ZipCode, Address_City, Address_Street, Address_Number) VALUES 
                ('3c7a97b7-5898-4030-a8d3-1f4d1b2049f3', 15, 'A cozy and comfortable apartment.', 'https://images.unsplash.com/photo-1493809842364-78817add7ffb?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', NULL, 'Cozy Apartment', '7FE95E81-98F0-4D54-B723-0634E067F7BE', 100, 'USA', 'California', '90001', 'Los Angeles', 'Sunset Boulevard', '456'),                              
                ('d0b0fa3d-2d3b-44f1-b568-23d3f7c1f2ad', 20, 'A beautiful luxury apartment.', 'https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', NULL, 'Luxury Apartment', '1866E30D-9813-462A-B307-4A442A8FBF74', 150, 'USA', 'New York', '10001', 'New York City', 'Fifth Avenue', '123'),
                ('c1d0e8b7-4b0b-4e2a-8a1c-1d2c7e3b4b7e', 18, 'A modern apartment with all amenities.', 'https://images.unsplash.com/photo-1560185007-5f0bb1866cab?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', NULL, 'Modern Apartment', '0CFCABA1-5B76-4B9A-9B37-BA3A8F6BC4AA', 120, 'Canada', 'Ontario', 'M5V3K2', 'Toronto', 'King Street', '789'),
                ('d2e1f3c4-5d2f-4e3a-9b2c-2e4d5f6e7f8b', 22, 'An elegant apartment in the city center.', 'https://images.unsplash.com/photo-1505873242700-f289a29e1e0f?q=80&w=2076&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', NULL, 'Elegant Apartment', '6FE6E56E-9B23-42BA-9525-CC0C8C5AC768', 170, 'UK', 'England', 'SW1A1AA', 'London', 'Baker Street', '221B'),
                ('4bbc1ec1-15e9-404e-a97e-a2ace6c2c518', 25, 'A spacious apartment with a great view.', 'https://images.unsplash.com/photo-1503174971373-b1f69850bded?q=80&w=2113&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', NULL, 'Spacious Apartment', '6FE6E56E-9B23-42BA-9525-CC0C8C5AC768', 200, 'Australia', 'New South Wales', '2000', 'Sydney', 'George Street', '100');
            ");

            // Insert data into AmenityApartment table
            migrationBuilder.Sql(@"
                INSERT INTO [Apartment].[AmenityApartment] (AmenitiesId, ApartmentId) VALUES 
                ('1b06cfeb-2f17-4e5b-b5e4-7f37a63c6121', '3c7a97b7-5898-4030-a8d3-1f4d1b2049f3'),
                ('e2f77dd6-80fa-4ef4-bcfc-89d2125ddf50', '3c7a97b7-5898-4030-a8d3-1f4d1b2049f3'),
                ('2a8bb93d-2b14-4d2b-9c38-12569bf93fba', 'd0b0fa3d-2d3b-44f1-b568-23d3f7c1f2ad'),
                ('3b7c991e-8d4b-4f1e-97c2-7b2f29bc72b3', 'd0b0fa3d-2d3b-44f1-b568-23d3f7c1f2ad'),
                ('4c9a8a9e-3a6d-4e1a-a8b3-9c6a9db7d7f1', 'c1d0e8b7-4b0b-4e2a-8a1c-1d2c7e3b4b7e'),
                ('1b06cfeb-2f17-4e5b-b5e4-7f37a63c6121', 'd2e1f3c4-5d2f-4e3a-9b2c-2e4d5f6e7f8b'),
                ('2a8bb93d-2b14-4d2b-9c38-12569bf93fba', 'd2e1f3c4-5d2f-4e3a-9b2c-2e4d5f6e7f8b'),
                ('3b7c991e-8d4b-4f1e-97c2-7b2f29bc72b3', '4bbc1ec1-15e9-404e-a97e-a2ace6c2c518'),
                ('4c9a8a9e-3a6d-4e1a-a8b3-9c6a9db7d7f1', '4bbc1ec1-15e9-404e-a97e-a2ace6c2c518');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //
        }
    }
}
