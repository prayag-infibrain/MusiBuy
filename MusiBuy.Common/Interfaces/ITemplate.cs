using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface ITemplate
    {
        IQueryable<TemplateViewModel> GetTemplateList(string templateName);
        TemplateViewModel GetEmailTemplateByName(string templateName);
        TemplateViewModel GetTemplateByID(int templateID);
        string ToSentenceCase(string templateName);
        bool SaveTemplateData(TemplateViewModel templateViewModel);
        bool DeleteTemplate(int[] ids);
        string GetTemplateNameById(int templateID);
    }
}
