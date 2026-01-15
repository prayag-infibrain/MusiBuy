using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;

namespace MusiBuy.Common.Repositories
{
    public class EnumRepository : IEnum
    {

        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        public readonly MusiBuyDB_Connection _Context;
        public EnumRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion


        #region Get enums 
        /// <summary>
        /// Get Enum
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns>It returns enums list</returns>
        public IQueryable<EnumViewModel> GetEnumsList(string enumValue)
        {
            List<string> enumList = new List<string>();
            var enumViewList = (from A in _Context.EnumTypes
                                join AD in _Context.Enums on A.Id equals AD.EnumTypeId
                                where (!string.IsNullOrWhiteSpace(enumValue) ? AD.EnumValue.Contains(enumValue) : true == true) || (!string.IsNullOrWhiteSpace(enumValue) ? A.EnumTypeName.Contains(enumValue) : true == true) ||
                          (!string.IsNullOrWhiteSpace(enumValue) ? (A.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(enumValue) : true)
                                select new EnumViewModel
                                {
                                    Id = AD.Id,
                                    EnumValue = AD.EnumValue,
                                    EnumTypeId = AD.EnumTypeId,
                                    EnumTypeName = A.EnumTypeName,
                                    Description = AD.Description,
                                    Active = AD.IsActive == true ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                                    IsActive = AD.IsActive ? AD.IsActive : false,
                                    ParentId = AD.ParantId.HasValue ? AD.ParantId.Value : 0,
                                    ParentType = AD.ParantId == 0 || AD.ParantId == null ? "None" : (from t in _Context.Enums join u in _Context.EnumTypes on t.EnumTypeId equals u.Id where t.Id == AD.ParantId select u.EnumTypeName).FirstOrDefault(),
                                    ParentTypeId = AD.ParantId == 0 || AD.ParantId == 0 ? 0 : (from t in _Context.Enums join u in _Context.EnumTypes on t.EnumTypeId equals u.Id where t.Id == AD.ParantId select u.Id).FirstOrDefault(),
                                    Parent = AD.ParantId == 0 || AD.ParantId == null ? "N/A" : (from t in _Context.Enums where t.Id == AD.ParantId select t.EnumValue).FirstOrDefault(),
                                    CreatedOn = AD.CreatedOn,
                                    IsDeleteable = AD.IsDeletable ? AD.IsDeletable : false,
                                    IsEditable = AD.IsEditable ? AD.IsEditable : false,
                                    IsRecordUsed = AD.EnumTypeId == (int)EnumTypes.PostStatus ? _Context.PostManagements.Any(p => p.StatusId == AD.Id) :
                                    AD.EnumTypeId == (int)EnumTypes.PostMediaType ? _Context.PostManagements.Any(p => p.TypeId == AD.Id) :
                                    AD.EnumTypeId == (int)EnumTypes.PostStatus ? _Context.PostManagements.Any(p => p.StatusId == AD.Id) :
                                    AD.EnumTypeId == (int)EnumTypes.CommentStatus ? _Context.CommentsManagements.Any(p => p.StatusId == AD.Id) :
                                    AD.EnumTypeId == (int)EnumTypes.EventType ? _Context.EventManagements.Any(p => p.EventTypeId == AD.Id) :
                                    AD.EnumTypeId == (int)EnumTypes.EventStatus ? _Context.EventManagements.Any(p => p.StatusId == AD.Id) : false
                                }).OrderBy(p => p.EnumValue).AsQueryable();
            return enumViewList;
        }
        #endregion

        #region Get enums by enum type
        /// <summary>
        /// Get enums by enum type
        /// </summary>
        /// <param name="EnumTypeId"></param>
        /// <returns>It returns parent value by enum type id</returns>
        public List<EnumViewModel> GetEnumsListByType(int EnumTypeId)
        {
            var enumViewList = (from A in _Context.EnumTypes
                                join AD in _Context.Enums on A.Id equals AD.EnumTypeId
                                where
                                A.IsActive == true && AD.IsActive == true && (EnumTypeId == 0 || A.Id == EnumTypeId)
                                select new EnumViewModel
                                {
                                    Id = AD.Id,
                                    EnumValue = AD.EnumValue,
                                }).OrderBy(p => p.EnumValue).ToList();
            return enumViewList;
        }
        #endregion

        #region For Save Changes
        /// <summary>
        /// Save Changes
        /// </summary>
        /// <returns>If Enum is saved successfully it returns true other wise it returns false.</returns>
        public bool SaveChanges(EnumViewModel objEnumViewModel)
        {
            DB.Enum objTblEnum = new DB.Enum();
            if (objEnumViewModel.Id > 0)
            {
                objTblEnum = _Context.Enums.Where(e => e.Id == objEnumViewModel.Id).FirstOrDefault() ?? new DB.Enum();
            }

            objTblEnum.EnumTypeId = objEnumViewModel.EnumTypeId;
            objTblEnum.EnumValue = objEnumViewModel.EnumValue.Trim();
            objTblEnum.Description = objEnumViewModel.Description;
            objTblEnum.ParantId = objEnumViewModel.ParentId;
            objTblEnum.IsActive = objEnumViewModel.IsActive;



            if (objTblEnum.Id == 0)
            {
                objTblEnum.IsDeletable = true;
                objTblEnum.IsEditable = true;
                objTblEnum.CreatedBy = objEnumViewModel.CreatedBy;
                objTblEnum.CreatedOn = DateTime.UtcNow;
                _Context.Enums.Add(objTblEnum);
            }
            else
            {
                objTblEnum.UpdatedBy = objEnumViewModel.UpdatedBy;
                objTblEnum.UpdatedOn = DateTime.UtcNow;
            }

            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion

        #region Check Enum Duplication
        /// <summary>
        /// Check Enum duplicate value by Enum Type ID and Enum value
        /// </summary>
        /// <returns>int</returns>
        public int CheckEnumDuplication(int enumTypeID, string enumValue, int enumID)
        {
            return (from p in _Context.Enums where p.EnumTypeId == enumTypeID && p.EnumValue == enumValue && (enumID == 0 || p.Id != enumID) select p).Count();
        }
        #endregion

        #region Get Enum Details
        /// <summary>
        /// Get enum details
        /// </summary>
        /// <param name="enumID"></param>
        /// <returns>It returns enum details by enum id</returns>
        public EnumViewModel GetEnumDetailsByEnumId(int enumID)
        {
            EnumViewModel? objEnum = (from A in _Context.EnumTypes
                                      join AD in _Context.Enums on A.Id equals AD.EnumTypeId
                                      where AD.Id == enumID
                                      select new EnumViewModel
                                      {
                                          Id = AD.Id,
                                          EnumValue = AD.EnumValue,
                                          EnumTypeId = AD.EnumTypeId,
                                          EnumTypeName = A.EnumTypeName,
                                          Description = AD.Description,
                                          Active = AD.IsActive == true ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                                          IsActive = AD.IsActive ? AD.IsActive : false,
                                          ParentId = AD.ParantId,
                                          ParentType = AD.ParantId == 0 || AD.ParantId == null ? "None" : (from t in _Context.Enums join u in _Context.EnumTypes on t.EnumTypeId equals u.Id where t.Id == AD.ParantId select u.EnumTypeName).FirstOrDefault(),
                                          ParentTypeId = AD.ParantId == 0 || AD.ParantId == 0 ? 0 : (from t in _Context.Enums join u in _Context.EnumTypes on t.EnumTypeId equals u.Id where t.Id == AD.ParantId select u.Id).FirstOrDefault(),
                                          Parent = AD.ParantId == 0 || AD.ParantId == null ? "N/A" : (from t in _Context.Enums where t.Id == AD.ParantId select t.EnumValue).FirstOrDefault(),
                                          IsDeleteable = AD.IsDeletable ? AD.IsDeletable : false,
                                          IsEditable = AD.IsEditable ? AD.IsEditable : false,
                                      }).FirstOrDefault();
            return objEnum ?? new EnumViewModel();
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete enum
        /// </summary>
        /// <returns>void</returns>
        public void DeleteEnum(long[] chkDelete)
        {
            try
            {
                #region Check for referiantial integrity

                bool IsEnumInParantId = _Context.Enums.Any(y => chkDelete.Contains(y.ParantId.Value));
                if (IsEnumInParantId)
                {
                    Exception ex = new Exception("Is Enum in Use");
                    throw ex;
                }
                #endregion //END : Check for referiantial integrity
                _Context.Enums.RemoveRange(_Context.Enums.Where(x => chkDelete.Contains(x.Id)).AsEnumerable());
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Duplication Check
        /// <summary>
        /// Check Duplication exists by EnumTypeID, EnumValue and EnumID
        /// </summary>
        /// <returns>int</returns>
        public int CheckEnumExist(long? enumTypeID, string enumValue, long? enumID)
        {
            return (from p in _Context.Enums where p.EnumTypeId == enumTypeID && p.EnumValue == enumValue && (enumID == null || p.Id != enumID) select p).Count();
        }
        #endregion

        #region Duplication Check
        /// <summary>
        /// Check Enum Duplicate value by EnumTypeID, EnumValue and ParantId
        /// </summary>
        /// <returns>int</returns>
        public int CheckEnumDuplicationByParantId(int enumTypeID, string enumValue, int parentID, int enumID)
        {
            return (from p in _Context.Enums where p.EnumTypeId == enumTypeID && p.EnumValue == enumValue && p.ParantId == parentID && (enumID == 0 || p.Id != enumID) select p).Count();
        }
        #endregion

        #region Get Enum List By Parent Enum ID
        public List<DropDownBindViewModel> GetEnumList(int parentTypeID)
        {
            return _Context.Enums.Where(e => e.EnumTypeId == parentTypeID && e.IsActive == true).Select(e => new DropDownBindViewModel { value = e.Id, name = e.EnumValue }).ToList();
        }
        #endregion

        #region Get EnumType List
        public List<EnumTypeViewModel> GetEnumTypeList()
        {
            List<EnumTypeViewModel> EnumData = (from p in _Context.EnumTypes
                                                where p.IsActive == true
                                                select new EnumTypeViewModel
                                                {
                                                    Id = p.Id,
                                                    EnumTypeValue = p.EnumTypeName,
                                                }).OrderBy(p => p.EnumTypeValue).ToList();
            return EnumData;
        }
        #endregion

    }
}
