using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Marketing.CustomBinding
{
    public static class ClientCustomBinding
    {
        public enum UserFields
        {
            Email,
            FirstName,
            LastName,
            RoleName,
            Active
        }

        public static IQueryable<ClientViewModel> ApplyFiltering(this IQueryable<ClientViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<ClientViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<ClientViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<ClientViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                UserFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == UserFields.Email)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Email, group.Member);
                    }
                    else if (UserEnum == UserFields.FirstName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.FirstName, group.Member);
                    }
                    else if (UserEnum == UserFields.LastName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.LastName, group.Member);
                    }
                    
                    else if (UserEnum == UserFields.Active)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Active, group.Member);
                    }
                }
                else
                {
                    if (UserEnum == UserFields.Email)
                    {
                        selector = BuildGroup(o => o.Email, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.FirstName)
                    {
                        selector = BuildGroup(o => o.FirstName, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.LastName)
                    {
                        selector = BuildGroup(o => o.LastName, selector, group.Member);
                    }
                   
                    else if (UserEnum == UserFields.Active)
                    {
                        selector = BuildGroup(o => o.Active, selector, group.Member);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<ClientViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<ClientViewModel, T> groupSelector, Func<IEnumerable<ClientViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<ClientViewModel> group, Func<ClientViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<ClientViewModel> ApplyPaging(this IQueryable<ClientViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<ClientViewModel> ApplySorting(this IQueryable<ClientViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<ClientViewModel> AddSortExpression(IQueryable<ClientViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            UserFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case UserFields.Email:
                        data = data.OrderBy(order => order.Email);
                        break;
                    case UserFields.FirstName:
                        data = data.OrderBy(order => order.FirstName);
                        break;
                    case UserFields.LastName:
                        data = data.OrderBy(order => order.LastName);
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
                    case UserFields.Email:
                        data = data.OrderByDescending(order => order.Email);
                        break;
                    case UserFields.FirstName:
                        data = data.OrderByDescending(order => order.FirstName);
                        break;
                    case UserFields.LastName:
                        data = data.OrderByDescending(order => order.LastName);
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