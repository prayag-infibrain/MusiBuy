using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Admin.CustomBinding
{
    public static class CustomerCustomBinding
    {
        public enum UserFields
        {
            FirstName,
            LastName,
            Email,
            Phone,
            ShareContactDetail,
            Active
        }

        public static IQueryable<CustomerViewModel> ApplyFiltering(this IQueryable<CustomerViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<CustomerViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<CustomerViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<CustomerViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                UserFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == UserFields.FirstName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.FirstName, group.Member);
                    }
                    else if (UserEnum == UserFields.LastName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.LastName, group.Member);
                    }
                    else if (UserEnum == UserFields.Email)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Email, group.Member);
                    }
                    else if (UserEnum == UserFields.Phone)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Phone, group.Member);
                    }
                    else if (UserEnum == UserFields.ShareContactDetail)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.ShareContactDetail, group.Member);
                    }
                    else if (UserEnum == UserFields.Active)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Active, group.Member);
                    }
                }
                else
                {
                    if (UserEnum == UserFields.FirstName)
                    {
                        selector = BuildGroup(o => o.FirstName, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.LastName)
                    {
                        selector = BuildGroup(o => o.LastName, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Email)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Email, group.Member);
                    }
                    else if (UserEnum == UserFields.Phone)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Phone, group.Member);
                    }
                    else if (UserEnum == UserFields.ShareContactDetail)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.ShareContactDetail, group.Member);
                    }
                    else if (UserEnum == UserFields.Active)
                    {
                        selector = BuildGroup(o => o.Active, selector, group.Member);
                    }
                    
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<CustomerViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<CustomerViewModel, T> groupSelector, Func<IEnumerable<CustomerViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<CustomerViewModel> group, Func<CustomerViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<CustomerViewModel> ApplyPaging(this IQueryable<CustomerViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<CustomerViewModel> ApplySorting(this IQueryable<CustomerViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<CustomerViewModel> AddSortExpression(IQueryable<CustomerViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            UserFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case UserFields.FirstName:
                        data = data.OrderBy(order => order.FirstName);
                        break;
                    case UserFields.LastName:
                        data = data.OrderBy(order => order.LastName);
                        break;
                    case UserFields.Email:
                        data = data.OrderBy(order => order.Email);
                        break;
                    case UserFields.Phone:
                        data = data.OrderBy(order => order.Phone);
                        break;
                    case UserFields.Active:
                        data = data.OrderBy(order => order.Active);
                        break;
                    case UserFields.ShareContactDetail:
                        data = data.OrderBy(order => order.ShareContactDetail);
                        break;
                }
            }
            else
            {
                switch (UserEnum)
                {
                    case UserFields.FirstName:
                        data = data.OrderByDescending(order => order.FirstName);
                        break;
                    case UserFields.LastName:
                        data = data.OrderByDescending(order => order.LastName);
                        break;
                    case UserFields.Email:
                        data = data.OrderBy(order => order.Email);
                        break;
                    case UserFields.Phone:
                        data = data.OrderBy(order => order.Phone);
                        break;
                    case UserFields.Active:
                        data = data.OrderBy(order => order.Active);
                        break;
                    case UserFields.ShareContactDetail:
                        data = data.OrderBy(order => order.ShareContactDetail);
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