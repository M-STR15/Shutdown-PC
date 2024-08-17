using System;
namespace ShutdownPC.Models
{

    public static class BuildInfo
    {
        public static string VersionStr {get;set;}
        public static DateTime BuildDate = DateTime.Parse("2024-08-17 12:45:03");

        static BuildInfo()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null)
                VersionStr = string.Format("v. {0}.{1}.{2} | b. {3}", version.Major, version.Minor, version.Revision, BuildInfo.BuildDate.ToString("yyMMdd"));
        }
    }
}
