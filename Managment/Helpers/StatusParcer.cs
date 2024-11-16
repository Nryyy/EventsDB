using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using Managment.Classes;
using Models;

namespace Managment.Helpers
{
    public class StatusParser
    {
        private readonly StatusRepository _statusRepository;

        public StatusParser(StatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public void ParseAndStoreStatuses(string csvFilePath)
        {
            var statuses = new List<Status>();

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var statusField = csv.GetField<string>("Status");
                    var status = new Status { Name = statusField };
                    statuses.Add(status);
                }
            }

            StoreStatusesInDatabase(statuses);
        }

        private void StoreStatusesInDatabase(List<Status> statuses)
        {
            foreach (var status in statuses)
            {
                if (!_statusRepository.Get().Any(s => s.Name == status.Name))
                {
                    _statusRepository.Create(status);
                }
            }
            _statusRepository.SaveChanges();
        }
    }
}
