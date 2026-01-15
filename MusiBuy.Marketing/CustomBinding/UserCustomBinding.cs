using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Marketing.CustomBinding
{
    public static class UserCustomBinding
    {
        public enum UserFields
        {
            Username,
            FirstName,
            LastName,
            RoleName,
            Active
        }

        public static IQueryable<UserViewModel> ApplyFiltering(this IQueryable<UserViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<UserViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<UserViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<UserViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                UserFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == UserFields.Username)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Username, group.Member);
                    }
                    else if (UserEnum == UserFields.FirstName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.FirstName, group.Member);
                    }
                    else if (UserEnum == UserFields.LastName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.LastName, group.Member);
                    }
                    else if (UserEnum == UserFields.RoleName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.RoleName, group.Member);
                    }
                    else if (UserEnum == UserFields.Active)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Active, group.Member);
                    }
                }
                else
                {
                    if (UserEnum == UserFields.Username)
                    {
                        selector = BuildGroup(o => o.Username, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.FirstName)
                    {
                        selector = BuildGroup(o => o.FirstName, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.LastName)
                    {
                        selector = BuildGroup(o => o.LastName, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.RoleName)
                    {
                        selector = BuildGroup(o => o.RoleName, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Active)
                    {
                        selector = BuildGroup(o => o.Active, selector, group.Member);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<UserViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<UserViewModel, T> groupSelector, Func<IEnumerable<UserViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<UserViewModel> group, Func<UserViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<UserViewModel> ApplyPaging(this IQueryable<UserViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<UserViewModel> ApplySorting(this IQueryable<UserViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<UserViewModel> AddSortExpression(IQueryable<UserViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            UserFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case UserFields.Username:
                        data = data.OrderBy(order => order.Username);
                        break;
                    case UserFields.FirstName:
                        data = data.OrderBy(order => order.FirstName);
                        break;
                    case UserFields.LastName:
                        data = data.OrderBy(order => order.LastName);
                        break;
                    case UserFields.RoleName:
                        data = data.OrderBy(order => order.RoleName);
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
                    case UserFields.Username:
                        data = data.OrderByDescending(order => order.Username);
                        break;
                    case UserFields.FirstName:
                        data = data.OrderByDescending(order => order.FirstName);
                        break;
                    case UserFields.LastName:
                        data = data.OrderByDescending(order => order.LastName);
                        break;
                    case UserFields.RoleName:
                        data = data.OrderByDescending(order => order.RoleName);
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