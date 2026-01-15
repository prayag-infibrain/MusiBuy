using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
using static MusiBuy.Common.Models.API.PostAndEventViewModel;

namespace MusiBuy.Common.Interfaces
{
    public interface IPostManagement
    {
        IQueryable<PostManagementViewModel> GetPostManagementList(int userId, string searchValue);
        PostManagementViewModel GetPostManagementDetailsByID(int userID);
        bool DeletePost(int[] ids);
        bool Save(PostManagementViewModel userViewModel);        
        string? GetPostManagementMediaFile(int id);
        void RemovePostManagementMediaFile(int id);
        Dictionary<string, int> PostCountByType();
        List<DropDownBindViewModel> GetPostManagementDropDownList();
        List<DropDownBindViewModel> GetPostManagementDropDownListByCreatorId(int creatorId);


        #region API Method's

        List<PostManagementViewModel> GetPostManagementDetailsByUserID(int UserId, int TypeId);
        bool DeleteContentById(DeleteContent model);
        Task<List<HomeScreenContent>> GetHomeScreenData(int UserId);
        List<FrontUserViewModel> GetArtiestList(int UserId);
        HomeScreenContent GetPostManagementDetailsByIDAPI(int PostID);
        List<int> GetuserPrefIds(int UserId);
    }
        #endregion
}
