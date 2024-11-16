using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment.Helpers
{
    public class EventCsvRecord
    {
        public string Event_Name { get; set; }
        public string Date_of_event { get; set; }
        public string Start_time { get; set; }
        public string Location { get; set; }
        public string Organizers { get; set; }
        public string Number_of_participants { get; set; }
        public string Budget { get; set; }
        public string Categories { get; set; }
        public string Status { get; set; }
    }
}
