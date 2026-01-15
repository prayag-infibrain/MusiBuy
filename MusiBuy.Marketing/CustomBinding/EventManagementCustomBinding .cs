using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Marketing.CustomBinding
{
    public static class EventManagementCustomBinding
    {
        public enum EventFields
        {
            Title,
            EventTypeName,
            CreatorName,
            EventStartDateTime,
            EventEndDateTime,
            StatusName
        }

        public static IQueryable<EventManagementViewModel> ApplyFiltering(this IQueryable<EventManagementViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<EventManagementViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<EventManagementViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<EventManagementViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                EventFields EventEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (EventEnum == EventFields.Title)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Title, group.Member);
                    }
                    else if (EventEnum == EventFields.EventTypeName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.EventTypeName, group.Member);
                    }
                    else if (EventEnum == EventFields.CreatorName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.CreatorName, group.Member);
                    }
                    else if (EventEnum == EventFields.EventStartDateTime)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.EventStartDateTime, group.Member);
                    }
                    else if (EventEnum == EventFields.EventEndDateTime)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.EventEndDateTime, group.Member);
                    }
                    else if (EventEnum == EventFields.StatusName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.StatusName, group.Member);
                    }
                }
                else
                {
                    if (EventEnum == EventFields.Title)
                    {
                        selector = BuildGroup(o => o.Title, selector, group.Member);
                    }
                    else if (EventEnum == EventFields.EventTypeName)
                    {
                        selector = BuildGroup(o => o.EventTypeName, selector, group.Member);
                    }
                    else if (EventEnum == EventFields.CreatorName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.CreatorName, group.Member);
                    }
                    else if (EventEnum == EventFields.EventStartDateTime)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.EventStartDateTime, group.Member);
                    }
                    else if (EventEnum == EventFields.EventEndDateTime)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.EventEndDateTime, group.Member);
                    }
                    else if (EventEnum == EventFields.StatusName)
                    {
                        selector = BuildGroup(o => o.StatusName, selector, group.Member);
                    }
                    
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<EventManagementViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<EventManagementViewModel, T> groupSelector, Func<IEnumerable<EventManagementViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
        {
            var tempSelector = selectorBuilder;

            return g => g.GroupBy(groupSelector)
                         .Select(c => new AggregateFunctionsGroup
                         {
                             Key = c.Key,
                             Member = Value,
                             HasSubgroups = true,
                             Items = tempSelector.Invoke(c).ToList()
                         });
        }

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<EventManagementViewModel> group, Func<EventManagementViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<EventManagementViewModel> ApplyPaging(this IQueryable<EventManagementViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<EventManagementViewModel> ApplySorting(this IQueryable<EventManagementViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
        {
            if (groupDescriptors.Any())
            {
                foreach (var groupDescriptor in groupDescriptors.Reverse())
                {
                    data = AddSortExpression(data, groupDescriptor.SortDirection, groupDescriptor.Member);
                }
            }

            if (sortDescriptors.Any())
            {
                foreach (SortDescriptor sortDescriptor in sortDescriptors)
                {
                    data = AddSortExpression(data, sortDescriptor.SortDirection, sortDescriptor.Member);
                }
            }
            return data;
        }

        private static IQueryable<EventManagementViewModel> AddSortExpression(IQueryable<EventManagementViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            EventFields EventEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (EventEnum)
                {       
                    case EventFields.Title:
                        data = data.OrderBy(order => order.Title);
                        break;
                    case EventFields.EventTypeName:
                        data = data.OrderBy(order => order.EventTypeName);
                        break;
                    case EventFields.CreatorName:
                        data = data.OrderBy(order => order.CreatorName);
                        break;
                    case EventFields.EventStartDateTime:
                        data = data.OrderBy(order => order.EventStartDateTime);
                        break;
                    case EventFields.EventEndDateTime:
                        data = data.OrderBy(order => order.EventEndDateTime);
                        break;
                    case EventFields.StatusName:
                        data = data.OrderBy(order => order.StatusName);
                        break;
                }
            }
            else
            {
                switch (EventEnum)
                {
                    case EventFields.Title:
                        data = data.OrderByDescending(order => order.Title);
                        break;
                    case EventFields.EventTypeName:
                        data = data.OrderByDescending(order => order.EventTypeName);
                        break;
                    case EventFields.CreatorName:
                        data = data.OrderBy(order => order.CreatorName);
                        break;
                    case EventFields.EventStartDateTime:
                        data = data.OrderBy(order => order.EventStartDateTime);
                        break;
                    case EventFields.EventEndDateTime:
                        data = data.OrderBy(order => order.EventEndDateTime);
                        break;
                    case EventFields.StatusName:
                        data = data.OrderBy(order => order.StatusName);
                        break;
                }
            }
            return data;
        }

        private static EventFields GetUserFieldEnum(string FieldValue)
        {
            return (EventFields)Enum.Parse(typeof(EventFields), FieldValue);
        }
    }
}