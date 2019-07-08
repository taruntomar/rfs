using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoomBookingWebAPIs.DataLayer.UnitOfWork;
using TarunLab.RFS.RoomBooking.DataLayer.Models.POCO;
using TarunLab.RFS.RoomBooking.DataLayer.Repositories;
using TarunLab.RFS.RoomBooking.DataLayer.UnitOfWork;

namespace RoomBookingWebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private IUnitOfWork _unitOfWork; 
        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/Locations
        [HttpGet]
        public ActionResult<IEnumerable<Location>> Get()
        {
            
            ILocationRepository locationRepository = _unitOfWork.GetLocationRepository();
            IEnumerable<Location> locations = locationRepository.GetAllLocations();
            return Ok(locations);

        }

        // GET api/Locations/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "Location";
        }

        // POST api/Locations
        [HttpPost]
        public void Post([FromBody] string Location)
        {
        }

        // PUT api/Locations/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string Location)
        {
        }

        // DELETE api/Locations/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
