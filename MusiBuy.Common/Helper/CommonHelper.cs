using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models.API;
using MusiBuy.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Helper
{
    public class CommonHelper
    {
        public static string GetPostImagePath(IConfiguration _Config, int UserTypeId, int UserId)
        {
            string imagePath = _Config.GetSection("FilePath:FilePath").Value;  // e.g. C:\Projects\Uploads
            string imgUpload = Path.Combine(imagePath, "wwwroot", GlobalCode.PostMediaFile, UserTypeId.ToString(), UserId.ToString());

            if (!Directory.Exists(imgUpload))
                Directory.CreateDirectory(imgUpload);

            return imgUpload;
        }

        public static string GetPostImageReadPath(IConfiguration _Config, int UserTypeId, int UserId, string FileName)
        {
            string imagePath = _Config.GetSection("FilePath:FileReadPath").Value;  // e.g. C:\Projects\Uploads
            string MediaReadpath = Path.Combine(imagePath, GlobalCode.PostMediaFile, UserTypeId.ToString(), UserId.ToString());
            if (FileName == null)
                return null;
            string filePath = Path.Combine(MediaReadpath, FileName);
            //if (!File.Exists(filePath))
            //    return null;
            return filePath;
        }

        public static string GetDefaultImage(IConfiguration _Config,int MediaTypeId)
        {
            string imagePath = _Config.GetSection("FilePath:FileReadPath").Value;

            if (MediaTypeId == (int)PostMediaTypeEnum.Audio)
                return Path.Combine(imagePath, GlobalCode.DefaultAudiobenner);
            else if (MediaTypeId == (int)PostMediaTypeEnum.Video)
                return Path.Combine(imagePath, GlobalCode.DefaultVideobenner);
            else if (MediaTypeId == (int)PostMediaTypeEnum.Text)
                return Path.Combine(imagePath, GlobalCode.DefaultDocumentbenner);
            else
                return Path.Combine(imagePath, GlobalCode.DefaultLogo);
        }

        public static string GetDefaultIcon(IConfiguration _Config, int MediaTypeId)
        {
            string imagePath = _Config.GetSection("FilePath:FileReadPath").Value;

            if (MediaTypeId == (int)PostMediaTypeEnum.Audio)
                return Path.Combine(imagePath, GlobalCode.DefaultAudioIcon);
            else if (MediaTypeId == (int)PostMediaTypeEnum.Video)
                return Path.Combine(imagePath, GlobalCode.DefaultVideoIcon);
            else if (MediaTypeId == (int)PostMediaTypeEnum.Text)
                return Path.Combine(imagePath, GlobalCode.DefaultDocumentIcon);
            else
                return Path.Combine(imagePath, GlobalCode.DefaultLogo);
        }


        public static string GetMediaFileSize(IConfiguration _Config, int UserTypeId, int UserId, string FileName)
        {
            string imagePath = _Config.GetSection("FilePath:FileReadPath").Value;  // e.g. C:\Projects\Uploads
            string MediaReadpath = Path.Combine(imagePath, "wwwroot", GlobalCode.PostMediaFile, UserTypeId.ToString(), UserId.ToString());
            if (FileName == null)
                return "0 B";
            string filePath = Path.Combine(MediaReadpath, FileName);
            if (!File.Exists(filePath))
                return "0 B";

            FileInfo fileInfo = new FileInfo(filePath);
            long bytes = fileInfo.Length;

            if (bytes < 1024)
                return $"{bytes} B";

            double kb = bytes / 1024.0;
            if (kb < 1024)
                return $"{kb:0.##} KB";

            double mb = kb / 1024.0;
            if (mb < 1024)
                return $"{mb:0.##} MB";

            double gb = mb / 1024.0;
            return $"{gb:0.##} GB";
        }

        public static string GetEventStatusName(int EventStatusId)
        {
            if (EventStatusId == (int)EventStatusEnum.Upcoming)
                return "Upcoming";
            if (EventStatusId == (int)EventStatusEnum.Ongoing)
                return "Ongoing";
            if (EventStatusId == (int)EventStatusEnum.Completed)
                return "Completed";
            if (EventStatusId == (int)EventStatusEnum.Cancelled)
                return "Cancelled";
            else
                return "";
        }

        public static List<FrontUserViewModel> GetFollowers(MusiBuyDB_Connection _Context, int UserId,string FilePath)
        {
            var FollowersList = _Context.FollowersManagements.Include(a => a.FollowIng).ThenInclude(b => b.Role).Include(a => a.User).ThenInclude(b => b.Role).ToList();
            var Following = FollowersList.Where(a => a.FollowIngId == UserId).Select(folow => new FrontUserViewModel
            {
                Id = folow.User.Id,
                FirstName = folow.User.FirstName,
                LastName = folow.User.LastName,
                StrImage = folow.User.Image != null ? $"{FilePath}/{folow.User.Image}" : null,
                ImageName = folow.User.Image,
                RoleName = folow.User?.Role?.EnumValue,
            }).ToList();
            return Following;
        }

        public static List<FrontUserViewModel> GetFollowing(MusiBuyDB_Connection _Context, int UserId,string FilePath)
        {
            var FollowersList = _Context.FollowersManagements.Include(a => a.FollowIng).ThenInclude(b => b.Role).Include(a => a.User).ThenInclude(b => b.Role).ToList();
            var Following = FollowersList.Where(a => a.UserId == UserId).Select(folow => new FrontUserViewModel
            {
                Id = folow.FollowIng.Id,
                FirstName = folow.FollowIng.FirstName,
                LastName = folow.FollowIng.LastName,
                StrImage = folow.FollowIng.Image != null ? $"{FilePath}/{folow.FollowIng.Image}" : null,
                ImageName = folow.FollowIng.Image,
                RoleName = folow.FollowIng?.Role?.EnumValue,
            }).ToList();
            return Following;
        }


    }
}
