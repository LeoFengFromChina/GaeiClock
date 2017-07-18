using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media.Animation;
using SmokeNote.Client.Views;
using SmokeNote.Logic.Helpers;
using System.Collections.Generic;
using System;
using System.Media;
using System.Drawing.Drawing2D;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Interop;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Diagnostics;
using GlobalHotKeyDemo;

namespace SmokeNote.Client
{
    /// <summary>
    /// Clock.xaml 的交互逻辑
    /// </summary>
    public partial class Clock : Window
    {
        public Clock()
        {
            InitializeComponent();
            this.Loaded += Clock_Loaded;
            this.MouseEnter += Clock_MouseEnter;

        }

        private PowerPoint.Application oPPT;
        public void LoadContextMenu()
        {
            ContextMenu aMenu = new ContextMenu();
            aMenu.FontSize = 16;
            MenuItem StartMenu = new MenuItem();
            StartMenu.Header = "开始";
            StartMenu.Name = "Start";
            StartMenu.Click += MenuItem_Click;
            aMenu.Items.Add(StartMenu);

            MenuItem StopMenu = new MenuItem();
            StopMenu.Header = "暂停";
            StopMenu.Name = "Stop";
            StopMenu.Click += MenuItem_Click;
            aMenu.Items.Add(StopMenu);

            MenuItem EndMenu = new MenuItem();
            EndMenu.Header = "结束";
            EndMenu.Name = "End";
            EndMenu.Click += MenuItem_Click;
            aMenu.Items.Add(EndMenu);

            Separator spearator = new Separator();
            aMenu.Items.Add(spearator);

            MenuItem ConfigMenu = new MenuItem();
            ConfigMenu.Header = "设置";
            ConfigMenu.Name = "Config";
            ConfigMenu.Click += MenuItem_Click;
            aMenu.Items.Add(ConfigMenu);

            MenuItem FilesMenu = new MenuItem();
            FilesMenu.Header = "文档";
            FilesMenu.Name = "Files";
            FilesMenu.Click += MenuItem_Click;
            #region 自菜单

            if (GlobalVars.FileList != null)
            {
                foreach (var item in GlobalVars.FileList as List<ListItem>)
                {
                    MenuItem fileItem = new MenuItem();
                    fileItem.Header = item.FileName;
                    fileItem.Tag = item;
                    fileItem.Click += FileItem_Click;
                    FilesMenu.Items.Add(fileItem);
                }
            }
            #endregion
            aMenu.Items.Add(FilesMenu);

            Separator spearator2 = new Separator();
            aMenu.Items.Add(spearator2);

            MenuItem HelpMenu = new MenuItem();
            HelpMenu.Header = "帮助";
            HelpMenu.Name = "Help";
            HelpMenu.Click += MenuItem_Click;
            aMenu.Items.Add(HelpMenu);

            MenuItem ExitMenu = new MenuItem();
            ExitMenu.Header = "退出";
            ExitMenu.Name = "Exit";
            ExitMenu.Click += MenuItem_Click;
            aMenu.Items.Add(ExitMenu);


            mainGrid.ContextMenu = aMenu;
        }
        System.Windows.Threading.DispatcherTimer dtimer;
        SlideShowSettings slideShow;
        private void Clock_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            HotKeySettingsManager.Instance.RegisterGlobalHotKeyEvent += Instance_RegisterGlobalHotKeyEvent;
            LoadContextMenu();

            double x = SystemParameters.WorkArea.Width;//得到屏幕工作区域宽度
            double y = SystemParameters.WorkArea.Height;//得到屏幕工作区域高度

            double newX = x - this.Width - 100;
            double newY = 100;

            this.Left = newX;
            this.Top = newY;

            LoadLocalConfig();

            this.Closing += Clock_Closing;
            dtimer = new System.Windows.Threading.DispatcherTimer();
            dtimer.Interval = TimeSpan.FromSeconds(1);
            dtimer.Tick += dtimer_Tick;

        }
        private void SlideShowEnd(PowerPoint.Presentation Pres)
        {
            Stop();
        }

        private void SlideShowBegin(PowerPoint.SlideShowWindow Wn)
        {
            Start();
        }

        private void Clock_Closing(object sender, CancelEventArgs e)
        {
            //slideShow.Application.Quit();
            //KillProcess();
            SetBackVolume();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }

