using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IEventManagement
    {
        IQueryable<EventManagementViewModel> GetEventManagementList(int EventId, string searchValue);
        EventManagementViewModel GetEventManagementDetailsByID(int EventID);
        bool DeleteEvent(int[] ids);
        bool Save(EventManagementViewModel EventViewModel);
        //Dictionary<string, int> EventCountByStatus();

        List<EventManagementViewModel> GetEventByUserID(int UserId);
        bool DeleteEventById(int id);

    }
}
