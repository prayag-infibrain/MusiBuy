using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace MusiBuy.Common.Helpers
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.Set(key, Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(value)));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            session.TryGetValue(key, out byte[] value);
            var stringData = string.Empty;
            if (value != null)
            {
                stringData = Encoding.ASCII.GetString(value);
            }
            return string.IsNullOrWhiteSpace(stringData) ? default(T) : JsonConvert.DeserializeObject<T>(stringData);
        }
    }
}