        Dictionary<string, string> initDic = new Dictionary<string, string>();
        /// <summary>
        /// 初始化本地配置文件
        /// </summary>
        private void LoadLocalConfig()
        {
            #region Load

            string baseSectionName = "BaseConfig";
            string iniPath = System.Environment.CurrentDirectory + @"\Config.ini";
            string allKeys = InitFileHelper.ReadIniData(baseSectionName, "AllKeys", string.Empty, iniPath);
            string[] keyList = null;
            if (!string.IsNullOrEmpty(allKeys))
            {
                keyList = allKeys.Split(',');
            }

            if (keyList == null || keyList.Length <= 0)
                return;
            string tempValue = string.Empty;
            foreach (string item in keyList)
            {
                tempValue = InitFileHelper.ReadIniData(baseSectionName, item, string.Empty, iniPath);
                if (!string.IsNullOrEmpty(item))
                {
                    if (!initDic.ContainsKey(item))
                    {
                        initDic.Add(item, tempValue);
                    }
                    else
                    {
                        initDic[item] = tempValue;
                    }

                    tempValue = string.Empty;
                }
            }

            #endregion

            #region SetDefault

            SetDefault();

            #endregion

            #region Files
            LoadContextMenu();
            #endregion
        }

        bool isOpacity = false;
        /// <summary>
        /// 
        /// </summary>
        private void SetLocalConfig()
        {

        }
        private void SetDefault()
        {
            //是否置顶
            string isTopmostStr = "IsTopmost";
            if (initDic != null
                && initDic.ContainsKey(isTopmostStr))
            {
                bool istop = false;
                bool.TryParse(initDic[isTopmostStr].Trim(), out istop);
                this.Topmost = istop;
            }

            //背景颜色
            if (initDic.ContainsKey("BackgroundName")
               && FindResource(initDic["BackgroundName"]) != null)
            {
                MainBorder.Background = FindResource(initDic["BackgroundName"]) as Brush;

                BrushConverter bc = new BrushConverter();
                if (initDic["BackgroundName"] == "Transpant")
                {
                    lbl_Click.Foreground = (Brush)bc.ConvertFromString("#CD0000");

                }
                else
                {
                    lbl_Click.Foreground = new SolidColorBrush(Colors.White);
                }
                //#CD0000
            }


            //默认倒计时间
            if (initDic != null
                && initDic.ContainsKey("DefaultDuration"))
            {
                this.lbl_Click.Content = initDic["DefaultDuration"];
                this.lbl_InitalTime.Content = GetMin(initDic["DefaultDuration"]);//.Replace("0", "").Replace(":", "") + "min";
            }

            //默认倒计时间
            if (initDic != null
                && initDic.ContainsKey("IsOpacity"))
            {

                bool.TryParse(initDic["IsOpacity"].Trim(), out isOpacity);
                if (isOpacity)
                {
                    Storyboard std = this.Resources["slideToOpacity"] as Storyboard;
                    std.Begin();
                }
                else
                {
                    Storyboard std = this.Resources["slideToNonOpacity"] as Storyboard;
                    std.Begin();
                }
            }

        }

        DateTime startTime;

