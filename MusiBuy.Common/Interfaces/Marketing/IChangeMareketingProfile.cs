using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces.Marketing
{
    public interface IChangeMareketingProfile
    {
        ChangeProfileViewModel GetUserByID(int userID);
        bool IsEmailExists(string email, int userId);
        bool UpdateProfile(ChangeProfileViewModel changeProfileViewModel);

        public ChangeProfileViewModel GetFrontUserByID(int userID);

        public bool IsEmailFrontUserExists(string email, int userId);
        public bool UpdateProfileFrontUser(ChangeProfileViewModel changeProfileViewModel);
    }
}
