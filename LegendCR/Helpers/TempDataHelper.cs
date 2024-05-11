//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using System.Text.Json;

//namespace LegendCR.Portal.Helpers
//{
//    public static class TempDataHelper
//    {
//        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
//        {
//            tempData[key] = JsonSerializer.Serialize(value);
//        }

//        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
//        {
//            object o;
//            tempData.TryGetValue(key, out o);
//            return o == null ? string.Empty : JsonSerializer.Deserialize<T>((string)o);
//        }
//    }
//}
