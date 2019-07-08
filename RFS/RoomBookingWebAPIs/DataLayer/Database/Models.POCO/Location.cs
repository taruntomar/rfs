using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TarunLab.RFS.RoomBooking.DataLayer.Models.POCO
{
    public class Location
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public bool enabled { get; set; }
    }
}
