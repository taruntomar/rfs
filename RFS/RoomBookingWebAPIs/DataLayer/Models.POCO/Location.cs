using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBookingWebAPIs.DataLayer.Models.POCO
{
    public partial class Location
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public bool enabled { get; set; }
    }
}
