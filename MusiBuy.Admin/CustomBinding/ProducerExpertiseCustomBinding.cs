using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Admin.CustomBinding
{
    public static class ProducerExpertiseCustomBinding
    {
        public enum UserFields
        {
            ProducerExpertiseName,
            Description,
            SortOrder,
            Active
        }

        public static IQueryable<ProducerExpertiseViewModel> ApplyFiltering(this IQueryable<ProducerExpertiseViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<ProducerExpertiseViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<ProducerExpertiseViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<ProducerExpertiseViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                UserFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == UserFields.ProducerExpertiseName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.ProducerExpertiseName, group.Member);
                    }
                    else if (UserEnum == UserFields.Description)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Description, group.Member);
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
                    if (UserEnum == UserFields.ProducerExpertiseName)
                    {
                        selector = BuildGroup(o => o.ProducerExpertiseName, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Description)
                    {
                        selector = BuildGroup(o => o.Description, selector, group.Member);
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

        private static Func<IEnumerable<ProducerExpertiseViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<ProducerExpertiseViewModel, T> groupSelector, Func<IEnumerable<ProducerExpertiseViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<ProducerExpertiseViewModel> group, Func<ProducerExpertiseViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<ProducerExpertiseViewModel> ApplyPaging(this IQueryable<ProducerExpertiseViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<ProducerExpertiseViewModel> ApplySorting(this IQueryable<ProducerExpertiseViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<ProducerExpertiseViewModel> AddSortExpression(IQueryable<ProducerExpertiseViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            UserFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case UserFields.ProducerExpertiseName:
                        data = data.OrderBy(order => order.ProducerExpertiseName);
                        break;
                    case UserFields.Description:
                        data = data.OrderBy(order => order.Description);
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
                    case UserFields.ProducerExpertiseName:
                        data = data.OrderByDescending(order => order.ProducerExpertiseName);
                        break;
                    case UserFields.Description:
                        data = data.OrderByDescending(order => order.Description);
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