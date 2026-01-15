
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
namespace MusiBuy.Common.Repositories
{
    public class EventManagementRepository : IEventManagement
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;
        private readonly IConfiguration _config;

        public EventManagementRepository(MusiBuyDB_Connection context, IConfiguration configuration)
        {
            this._Context = context;
            _config = configuration;
        }
        #endregion

        #region Get Event data for grid
        /// <summary>
        /// Get Event data for grid
        /// </summary>
        /// <param name="EventId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns Event list</returns>
        public IQueryable<EventManagementViewModel> GetEventManagementList(int EventId, string searchValue)
        {
            var result = (from p in _Context.EventManagements
                          join c in _Context.Creatores on p.CreatorId equals c.Id
                          join m in _Context.Enums on p.EventTypeId equals m.Id
                          where m.EnumTypeId == (int)EnumTypes.EventType
                          join es in _Context.Enums on p.StatusId equals es.Id
                          where es.EnumTypeId == (int)EnumTypes.EventStatus
                          where (string.IsNullOrWhiteSpace(searchValue) ||
                          p.Title.ToLower().Contains(searchValue.ToLower()) ||
                          (c.FirstName.ToLower() + " " + c.LastName.ToLower()).Contains(searchValue.ToLower()) ||
                          m.EnumValue.ToLower().Contains(searchValue.ToLower()) ||
                          es.EnumValue.ToLower().Contains(searchValue.ToLower()))
                          orderby p.Title
                          select new EventManagementViewModel
                          {
                              Id = p.Id,
                              Title = p.Title,
                              Description = p.Description,
                              CreatorId = p.CreatorId,
                              CreatorName = c.FirstName + " " + c.LastName,
                              EventTypeId = p.EventTypeId,
                              EventTypeName = m.EnumValue,
                              EventStartDateTime = p.EventStartDateTime,
                              EventEndDateTime = p.EventEndDateTime,
                              StatusId = p.StatusId,
                              StatusName = es.EnumValue
                          });

            return result;
        }
        #endregion

        #region Get Event Details by EventID
        /// <summary>
        /// Get single Event by EventID
        /// </summary>
        /// <returns>It returns Event details by id </returns>
        public EventManagementViewModel GetEventManagementDetailsByID(int EventID)
        {
            var validateEvent = (from e in _Context.EventManagements
                                 join c in _Context.Creatores on e.CreatorId equals c.Id
                                 join m in _Context.Enums on e.EventTypeId equals m.Id
                                 where m.EnumTypeId == (int)EnumTypes.EventType
                                 join es in _Context.Enums on e.StatusId equals es.Id
                                 where es.EnumTypeId == (int)EnumTypes.EventStatus
                                 where e.Id == EventID
                                 select new EventManagementViewModel
                                 {
                                     Id = e.Id,
                                     Title = e.Title,
                                     Description = e.Description,
                                     CreatorId = e.CreatorId,
                                     CreatorName = c.FirstName + " " + c.LastName,
                                     EventTypeId = e.EventTypeId,
                                     EventTypeName = m.EnumValue,
                                     EventStartDateTime = e.EventStartDateTime,
                                     EventEndDateTime = e.EventEndDateTime,
                                     StatusId = e.StatusId,
                                     StatusName = es.EnumValue,
                                     Daterange = e.EventStartDateTime.ToString("yyyy-MM-dd HH:mm:ss") + " - " + e.EventEndDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                     RecordingURL = e.RecordingUrl
                                 }).FirstOrDefault();

            // Step 2: Calculate Duration in memory
            if (validateEvent != null)
            {
                validateEvent.Duration = GetDurationString(validateEvent.EventStartDateTime, validateEvent.EventEndDateTime);
            }
            return validateEvent;

            //EventManagementViewModel? validateEvent = (from e in _Context.EventManagements
            //                                             join c in _Context.Creatores on e.CreatorId equals c.Id
            //                                           join m in _Context.Enums on e.EventTypeId equals m.Id where m.EnumTypeId == (int)EnumTypes.EventType
            //                                           join es in _Context.Enums on e.StatusId equals es.Id where es.EnumTypeId == (int)EnumTypes.EventStatus
            //                                           where e.Id == EventID
            //                                       select new EventManagementViewModel
            //                                       {
            //                                           Id = e.Id,
            //                                           Title = e.Title,
            //                                           Description = e.Description,
            //                                           CreatorId = e.CreatorId,
            //                                           CreatorName = c.FirstName + " " + c.LastName,
            //                                           EventTypeId = e.EventTypeId,
            //                                           EventTypeName = m.EnumValue,
            //                                           EventStartDateTime = e.EventStartDateTime,
            //                                           EventEndDateTime = e.EventEndDateTime,
            //                                           StatusId = e.StatusId,
            //                                           StatusName = es.EnumValue,
            //                                           Daterange = e.EventStartDateTime.ToString("yyyy-MM-dd HH:mm:ss") + " - " +e.EventEndDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            //                                           Duration = GetDurationString(e.EventStartDateTime, e.EventEndDateTime),
            //                                           RecordingURL = e.RecordingUrl,
            //                                       }).FirstOrDefault();
            //return validateEvent;
        }

