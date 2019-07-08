using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarunLab.RFS.RoomBooking.DataLayer.Repositories;

namespace TarunLab.RFS.RoomBooking.DataLayer.UnitOfWork
{
    interface IUnitOfWork<TContext>:IDisposable
    {
        void SaveChanges();
    }

    public interface IUnitOfWork
    {
        ILocationRepository GetLocationRepository();
    }
}
