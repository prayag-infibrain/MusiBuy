using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IContentManagement
    {
        bool Save(ContentManagementsViewModel validateCMS);
        ContentManagementsViewModel GetContentManagementsContentByID(int pageId);

    }
}
