using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Admin.CustomBinding
{
    public static class TransactionsHistoryCustomBinding
    {//hos valo ko kbr kya
        public enum HistoryFields
        {
            FirstName,
            LastName,
            EnumValue,
            Description,
            Status,
            Quantity
        }

        public static IQueryable<TransactionsHistory> ApplyFiltering(this IQueryable<TransactionsHistory> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<TransactionsHistory>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<TransactionsHistory> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<TransactionsHistory>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                HistoryFields EventEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (EventEnum == HistoryFields.FirstName || EventEnum == HistoryFields.LastName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Username, group.Member);
                    }
                    else if (EventEnum == HistoryFields.EnumValue)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.TokenTypeName, group.Member);
                    }
                    else if (EventEnum == HistoryFields.Status)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Status, group.Member);
                    }
                    else if (EventEnum == HistoryFields.Quantity)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Quantity, group.Member);
                    }
                }
                else
                {
                    if (EventEnum == HistoryFields.FirstName || EventEnum == HistoryFields.LastName)
                    {
                        selector = BuildGroup(o => o.Username, selector, group.Member);
                    }
                    else if (EventEnum == HistoryFields.EnumValue)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.TokenTypeName, group.Member);
                    }
                    else if (EventEnum == HistoryFields.Status)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Status, group.Member);
                    }
                    else if (EventEnum == HistoryFields.Quantity)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Quantity, group.Member);
                    }
                    
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<TransactionsHistory>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<TransactionsHistory, T> groupSelector, Func<IEnumerable<TransactionsHistory>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<TransactionsHistory> group, Func<TransactionsHistory, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<TransactionsHistory> ApplyPaging(this IQueryable<TransactionsHistory> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<TransactionsHistory> ApplySorting(this IQueryable<TransactionsHistory> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<TransactionsHistory> AddSortExpression(IQueryable<TransactionsHistory> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            HistoryFields EventEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (EventEnum)
                {    
                    case HistoryFields.FirstName:
                        data = data.OrderBy(order => order.Username);
                        break;
                    case HistoryFields.EnumValue:
                        data = data.OrderBy(order => order.TokenTypeName);
                        break;
                    case HistoryFields.Status:
                        data = data.OrderBy(order => order.Status);
                        break;
                    case HistoryFields.Quantity:
                        data = data.OrderBy(order => order.Quantity);
                        break;
                }
            }
            else
            {
                switch (EventEnum)
                {
                    case HistoryFields.FirstName:
                        data = data.OrderByDescending(order => order.Username);
                        break;
                    case HistoryFields.EnumValue:
                        data = data.OrderByDescending(order => order.TokenTypeName);
                        break;
                    case HistoryFields.Status:
                        data = data.OrderBy(order => order.Status);
                        break;
                    case HistoryFields.Quantity:
                        data = data.OrderBy(order => order.Quantity);
                        break;
                }
            }
            return data;
        }

        private static HistoryFields GetUserFieldEnum(string FieldValue)
        {
            return (HistoryFields)Enum.Parse(typeof(HistoryFields), FieldValue);
        }
    }
}