using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomManagement.Entity
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public bool Projector { get; set; }
        public int Sitting { get; set; }

    }
}
