using MusiBuy.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Interfaces
{
    public interface IProducerExpertise
    {
        IQueryable<ProducerExpertiseViewModel> GetProducerExpertiseList(int userId, string searchValue);
        bool IsProducerExpertiseExists(int Id, string PodcastGenresName);
        bool Save(ProducerExpertiseViewModel objmodel);
        ProducerExpertiseViewModel GeProducerExpertiseByID(int ID);
        bool DeleteProducerExpertise(int[] ids);
    }
}
