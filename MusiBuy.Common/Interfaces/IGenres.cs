using MusiBuy.Common.DB;
using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IGenres
    {
        IQueryable<GenresViewModel> GetGenresList(string searchValue);
        bool IsGenresExists(int GenresID, string GenresName);
        GenresViewModel GetGenresDetailsByID(int GenresID);
        bool DeleteGenress(int[] ids);
        bool Save(GenresViewModel GenresViewModel);
        int GetGenresCount();
        List<DropDownBindViewModel> GetGenresDropDownList(bool IsActive = true);
    }
}
