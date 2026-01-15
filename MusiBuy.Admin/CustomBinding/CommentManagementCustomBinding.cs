using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Admin.CustomBinding
{
    public static class CommentManagementCustomBinding
    {//hos valo ko kbr kya
        public enum CommentFields
        {
            PostName,
            UserName,
            Comment,
            Timestamp,
            StatusName
        }

        public static IQueryable<CommentManagementViewModel> ApplyFiltering(this IQueryable<CommentManagementViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<CommentManagementViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<CommentManagementViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<CommentManagementViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                CommentFields EventEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (EventEnum == CommentFields.PostName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.PostName, group.Member);
                    }
                    else if (EventEnum == CommentFields.UserName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.UserName, group.Member);
                    }
                    else if (EventEnum == CommentFields.Comment)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Comment, group.Member);
                    }
                    else if (EventEnum == CommentFields.Timestamp)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Timestamp, group.Member);
                    }
                    else if (EventEnum == CommentFields.StatusName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.StatusName, group.Member);
                    }
                }
                else
                {
                    if (EventEnum == CommentFields.PostName)
                    {
                        selector = BuildGroup(o => o.PostName, selector, group.Member);
                    }
                    else if (EventEnum == CommentFields.UserName)
                    {
                        selector = BuildGroup(o => o.UserName, selector, group.Member);
                    }
                    else if (EventEnum == CommentFields.Comment)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Comment, group.Member);
                    }
                    else if (EventEnum == CommentFields.Timestamp)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Timestamp, group.Member);
                    }
                    else if (EventEnum == CommentFields.StatusName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.StatusName, group.Member);
                    }
                    
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<CommentManagementViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<CommentManagementViewModel, T> groupSelector, Func<IEnumerable<CommentManagementViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<CommentManagementViewModel> group, Func<CommentManagementViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<CommentManagementViewModel> ApplyPaging(this IQueryable<CommentManagementViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<CommentManagementViewModel> ApplySorting(this IQueryable<CommentManagementViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<CommentManagementViewModel> AddSortExpression(IQueryable<CommentManagementViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            CommentFields EventEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (EventEnum)
                {    
                    case CommentFields.PostName:
                        data = data.OrderBy(order => order.PostName);
                        break;
                    case CommentFields.UserName:
                        data = data.OrderBy(order => order.UserName);
                        break;
                    case CommentFields.Comment:
                        data = data.OrderBy(order => order.Comment);
                        break;
                    case CommentFields.Timestamp:
                        data = data.OrderBy(order => order.Timestamp);
                        break;
                    case CommentFields.StatusName:
                        data = data.OrderBy(order => order.StatusName);
                        break;
                }
            }
            else
            {
                switch (EventEnum)
                {
                    case CommentFields.PostName:
                        data = data.OrderByDescending(order => order.PostName);
                        break;
                    case CommentFields.UserName:
                        data = data.OrderByDescending(order => order.UserName);
                        break;
                    case CommentFields.Comment:
                        data = data.OrderBy(order => order.Comment);
                        break;
                    case CommentFields.Timestamp:
                        data = data.OrderBy(order => order.Timestamp);
                        break;
                    case CommentFields.StatusName:
                        data = data.OrderBy(order => order.StatusName);
                        break;
                }
            }
            return data;
        }

        private static CommentFields GetUserFieldEnum(string FieldValue)
        {
            return (CommentFields)Enum.Parse(typeof(CommentFields), FieldValue);
        }
    }
}