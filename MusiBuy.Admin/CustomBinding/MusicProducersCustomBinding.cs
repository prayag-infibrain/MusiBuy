using Kendo.Mvc;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;
using System.Collections;

namespace MusiBuy.Admin.CustomBinding
{
    public static class MusicProducersCustomBinding
    {
        public enum UserFields
        {
            ProducerType,
            PrimaryExpertise,
            GenresSpecialized,
            KeyContributions,
            SortOrder,
            Active
        }

        public static IQueryable<MusicProducersViewModel> ApplyFiltering(this IQueryable<MusicProducersViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<MusicProducersViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<MusicProducersViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<MusicProducersViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                UserFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == UserFields.ProducerType)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.ProducerType, group.Member);
                    }
                    else if (UserEnum == UserFields.PrimaryExpertise)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.PrimaryExpertise, group.Member);
                    }
                    else if (UserEnum == UserFields.GenresSpecialized)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.GenresSpecialized, group.Member);
                    }
                    else if (UserEnum == UserFields.KeyContributions)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.KeyContributions, group.Member);
                    }
                    else if (UserEnum == UserFields.SortOrder)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.SortOrder, group.Member);
                    }
                    else if (UserEnum == UserFields.Active)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Active, group.Member);
                    }
                }
                else
                {
                    if (UserEnum == UserFields.ProducerType)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.ProducerType, group.Member);
                    }
                    else if (UserEnum == UserFields.PrimaryExpertise)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.PrimaryExpertise, group.Member);
                    }
                    else if (UserEnum == UserFields.GenresSpecialized)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.GenresSpecialized, group.Member);
                    }
                    else if (UserEnum == UserFields.KeyContributions)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.KeyContributions, group.Member);
                    }
                    else if (UserEnum == UserFields.SortOrder)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.SortOrder, group.Member);
                    }
                    else if (UserEnum == UserFields.Active)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Active, group.Member);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<MusicProducersViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<MusicProducersViewModel, T> groupSelector, Func<IEnumerable<MusicProducersViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<MusicProducersViewModel> group, Func<MusicProducersViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<MusicProducersViewModel> ApplyPaging(this IQueryable<MusicProducersViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<MusicProducersViewModel> ApplySorting(this IQueryable<MusicProducersViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<MusicProducersViewModel> AddSortExpression(IQueryable<MusicProducersViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            UserFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case UserFields.ProducerType:
                        data = data.OrderBy(order => order.ProducerType);
                        break;
                    case UserFields.PrimaryExpertise:
                        data = data.OrderBy(order => order.PrimaryExpertise);
                        break;
                    case UserFields.GenresSpecialized:
                        data = data.OrderBy(order => order.GenresSpecialized);
                        break;
                    case UserFields.KeyContributions:
                        data = data.OrderBy(order => order.KeyContributions);
                        break;
                    case UserFields.SortOrder:
                        data = data.OrderBy(order => order.SortOrder);
                        break;
                    case UserFields.Active:
                        data = data.OrderBy(order => order.Active);
                        break;
                }
            }
            else
            {
                switch (UserEnum)
                {
                    case UserFields.ProducerType:
                        data = data.OrderBy(order => order.ProducerType);
                        break;
                    case UserFields.PrimaryExpertise:
                        data = data.OrderBy(order => order.PrimaryExpertise);
                        break;
                    case UserFields.GenresSpecialized:
                        data = data.OrderBy(order => order.GenresSpecialized);
                        break;
                    case UserFields.KeyContributions:
                        data = data.OrderBy(order => order.KeyContributions);
                        break;
                    case UserFields.SortOrder:
                        data = data.OrderBy(order => order.SortOrder);
                        break;
                    case UserFields.Active:
                        data = data.OrderBy(order => order.Active);
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
