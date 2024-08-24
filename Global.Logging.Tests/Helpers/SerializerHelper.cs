using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Logging.Tests.Helpers
{
    internal static class SerializerHelper
    {
        public static byte[] SerializeToByteAray<T>(T entity) where T : class
        {
            var properties = entity.GetType().GetProperties();

            var propertyValues = new Dictionary<string, object?>();

            foreach (var property in properties)
            {
                string propertyName = property.Name;
                object? propertyValue = property.GetValue(entity);
                propertyValues.Add(propertyName, propertyValue);
            }

            return Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(propertyValues));
        }
    }
}
