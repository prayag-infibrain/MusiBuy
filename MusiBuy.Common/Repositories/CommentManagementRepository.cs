
using Microsoft.Extensions.Configuration;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
namespace MusiBuy.Common.Repositories
{
    public class CommentManagementRepository : ICommentManagement
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;
        private readonly IConfiguration _config;

        public CommentManagementRepository(MusiBuyDB_Connection context, IConfiguration configuration)
        {
            this._Context = context;
            _config = configuration;
        }
        #endregion

        #region Get Comment data for grid
        /// <summary>
        /// Get Comment data for grid
        /// </summary>
        /// <param name="CommentId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns Comment list</returns>
        public IQueryable<CommentManagementViewModel> GetCommentManagementList(int CommentId, string searchValue, int? creatorId, int? postId)
        {
            var query = from c in _Context.CommentsManagements
                        join u in _Context.AdminUsers on c.UserId equals u.Id
                        join p in _Context.PostManagements on c.PostId equals p.Id
                        join crt in _Context.Creatores on p.CreatorId equals crt.Id
                        join cs in _Context.Enums on c.StatusId equals cs.Id
                        where cs.EnumTypeId == (int)EnumTypes.CommentStatus
                        select new { c, u, p, crt, cs };

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                query = query.Where(x => x.c.Comment.Contains(searchValue) || x.p.Title.Contains(searchValue) || x.u.Username.Contains(searchValue) || x.cs.EnumValue.Contains(searchValue));
            }
            if (creatorId.HasValue && creatorId > 0 && creatorId != null)
            {
                query = query.Where(x => x.crt.Id == creatorId.Value);
            }

            if (postId.HasValue && postId > 0 && postId != null)
            {
                query = query.Where(x => x.p.Id == postId.Value);
            }


            var result = query.Select(x => new CommentManagementViewModel
            {
                Id = x.c.Id,
                PostId = x.c.PostId,
                PostName = x.p.Title,
                UserId = x.c.UserId,
                UserName = x.u.Username,
                Comment = x.c.Comment,
                Timestamp = x.c.Timestamp,
                StatusId = x.c.StatusId,
                StatusName = x.cs.EnumValue,
                CreatorId = x.crt.Id
            }).OrderBy(x => x.UserName);

            return result;
        }
        #endregion

        #region Get Comment Details by CommentID
        /// <summary>
        /// Get single Comment by CommentID
        /// </summary>
        /// <returns>It returns Comment details by id </returns>
        public CommentManagementViewModel GetCommentManagementDetailsByID(int CommentID)
        {

            CommentManagementViewModel? validateComment = (from c in _Context.CommentsManagements
                                                           join u in _Context.AdminUsers on c.UserId equals u.Id
                                                           join p in _Context.PostManagements on c.PostId equals p.Id
                                                           join cs in _Context.Enums on c.StatusId equals cs.Id
                                                           where cs.EnumTypeId == (int)EnumTypes.CommentStatus
                                                           where c.Id == CommentID
                                                           select new CommentManagementViewModel
                                                           {
                                                               Id = c.Id,
                                                               PostId = c.PostId,
                                                               PostName = p.Title,
                                                               UserId = c.UserId,
                                                               UserName = u.Username,
                                                               Comment = c.Comment,
                                                               Timestamp = c.Timestamp,
                                                               StatusId = c.StatusId,
                                                               StatusName = cs.EnumValue
                                                           }).FirstOrDefault();
            return validateComment;
        }
        #endregion


        #region Delete multiple Comments
        /// <summary>
        /// Delete multiple Comments
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteComment(int[] ids)
        {
            _Context.CommentsManagements.RemoveRange(_Context.CommentsManagements.Where(u => ids.Contains(u.Id)).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion        

        #region Save Comment
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommentViewModel"></param>
        /// <returns></returns>
        public bool Save(CommentManagementViewModel CommentManagementViewModel)
        {
            var CommentData = new CommentsManagement();
            if (CommentManagementViewModel.Id > 0)
            {
                CommentData = _Context.CommentsManagements.FirstOrDefault(x => x.Id == CommentManagementViewModel.Id) ?? new CommentsManagement();
            }
            CommentData.PostId = CommentManagementViewModel.PostId;
            CommentData.UserId = CommentManagementViewModel.UserId;
            CommentData.UserType = CommentManagementViewModel.UserType;
            CommentData.Comment = CommentManagementViewModel.Comment;
            CommentData.Timestamp = CommentManagementViewModel.Timestamp;
            CommentData.StatusId = CommentManagementViewModel.StatusId;

            if (CommentData.Id == 0)
            {
                CommentData.CreatedOn = DateTime.Now;
                CommentData.CreatedBy = CommentManagementViewModel.CreatedBy;
                _Context.CommentsManagements.Add(CommentData);
            }
            else
            {
                CommentData.UpdatedOn = DateTime.Now;
                CommentData.UpdatedBy = CommentManagementViewModel.UpdatedBy;
                _Context.CommentsManagements.Update(CommentData);
            }

            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Change Comment Status
        public bool UpdateStatus(int id, int statusId)
        {
            try
            {
                var comment = _Context.CommentsManagements.FirstOrDefault(c => c.Id == id);
                if (comment != null)
                {
                    comment.StatusId = statusId;
                    if (_Context.SaveChanges() > 0)
                        return true;
                    else
                        return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion


        //#region Get Comment Count By Status For Dashboard (For Feature Id Need)
        //public Dictionary<string, int> CommentCountByStatus()
        //{
        //    List<CommentsManagement>? CommentManagement = _Context.CommentsManagements.ToList();
        //    Dictionary<string, int> Count = new Dictionary<string, int>();
        //    Count.Add("Upcoming", CommentManagement.Count(p => p.StatusId == (int)CommentStatusEnum.Approved));
        //    Count.Add("Ongoing", CommentManagement.Count(p => p.StatusId == (int)CommentStatusEnum.Pending));
        //    Count.Add("Completed", CommentManagement.Count(p => p.StatusId == (int)CommentStatusEnum.Rejected));
        //    //Count.Add("Cancelled", CommentManagement.Count(p => p.StatusId == (int)CommentStatusEnum.Delete));
        //    return Count;
        //}
        //#endregion

    }
}
