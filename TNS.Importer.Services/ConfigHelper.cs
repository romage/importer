using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS.Importer.Services
{
    public class ConfigHelper
    {
        public static T TryGetVal<T>(string key, string defaultVal = "Not Found")
        {
            var appSettings = ConfigurationManager.AppSettings;
            string result = appSettings[key] ?? defaultVal;
            T ret = (T)Convert.ChangeType(result, typeof(T));
            return ret;
        }

        public static string ToBeProcessedPath
        {
            get
            {
                return TryGetVal<string>("ToBeProcessedPath");
            }

        }
        public static string UnableToProcessPath
        {
            get
            {
                return TryGetVal<string>("UnableToProcess");
            }
        }
        public static string ProcessedPath
        {
            get
            {
                return TryGetVal<string>("ProcessedPath");
            }
        }
        public static string UploadFileRoot
        {
            get
            {
                return TryGetVal<string>("UploadFileRoot");
            }
        }



    }
}
