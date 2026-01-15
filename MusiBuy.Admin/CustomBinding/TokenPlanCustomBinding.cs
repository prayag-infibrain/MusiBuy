using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Admin.CustomBinding
{
    public static class TokenPlanCustomBinding
    {
        public enum PlanFields
        {
            TokenTypeName,
            Price,
            Description
        }

        public static IQueryable<TokenPlanViewModel> ApplyFiltering(this IQueryable<TokenPlanViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<TokenPlanViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<TokenPlanViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<TokenPlanViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                PlanFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == PlanFields.TokenTypeName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.TokenTypeName, group.Member);
                    }
                    else if (UserEnum == PlanFields.Price)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Price, group.Member);
                    }
                    else if (UserEnum == PlanFields.Description)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Description, group.Member);
                    }
                }
                else
                {
                    if (UserEnum == PlanFields.TokenTypeName)
                    {
                        selector = BuildGroup(o => o.TokenTypeName, selector, group.Member);
                    }
                    else if (UserEnum == PlanFields.Price)
                    {
                        selector = BuildGroup(o => o.Price, selector, group.Member);
                    }
                    else if (UserEnum == PlanFields.Description)
                    {
                        selector = BuildGroup(o => o.Description, selector, group.Member);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<TokenPlanViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<TokenPlanViewModel, T> groupSelector, Func<IEnumerable<TokenPlanViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<TokenPlanViewModel> group, Func<TokenPlanViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<TokenPlanViewModel> ApplyPaging(this IQueryable<TokenPlanViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<TokenPlanViewModel> ApplySorting(this IQueryable<TokenPlanViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<TokenPlanViewModel> AddSortExpression(IQueryable<TokenPlanViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            PlanFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case PlanFields.TokenTypeName:
                        data = data.OrderBy(order => order.TokenTypeName);
                        break;
                    case PlanFields.Price:
                        data = data.OrderBy(order => order.Price);
                        break;
                    case PlanFields.Description:
                        data = data.OrderBy(order => order.Description);
                        break;
                }
            }
            else
            {
                switch (UserEnum)
                {
                    case PlanFields.TokenTypeName:
                        data = data.OrderByDescending(order => order.TokenTypeName);
                        break;
                    case PlanFields.Price:
                        data = data.OrderByDescending(order => order.Price);
                        break;
                    case PlanFields.Description:
                        data = data.OrderByDescending(order => order.Description);
                        break;
                }
            }
            return data;
        }

        private static PlanFields GetUserFieldEnum(string FieldValue)
        {
            return (PlanFields)Enum.Parse(typeof(PlanFields), FieldValue);
        }
    }
}