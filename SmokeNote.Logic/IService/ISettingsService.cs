using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Logic.IService
{
    public interface ISettingsService
    {
        /// <summary>
        /// 得到配置项实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetSetting<T>()
            where T : Models.SettingsBase;

        /// <summary>
        /// 保存配置项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settings"></param>
        void SaveSettings<T>(T settings)
            where T : Models.SettingsBase;
    }
}
