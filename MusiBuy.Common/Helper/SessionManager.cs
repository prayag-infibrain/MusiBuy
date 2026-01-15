using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusiBuy.Common.Helpers
{
    public class SessionManager
    {
        private static HttpContextAccessor _HttpContextAccessor = new HttpContextAccessor();

        public static void SetSession(string sessionName, object value)
        {
            _HttpContextAccessor.HttpContext.Session.SetObject(sessionName, value);
        }

        public static T GetSession<T>(string sessionName)
        {
            return _HttpContextAccessor.HttpContext.Session.GetObject<T>(sessionName);
        }

        public static object GetSession(string sessionName)
        {
            return _HttpContextAccessor.HttpContext.Session.GetObject<object>(sessionName);
        }

        public static void ClearSession()
        {
            _HttpContextAccessor.HttpContext.Session.Clear();
        }

        public static string GetIP()
        {
            return _HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
