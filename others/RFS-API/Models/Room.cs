using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RFS_API.Models
{
    public class Room
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public bool IsProjectorAvailable { get; set; }

    }
}