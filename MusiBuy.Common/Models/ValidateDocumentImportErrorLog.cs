using Microsoft.AspNetCore.Mvc.Rendering;


namespace MusiBuy.Common.Models
{
    public class ValidateDocumentImportErrorLog
    {
        public IEnumerable<DocumentImportErrorLog> lstDocumentImportErrorLogs { get; set; }
        public SelectList ImportedFiles { get; set; }
        public string FileName { get; set; }
    }

    public class DocumentImportErrorLog
    {

        public int ErrorLogID { get; set; }
        public string ColumnName { get; set; }
        public string FileName { get; set; }
        public string ErrorMessage { get; set; }
        public int? RecordNo { get; set; }
        public string RecordNoStr { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int ImportedFileID { get; set; }
        public string FilePath { get; set; }
        public IEnumerable<DocumentImportErrorLog> lstDocumentImportErrorLogs { get; set; }


    }
}

