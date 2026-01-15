using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Admin.CustomBinding
{
    public static class CategoryMasterCustomBinding
    {
        public enum RoleFields
        {
            Name,
            Active
        }

        public static IQueryable<CategoryMasterViewModel> ApplyFiltering(this IQueryable<CategoryMasterViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<CategoryMasterViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<CategoryMasterViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<CategoryMasterViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                RoleFields RoleEnum = GetRoleFieldEnum(group.Member);
                if (selector == null)
                {
                    if (RoleEnum == RoleFields.Name)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Name, group.Member);
                    }
                    else if (RoleEnum == RoleFields.Active)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Active, group.Member);
                    }
                }
                else
                {
                    if (RoleEnum == RoleFields.Name)
                    {
                        selector = BuildGroup(o => o.Name, selector, group.Member);
                    }
                    else if (RoleEnum == RoleFields.Active)
                    {
                        selector = BuildGroup(o => o.Active, selector, group.Member);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<CategoryMasterViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<CategoryMasterViewModel, T> groupSelector, Func<IEnumerable<CategoryMasterViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<CategoryMasterViewModel> group, Func<CategoryMasterViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<CategoryMasterViewModel> ApplyPaging(this IQueryable<CategoryMasterViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<CategoryMasterViewModel> ApplySorting(this IQueryable<CategoryMasterViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<CategoryMasterViewModel> AddSortExpression(IQueryable<CategoryMasterViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            RoleFields RoleEnum = GetRoleFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (RoleEnum)
                {
                    case RoleFields.Name:
                        data = data.OrderBy(order => order.Name);
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
                    case RoleFields.Name:
                        data = data.OrderByDescending(order => order.Name);
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