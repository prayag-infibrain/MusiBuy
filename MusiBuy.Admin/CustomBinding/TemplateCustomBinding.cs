using Kendo.Mvc;
using Kendo.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MusiBuy.Common.Models;

namespace MusiBuy.Admin.CustomBinding
{
    public static class TemplateCustomBinding
    {
        public enum TemplateFields
        {
            TemplateName,
            Active
        }

        public static IQueryable<TemplateViewModel> ApplyFiltering(this IQueryable<TemplateViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<TemplateViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<TemplateViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<TemplateViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                TemplateFields TemplateEnum = GetFrontUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (TemplateEnum == TemplateFields.TemplateName)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.TemplateName, group.Member);
                    }
                    else if (TemplateEnum == TemplateFields.Active)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Active, group.Member);
                    }
                }
                else
                {
                    if (TemplateEnum == TemplateFields.TemplateName)
                    {
                        selector = BuildGroup(o => o.TemplateName, selector, group.Member);
                    }
                    else if (TemplateEnum == TemplateFields.Active)
                    {
                        selector = BuildGroup(o => o.Active, selector, group.Member);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<TemplateViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<TemplateViewModel, T> groupSelector, Func<IEnumerable<TemplateViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<TemplateViewModel> group, Func<TemplateViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<TemplateViewModel> ApplyPaging(this IQueryable<TemplateViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<TemplateViewModel> ApplySorting(this IQueryable<TemplateViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<TemplateViewModel> AddSortExpression(IQueryable<TemplateViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            TemplateFields FrontUserEnum = GetFrontUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (FrontUserEnum)
                {
                    case TemplateFields.TemplateName:
                        data = data.OrderBy(order => order.TemplateName);
                        break;

                    case TemplateFields.Active:
                        data = data.OrderBy(order => order.Active);
                        break;
                }
            }
            else
            {
                switch (FrontUserEnum)
                {
                    case TemplateFields.TemplateName:
                        data = data.OrderByDescending(order => order.TemplateName);
                        break;

                    case TemplateFields.Active:
                        data = data.OrderByDescending(order => order.Active);
                        break;
                }
            }
            return data;
        }

        private static TemplateFields GetFrontUserFieldEnum(string FieldValue)
        {
            return (TemplateFields)Enum.Parse(typeof(TemplateFields), FieldValue);
        }
    }
}