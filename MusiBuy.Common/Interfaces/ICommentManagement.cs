using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface ICommentManagement
    {
        IQueryable<CommentManagementViewModel> GetCommentManagementList(int CommentId, string searchValue, int? creatorId, int? postId);
        CommentManagementViewModel GetCommentManagementDetailsByID(int CommentID);
        bool DeleteComment(int[] ids);
        bool Save(CommentManagementViewModel CommentViewModel);
        bool UpdateStatus(int id, int statusId);
        //Dictionary<string, int> CommentCountByStatus();
    }
}