        bool isStop = false;
        Microsoft.Office.Interop.PowerPoint.Application PPTApplication;
        private void OpenPPT(string pptPath)
        {
            PPTApplication = new Microsoft.Office.Interop.PowerPoint.Application();
            //以非只读方式打开,方便操作结束后保存.  
            Presentation PPTPresentation = PPTApplication.Presentations.Open(pptPath,
                MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoCTrue);
            slideShow = PPTPresentation.SlideShowSettings;
            //slideShow.Run();
            slideShow.Application.Activate();
            slideShow.Application.SlideShowBegin += new PowerPoint.EApplication_SlideShowBeginEventHandler(SlideShowBegin);
            slideShow.Application.SlideShowEnd += new PowerPoint.EApplication_SlideShowEndEventHandler(SlideShowEnd);
        }
        private void KillProcess()
        {
            Process[] processes = System.Diagnostics.Process.GetProcessesByName("POWERPNT.EXE");
            if (processes != null)
            {
                processes[0].Kill();
            }
        }
        private void Start()
        {
            if (!isStop)
            {
                IsNowShow = false;
                isManulStopSound = false;
                startTime = DateTime.Now;
                if (std != null)
                    std.Stop();
                SetBackVolume();
                MediaPlayerHelper.Stop();
                UpTimes = 0;
                SecondCount = 0;
                std = null;
                //IsNowShow = false;
            }
            else if (isStop)
            {
                if (!string.IsNullOrEmpty(currentSoundPath))
                    MediaPlayerHelper.PlaySound(currentSoundPath, true);
                isStop = false;
                IsNowShow = true ;
            }

            dtimer.Start();
        }
        private void Stop()
        {
            isStop = true;
            dtimer.Stop();
            MediaPlayerHelper.Stop();
        }
        private void End()
        {
            isManulStopSound = false;
            dtimer.Stop();
            SecondCount = 0;
            isStop = false;
            if (std != null)
                std.Stop();
            std = null;
            IsNowShow = false;
            MediaPlayerHelper.Stop();
            SetBackVolume();
            currentSoundPath = string.Empty;
        }
        private void Exit()
        {
            if (MessageBoxResult.Yes == MessageBox.Show("是否确认退出？", "会议定时器", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                this.Close();
                System.Windows.Application.Current.Shutdown(-1);
            }
        }
        private void FileItem_Click(object sender, RoutedEventArgs e)
        {
            #region 打开新文档，就等于重新开始了==End后了

            End();
            #endregion

            ListItem item = ((MenuItem)sender).Tag as ListItem;
            string filePaht = item.FilePath;
            if (!string.IsNullOrEmpty(filePaht))
            {
                //System.Diagnostics.Process.Start(filePaht);
                OpenPPT(filePaht);
            }
            //isStop = true;
            dtimer.Stop();
            MediaPlayerHelper.Stop();

            this.lbl_Click.Content = item.Time;
            this.lbl_InitalTime.Content = GetMin(item.Time);//.Replace("0", "").Replace(":", "") + "min"; ;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem)sender).Name)
            {
                case "Start":
                    {
                        Start();
                    }
                    break;
                case "Stop":
                    {
                        Stop();
                    }
                    break;
                case "End":
                    {
                        #region MyRegion
                        End();
                        #endregion
                    }
                    break;
                case "Config":
                    {
                        var clockConfig = new ClockConfig(initDic);
                        clockConfig.Submit += clockConfig_Submit;
                        clockConfig.Show();
                    }
                    break;
                case "Help":
                    {
                        new Help().ShowDialog();
                    }
                    break;
                case "Exit":
                    {
                        Exit();
                    }
                    break;
                default:
                    break;
            }
        }

        private void clockConfig_Submit(Dictionary<string, string> dataDic)
        {
            string baseSectionName = "BaseConfig";
            string iniPath = System.Environment.CurrentDirectory + @"\Config.ini";
            foreach (KeyValuePair<string, string> item in dataDic)
            {
                if (!InitFileHelper.WriteIniData(baseSectionName, item.Key, item.Value, iniPath))
                {
                    MessageBox.Show("配置保存出错，请知悉！", "LeoFeng");
                }
            }
            LoadLocalConfig();
        }

        int SecondCount = 0;

        Storyboard std = null;
        string currentSoundPath = string.Empty;

        void dtimer_Tick(object sender, EventArgs e)
        {
            if (isStop && !isManulStopSound)
                return;
            SecondCount += 1;

            this.lbl_Click.Content = GetTimeFormat(SecondCount);
            int volumeAdd = 0;
            if (IsNow())
            {
                #region MyRegion

                bool isTwinkle = false;
                bool.TryParse(initDic["IsTwinkle"].Trim(), out isTwinkle);
                if (isTwinkle)
                {
                    std = this.Resources["TwinkleStoryboard"] as Storyboard;
                    std.RepeatBehavior = RepeatBehavior.Forever;
                    std.Begin();
                }

                int.TryParse(initDic["EqualVolumeAdd"], out volumeAdd);
                VolumeUp(volumeAdd / 2);
                currentSoundPath = System.Environment.CurrentDirectory + @"\Sounds\" + initDic["EqualSound"];
                MediaPlayerHelper.PlaySound(currentSoundPath, true);
                #endregion
            }
            if ((initDic != null
                && !string.IsNullOrEmpty(initDic["FirstWarmSound"])
                 && initDic.ContainsKey("FirstWarmTime")
                 && initDic["FirstWarmTime"].Equals(this.lbl_Click.Content)))
            {

                bool isTwinkle = false;
                bool.TryParse(initDic["IsTwinkle"].Trim(), out isTwinkle);
                if (isTwinkle)
                {
                    std = this.Resources["TwinkleStoryboard"] as Storyboard;
                    std.RepeatBehavior = RepeatBehavior.Forever;
                    std.Begin();
                }

                int.TryParse(initDic["FirstVolumeAdd"], out volumeAdd);
                VolumeUp(volumeAdd / 2);
                currentSoundPath = System.Environment.CurrentDirectory + @"\Sounds\" + initDic["FirstWarmSound"];
                MediaPlayerHelper.PlaySound(currentSoundPath, true);
            }
            else if (initDic != null
                && !string.IsNullOrEmpty(initDic["SecondWarmSound"])
                 && initDic.ContainsKey("SecondWarmTime")
                 && initDic["SecondWarmTime"].Equals(this.lbl_Click.Content))
            {
                int.TryParse(initDic["SecondVolumeAdd"], out volumeAdd);

                VolumeUp(volumeAdd / 2);

                currentSoundPath = System.Environment.CurrentDirectory + @"\Sounds\" + initDic["SecondWarmSound"];
                MediaPlayerHelper.PlaySound(currentSoundPath, true);
            }
            else if (initDic != null
                && !string.IsNullOrEmpty(initDic["ThirdWarmSound"])
                 && initDic.ContainsKey("ThirdWarmTime")
                 && initDic["ThirdWarmTime"].Equals(this.lbl_Click.Content))
            {
                int.TryParse(initDic["ThirdVolumeAdd"], out volumeAdd);

                VolumeUp(volumeAdd / 2);
                currentSoundPath = System.Environment.CurrentDirectory + @"\Sounds\" + initDic["ThirdWarmSound"];
                MediaPlayerHelper.PlaySound(currentSoundPath, true);
            }
            else if (initDic != null
                && !string.IsNullOrEmpty(initDic["FourthWarmSound"])
                 && initDic.ContainsKey("FourthWarmTime")
                 && initDic["FourthWarmTime"].Equals(this.lbl_Click.Content))
            {
                int.TryParse(initDic["FourthVolumeAdd"], out volumeAdd);
                VolumeUp(volumeAdd / 2);
                currentSoundPath = System.Environment.CurrentDirectory + @"\Sounds\" + initDic["FourthWarmSound"];
                MediaPlayerHelper.PlaySound(currentSoundPath, true);
            }
            else if (initDic != null
                && !string.IsNullOrEmpty(initDic["FifthWarmSound"])
                 && initDic.ContainsKey("FifthWarmTime")
                 && initDic["FifthWarmTime"].Equals(this.lbl_Click.Content))
            {
                int.TryParse(initDic["FifthVolumeAdd"], out volumeAdd);
                VolumeUp(volumeAdd / 2);
                currentSoundPath = System.Environment.CurrentDirectory + @"\Sounds\" + initDic["FifthWarmSound"];
                MediaPlayerHelper.PlaySound(currentSoundPath, true);
            }
            else if (initDic != null
                 && initDic.ContainsKey("DefaultMaximunDuration")
                 && initDic["DefaultMaximunDuration"].Equals(this.lbl_Click.Content))
            {
                dtimer.Stop();
                isStop = false;

            }
        }

        bool IsNowShow = false;
        private bool IsNow()
        {
            if (IsNowShow)
            {
                return false;
            }
            bool result = false;
            int currentMin = int.Parse(lbl_Click.Content.ToString().Split(':')[0]);
            int inintMin = int.Parse(lbl_InitalTime.Content.ToString());
            if (currentMin >= inintMin)
            {
                result = true;
                IsNowShow = true;
            }
            return result;
        }
        int UpTimes = 0;
        /// <summary>
        /// 设置音量+
        /// </summary>
        private void VolumeUp(int soundPlusCount)
        {
            for (int i = 0; i < soundPlusCount; i++)
            {
                VolumeControlHelper.VolumeUp();
                UpTimes += 1;
            }

        }
        private string GetTimeFormat(int sec)
        {
            string result = string.Empty;

            result = ((sec % 3600) / 60).ToString().PadLeft(2, '0') + ":" + (sec % 60).ToString().PadLeft(2, '0');


            return result;
        }
        /// <summary>
        /// 设置回音量
        /// </summary>
        private void SetBackVolume()
        {
            for (int i = 0; i < UpTimes; i++)
            {
                VolumeControlHelper.VolumeDown();
            }
            UpTimes = 0;
        }
        private void myClock_MouseLeave(object sender, MouseEventArgs e)
        {

            bool.TryParse(initDic["IsOpacity"].Trim(), out isOpacity);
            if (!isOpacity)
            {
                return;
            }
            if (std != null)
            {
                std.Begin();
            }
            else
            {
                Storyboard std = this.Resources["mouseLeaveStoryboard"] as Storyboard;
                std.Begin();
            }
        }

        private void Clock_MouseEnter(object sender, MouseEventArgs e)
        {
            bool.TryParse(initDic["IsOpacity"].Trim(), out isOpacity);
            if (!isOpacity)
            {
                return;
            }
            if (std != null)
            {
                std.Begin();
            }
            else
            {
                if (this.Opacity < 1)
                {
                    Storyboard std = this.Resources["mouseEnterStoryboard"] as Storyboard;
                    std.Begin();
                }
            }
        }

        private void myClock_Closing(object sender, CancelEventArgs e)
        {
            SetBackVolume();
        }

        private string GetMin(string time)
        {
            string result = string.Empty;
            if (!time.Contains(":"))
            {
                return time;
            }
            result = int.Parse(time.Split(':')[0]).ToString();

            return result;
        }


        #region 快捷键相关
        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr m_Hwnd = new IntPtr();

        /// <summary>
        /// 记录快捷键注册项的唯一标识符
        /// </summary>
        private Dictionary<EHotKeySetting, int> m_HotKeySettings = new Dictionary<EHotKeySetting, int>();

        /// <summary>
        /// 通知注册系统快捷键事件处理函数
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        /// <returns></returns>
        private bool Instance_RegisterGlobalHotKeyEvent(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            return InitHotKey(hotKeyModelList);
        }
        /// <summary>
        /// 初始化注册快捷键
        /// </summary>
        /// <param name="hotKeyModelList">待注册热键的项</param>
        /// <returns>true:保存快捷键的值；false:弹出设置窗体</returns>
        private bool InitHotKey(ObservableCollection<HotKeyModel> hotKeyModelList = null)
        {
            var list = hotKeyModelList ?? HotKeySettingsManager.Instance.LoadDefaultHotKey();
            // 注册全局快捷键
            string failList = HotKeyHelper.RegisterGlobalHotKey(list, m_Hwnd, out m_HotKeySettings);
            if (string.IsNullOrEmpty(failList))
                return true;
            MessageBoxResult mbResult = MessageBox.Show(string.Format("无法注册下列快捷键\n\r{0}是否要改变这些快捷键？", failList), "提示", MessageBoxButton.YesNo);

            return true;
        }

        /// <summary>
        /// WPF窗体的资源初始化完成，并且可以通过WindowInteropHelper获得该窗体的句柄用来与Win32交互后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // 获取窗体句柄
            m_Hwnd = new WindowInteropHelper(this).Handle;
            HwndSource hWndSource = HwndSource.FromHwnd(m_Hwnd);
            // 添加处理程序
            if (hWndSource != null) hWndSource.AddHook(WndProc);
        }

