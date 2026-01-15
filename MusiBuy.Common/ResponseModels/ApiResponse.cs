using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.ResponseModels
{
    public class ApiResponse
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public object Data { get; set; }
        public ApiResponse(bool success, string messages, object result)
        {
            Status = success;
            Message = messages;
            Data = result;
        }
    }

    public class DeleteApiResponse
    {
        public bool Status { get; set; }
        public string? Message { get; set; }

        public DeleteApiResponse(bool success, string? message)
        {
            Status = success;
            Message = message;
        }
    }
}
