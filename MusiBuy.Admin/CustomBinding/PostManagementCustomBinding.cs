using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;
using System.Reflection.Metadata;

namespace MusiBuy.Admin.CustomBinding
{
    public static class PostManagementCustomBinding
    {
        public enum PostManagementFields
        {

            CreatorName,
            TypeName,
            Title,
            Description,
            Url,
            Tags,
            StatusName,
            PublishDate
        }

        public static IQueryable<PostManagementViewModel> ApplyFiltering(this IQueryable<PostManagementViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<PostManagementViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<PostManagementViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<PostManagementViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                PostManagementFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == PostManagementFields.CreatorName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.CreatorName, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.TypeName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.TypeName, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.Title)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Title, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.Description)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Description, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.Url)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Url, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.Tags)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Tags, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.StatusName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.StatusName, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.PublishDate)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.PublishDate, group.Member);
                    }
                    
                }
                else
                {
                    if (UserEnum == PostManagementFields.CreatorName)
                    {
                        selector = BuildGroup(o => o.CreatorName, selector, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.TypeName)
                    {
                        selector = BuildGroup(o => o.TypeName, selector, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.Title)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Title, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.Description)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Description, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.Url)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Url, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.Tags)
                    {
                        selector = BuildGroup(o => o.Tags, selector, group.Member);
                    }
                    else if (UserEnum == PostManagementFields.PublishDate)
                    {
                        selector = BuildGroup(o => o.PublishDate, selector, group.Member);
                    }

                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<PostManagementViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<PostManagementViewModel, T> groupSelector, Func<IEnumerable<PostManagementViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<PostManagementViewModel> group, Func<PostManagementViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<PostManagementViewModel> ApplyPaging(this IQueryable<PostManagementViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<PostManagementViewModel> ApplySorting(this IQueryable<PostManagementViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<PostManagementViewModel> AddSortExpression(IQueryable<PostManagementViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            PostManagementFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case PostManagementFields.CreatorName:
                        data = data.OrderBy(order => order.CreatorName);
                        break;
                    case PostManagementFields.TypeName:
                        data = data.OrderBy(order => order.TypeName);
                        break;
                    case PostManagementFields.Title:
                        data = data.OrderBy(order => order.Title);
                        break;
                    case PostManagementFields.Description:
                        data = data.OrderBy(order => order.Description);
                        break;
                    case PostManagementFields.Url:
                        data = data.OrderBy(order => order.Url);
                        break;
                    case PostManagementFields.Tags:
                        data = data.OrderBy(order => order.Tags);
                        break;
                    case PostManagementFields.StatusName:
                        data = data.OrderBy(order => order.StatusName);
                        break;
                    case PostManagementFields.PublishDate:
                        data = data.OrderBy(order => order.PublishDate);
                        break;
                }
            }
            else
            {
                switch (UserEnum)
                {
                    case PostManagementFields.CreatorName:
                        data = data.OrderByDescending(order => order.CreatorName);
                        break;
                    case PostManagementFields.TypeName:
                        data = data.OrderByDescending(order => order.TypeName);
                        break;
                    case PostManagementFields.Title:
                        data = data.OrderBy(order => order.Title);
                        break;
                    case PostManagementFields.Description:
                        data = data.OrderBy(order => order.Description);
                        break;
                    case PostManagementFields.Url:
                        data = data.OrderBy(order => order.Url);
                        break;
                    case PostManagementFields.Tags:
                        data = data.OrderBy(order => order.Tags);
                        break;
                    case PostManagementFields.StatusName:
                        data = data.OrderBy(order => order.StatusName);
                        break;
                    case PostManagementFields.PublishDate:
                        data = data.OrderBy(order => order.PublishDate);
                        break;
                }
            }
            return data;
        }

        private static PostManagementFields GetUserFieldEnum(string FieldValue)
        {
            return (PostManagementFields)Enum.Parse(typeof(PostManagementFields), FieldValue);
        }
    }
}