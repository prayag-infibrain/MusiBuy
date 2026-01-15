using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MusiBuy.Common.Models;
using Kendo.Mvc;
using Kendo.Mvc.Infrastructure;

namespace MusiBuy.Marketing.CustomGridBinding
{
    public static class EnumCustomBinding
    {
        public static IQueryable<EnumViewModel> ApplyFiltering(this IQueryable<EnumViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<EnumViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<EnumViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<EnumViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;
            foreach (var group in groupDescriptors.Reverse())
            {
                if (selector == null)
                {
                    if (group.Member == "EnumTypeName")
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.EnumTypeName, group.Member);
                    }
                    else if (group.Member == "EnumValue")
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.EnumValue, group.Member);
                    }
                    else if (group.Member == "ParentType")
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.ParentType, group.Member);
                    }
                    else if (group.Member == "Parent")
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Parent, group.Member);
                    }
                    else if (group.Member == "Active")
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Active, group.Member);
                    }
                }
                else
                {
                    if (group.Member == "EnumTypeName")
                    {
                        selector = BuildGroup(o => o.EnumTypeName, selector, group.Member);
                    }
                    else if (group.Member == "EnumValue")
                    {
                        selector = BuildGroup(o => o.EnumValue, selector, group.Member);
                    }
                    else if (group.Member == "ParentType")
                    {
                        selector = BuildGroup(o => o.ParentType, selector, group.Member);
                    }
                    else if (group.Member == "Parent")
                    {
                        selector = BuildGroup(o => o.Parent, selector, group.Member);
                    }
                    else if (group.Member == "Active")
                    {
                        selector = BuildGroup(o => o.Active, selector, group.Member);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<EnumViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<EnumViewModel, T> groupSelector, Func<IEnumerable<EnumViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
        {
            var tempSelector = selectorBuilder;

            return g => g.GroupBy(groupSelector)
                         .Select(c => new AggregateFunctionsGroup
                         {
                             Key = c.Key,
                             HasSubgroups = true,
                             Member = Value,
                             Items = tempSelector.Invoke(c).ToList()
                         });
        }

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<EnumViewModel> group, Func<EnumViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<EnumViewModel> ApplyPaging(this IQueryable<EnumViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<EnumViewModel> ApplySorting(this IQueryable<EnumViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<EnumViewModel> AddSortExpression(IQueryable<EnumViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (memberName)
                {
                    case "EnumTypeName":
                        data = data.OrderBy(order => order.EnumTypeName);
                        break;
                    case "EnumValue":
                        data = data.OrderBy(order => order.EnumValue);
                        break;
                    case "ParentType":
                        data = data.OrderBy(order => order.ParentType);
                        break;
                    case "Parent":
                        data = data.OrderBy(order => order.Parent);
                        break;
                    case "Active":
                        data = data.OrderBy(order => order.Active);
                        break;
                }
            }
            else
            {
                switch (memberName)
                {
                    case "EnumTypeName":
                        data = data.OrderByDescending(order => order.EnumTypeName);
                        break;
                    case "EnumValue":
                        data = data.OrderByDescending(order => order.EnumValue);
                        break;
                    case "ParentType":
                        data = data.OrderByDescending(order => order.ParentType);
                        break;
                    case "Parent":
                        data = data.OrderByDescending(order => order.Parent);
                        break;
                    case "Active":
                        data = data.OrderByDescending(order => order.Active);
                        break;
                }
            }
            return data;
        }
    }
}