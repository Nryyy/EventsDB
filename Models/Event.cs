using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfPerfomance { get; set; }
        public DateTime TimeOfPerfomance { get; set; }
        public Location Location { get; set; }
        public Organizers Organizers { get; set; }
        public int Participants { get; set; }
        public int Budget { get; set; }
        public Category Categoty { get; set; }
        public Status Status { get; set; }

    }
}
