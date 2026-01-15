using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models.API
{
    public class CommonModel
    {
        public int Id { get; set; }
    }

    public class GetByIdAndType
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
    }

    public class GetUserProfileDataById
    {
        public int Id { get; set; }
        public int createTypeId { get; set; }
        public int MediaTypeId { get; set; }
    }

    public class DeleteContent
    {
        public int Id { get; set; }
        public int ContentTypeId { get; set; }
    }

    public class GetByUserId
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }

    public class GetHomeScreenRecordCount
    {
        public int UserId { get; set; }
        public int DisplayRecordCount { get; set; }
        public string GetListByName { get; set; }
        public string Search { get; set; }
        public List<int> CreatorTypeId { get; set; }
        public List<int> ContentTypeId { get; set; }
    }
}
