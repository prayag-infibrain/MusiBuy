using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Marketing.CustomBinding
{
    public static class ProteinsSummaryCustomBinding
    {
        public enum UserFields
        {
            AccessionID,
            Description,
            AccessiionName,
            GeneNames,


        }

        public static IQueryable<ProteinSummaryViewModel> ApplyFiltering(this IQueryable<ProteinSummaryViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<ProteinSummaryViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<ProteinSummaryViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<ProteinSummaryViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                UserFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == UserFields.AccessionID)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.AccessionID, group.Member);
                    }
                    else if (UserEnum == UserFields.Description)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Description, group.Member);
                    }
                    else if (UserEnum == UserFields.AccessiionName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.AccessiionName, group.Member);
                    }
                    else if (UserEnum == UserFields.GeneNames)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.GeneNames, group.Member);
                    }


                }
                else
                {
                    if (UserEnum == UserFields.AccessionID)
                    {
                        selector = BuildGroup(o => o.AccessionID, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Description)
                    {
                        selector = BuildGroup(o => o.Description, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.AccessiionName)
                    {
                        selector = BuildGroup(o => o.AccessiionName, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.GeneNames)
                    {
                        selector = BuildGroup(o => o.GeneNames, selector, group.Member);
                    }


                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<ProteinSummaryViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<ProteinSummaryViewModel, T> groupSelector, Func<IEnumerable<ProteinSummaryViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<ProteinSummaryViewModel> group, Func<ProteinSummaryViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<ProteinSummaryViewModel> ApplyPaging(this IQueryable<ProteinSummaryViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<ProteinSummaryViewModel> ApplySorting(this IQueryable<ProteinSummaryViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<ProteinSummaryViewModel> AddSortExpression(IQueryable<ProteinSummaryViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            UserFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case UserFields.AccessionID:
                        data = data.OrderBy(order => order.AccessionID);
                        break;
                    case UserFields.Description:
                        data = data.OrderBy(order => order.Description);
                        break;
                    case UserFields.AccessiionName:
                        data = data.OrderBy(order => order.AccessiionName);
                        break;
                    case UserFields.GeneNames:
                        data = data.OrderBy(order => order.GeneNames);
                        break;

                    default:
                        throw new ArgumentException("Invalid user field");
                }
            }
            else
            {
                switch (UserEnum)
                {
                    case UserFields.AccessionID:
                        data = data.OrderByDescending(order => order.AccessionID);
                        break;
                    case UserFields.Description:
                        data = data.OrderByDescending(order => order.Description);
                        break;
                    case UserFields.AccessiionName:
                        data = data.OrderByDescending(order => order.AccessiionName);
                        break; 
                    case UserFields.GeneNames:
                        data = data.OrderByDescending(order => order.GeneNames);
                        break;

                    default:
                        throw new ArgumentException("Invalid user field");
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