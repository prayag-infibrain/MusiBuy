using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Marketing.CustomBinding
{
    public static class RoleCustomBinding
    {
        public enum RoleFields
        {
            RoleName,
            Active
        }

        public static IQueryable<RoleViewModel> ApplyFiltering(this IQueryable<RoleViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<RoleViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<RoleViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<RoleViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                RoleFields RoleEnum = GetRoleFieldEnum(group.Member);
                if (selector == null)
                {
                    if (RoleEnum == RoleFields.RoleName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.RoleName, group.Member);
                    }
                    else if (RoleEnum == RoleFields.Active)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Active, group.Member);
                    }
                }
                else
                {
                    if (RoleEnum == RoleFields.RoleName)
                    {
                        selector = BuildGroup(o => o.RoleName, selector, group.Member);
                    }
                    else if (RoleEnum == RoleFields.Active)
                    {
                        selector = BuildGroup(o => o.Active, selector, group.Member);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<RoleViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<RoleViewModel, T> groupSelector, Func<IEnumerable<RoleViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<RoleViewModel> group, Func<RoleViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<RoleViewModel> ApplyPaging(this IQueryable<RoleViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<RoleViewModel> ApplySorting(this IQueryable<RoleViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<RoleViewModel> AddSortExpression(IQueryable<RoleViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            RoleFields RoleEnum = GetRoleFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (RoleEnum)
                {
                    case RoleFields.RoleName:
                        data = data.OrderBy(order => order.RoleName);
                        break;
                    case RoleFields.Active:
                        data = data.OrderBy(order => order.Active);
                        break;
                }
            }
            else
            {
                switch (RoleEnum)
                {
                    case RoleFields.RoleName:
                        data = data.OrderByDescending(order => order.RoleName);
                        break;
                    case RoleFields.Active:
                        data = data.OrderByDescending(order => order.Active);
                        break;
                }
            }
            return data;
        }

        private static RoleFields GetRoleFieldEnum(string FieldValue)
        {
            return (RoleFields)Enum.Parse(typeof(RoleFields), FieldValue);
        }
    }
}