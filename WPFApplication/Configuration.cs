using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS.WPFJamme
{
    public class Configuration
    {
        /// <summary>
        /// This method reads app settings the local app.config file for the calling assembly/dll"/>
        /// </summary>
        /// <param name="section"></param>
        /// <param name="setting"></param>
        /// <param name="obj">valid return value if returned true</param>
        /// <returns>true if value exists</returns>
        public static bool GetDllConfigAppSetting(string setting, out object obj)
        {
            try
            {
                // The dllPath can't just use Assembly.GetExecutingAssembly().Location as ASP.NET doesn't copy the config to shadow copy path
                var dllPath = new Uri(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase).LocalPath;
                var dllConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(dllPath);

                // Get the appSettings section
                var appSettings = (System.Configuration.AppSettingsSection)dllConfig.GetSection("appSettings");
                System.Configuration.KeyValueConfigurationElement element = appSettings.Settings[setting];
                obj = element.Value;
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(string.Format("ConfigurationManager.GetDllConfigAppSetting(...) - error attempting to get value from dll 'appSettings' section , param:{0} - {1}", setting, ex.Message), ex);
            }
        }
    }
}
