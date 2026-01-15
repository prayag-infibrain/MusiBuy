using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Marketing.CustomBinding
{
    public static class ProteinsCustomBinding
    {
        public enum UserFields
        {
            SubjectId,
            Accession,
            Description,
            Score,
            Coverage,
            Proteins,
            UniquePeptides,
            Peptides,
            Psms,
            Aas,
            MwkDa,
            CalcpI,

        }

        public static IQueryable<ProteinsDataViewModel> ApplyFiltering(this IQueryable<ProteinsDataViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<ProteinsDataViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<ProteinsDataViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<ProteinsDataViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                UserFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == UserFields.SubjectId)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.SubjectId, group.Member);
                    }
                    if (UserEnum == UserFields.Accession)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Accession, group.Member);
                    }
                    else if (UserEnum == UserFields.Description)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Description, group.Member);
                    }
                    else if (UserEnum == UserFields.Score)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Score, group.Member);
                    }
                    else if (UserEnum == UserFields.Coverage)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Coverage, group.Member);
                    }

                    else if (UserEnum == UserFields.Proteins)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Proteins, group.Member);
                    }
                    else if (UserEnum == UserFields.Peptides)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Peptides, group.Member);
                    }
                    else if (UserEnum == UserFields.Psms)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Psms, group.Member);
                    }
                    else if (UserEnum == UserFields.Aas)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Aas, group.Member);
                    }
                    else if (UserEnum == UserFields.MwkDa)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.MwkDa, group.Member);
                    }
                    else if (UserEnum == UserFields.CalcpI)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.CalcpI, group.Member);
                    }
                }
                else
                {
                    if (UserEnum == UserFields.SubjectId)
                    {
                        selector = BuildGroup(o => o.SubjectId, selector, group.Member);
                    }
                    if (UserEnum == UserFields.Accession)
                    {
                        selector = BuildGroup(o => o.Accession, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Description)
                    {
                        selector = BuildGroup(o => o.Description, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Score)
                    {
                        selector = BuildGroup(o => o.Score, selector, group.Member);
                    }

                    else if (UserEnum == UserFields.Proteins)
                    {
                        selector = BuildGroup(o => o.Proteins, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Coverage)
                    {
                        selector = BuildGroup(o => o.Coverage, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.UniquePeptides)
                    {
                        selector = BuildGroup(o => o.UniquePeptides, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Peptides)
                    {
                        selector = BuildGroup(o => o.Peptides, selector, group.Member);
                    }

                    else if (UserEnum == UserFields.Psms)
                    {
                        selector = BuildGroup(o => o.Psms, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Aas)
                    {
                        selector = BuildGroup(o => o.Aas, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.MwkDa)
                    {
                        selector = BuildGroup(o => o.MwkDa, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.CalcpI)
                    {
                        selector = BuildGroup(o => o.CalcpI, selector, group.Member);
                    }

                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<ProteinsDataViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<ProteinsDataViewModel, T> groupSelector, Func<IEnumerable<ProteinsDataViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<ProteinsDataViewModel> group, Func<ProteinsDataViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<ProteinsDataViewModel> ApplyPaging(this IQueryable<ProteinsDataViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<ProteinsDataViewModel> ApplySorting(this IQueryable<ProteinsDataViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<ProteinsDataViewModel> AddSortExpression(IQueryable<ProteinsDataViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            UserFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case UserFields.SubjectId:
                        data = data.OrderBy(order => order.SubjectId);
                        break;
                    case UserFields.Accession:
                        data = data.OrderBy(order => order.Accession);
                        break;
                    case UserFields.Description:
                        data = data.OrderBy(order => order.Description);
                        break;
                    case UserFields.Score:
                        data = data.OrderBy(order => order.Score);
                        break;
                    case UserFields.Proteins:
                        data = data.OrderBy(order => order.Proteins);
                        break;
                    case UserFields.Coverage:
                        data = data.OrderBy(order => order.Coverage);
                        break;
                    case UserFields.UniquePeptides:
                        data = data.OrderBy(order => order.UniquePeptides);
                        break;
                    case UserFields.Peptides:
                        data = data.OrderBy(order => order.Peptides);
                        break;
                    case UserFields.Psms:
                        data = data.OrderBy(order => order.Psms);
                        break;
                    case UserFields.Aas:
                        data = data.OrderBy(order => order.Aas);
                        break;
                    case UserFields.MwkDa:
                        data = data.OrderBy(order => order.MwkDa);
                        break;
                    case UserFields.CalcpI:
                        data = data.OrderBy(order => order.CalcpI);
                        break;
                    default:
                        throw new ArgumentException("Invalid user field");
                }
            }
            else
            {
                switch (UserEnum)
                {
                    case UserFields.SubjectId:
                        data = data.OrderByDescending(order => order.SubjectId);
                        break;
                    case UserFields.Accession:
                        data = data.OrderByDescending(order => order.Accession);
                        break;
                    case UserFields.Description:
                        data = data.OrderByDescending(order => order.Description);
                        break;
                    case UserFields.Score:
                        data = data.OrderByDescending(order => order.Score);
                        break;
                    case UserFields.Proteins:
                        data = data.OrderByDescending(order => order.Proteins);
                        break;
                    case UserFields.Coverage:
                        data = data.OrderByDescending(order => order.Coverage);
                        break;
                    case UserFields.UniquePeptides:
                        data = data.OrderByDescending(order => order.UniquePeptides);
                        break;
                    case UserFields.Peptides:
                        data = data.OrderByDescending(order => order.Peptides);
                        break;
                    case UserFields.Psms:
                        data = data.OrderByDescending(order => order.Psms);
                        break;
                    case UserFields.Aas:
                        data = data.OrderByDescending(order => order.Aas);
                        break;
                    case UserFields.MwkDa:
                        data = data.OrderByDescending(order => order.MwkDa);
                        break;
                    case UserFields.CalcpI:
                        data = data.OrderByDescending(order => order.CalcpI);
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