        private static string GetDurationString(DateTime start, DateTime end)
        {
            var diff = end - start;
            var totalDays = (int)diff.TotalDays;
            var totalHours = (int)diff.TotalHours;
            var hours = totalHours - (totalDays * 24);
            var minutes = diff.Minutes;

            var parts = new List<string>();
            if (totalDays > 0) parts.Add($"{totalDays} day{(totalDays > 1 ? "s" : "")}");
            if (hours > 0) parts.Add($"{hours} hour{(hours > 1 ? "s" : "")}");
            if (minutes > 0) parts.Add($"{minutes} minute{(minutes > 1 ? "s" : "")}");

            return parts.Count > 0 ? string.Join(" ", parts) : "Less than a minute";
        }


        #endregion


        #region Delete multiple Events
        /// <summary>
        /// Delete multiple Events
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteEvent(int[] ids)
        {
            _Context.EventManagements.RemoveRange(_Context.EventManagements.Where(u => ids.Contains(u.Id)).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion        

        #region Save Event
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EventViewModel"></param>
        /// <returns></returns>
        public bool Save(EventManagementViewModel EventManagementViewModel)
        {
            var EventData = new EventManagement();
            if (EventManagementViewModel.Id > 0)
            {
                EventData = _Context.EventManagements.FirstOrDefault(x => x.Id == EventManagementViewModel.Id) ?? new EventManagement();
            }
            EventData.Title = EventManagementViewModel.Title;
            EventData.Description = EventManagementViewModel.Description;
            EventData.CreatorId = EventManagementViewModel.CreatorId;
            EventData.EventTypeId = EventManagementViewModel.EventTypeId;
            EventData.EventStartDateTime = EventManagementViewModel.EventStartDateTime;
            EventData.EventEndDateTime = EventManagementViewModel.EventEndDateTime;
            EventData.StatusId = EventManagementViewModel.StatusId;
            EventData.RecordingUrl = EventManagementViewModel.RecordingURL;

            EventData.UserTypeId = EventManagementViewModel.UserTypeId;
            EventData.RoleId = EventManagementViewModel.RoleId;
            EventData.UserId = EventManagementViewModel.UserId;

            if (EventData.Id == 0)
            {
                EventData.IsActive = true;
                EventData.CreatedOn = DateTime.Now;
                EventData.CreatedBy = EventManagementViewModel.CreatedBy;
                _Context.EventManagements.Add(EventData);
            }
            else
            {
                EventData.UpdatedOn = DateTime.Now;
                EventData.UpdatedBy = EventManagementViewModel.UpdatedBy;
                _Context.EventManagements.Update(EventData);
            }

            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion


        //#region Get Event Count By Status For Dashboard (For Feature)
        //public Dictionary<string,int> EventCountByStatus()
        //{
        //    List<EventManagement>? EventManagement = _Context.EventManagements.ToList();
        //    Dictionary<string, int> Count = new Dictionary<string, int>();
        //    Count.Add("Upcoming", EventManagement.Count(p => p.EventTypeId == (int)EventStatusEnum.Upcoming));
        //    Count.Add("Ongoing", EventManagement.Count(p => p.EventTypeId == (int)EventStatusEnum.Ongoing));
        //    Count.Add("Completed", EventManagement.Count(p => p.EventTypeId == (int)EventStatusEnum.Completed));
        //    Count.Add("Cancelled", EventManagement.Count(p => p.EventTypeId == (int)EventStatusEnum.Cancelled));
        //    return Count;
        //}
        //#endregion



        #region API Method's
        #region Get Event by UserID
        public List<EventManagementViewModel> GetEventByUserID(int UserId)
        {
            string fileReadPath = _config.GetSection("FilePath:FileReadPath").Value;
            string relativePath = GlobalCode.PostMediaFile.Replace("\\", "/").Trim('/');
            List<EventManagementViewModel> EventList = new List<EventManagementViewModel>();

            EventManagement? Event = _Context.EventManagements.FirstOrDefault(p => p.CreatedBy == UserId);
            if (Event != null)
            {
                EventList = (from e in _Context.EventManagements
                             join c in _Context.Creatores on e.CreatorId equals c.Id
                             join m in _Context.Enums on e.EventTypeId equals m.Id
                             where m.EnumTypeId == (int)EnumTypes.EventType
                             join es in _Context.Enums on e.StatusId equals es.Id
                             where es.EnumTypeId == (int)EnumTypes.EventStatus
                             where e.CreatedBy == UserId
                             select new EventManagementViewModel
                             {
                                 Id = e.Id,
                                 Title = e.Title,
                                 Description = e.Description,
                                 CreatorId = e.CreatorId,
                                 CreatorName = c.FirstName + " " + c.LastName,
                                 EventTypeId = e.EventTypeId,
                                 EventTypeName = m.EnumValue,
                                 EventStartDateTime = e.EventStartDateTime,
                                 EventEndDateTime = e.EventEndDateTime,
                                 StatusId = e.StatusId,
                                 StatusName = es.EnumValue,
                                 Daterange = e.EventStartDateTime.ToString("yyyy-MM-dd HH:mm:ss") + " - " + e.EventEndDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 RecordingURL = e.RecordingUrl
                             }).ToList();
            }


            //var FiltredData = validateCreatore;
            //if (TypeId != 0)
            //{
            //    FiltredData = validateCreatore.Where(a => a.EventTypeId == TypeId).ToList();

            return EventList;

        }
        #endregion


        #region Delete Event By Id
        /// <summary>
        /// Delete multiple users
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteEventById(int id)
        {
            _Context.EventManagements.RemoveRange(_Context.EventManagements.Where(u => id == id).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion 
        #endregion

    }
}
