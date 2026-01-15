using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Marketing.CustomBinding
{
    public static class InsuranceMasterCustomBinding
    {
        public enum RoleFields
        {
            ProductName,
            InsuredName,
            PolicyNumber,
            StartDate,
            PremiumAmount,
            Active
        }

        public static IQueryable<InsuranceMasterViewModel> ApplyFiltering(this IQueryable<InsuranceMasterViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<InsuranceMasterViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<InsuranceMasterViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<InsuranceMasterViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                RoleFields RoleEnum = GetRoleFieldEnum(group.Member);
                if (selector == null)
                {
                    if (RoleEnum == RoleFields.ProductName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.ProductName, group.Member);
                    }
                    else if (RoleEnum == RoleFields.Active)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.InsuredName, group.Member);
                    }
                    else if (RoleEnum == RoleFields.PolicyNumber)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.PolicyNumber, group.Member);
                    }
                    else if (RoleEnum == RoleFields.PremiumAmount)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.PremiumAmount, group.Member);
                    }
                    else if (RoleEnum == RoleFields.StartDate)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.StartDate, group.Member);
                    }
                }
                else
                {
                    if (RoleEnum == RoleFields.ProductName)
                    {
                        selector = BuildGroup(o => o.ProductName, selector, group.Member);
                    }
                    else if (RoleEnum == RoleFields.InsuredName)
                    {
                        selector = BuildGroup(o => o.InsuredName, selector, group.Member);
                    }
                    else if (RoleEnum == RoleFields.PolicyNumber)
                    {
                        selector = BuildGroup(o => o.PolicyNumber, selector, group.Member);
                    }
                    else if (RoleEnum == RoleFields.PremiumAmount)
                    {
                        selector = BuildGroup(o => o.PremiumAmount, selector, group.Member);
                    }
                    else if (RoleEnum == RoleFields.Active)
                    {
                        selector = BuildGroup(o => o.Active, selector, group.Member);
                    }
                    else if (RoleEnum == RoleFields.StartDate)
                    {
                        selector = BuildGroup(o => o.StartDate, selector, group.Member);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<InsuranceMasterViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<InsuranceMasterViewModel, T> groupSelector, Func<IEnumerable<InsuranceMasterViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<InsuranceMasterViewModel> group, Func<InsuranceMasterViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<InsuranceMasterViewModel> ApplyPaging(this IQueryable<InsuranceMasterViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<InsuranceMasterViewModel> ApplySorting(this IQueryable<InsuranceMasterViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<InsuranceMasterViewModel> AddSortExpression(IQueryable<InsuranceMasterViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberProductName)
        {
            RoleFields RoleEnum = GetRoleFieldEnum(memberProductName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (RoleEnum)
                {
                    case RoleFields.ProductName:
                        data = data.OrderBy(order => order.ProductName);
                        break;
                    case RoleFields.Active:
                        data = data.OrderBy(order => order.Active);
                        break;
                    case RoleFields.InsuredName:
                        data = data.OrderBy(order => order.InsuredName);
                        break;
                    case RoleFields.PolicyNumber:
                        data = data.OrderBy(order => order.PolicyNumber);
                        break;
                    case RoleFields.PremiumAmount:
                        data = data.OrderBy(order => order.PremiumAmount);
                        break;
                    case RoleFields.StartDate:
                        data = data.OrderBy(order => order.StartDate);
                        break;
                }
            }
            else
            {
                switch (RoleEnum)
                {
                    case RoleFields.ProductName:
                        data = data.OrderByDescending(order => order.ProductName);
                        break;
                    case RoleFields.Active:
                        data = data.OrderByDescending(order => order.Active);
                        break;
                    case RoleFields.InsuredName:
                        data = data.OrderByDescending(order => order.InsuredName);
                        break;
                    case RoleFields.PolicyNumber:
                        data = data.OrderByDescending(order => order.PolicyNumber);
                        break;
                    case RoleFields.PremiumAmount:
                        data = data.OrderByDescending(order => order.PremiumAmount);
                        break;
                    case RoleFields.StartDate:
                        data = data.OrderByDescending(order => order.StartDate);
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