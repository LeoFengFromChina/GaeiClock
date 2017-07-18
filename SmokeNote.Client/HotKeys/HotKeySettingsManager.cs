using System.Collections.ObjectModel;

namespace GlobalHotKeyDemo
{
    /// <summary>
    /// 快捷键设置管理器
    /// </summary>
    public class HotKeySettingsManager
    {
        private static HotKeySettingsManager m_Instance;
        /// <summary>
        /// 单例实例
        /// </summary>
        public static HotKeySettingsManager Instance
        {
            get { return m_Instance ?? (m_Instance = new HotKeySettingsManager()); }
        }

        /// <summary>
        /// 加载默认快捷键
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<HotKeyModel> LoadDefaultHotKey()
        {
            var hotKeyList = new ObservableCollection<HotKeyModel>();
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.StopSound.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = false, IsSelectShift = false, SelectKey = EKey.F1 });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.Start.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = false, IsSelectShift = false, SelectKey = EKey.F2 });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.Pause.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = false, IsSelectShift = false, SelectKey = EKey.F3 });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.Stop.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = false, IsSelectShift = false, SelectKey = EKey.F4 });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.Show.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = false, IsSelectShift = false, SelectKey = EKey.F5 });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.Exit.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = false, IsSelectShift = false, SelectKey = EKey.F6 });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.保存.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.B });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.打开.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.X });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.新建.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.H });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.删除.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.G });
            return hotKeyList;
        }

        /// <summary>
        /// 通知注册系统快捷键委托
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        public delegate bool RegisterGlobalHotKeyHandler(ObservableCollection<HotKeyModel> hotKeyModelList);
        public event RegisterGlobalHotKeyHandler RegisterGlobalHotKeyEvent;
        public bool RegisterGlobalHotKey(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            if (RegisterGlobalHotKeyEvent != null)
            {
                return RegisterGlobalHotKeyEvent(hotKeyModelList);
            }
            return false;
        }

    }
}
