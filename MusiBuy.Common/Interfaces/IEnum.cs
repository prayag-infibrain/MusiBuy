using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IEnum
    {
       
        IQueryable<EnumViewModel> GetEnumsList(string enumValue);
        bool SaveChanges(EnumViewModel objEnumViewModel);
        int CheckEnumDuplication(int enumTypeID, string enumValue, int enumID);
        EnumViewModel GetEnumDetailsByEnumId(int enumID);
        void DeleteEnum(long[] chkDelete);
        int CheckEnumExist(long? enumTypeID, string enumValue, long? enumID);
        int CheckEnumDuplicationByParantId(int enumTypeID, string enumValue, int parentID, int enumID);
        List<DropDownBindViewModel> GetEnumList(int parentTypeID);
        List<EnumViewModel> GetEnumsListByType(int EnumTypeId);
        List<EnumTypeViewModel> GetEnumTypeList();

    }
}