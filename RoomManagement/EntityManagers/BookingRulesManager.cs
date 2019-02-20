using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomManagement.Entities;

namespace RoomManagement
{
    //public class BookingRulesManager : IBookingRulesManager
    //{
    //    private IRoomManagementDatabasseEntities _dbContext;
    //    public BookingRulesManager(IRoomManagementDatabasseEntities dbContext)
    //    {
    //        _dbContext = dbContext;
    //    }
   
    //    public void AddNewBookingRules(BookingRules bookingRule)
    //    {
    //        _dbContext.BookingRuless.Add(bookingRule);

    //    }

    //    public bool CreateBookingRules()s
    //    {
    //        return true;
    //    }

    //    public void DeleteBookingRules(string id)
    //    {
    //        BookingRules bookingRule = GetBookingRulesById(id);
    //        if (bookingRule != null)
    //        {
    //            _dbContext.BookingRuless.Remove(bookingRule);
    //            _dbContext.SaveChanges();
    //        }
    //    }

    //    public BookingRules GetBookingRulesById(string id)
    //    {
    //        return _dbContext.BookingRuless.FirstOrDefault(x => x.Id == id);
    //    }

    //    public IEnumerable<BookingRules> GetBookingRulesForRoom(string roomId,DateTime starttime,DateTime endTime)
    //    {
    //        return _dbContext.BookingRuless.Where(x => x.RoomId == roomId && (x.starttime.CompareTo(starttime)>=0) && (x.endtime.CompareTo(endTime)<=0));
    //    }

    //    public IEnumerable<BookingRules> GetBookingRulesCreatedBetween(DateTime starttime, DateTime endTime)
    //    {
    //        return _dbContext.BookingRuless.Where(x => (x.starttime.CompareTo(starttime) >= 0) && (x.endtime.CompareTo(endTime) <= 0));
    //    }

    //    public IEnumerable<BookingRules> GetBookingRulesDoneByUser(string userId, DateTime starttime, DateTime endTime)
    //    {
    //        return _dbContext.BookingRuless.Where(x => x.createdBy == userId && (x.starttime.CompareTo(starttime) >= 0) && (x.endtime.CompareTo(endTime) <= 0));
    //    }

    //    public void UpdateBookingRules(string id, BookingRules bookingRule)
    //    {
    //        BookingRules __bookingRule = GetBookingRulesById(id);
    //        if (bookingRule != null)
    //        {
    //            __bookingRule.starttime = bookingRule.starttime;
    //            __bookingRule.endtime = bookingRule.endtime;
    //            __bookingRule.RoomId = bookingRule.RoomId;
    //            _dbContext.SaveChanges();
    //        }
    //    }
    }
}