        /// <summary>
        /// 所有控件初始化完成后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            // 注册热键
            InitHotKey();
        }
        bool isManulStopSound = false;
        /// <summary>
        /// 窗体回调函数，接收所有窗体消息的事件处理函数
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wideParam">附加参数1</param>
        /// <param name="longParam">附加参数2</param>
        /// <param name="handled">是否处理</param>
        /// <returns>返回句柄</returns>
        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
            var hotkeySetting = new EHotKeySetting();
            switch (msg)
            {
                case HotKeyManager.WM_HOTKEY:
                    int sid = wideParam.ToInt32();

                    if (sid == m_HotKeySettings[EHotKeySetting.Show])
                    {
                        hotkeySetting = EHotKeySetting.Show;
                        this.Visibility = this.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.Pause])
                    {
                        isManulStopSound = false;
                        Stop();
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.Stop])
                    {
                        isManulStopSound = false;
                        End();
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.Start])
                    {
                        Start();
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.StopSound])
                    {
                        if (!isManulStopSound && MediaPlayerHelper.isPlaying)
                        {
                            //isStop = true;
                            MediaPlayerHelper.Stop();
                        }
                        else
                        {
                            MediaPlayerHelper.PlaySound(currentSoundPath, true);

                        }
                        isManulStopSound = !isManulStopSound;
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.Exit])
                    {
                        Exit();
                    }
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }


        #endregion
    }
}
