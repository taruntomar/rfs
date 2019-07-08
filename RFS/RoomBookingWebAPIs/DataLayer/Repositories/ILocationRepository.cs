
using System.Collections;
using System.Collections.Generic;
using TarunLab.RFS.RoomBooking.DataLayer.Models.POCO;
using TarunLab.RFS.RoomBooking.DataLayer.Paging;

namespace TarunLab.RFS.RoomBooking.DataLayer.Repositories
{
    public interface ILocationRepository
    {
        void AddLocation(string locationName);
        Location GetLocationById(string id);
        IEnumerable<Location> GetAllLocations();
        PagedResult<Location> GetListOfLocations();
    }
}