using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Admin.CustomBinding
{
    public static class GenreCustomBinding
    {
        public enum PlanFields
        {
            GenreName,
            CountryName,
            Description
        }

        public static IQueryable<GenresViewModel> ApplyFiltering(this IQueryable<GenresViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<GenresViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<GenresViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<GenresViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                PlanFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == PlanFields.GenreName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.GenreName, group.Member);
                    }
                    else if (UserEnum == PlanFields.CountryName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.CountryName, group.Member);
                    }
                    else if (UserEnum == PlanFields.Description)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Description, group.Member);
                    }
                }
                else
                {
                    if (UserEnum == PlanFields.GenreName)
                    {
                        selector = BuildGroup(o => o.GenreName, selector, group.Member);
                    }
                    else if (UserEnum == PlanFields.CountryName)
                    {
                        selector = BuildGroup(o => o.CountryName, selector, group.Member);
                    }
                    else if (UserEnum == PlanFields.Description)
                    {
                        selector = BuildGroup(o => o.Description, selector, group.Member);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<GenresViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<GenresViewModel, T> groupSelector, Func<IEnumerable<GenresViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<GenresViewModel> group, Func<GenresViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<GenresViewModel> ApplyPaging(this IQueryable<GenresViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<GenresViewModel> ApplySorting(this IQueryable<GenresViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<GenresViewModel> AddSortExpression(IQueryable<GenresViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            PlanFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case PlanFields.GenreName:
                        data = data.OrderBy(order => order.GenreName);
                        break;
                    case PlanFields.CountryName:
                        data = data.OrderBy(order => order.CountryName);
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
                    case PlanFields.GenreName:
                        data = data.OrderByDescending(order => order.GenreName);
                        break;
                    case PlanFields.CountryName:
                        data = data.OrderByDescending(order => order.CountryName);
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