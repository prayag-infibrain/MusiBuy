using MusiBuy.Common.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace MusiBuy.Common.Common
{
    public static class GlobalCode
    {
        public static string DefaultAudioIcon = "images/DefaultImages/Audio-Icon.png";
        public static string DefaultVideoIcon = "images/DefaultImages/Video-Icon.png";
        public static string DefaultDocumentIcon = "images/DefaultImages/Document-Icon.png";
        
        public static string DefaultAudiobenner = "images/DefaultImages/Audio-benner.png";
        public static string DefaultVideobenner = "images/DefaultImages/Video-benner.png";
        public static string DefaultDocumentbenner = "images/DefaultImages/Document-benner.png";
        public static string DefaultLogo = "images/DefaultImages/logo.png";

        public static string ClientProfileImage = "appdata//ClientProfile//";
        public static string CreatorImages = "appdata//CreatorImages//";
        public static string FrontUserImages = "appdata//FrontUserImages//";
        public static string CustomerImages = "appdata//CustomerImages//";

        public static string PostMediaFile = "appdata\\PostMediaFile";

        public static string IncuranceDocument = "appdata//IncuranceDocument//";

        public static string EmployerLogoPath = "appdata//EmployerLogo//";
        public static string CompanyLogoPath = "appdata//CompanyLogo//";
        public static string UniversityLogoPath = "appdata//UniversityLogo//";
        public static string PSMMatrix = "appdata//PSMMatrix//";
        public static string DownloadDemoExcel = "appdata//DownloadDemoExcel//";
        public static string ProteinSummary = "appdata//ProteinSummary//";
        public static string ProteinData = "appdata//ProteinData//";
        public static string ClinicalData = "appdata//ClinicalData//";
        public static string EmployeeImagePath = "appdata//EmployeeProfile//";
        public static string EmployeeIDProofPath = "appdata//EmployeeIDProof//";
        public static string SupportTicketFilePath = "appdata//SupportTicketFile//";
        public static string CategoryPath = "appdata//Category//";
        public static string strEncryptionKey = "MusiBuy";
        public static string Currency = "$ ";
        public static int ImageSize = 25; //10mb
        public static string ActiveText = "Active";
        public static string InActiveText = "Inactive";

        public static string DeletedText = "<span class='text-danger'>Self-deleted</span>";
        public static string AccountDeletedText = "<span class='text-danger'>Account deleted</span>";

        public static string YesText = "Yes";
        public static string NoText = "No";

        public static string CityForceSolutionsFolderId = "1xObH8CIAgre-vBs4ppfI9VUgagBg0Erf";
        public static string CityForceSolutionsLocalFolderId = "1UdwUOCLV3RaFXhNgneXw9tugB4qp2HDx";
        public static string[] DocumentsTypes = new string[] { ".doc", ".Doc", ".docx", ".xls", ".Xls", ".xlsx", ".pdf", ".Pdf", ".ppt", ".Ppt", ".pptx", ".Txt", ".txt" };
        public static string[] AllowDocuments = new string[] { ".doc", ".Doc", ".docx", ".xls", ".Xls", ".xlsx", ".pdf", ".Pdf", ".ppt", ".Ppt", ".pptx", ".Txt", ".txt", ".jpg", ".jpeg", ".gif", ".png", ".bmp", ".tif", ".tiff", ".bmp" };
        public static string[] ImageTypes = new string[] { ".jpg", ".jpeg", ".gif", ".png", ".bmp", ".tif", ".tiff", ".bmp" };
        public static string[] AllowPdfFileExt = new[] { "pdf" };
        public static string[] AllowExcelFileExt = new[] {  ".xlsx", ".xls","csv" };
        public static string InfoCulture = "en-US";
        public static byte ButtonCount = 5;
        public static byte PageSize = 10;
        public static byte PageSize50 = 50;
        public static byte MobilePageSize = 15;
        public static int[] RecordPerPageList = new[] { 5, 10, 20, 50, 100, 200, 500 };
        public static string DefaultDateFormat = "MM/dd/yyyy";
        public static string DDMMYYFormat = "dd/MM/yyyy";
        public static string MMDDYY = "MM-dd-yy";
        public static string MMDDYYDash = "dd-MM-yyyy";
        public static string MMyyyyFormat = "MM/yyyy";
        public static string DDMMYYHHTTFormat = "dd/MM/yyyy hh:mm";
        public static string DDMMYYHHTTFormat1 = "dd/MM/yyyy HH:mm";
        public static string DDMMYYFormat36 = "yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'";
        public static string NewDDMMYYFormat = "dd MMM, yyyy";
        public static string LongDateFormat = "MMM dd, yyyy ";
        public static string GridDateFormat = "dddd dd MMM,yyyy";
        public static string DefaultTimeFormat = "HH:mm";
        public static string DefaultGridDateTimeFormat = "{0: dd/MM/yyyy HH:mm}";
        public static string DefaultGridDateFormat = "{0: dd/MM/yyyy}";
        public static string USALongDateFormat = "MMM/dd/yyyy";
        public static string USALongDateFormat1 = "ddd.dd MMMM";
        public static string USALongDateFormat2 = "ddd dd MMMM";
        public static string USALongDateTimeFormat = "MMM/dd/yyyy hh:mm tt";
        public static string DateTimeUsaFormat = "MMM dd, yyyy hh:mm tt";
        public static string ShortDateTimeUsaFormat = "MM-dd-yy HH:mm";
        public static string LongDateTimeFormat = "MMM dd, yyyy hh:mm tt";
        public static string TimeFormat = "HH:mm tt";
        public static string TimeFormat1 = "HH:mm";
        public static string TimeFormat2 = " HH:mm";
        public static string BookDateTime = "MMM dd, yyyy (hh:mm tt)";
        public static string ApiRequestDateFormat = "MM-dd-yyyy HH:mm";
        public static string ViewDateTime = "MMM dd, yyyy 'at' (hh:mm tt)";
        public static string DefaultDateTimeFormat = "dd/MM/yyyy HH:mm";
        public static string DefaultDateTimeFormatWith12 = "dd/MM/yyyy hh:mm tt";
        public static string DefaultYearWiseDateTimeFormat = "yyyy/M/d HH:mm";
        public static string DateTimePickerFormat = "yyyy/M/d";
        public static string DefaultUTCDateFormat = "yyyy-MM-dd HH:mm:ss.000Z";
        public static string StartDateFormatForEmail = "ddd dd MMM, hh:mm tt";
        public static string StartDateFormatForEmail1 = "dddd, hh:mm tt";
        public static string EndTimeFormat = "hh:mm tt";
        public static string StartTimeFormat = "hh:mm";
        public static string ShortDateFormat = "dd MMM";
        public static string StartDateFormatForGroupBooking = "dddd dd MMM, hh:mm tt";
        public static string DateMonth = "ddd dd MMM";
        public static string DateMonth1 = "ddd MMM dd yyyy HH:mm:ss";
        public static string FollowUpDateTimeFormat = "dddd dd MMMM";
        public static string ForMin = "mm";
        public static string FireBaseTimeFormat = "yyyy-MM-dd HH:mm:ss";
      
        public static string DefaultDateTimeFormatWith123 = "ddd dd MMM hh:mm tt";
        public static string SimpleDateFormat = "{0:ddd dd MMM HH:mm}";

        public static string decimalPointValue = "00.00";
        public static int wrBookingConflicts = 3;
        public static int otpExpiryInSeconds = 6000;
        public static int DefaultFrontSubjectId = 1;
        public static bool showQuickBooks = true;

        public static string UserFiles = "appdata//UserFiles//";
        public static string ProfileImagePath = "appdata//ProfileImage//";
        public static string DefaultCarImage = "featured-1.jpg";
        public static string DefaultUserProfileImage = "user.png";

        public static string foreignKeyReference = "The DELETE statement conflicted with the REFERENCE constraint";

        public static int AdminRoleID = 1;
        public static int AdminUserID = 1;
        public static int TherapistRole = 4;
        public static int ProfileImageSize = 1; //1mb

        public static string[] AllowImgFileExt = new[] { "jpg", "jpeg", "png", "bmp", "JPG", "JPEG", "PNG", "BMP" };
        public static string[] AllowAttechmentFileExt = new[] { "jpg", "jpeg", "png", "bmp", "doc", "docx", "xls", "xlsx", "odt", "ppt", "pptx", "pdf", "ods", "odp", "txt" };
        public static string[] AllowDocumentFileExt = new[] { "jpg", "jpeg", "png", "doc", "docx", "pdf" };

        public static string[] AllowAudioFileExt = new[] { "WAV", "AIFF", "FLAC", "AAC", "WMA", "M4A", "mp3"};
        public static string[] AllowVideoFileExt = new[] { "MP4", "AVI", "MKV", "MOV", "WMV", "FLV", "WEBM", "3GP", "MPEG", "M4V", "MPG" };
        public static string[] AllowTextFileExt = new[] { "TXT", "CSV", "LOG", "JSON", "XML", "MD", "INI", "YML"};

        public static int DefaultMessageCount = 30;
        public static int CacheTime = 1200; // minutes

        public static string OneSignal_APPID = "2de01b8b-70de-4897-a41b-4c1c188becf9";
        public static string OneSignal_RESTAPI = "NzdjN2E3YTMtYjM3Yi00MzA0LTk5YWYtYzkzOTZmMzcwODQw";

        #region Get Date 
        public static string GetDateDifference(DateTime? date)
        {
            DateTime currentDate = DateTime.UtcNow;
            var dateDifferece = currentDate - date;
            var totalDays = Math.Round(dateDifferece.Value.TotalDays);
            string dateInWord = string.Empty;

            if (totalDays == 1)
                dateInWord = "a day ago";
            else if (totalDays == 2)
                dateInWord = "two days ago";
            else if (totalDays == 3)
                dateInWord = "three days ago";
            else if (totalDays == 4)
                dateInWord = "four days ago";
            else if (totalDays == 5)
                dateInWord = "five days ago";
            else if (totalDays == 6)
                dateInWord = "six days ago";
            else if (totalDays > 6 && totalDays < 15)
                dateInWord = "a week ago";
            else if (totalDays > 15 && totalDays <= 31)
                dateInWord = "a month ago";
            else if (totalDays > 31 && totalDays <= 60)
                dateInWord = "two months ago";
            else if (totalDays > 61 && totalDays <= 90)
                dateInWord = "theee months ago";
            else if (totalDays > 91 && totalDays <= 120)
                dateInWord = "four months ago";
            else if (totalDays > 121 && totalDays <= 150)
                dateInWord = "five months ago";
            else if (totalDays > 151 && totalDays <= 180)
                dateInWord = "six months ago";
            else if (totalDays > 181 && totalDays <= 210)
                dateInWord = "seven months ago";
            else if (totalDays > 211 && totalDays <= 240)
                dateInWord = "eight months ago";
            else if (totalDays > 241 && totalDays <= 270)
                dateInWord = "nine months ago";
            else if (totalDays > 271 && totalDays <= 300)
                dateInWord = "ten months ago";
            else if (totalDays > 301 && totalDays <= 330)
                dateInWord = "eleven months ago";
            else if (totalDays > 331 && totalDays <= 365)
                dateInWord = "twelve months ago";
            else if (totalDays > 365 && totalDays <= 730)
                dateInWord = "a year ago";
            else
                dateInWord = "few years ago";

            return dateInWord;
        }

        #endregion

        #region Action Enum
        public enum Actions
        {
            Index,
            Create,
            Edit,
            Delete,
            Detail,
            Search
        }
        #endregion

        
        public static string RemoveSpecialCharacters(string input)
        {
            Regex reg = new Regex("[.$-*'\",_&#^@]");
            return reg.Replace(input, string.Empty);
        }

        #region Send Email
        public static async Task<bool> SendEmail(string recipients, string subject, string emailBody, CommonSettingViewModel commonSettingViewModel, Attachment? attachment = null)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(commonSettingViewModel.Email ?? string.Empty);
                    mailMessage.To.Add(recipients);
                    mailMessage.Subject = subject;
                    mailMessage.Body = emailBody;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Priority = MailPriority.High;

                    var port = commonSettingViewModel.Port;
                    using (SmtpClient smtpClient = new SmtpClient(commonSettingViewModel.SMTPServer, Convert.ToInt32(port)))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(commonSettingViewModel.Email, commonSettingViewModel.Password);
                        smtpClient.EnableSsl = commonSettingViewModel.IsSSL;
                        smtpClient.Send(mailMessage);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion


        #region Send Email
        public static bool SendEmail(string recipients, string subject, string emailBody, CommonSettingViewModel commonSettingViewModel)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(commonSettingViewModel.Email ?? string.Empty);
                    mailMessage.To.Add(recipients);
                    mailMessage.Subject = subject;
                    mailMessage.Body = emailBody;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Priority = MailPriority.High;

                    var port = commonSettingViewModel.Port;
                    using (SmtpClient smtpClient = new SmtpClient(commonSettingViewModel.SMTPServer, Convert.ToInt32(port)))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(commonSettingViewModel.Email, commonSettingViewModel.Password);
                        smtpClient.EnableSsl = commonSettingViewModel.IsSSL;
                        smtpClient.Send(mailMessage);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool SendEmailForContactUs(string FromMail,string ToMail, string subject, string emailBody, CommonSettingViewModel commonSettingViewModel)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    //mailMessage.From = new MailAddress(commonSettingViewModel.Email ?? string.Empty);
                    //mailMessage.To.Add(recipients);

                    mailMessage.From = new MailAddress(FromMail ?? string.Empty);
                    mailMessage.To.Add(new MailAddress(ToMail ?? string.Empty));
                    mailMessage.Subject = subject;
                    mailMessage.Body = emailBody;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Priority = MailPriority.High;

                    var port = commonSettingViewModel.Port;
                    using (SmtpClient smtpClient = new SmtpClient(commonSettingViewModel.SMTPServer, Convert.ToInt32(port)))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(commonSettingViewModel.Email, commonSettingViewModel.Password);
                        smtpClient.EnableSsl = commonSettingViewModel.IsSSL;
                        smtpClient.Send(mailMessage);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Convert List to Json string
        public static string ObjectToString(object data)
        {
            if (data != null)
            {
                return JsonConvert.SerializeObject(data);
            }
            return string.Empty;
        }
        #endregion

        #region Generate Random Code
        public static string RandomString(int length, bool? isNumberOnly = false, bool? hasCapitalOnly = false)
        {
            Random random = new Random();
            string output = "";
            string chars = (isNumberOnly.HasValue && isNumberOnly.Value) ? "0123456789" : ((hasCapitalOnly ?? false) ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" : "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

            output = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

            return output;
        }
        #endregion

        public static string GetEnumDescription(this System.Enum value)
        {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                // return description
                return attributes.Any() ? ((DescriptionAttribute)attributes.ElementAt(0)).Description : string.Empty;
            }
            else { return string.Empty; }
        }

        #region Convert string to base64
        public static string Base64Encode(string text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        #endregion

        #region Convert base64 to string
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        #endregion

        public static List<TEnum> GetEnumList<TEnum>() where TEnum : System.Enum => ((TEnum[])System.Enum.GetValues(typeof(TEnum))).ToList();

        public static List<T> GetListFromXML<T>(string tagName, string content)
        {
            var str = "<" + tagName + ">" + content + "</" + tagName + ">";
            var dataList = new List<T>();
            var serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(tagName));
            using (TextReader reader = new StringReader(str))
            {
                dataList = (List<T>)serializer.Deserialize(reader);
            }

            return dataList;
        }

        #region Write Exception
        public static void WriteExceptions(string cmdText = "", Exception ex = null, string filePath = "")
        {
            if (!System.IO.File.Exists(filePath))
            {
                using (System.IO.File.Create(filePath)) { };
            }
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("---------------------------------------New Log--------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());

                if (!(string.IsNullOrEmpty(cmdText)))
                    writer.WriteLine("Log: " + cmdText.ToString());

                while (ex != null)
                {
                    writer.WriteLine(ex.GetType().FullName);
                    writer.WriteLine("Message : " + ex.Message);
                    if (!(string.IsNullOrEmpty(ex.StackTrace)))
                        writer.WriteLine("StackTrace : " + ex.StackTrace);
                    if (ex.InnerException != null)
                        writer.WriteLine("InnerException : " + ex.InnerException.ToString());
                    if (!(string.IsNullOrEmpty(cmdText)))
                        writer.WriteLine("CommandText ErrorCommand: " + cmdText.ToString());
                    ex = ex.InnerException;
                }
                writer.WriteLine();
            }
        }
        #endregion

        public static string SendPushNotification(string PlayerID, string Title, string Message, string notificationDateTime = "", int notificationType = 0, int SupportSuggestionDetailId = 0)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("authorization", "Basic " + OneSignal_RESTAPI);
            string reqbody = string.Empty;
            reqbody = "{"
                            + "\"app_id\": \"" + OneSignal_APPID + "\","
                            + "\"contents\": {\"en\": \"" + Message + "\"},"
                            + "\"headings\": {\"en\": \"" + Title + "\"},"
                            + "\"data\": {\"type\": \"" + notificationType + "\", \"supportId\": \"" + SupportSuggestionDetailId + "\" },"
                            + "\"include_player_ids\": [\"" + PlayerID + "\"]}";
            byte[] byteArray = Encoding.UTF8.GetBytes(reqbody);
            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }
            return responseContent;
        }
        #region Importing file
        public static string WriteErrorLogFileForFileImport(List<string> errorList, string folderPath, string filename)
        {
            string newFileName = filename + "_" + DateTime.Now.Ticks + ".txt";
            string filePath = Path.Combine(folderPath, "wwwroot//appdata//FileImportErrorLog//" + newFileName);
            using (File.Create(filePath)) { };
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine("-----------------------------------------------------------------------------");
                foreach (var item in errorList)
                {
                    writer.WriteLine(item);
                }
            }

            return "/appdata/FileImportErrorLog/" + newFileName;
        }
        #endregion


        #region  true false to 1 0 

        public static string ConvertBool(bool val)
        {
            string result = "0";
            
            if(val ==true)
            {
                result = "1";
            }
            return result;

        }


        #endregion
    }
}