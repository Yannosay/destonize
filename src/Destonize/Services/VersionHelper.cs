using System;
using System.Reflection;

namespace Destonize.Services
{
    public static class VersionHelper
    {
        public static string GetVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return version != null ? version.ToString(3) : "0.0.0";
        }
    }
}