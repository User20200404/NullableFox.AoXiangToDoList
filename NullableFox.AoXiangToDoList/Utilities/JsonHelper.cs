using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Utilities
{
    internal static class JsonHelper
    {
        static JsonSerializerOptions defaultJSONOptions;
        static JsonSerializerOptions defaultIndentedJSONOptions;
        public static JsonSerializerOptions DefaultJSONOptions => defaultJSONOptions ??= GenerateDefaultJSONOptions();
        public static JsonSerializerOptions DefaultIndentedJSONOptions => defaultIndentedJSONOptions ??= new JsonSerializerOptions(DefaultJSONOptions) { WriteIndented = true };
        public static string ToJsonString<T>(this T obj, JsonSerializerOptions optionOverride = null)
        {
            return JsonSerializer.Serialize(obj, optionOverride ?? DefaultJSONOptions);
        }

        public static T ObjectFromJsonString<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, DefaultJSONOptions);
        }

        static JsonSerializerOptions GenerateDefaultJSONOptions()
        {
            return new JsonSerializerOptions
            {
                IncludeFields = true,
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = false,
                Converters = { new DateTimeJSONConverter(), new JsonStringEnumConverter()},
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }
    }

    class DateTimeJSONConverter : JsonConverter<DateTime>
    {
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // 将DateTime值转换为指定的格式字符串
            string dateTimeString = value.ToString("yyyy-MM-ddTHH:mm:ss");
            // 将字符串写入JSON输出
            writer.WriteStringValue(dateTimeString);
        }

        // 重写Read方法，实现自定义的反序列化逻辑
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // 从JSON输入读取字符串值
            string dateTimeString = reader.GetString();
            // 尝试将字符串值解析为DateTime，如果成功则返回，否则抛出异常
            if (DateTime.TryParseExact(dateTimeString, "yyyy-MM-ddTHH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime value))
            {
                return value;
            }
            else
            {
                throw new JsonException("Invalid date time format.");
            }
        }
    }
}
