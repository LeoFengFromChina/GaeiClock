using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmokeNote.Logic.IService;
using SmokeNote.Logic.Models;
using System.IO;
using Microsoft.Practices.Unity;

namespace SmokeNote.Logic.Services
{
    public class SettingsService : ISettingsService
    {
        private static readonly string directoryName = Environment.CurrentDirectory + @"\conf\";

        static SettingsService()
        {
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        public SettingsService(IUnityContainer unityContainer)
        {
            this.UnityContainer = unityContainer;
        }

        public IUnityContainer UnityContainer
        {
            get;
            private set;
        }

        public T GetSetting<T>() 
            where T : SettingsBase
        {
            try
            {
                var filePath = this.GetSettingsFilePath(typeof(T));
                return Helpers.XmlHelper.Deserialize<T>(filePath);
            }
            catch
            {
                var settings = this.UnityContainer.Resolve<T>();
                return settings;
            }
        }

        public void SaveSettings<T>(T settings) 
            where T : SettingsBase
        {
            var filePath = this.GetSettingsFilePath(settings.GetType());
            Helpers.XmlHelper.Serialize(filePath, settings);
        }

        private string GetSettingsFilePath(Type settingsType)
        {
            string path = string.Format(@"{0}{1}.config", directoryName, settingsType.Name);
            return path;
        }
    }
}
