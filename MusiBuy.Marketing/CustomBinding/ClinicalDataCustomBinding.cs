using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using System.Collections;
using Kendo.Mvc.Infrastructure;
using MusiBuy.Common.Models;

namespace MusiBuy.Marketing.CustomBinding
{
    public static class ClinicalDataCustomBinding
    {
        public enum UserFields
        {
            SubjectID,
            SampleID,
            CollectionTime,
            Eye,
            MmOfSample,
            OSDIScore,
            TBUT,
            CornealStainingMid,
            CornealStainingTop,
            CornealStainingLeft,
            CornealStainingRight,
            CornealStainingBottom,
            ConjunctivalStainingLeft,
            ConjunctivalStainingLeftTop,
            ConjunctivalStainingLeftBottom,
            ConjunctivalStainingRight,
            ConjunctivalStainingRightTop,
            ConjunctivalStainingRightBottom,
            LidMarginScore,
            ExpressibilityScore,
            MeibumScore,
            Age,
            Sex,
            Race,
            Ethnicity,
            OcularDiseases,
            OcularMedications,
            OtherDiseases,
            OtherMedications,
            ArtificialTears,
            Xiidra,
            Restasis,
            Sequoa,
            TerviaNasalDrops,
            TopicalSteroids,
            Antihistamine,
            Prostaglandin,
            BetaBlocker,

        }

        public static IQueryable<ClinicalDataViewModel> ApplyFiltering(this IQueryable<ClinicalDataViewModel> data, IList<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                data = data.Where(ExpressionBuilder.Expression<ClinicalDataViewModel>(filterDescriptors));
            }
            return data;
        }

        public static IEnumerable ApplyGrouping(this IQueryable<ClinicalDataViewModel> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<ClinicalDataViewModel>, IEnumerable<AggregateFunctionsGroup>> selector = null;

            foreach (var group in groupDescriptors.Reverse())
            {
                UserFields UserEnum = GetUserFieldEnum(group.Member);
                if (selector == null)
                {
                    if (UserEnum == UserFields.SubjectID)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.SubjectID, group.Member);
                    }
                    else if (UserEnum == UserFields.SampleID)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.SampleID, group.Member);
                    }
                    else if (UserEnum == UserFields.CollectionTime)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.CollectionTime, group.Member);
                    }

                    else if (UserEnum == UserFields.Eye)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Eye, group.Member);
                    }
                    else if (UserEnum == UserFields.MmOfSample)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.MmOfSample, group.Member);
                    }
                    else if (UserEnum == UserFields.OSDIScore)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.OSDIScore, group.Member);
                    }
                    else if (UserEnum == UserFields.Ethnicity)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.Ethnicity, group.Member);
                    }
                    else if (UserEnum == UserFields.OcularDiseases)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.OcularDiseases, group.Member);
                    }
                    else if (UserEnum == UserFields.OcularMedications)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.OcularMedications, group.Member);
                    }
                    else if (UserEnum == UserFields.OtherDiseases)
                    {
                        selector = orders => BuildInnerGroup(orders, o => o.OtherDiseases, group.Member);
                    }
                   
                }
                else
                {
                    if (UserEnum == UserFields.SubjectID)
                    {
                        selector = BuildGroup(o => o.SubjectID, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.SampleID)
                    {
                        selector = BuildGroup(o => o.SampleID, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.CollectionTime)
                    {
                        selector = BuildGroup(o => o.CollectionTime, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Eye)
                    {
                        selector = BuildGroup(o => o.Eye, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.MmOfSample)
                    {
                        selector = BuildGroup(o => o.MmOfSample, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.OSDIScore)
                    {
                        selector = BuildGroup(o => o.OSDIScore, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.Ethnicity)
                    {
                        selector = BuildGroup(o => o.Ethnicity, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.OcularDiseases)
                    {
                        selector = BuildGroup(o => o.OcularDiseases, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.OcularMedications)
                    {
                        selector = BuildGroup(o => o.OcularMedications, selector, group.Member);
                    }
                    else if (UserEnum == UserFields.OtherDiseases)
                    {
                        selector = BuildGroup(o => o.OtherDiseases, selector, group.Member);
                    }
                   

                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<ClinicalDataViewModel>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<ClinicalDataViewModel, T> groupSelector, Func<IEnumerable<ClinicalDataViewModel>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder, string Value)
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

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<ClinicalDataViewModel> group, Func<ClinicalDataViewModel, T> groupSelector, string Value)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Member = Value,
                        Items = i.ToList()
                    });
        }

        public static IQueryable<ClinicalDataViewModel> ApplyPaging(this IQueryable<ClinicalDataViewModel> data, int currentPage, int pageSize)
        {
            if (pageSize > 0 && currentPage > 0)
            {
                data = data.Skip((currentPage - 1) * pageSize);
            }

            data = data.Take(pageSize);
            return data;
        }

        public static IQueryable<ClinicalDataViewModel> ApplySorting(this IQueryable<ClinicalDataViewModel> data, IList<GroupDescriptor> groupDescriptors, IList<SortDescriptor> sortDescriptors)
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

        private static IQueryable<ClinicalDataViewModel> AddSortExpression(IQueryable<ClinicalDataViewModel> data, Kendo.Mvc.ListSortDirection sortDirection, string memberName)
        {
            UserFields UserEnum = GetUserFieldEnum(memberName);
            if (sortDirection == Kendo.Mvc.ListSortDirection.Ascending)
            {
                switch (UserEnum)
                {
                    case UserFields.SubjectID:
                        data = data.OrderBy(order => order.SubjectID);
                        break;
                    case UserFields.SampleID:
                        data = data.OrderBy(order => order.SampleID);
                        break;
                    case UserFields.CollectionTime:
                        data = data.OrderBy(order => order.CollectionTime);
                        break;

                    case UserFields.Eye:
                        data = data.OrderBy(order => order.Eye);
                        break;
                    case UserFields.MmOfSample:
                        data = data.OrderBy(order => order.MmOfSample);
                        break;
                    case UserFields.OSDIScore:
                        data = data.OrderBy(order => order.OSDIScore);
                        break;
                }
            }
            else
            {
                switch (UserEnum)
                {
                    case UserFields.SubjectID:
                        data = data.OrderByDescending(order => order.SubjectID);
                        break;
                    case UserFields.SampleID:
                        data = data.OrderByDescending(order => order.SampleID);
                        break;
                    case UserFields.CollectionTime:
                        data = data.OrderByDescending(order => order.CollectionTime);
                        break;

                    case UserFields.Eye:
                        data = data.OrderByDescending(order => order.Eye);
                        break;
                    case UserFields.MmOfSample:
                        data = data.OrderByDescending(order => order.MmOfSample);
                        break;
                    case UserFields.OSDIScore:
                        data = data.OrderByDescending(order => order.OSDIScore);
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