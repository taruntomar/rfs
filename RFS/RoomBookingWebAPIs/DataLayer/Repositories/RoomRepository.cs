using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarunLab.RFS.RoomBooking.DataLayer.UnitOfWork.EF;

namespace TarunLab.RFS.RoomBooking.DataLayer.Repositories
{
    public class RoomRepository:IRoomRepository
    {
        private IRFSDbContext _dbContext;
        public RoomRepository()
        {

        }
    }
}
