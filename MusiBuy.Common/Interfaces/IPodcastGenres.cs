using MusiBuy.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Interfaces
{
    public interface IPodcastGenres
    {
        IQueryable<PodcastGenresViewModel> GetPodcastGenresList(int userId, string searchValue);
        bool IsPodcastGenresExit(int Id, string PodcastGenresName);
        bool Save(PodcastGenresViewModel objmodel);
        PodcastGenresViewModel GetPodcastGenresByID(int ID);
        bool DeletePodcastGenres(int[] ids);
    }
}
