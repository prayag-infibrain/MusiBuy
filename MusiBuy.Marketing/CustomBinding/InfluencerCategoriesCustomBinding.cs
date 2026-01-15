using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Marketing.CustomBinding
{
    public static class InfluencerCategoriesCustomBinding
    {
        public enum UserFields
        {
            InfluencerTypes,
            Criteria,
            EstimatedNumber,
            SortOrder,
            Active
        }

        public static IQueryable<InfluencerCategoriesViewModel> ApplyFiltering(this IQueryable<InfluencerCategoriesViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<InfluencerCategoriesViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<InfluencerCategoriesViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<InfluencerCategoriesViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                UserFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == UserFields.InfluencerTypes)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.InfluencerTypes, group.Member);
                    }
                    else if (UserEnum == UserFields.Criteria)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Criteria, group.Member);
                    }
                    else if (UserEnum == UserFields.EstimatedNumber)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.EstimatedNumber, group.Member);
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
                    if (UserEnum == UserFields.InfluencerTypes)
                    {
                        selector = BuildGroup(o => o.InfluencerTypes, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Criteria)
                    {
                        selector = BuildGroup(o => o.Criteria, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.EstimatedNumber)
                    {
                        selector = BuildGroup(o => o.EstimatedNumber, selector, group.Member);
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

        private static Func<IEnumerable<InfluencerCategoriesViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<InfluencerCategoriesViewModel, T> groupSelector, Func<IEnumerable<InfluencerCategoriesViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<InfluencerCategoriesViewModel> group, Func<InfluencerCategoriesViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<InfluencerCategoriesViewModel> ApplyPaging(this IQueryable<InfluencerCategoriesViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<InfluencerCategoriesViewModel> ApplySorting(this IQueryable<InfluencerCategoriesViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<InfluencerCategoriesViewModel> AddSortExpression(IQueryable<InfluencerCategoriesViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            UserFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case UserFields.InfluencerTypes:
                        data = data.OrderBy(order => order.InfluencerTypes);
                        break;
                    case UserFields.Criteria:
                        data = data.OrderBy(order => order.Criteria);
                        break;
                    case UserFields.EstimatedNumber:
                        data = data.OrderBy(order => order.EstimatedNumber);
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
                    case UserFields.InfluencerTypes:
                        data = data.OrderByDescending(order => order.InfluencerTypes);
                        break;
                    case UserFields.Criteria:
                        data = data.OrderByDescending(order => order.Criteria);
                        break;
                    case UserFields.EstimatedNumber:
                        data = data.OrderByDescending(order => order.EstimatedNumber);
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