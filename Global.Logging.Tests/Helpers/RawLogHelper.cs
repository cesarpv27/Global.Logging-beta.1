
namespace Global.Logging.Tests
{
    internal static class RawLogHelper
    {
        public static string CommonSuffix { get; set; } = "-1";

        public static string source => $"Source{CommonSuffix}";
        public static SeverityLevel severityLevel => SeverityLevel.Info;
        public static string message => $"Message{CommonSuffix}";
        public static string category => $"Category{CommonSuffix}";
        public static Dictionary<string, string> verbose => BuildDefaultVerbose();
        public static string callStack => $"CallStack{CommonSuffix}";
        public static string callStackMethod => $"CallStackMethod{CommonSuffix}";
        public static int callStackLine => 1;
        public static int callStackColumn => 1;
        public static Exception exception => new Exception(ExceptionMessage);

        #region Constants properties

        public static string VerboseKeyPrefix => "RawLog_Verbose_Key_";
        public static string VerboseMessagePrefix => "RawLog_Verbose_Message_";
        public static string ExceptionMessage => "RawLog_ExceptionMessage";

        #endregion

        public static Dictionary<string, string> BuildDefaultVerbose()
        {
            var verbose = new Dictionary<string, string>(10);
            for (int i = 0; i < 10; i++)
                verbose.Add(VerboseKeyPrefix + i, VerboseMessagePrefix + i);

            return verbose;
        }
    }
}
