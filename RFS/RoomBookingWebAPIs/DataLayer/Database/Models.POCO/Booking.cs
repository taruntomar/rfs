using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TarunLab.RFS.RoomBooking.DataLayer.Models.POCO
{
    // persistet ignorance class
    public class Booking
    {
        public Booking()
        {

        }

        public string Id { get; set; }
        public string RoomId { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public DateTime createdOn { get; set; }
        public string createdBy { get; set; }
        public bool isCancelled { get; set; }
        public DateTime CancelledDate { get; set; }
    }
}
