using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using Managment.Classes;
using Models;

namespace Managment.Helpers
{
    public class LocationParser
    {
        private readonly LocationRepository _locationRepository;

        public LocationParser(LocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public void ParseAndStoreLocations(string csvFilePath)
        {
            var locations = new List<Location>();

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var locationName = csv.GetField<string>("Location");
                    var location = new Location { Name = locationName };
                    locations.Add(location);
                }
            }

            StoreLocationsInDatabase(locations);
        }


        private void StoreLocationsInDatabase(List<Location> locations)
        {
            foreach (var location in locations)
            {
                _locationRepository.Create(location);
            }
            _locationRepository.SaveChanges();
        }
    }
}
