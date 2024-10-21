using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_253505_Sniazhko.Tests.Serializer
{
    internal class TempSerializer : TempDataSerializer
    {
        public override IDictionary<string, object> Deserialize(byte[] unprotectedData)
        {
            if (unprotectedData == null || unprotectedData.Length == 0)
            {
                return new Dictionary<string, object>();
            }

            // Преобразуем байты в строку
            var jsonString = System.Text.Encoding.UTF8.GetString(unprotectedData);

            // Десериализуем JSON-строку в словарь
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonString);
        }

        public override byte[] Serialize(IDictionary<string, object> values)
        {
            if (values == null || values.Count == 0)
            {
                return Array.Empty<byte>();
            }

            // Сериализуем словарь в JSON-строку
            var jsonString = JsonConvert.SerializeObject(values);

            // Преобразуем строку в байты
            return System.Text.Encoding.UTF8.GetBytes(jsonString);
        }
    }
}
