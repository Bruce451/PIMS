using Pims.Core.Extensions;
using Pims.Core.Helpers;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class PropertyServiceTest
    {
        #region Data
        public static IEnumerable<object[]> ParcelFilters =>
            new List<object[]>
            {
                new object[] { new ParcelFilter(50, 25, 50, 20), 1, 1 },
                new object[] { new ParcelFilter(50, 24, 50, 26), 0, 0 },
                new object[] { new ParcelFilter() { Agencies = new int[] { 3 } }, 1, 1 },
                new object[] { new ParcelFilter() { ClassificationId = 2 }, 1, 1 },
                new object[] { new ParcelFilter() { Description = "DescriptionTest" }, 1, 1 },
                new object[] { new ParcelFilter() { Municipality = "Municipality" }, 1, 1 },
                new object[] { new ParcelFilter() { Zoning = "Zoning" }, 1, 1 },
                new object[] { new ParcelFilter() { ZoningPotential = "ZoningPotential" }, 1, 1 }
            };

        public static IEnumerable<object[]> BuildingFilters =>
            new List<object[]>
            {
                new object[] { new BuildingFilter(50, 25, 50, 20), 1, 1 },
                new object[] { new BuildingFilter(50, 24, 50, 26), 0, 0 },
                new object[] { new BuildingFilter() { Agencies = new int[] { 3 } }, 1, 1 },
                new object[] { new BuildingFilter() { ClassificationId = 2 }, 1, 1 },
                new object[] { new BuildingFilter() { Description = "DescriptionTest" }, 1, 1 },
                new object[] { new BuildingFilter() { Municipality = "Municipality" }, 5, 5 },
                new object[] { new BuildingFilter() { Tenancy = "BuildingTenancy" }, 1, 1 },
                new object[] { new BuildingFilter() { ConstructionTypeId = 2 }, 1, 1 },
                new object[] { new BuildingFilter() { PredominateUseId = 2 }, 1, 1 },
                new object[] { new BuildingFilter() { MinRentableArea = 100 }, 1, 1 },
                new object[] { new BuildingFilter() { MinRentableArea = 50, MaxRentableArea = 50 }, 1, 1 }
            };

        public static IEnumerable<object[]> AllPropertyFilters =>
            new List<object[]>
            {
                new object[] { new AllPropertyFilter(50, 25, 50, 20), 2, 2 },
                new object[] { new AllPropertyFilter(50, 24, 50, 26), 0, 0 },
                new object[] { new AllPropertyFilter() { Agencies = new int[] { 3 } }, 7, 7 },
                new object[] { new AllPropertyFilter() { ClassificationId = 2 }, 2, 2 },
                new object[] { new AllPropertyFilter() { Page = 1, Quantity = 10, Description = "DescriptionTest" }, 20, 10 },
                new object[] { new AllPropertyFilter() { Municipality = "Municipality" }, 11, 10 },
                new object[] { new AllPropertyFilter() { Tenancy = "BuildingTenancy" }, 1, 1 },
                new object[] { new AllPropertyFilter() { Zoning = "Zoning" }, 20, 10 },
                new object[] { new AllPropertyFilter() { ZoningPotential = "ZoningPotential" }, 20, 10 },
                new object[] { new AllPropertyFilter() { ConstructionTypeId = 2 }, 1, 1 },
                new object[] { new AllPropertyFilter() { PredominateUseId = 2 }, 1, 1 },
                new object[] { new AllPropertyFilter() { MinRentableArea = 100 }, 1, 1 },
                new object[] { new AllPropertyFilter() { MinRentableArea = 50, MaxRentableArea = 50 }, 1, 1 },
            };
        #endregion

        #region Constructors
        public PropertyServiceTest() { }
        #endregion

        #region Tests
        #region Get Paged Parcels
        /// <summary>
        /// User does not have 'property-view' claim.
        /// </summary>
        [Fact]
        public void GetPage_Properties_ArgumentNullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);
            var service = helper.CreateService<PropertyService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.GetPage((AllPropertyFilter)null));
        }

        /// <summary>
        /// User does not have 'property-view' claim.
        /// </summary>
        [Fact]
        public void GetPage_Parcels_ArgumentNullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);
            var service = helper.CreateService<PropertyService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.GetPage((ParcelFilter)null));
        }

        /// <summary>
        /// User does not have 'property-view' claim.
        /// </summary>
        [Fact]
        public void GetPage_Buildings_ArgumentNullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);
            var service = helper.CreateService<PropertyService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.GetPage((BuildingFilter)null));
        }

        /// <summary>
        /// User does not have 'property-view' claim.
        /// </summary>
        [Fact]
        public void GetPage_Properties_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var filter = new ParcelFilter(50, 25, 50, 20);

            var service = helper.CreateService<PropertyService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.GetPage(filter));
        }

        /// <summary>
        /// User does not have 'property-view' claim.
        /// </summary>
        [Fact]
        public void GetPage_Parcels_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var filter = new ParcelFilter(50, 25, 50, 20);

            var service = helper.CreateService<PropertyService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.GetPage(filter));
        }

        /// <summary>
        /// User does not have 'property-view' claim.
        /// </summary>
        [Fact]
        public void GetPage_Buildings_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var filter = new BuildingFilter(50, 25, 50, 20);

            var service = helper.CreateService<PropertyService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.GetPage(filter));
        }

        [Theory]
        [MemberData(nameof(ParcelFilters))]
        public void GetPage_Parcels(ParcelFilter filter, int expectedTotal, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);

            var dbName = StringHelper.Generate(10);
            using var init = helper.InitializeDatabase(dbName, user);
            var parcels = init.CreateParcels(1, 20);
            parcels.Next(0).Latitude = 50;
            parcels.Next(0).Longitude = 25;
            parcels.Next(1).Agency = init.Agencies.Find(3);
            parcels.Next(1).AgencyId = 3;
            parcels.Next(2).ClassificationId = 2;
            parcels.Next(3).Description = "-DescriptionTest-";
            parcels.Next(4).Municipality = "-Municipality-";
            parcels.Next(5).Zoning = "-Zoning-";
            parcels.Next(6).ZoningPotential = "-ZoningPotential-";
            init.SaveChanges();

            var service = helper.CreateService<PropertyService>(dbName, user);

            // Act
            var result = service.GetPage(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Entity.Views.Property>>(result);
            Assert.Equal(expectedTotal, result.Total);
            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [MemberData(nameof(BuildingFilters))]
        public void GetPage_Buildings(BuildingFilter filter, int expectedTotal, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);

            var dbName = StringHelper.Generate(10);
            using var init = helper.InitializeDatabase(dbName, user);
            var parcel1 = init.CreateParcel(1);
            var buildings = init.CreateBuildings(parcel1, 3, 20);
            buildings.Next(0).Latitude = 50;
            buildings.Next(0).Longitude = 25;
            buildings.Next(1).Agency = init.Agencies.Find(3);
            buildings.Next(1).AgencyId = 3;
            buildings.Next(2).ClassificationId = 2;
            buildings.Next(3).Description = "-DescriptionTest-";
            buildings.Next(4).BuildingTenancy = "-BuildingTenancy-";
            buildings.Next(5).BuildingConstructionTypeId = 2;
            buildings.Next(6).BuildingPredominateUseId = 2;
            buildings.Next(7).RentableArea = 100;
            buildings.Next(8).RentableArea = 50;
            var parcel2 = init.CreateParcel(2);
            parcel2.Municipality = "-Municipality-";
            init.CreateBuildings(parcel2, 24, 5);
            init.SaveChanges();

            var service = helper.CreateService<PropertyService>(dbName, user);

            // Act
            var result = service.GetPage(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Entity.Views.Property>>(result);
            Assert.Equal(expectedTotal, result.Total);
            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [MemberData(nameof(AllPropertyFilters))]
        public void GetPage_Properties(AllPropertyFilter filter, int expectedTotal, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);

            var dbName = StringHelper.Generate(10);
            using var init = helper.InitializeDatabase(dbName, user);

            var parcels = init.CreateParcels(1, 20);
            parcels.Next(0).Latitude = 50;
            parcels.Next(0).Longitude = 25;
            parcels.Next(1).Agency = init.Agencies.Find(3);
            parcels.Next(1).AgencyId = 3;
            parcels.Next(2).ClassificationId = 2;
            parcels.Next(3).Description = "-Description-";
            parcels.Next(4).Municipality = "-Municipality-";
            parcels.Next(5).Zoning = "-Zoning-";
            parcels.Next(6).ZoningPotential = "-ZoningPotential-";

            var buildings = init.CreateBuildings(parcels.First(), 50, 5);
            buildings.Next(0).Latitude = 50;
            buildings.Next(0).Longitude = 25;
            buildings.Next(1).Agency = init.Agencies.Find(3);
            buildings.Next(1).AgencyId = 3;
            buildings.Next(2).ClassificationId = 2;
            buildings.Next(3).Description = "-Description-";
            buildings.Next(4).BuildingTenancy = "-BuildingTenancy-";

            buildings.AddRange(init.CreateBuildings(parcels.Next(1), 56, 5));
            buildings.Next(5).BuildingConstructionTypeId = 2;
            buildings.Next(6).BuildingPredominateUseId = 2;
            buildings.Next(7).RentableArea = 100;
            buildings.Next(8).RentableArea = 50;

            buildings.AddRange(init.CreateBuildings(parcels.Next(4), 61, 10));

            init.SaveChanges();

            var service = helper.CreateService<PropertyService>(dbName, user);

            // Act
            var result = service.GetPage(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Entity.Views.Property>>(result);
            Assert.Equal(expectedTotal, result.Total);
            Assert.Equal(expectedCount, result.Count());
        }
        #endregion
        #endregion
    }
}