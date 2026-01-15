using MusiBuy.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Interfaces
{
    public interface IMusicProducers
    {
        IQueryable<MusicProducersViewModel> GetMusicProducersList(int userId, string searchValue);
        bool IsMusicProducersExists(int userID, string userName);
        bool Save(MusicProducersViewModel objmodel);
        MusicProducersViewModel GetMusicProducersByID(int ID);
        bool DeleteMusicProducers(int[] ids);
    }
}
