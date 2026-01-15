using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IRecipient
    {
        IEnumerable<RecipientViewModel> GetRecipientViewList(RecipientViewModel recipientViewModel);
        IQueryable<RecipientViewModel> GetRecipientList(int? templateID);
        bool SaveRecipient(RecipientViewModel recipientViewModel, int templateID, int userId);
        bool DeleteRecipient(int[] recipientID);
        RecipientViewModel GetRecipientById(int recipientID);
        int? GetTemplateIdByRecipientId(int recipientID);
        bool IsRecepientExists(RecipientViewModel recipientViewModel);
    }
}
