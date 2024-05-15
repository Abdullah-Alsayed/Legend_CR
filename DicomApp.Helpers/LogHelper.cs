using log4net;
using System.Reflection;

namespace DicomApp.Helpers
{
    public class LogHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogException(string msg, string stackTrace)
        {
            //log.Error($"Exception Msg: {msg}");
            log.Error($"Exception StackTrace: {stackTrace}");
        }

        public static void LogInfo(string msg)
        {
            log.Info(msg);
        }
    }
}
