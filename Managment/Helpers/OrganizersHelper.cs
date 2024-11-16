using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using Managment.Classes;
using Models;

namespace Managment.Helpers
{
    public class OrganizersHelper
    {
        private readonly OrganizersRepository _organizersRepository;

        public OrganizersHelper(OrganizersRepository organizersRepository)
        {
            _organizersRepository = organizersRepository;
        }

        public void ParseAndStoreOrganizers(string csvFilePath)
        {
            var organizers = new List<Organizers>();

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var organizerField = csv.GetField<string>("Organizers");
                    var organizer = new Organizers { Name = organizerField };
                    organizers.Add(organizer);
                }
            }

            StoreOrganizersInDatabase(organizers);
        }

        private void StoreOrganizersInDatabase(List<Organizers> organizers)
        {
            foreach (var organizer in organizers)
            {
                _organizersRepository.Create(organizer);
            }
            _organizersRepository.SaveChanges();
        }
    }
}
