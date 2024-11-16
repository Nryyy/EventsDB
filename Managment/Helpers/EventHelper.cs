using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Managment.Classes;
using CsvHelper.Configuration;
using Models;

namespace Managment.Helpers
{
    public class EventHelper
    {
        private readonly EventRepository _eventRepository;
        private readonly LocationRepository _locationRepository;
        private readonly OrganizersRepository _organizersRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly StatusRepository _statusRepository;

        public EventHelper(EventRepository eventRepository, LocationRepository locationRepository, OrganizersRepository organizersRepository, CategoryRepository categoryRepository, StatusRepository statusRepository)
        {
            _eventRepository = eventRepository;
            _locationRepository = locationRepository;
            _organizersRepository = organizersRepository;
            _categoryRepository = categoryRepository;
            _statusRepository = statusRepository;
        }

        public void ParseAndStoreEvents(string csvFilePath)
        {
            var events = new List<Event>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<EventCsvRecord>().ToList();

                foreach (var record in records)
                {
                    var location = _locationRepository.Get().FirstOrDefault(l => l.Name == record.Location);
                    var organizer = _organizersRepository.Get().FirstOrDefault(o => o.Name == record.Organizers);
                    var category = _categoryRepository.Get().FirstOrDefault(c => c.Name == record.Categories);
                    var status = _statusRepository.Get().FirstOrDefault(s => s.Name == record.Status);

                    if (location == null || organizer == null || category == null || status == null)
                    {
                        // Handle the case where any of the required entities are not found
                        continue;
                    }

                    var newEvent = new Event
                    {
                        Name = record.Event_Name,
                        DateOfPerfomance = DateTime.Parse(record.Date_of_event),
                        TimeOfPerfomance = DateTime.Parse(record.Start_time),
                        Location = location,
                        Organizers = organizer,
                        Categoty = category,
                        Status = status,
                        Participants = int.Parse(record.Number_of_participants),
                        Budget = (int)decimal.Parse(record.Budget, CultureInfo.InvariantCulture)
                    };

                    events.Add(newEvent);
                }
            }
            StoreEventsInDatabase(events);
        }

        private void StoreEventsInDatabase(List<Event> events)
        {
            foreach (var eventItem in events)
            {
                _eventRepository.Create(eventItem);
            }
            _eventRepository.SaveChanges();
        }
    }
}
