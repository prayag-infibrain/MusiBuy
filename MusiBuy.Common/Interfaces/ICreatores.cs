using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface ICreatores
    {
        IQueryable<CreatoresViewModel> GetCreatoresList(int userId, string searchValue);
        //bool IsCreatoresExists(int userID, string userName);
        bool IsEmailExists(int userID, string email);
        bool Save(CreatoresViewModel userViewModel);
        CreatoresViewModel GetCreatoresDetailsByID(int userID);
        CreatoresViewModel GetCreatoresDetailsEmail(string email, string userName, string mobileNo);
        bool DeleteCreatores(int[] ids);
        string? GetCreatoreProfilePicture(int id);
        void RemoveCreatoreProfilePicture(int id);
        int GetCreatoreCount();
        List<DropDownBindViewModel> GetCreatoreDropDownList(bool? isActive = null, bool? isAdminRoleExclude = null);


        #region  API Method For Login And Registor
        (CreatoresViewModel User, string Message) ValidateCreator(string EmailId, string Password);
        (CreatoresViewModel User, string Message) ValidateCreatorForForgotPassword(string EmailId);
        bool UpdateCreatorOPT(int UserId, int OTP);
        (bool Status, string Message) VeryfyOTPByEmail(string Email, int OTP);
        (bool Status, string Message) UpdatePasswordByEmail(string Email, string Passrowd);
        #endregion
    }
}
