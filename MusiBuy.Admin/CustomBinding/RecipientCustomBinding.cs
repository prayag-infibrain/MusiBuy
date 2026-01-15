using Kendo.Mvc;
using Kendo.Mvc.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MusiBuy.Common.Models;

namespace MusiBuy.Admin.CustomBinding
{
    public static class RecipientCustomBinding
    {
        public enum RecipientFields
        {
            FirstName,
            LastName,
            Email,
        }

        public static IQueryable<RecipientViewModel> ApplyFiltering(this IQueryable<RecipientViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<RecipientViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<RecipientViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<RecipientViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                RecipientFields FrontUserEnum = GetFrontUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (FrontUserEnum == RecipientFields.FirstName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.FirstName, group.Member);
                    }
                    else if (FrontUserEnum == RecipientFields.LastName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.LastName, group.Member);
                    }
                    else if (FrontUserEnum == RecipientFields.Email)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Email, group.Member);
                    }


                }
                else
                {
                    if (FrontUserEnum == RecipientFields.FirstName)
                    {
                        selector = BuildGroup(o => o.FirstName, selector, group.Member);
                    }
                    else if (FrontUserEnum == RecipientFields.LastName)
                    {
                        selector = BuildGroup(o => o.LastName, selector, group.Member);
                    }
                    else if (FrontUserEnum == RecipientFields.Email)
                    {
                        selector = BuildGroup(o => o.Email, selector, group.Member);
                    }


                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<RecipientViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<RecipientViewModel, T> groupSelector, Func<IEnumerable<RecipientViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<RecipientViewModel> group, Func<RecipientViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<RecipientViewModel> ApplyPaging(this IQueryable<RecipientViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<RecipientViewModel> ApplySorting(this IQueryable<RecipientViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<RecipientViewModel> AddSortExpression(IQueryable<RecipientViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            RecipientFields FrontUserEnum = GetFrontUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (FrontUserEnum)
                {
                    case RecipientFields.FirstName:
                        data = data.OrderBy(order => order.FirstName);
                        break;
                    case RecipientFields.LastName:
                        data = data.OrderBy(order => order.LastName);
                        break;
                    case RecipientFields.Email:
                        data = data.OrderBy(order => order.Email);
                        break;
                }
            }
            else
            {
                switch (FrontUserEnum)
                {
                    case RecipientFields.FirstName:
                        data = data.OrderByDescending(order => order.FirstName);
                        break;
                    case RecipientFields.LastName:
                        data = data.OrderByDescending(order => order.LastName);
                        break;
                    case RecipientFields.Email:
                        data = data.OrderByDescending(order => order.Email);
                        break;

                }
            }
            return data;
        }

        private static RecipientFields GetFrontUserFieldEnum(string FieldValue)
        {
            return (RecipientFields)Enum.Parse(typeof(RecipientFields), FieldValue);
        }
    }
}