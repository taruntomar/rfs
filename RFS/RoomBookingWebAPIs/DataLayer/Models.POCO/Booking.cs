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

        [Key]
        public string Id { get; set; }
        public string RoomId { get; set; }
        public System.DateTime starttime { get; set; }
        public System.DateTime endtime { get; set; }
        public Nullable<System.DateTime> createdOn { get; set; }
        public string createdBy { get; set; }
        public Nullable<bool> isCancelled { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
    }
}
