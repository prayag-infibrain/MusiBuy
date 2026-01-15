using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Marketing.CustomBinding
{
    public static class PodcastGenresCustomBinding
    {
        public enum UserFields
        {
            PodcastGenresName,
            Description,
            ProvidersNumber,
            SortOrder,
            Active
        }

        public static IQueryable<PodcastGenresViewModel> ApplyFiltering(this IQueryable<PodcastGenresViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<PodcastGenresViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<PodcastGenresViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<PodcastGenresViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                UserFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == UserFields.PodcastGenresName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.PodcastGenresName, group.Member);
                    }
                    else if (UserEnum == UserFields.Description)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Description, group.Member);
                    }
                    else if (UserEnum == UserFields.ProvidersNumber)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.ProvidersNumber, group.Member);
                    }
                    else if (UserEnum == UserFields.SortOrder)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.SortOrder, group.Member);
                    }
                    else if (UserEnum == UserFields.Active)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Active, group.Member);
                    }
                }
                else
                {
                    if (UserEnum == UserFields.PodcastGenresName)
                    {
                        selector = BuildGroup(o => o.PodcastGenresName, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Description)
                    {
                        selector = BuildGroup(o => o.Description, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.ProvidersNumber)
                    {
                        selector = BuildGroup(o => o.ProvidersNumber, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.SortOrder)
                    {
                        selector = BuildGroup(o => o.SortOrder, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Active)
                    {
                        selector = BuildGroup(o => o.Active, selector, group.Member);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<PodcastGenresViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<PodcastGenresViewModel, T> groupSelector, Func<IEnumerable<PodcastGenresViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<PodcastGenresViewModel> group, Func<PodcastGenresViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<PodcastGenresViewModel> ApplyPaging(this IQueryable<PodcastGenresViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<PodcastGenresViewModel> ApplySorting(this IQueryable<PodcastGenresViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<PodcastGenresViewModel> AddSortExpression(IQueryable<PodcastGenresViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            UserFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case UserFields.PodcastGenresName:
                        data = data.OrderBy(order => order.PodcastGenresName);
                        break;
                    case UserFields.Description:
                        data = data.OrderBy(order => order.Description);
                        break;
                    case UserFields.ProvidersNumber:
                        data = data.OrderBy(order => order.ProvidersNumber);
                        break;
                    case UserFields.SortOrder:
                        data = data.OrderBy(order => order.SortOrder);
                        break;
                    case UserFields.Active:
                        data = data.OrderBy(order => order.Active);
                        break;
                }
            }
            else
            {
                switch (UserEnum)
                {
                    case UserFields.PodcastGenresName:
                        data = data.OrderByDescending(order => order.ProvidersNumber);
                        break;
                    case UserFields.Description:
                        data = data.OrderByDescending(order => order.Description);
                        break;
                    case UserFields.ProvidersNumber:
                        data = data.OrderByDescending(order => order.ProvidersNumber);
                        break;
                    case UserFields.SortOrder:
                        data = data.OrderByDescending(order => order.SortOrder);
                        break;
                    case UserFields.Active:
                        data = data.OrderByDescending(order => order.Active);
                        break;
                }
            }
            return data;
        }

        private static UserFields GetUserFieldEnum(string FieldValue)
        {
            return (UserFields)Enum.Parse(typeof(UserFields), FieldValue);
        }
    }
}