using SmokeNote.Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmokeNote.Client
{
    /// <summary>
    /// ClockConfig.xaml 的交互逻辑
    /// </summary>
    public partial class ClockConfig : Window
    {
        public ClockConfig()
        {
            InitializeComponent();
            this.Loaded += ClockConfig_Loaded;
            this.btn_Submit.Click += Btn_Submit_Click;
            this.btn_Cancel.Click += Btn_Cancel_Click;
        }

        public ClockConfig(Dictionary<string, string> _initDic)
        {
            InitializeComponent();
            this.Loaded += ClockConfig_Loaded;
            this.btn_Submit.Click += Btn_Submit_Click;
            this.btn_Cancel.Click += Btn_Cancel_Click;
            initDic = _initDic;
        }


        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ClockConfig_Loaded(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
            InitShow(grid_Panel);
        }
        Dictionary<string, string> initDic = new Dictionary<string, string>();
        Dictionary<string, string> settingDic = new Dictionary<string, string>();
        private void Btn_Submit_Click(object sender, RoutedEventArgs e)
        {

            GetChildren(grid_Panel);
            //保存
            string DefaultDuration = settingDic["DefaultDuration"];
            string DefaultMaximunDuration = settingDic["DefaultMaximunDuration"];

            if (settingDic != null)
            {
                //判断一个string型的时间格式是否正确
                DateTime dateTime_1 = new DateTime();
                DateTime dateTime_2 = new DateTime();
                DateTime dateTime_X = new DateTime();
                int volumeTemp;
                bool convertResult = DateTime.TryParse(GetNormalTimeString(DefaultDuration), out dateTime_1);
                convertResult = convertResult && DateTime.TryParse(GetNormalTimeString(DefaultMaximunDuration), out dateTime_2);
                convertResult = convertResult && DateTime.TryParse(GetNormalTimeString(settingDic["FirstWarmTime"]), out dateTime_X);
                convertResult = convertResult && DateTime.TryParse(GetNormalTimeString(settingDic["SecondWarmTime"]), out dateTime_X);
                convertResult = convertResult && DateTime.TryParse(GetNormalTimeString(settingDic["ThirdWarmTime"]), out dateTime_X);
                convertResult = convertResult && DateTime.TryParse(GetNormalTimeString(settingDic["FourthWarmTime"]), out dateTime_X);
                convertResult = convertResult && DateTime.TryParse(GetNormalTimeString(settingDic["FifthWarmTime"]), out dateTime_X);
                convertResult = convertResult && int.TryParse(settingDic["FirstVolumeAdd"], out volumeTemp);
                convertResult = convertResult && int.TryParse(settingDic["SecondVolumeAdd"], out volumeTemp);
                convertResult = convertResult && int.TryParse(settingDic["ThirdVolumeAdd"], out volumeTemp);
                convertResult = convertResult && int.TryParse(settingDic["FourthVolumeAdd"], out volumeTemp);
                convertResult = convertResult && int.TryParse(settingDic["FifthVolumeAdd"], out volumeTemp);
                if (GlobalVars.FileList != null)
                {
                    List<ListItem> source = GlobalVars.FileList as List<ListItem>;
                    DateTime temp = new DateTime();
                    foreach (var item in source)
                    {
                        convertResult = convertResult && DateTime.TryParse(GetNormalTimeString(item.Time), out temp);
                    }
                }
                if (!convertResult)
                {
                    MessageBox.Show("请检查数据是否符合格式", "LeoClock");
                    return;
                }
                if (dateTime_2 < dateTime_1)
                {
                    MessageBox.Show("默认最大上限时间必须大于默认倒计时长", "LeoClock");
                    return;
                }
            }

            Submit(settingDic);

            this.Close();
        }

        private string GetNormalTimeString(string curTime)
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH") + ":" + curTime;
        }
        List<UIElement> uiC = new List<UIElement>();
        TextBox currentTextBox;
        CheckBox currentCheckBox;
        ComboBox currentComboBox;
        ListView currentListView;
        private void GetChildren(object SourceControl)
        {
            if (SourceControl.GetType().Name == "StackPanel")
            {
                foreach (var item in ((StackPanel)SourceControl).Children)
                {
                    GetChildren(item);
                }
            }
            else if (SourceControl.GetType().Name == "Grid")
            {
                foreach (var item in ((Grid)SourceControl).Children)
                {
                    GetChildren(item);
                }
            }
            else if (SourceControl.GetType().Name == "CheckBox")
            {
                currentCheckBox = null;
                currentCheckBox = ((CheckBox)SourceControl);
                if (!string.IsNullOrEmpty(currentCheckBox.Name))
                {
                    if (!settingDic.ContainsKey(currentCheckBox.Name))
                        settingDic.Add(currentCheckBox.Name, currentCheckBox.IsChecked.ToString());
                    else
                        settingDic[currentCheckBox.Name] = currentCheckBox.IsChecked.ToString();
                }
            }
            else if (SourceControl.GetType().Name == "TextBox")
            {
                currentTextBox = null;
                currentTextBox = ((TextBox)SourceControl);
                if (!string.IsNullOrEmpty(currentTextBox.Name))
                {
                    if (!settingDic.ContainsKey(currentTextBox.Name))
                        settingDic.Add(currentTextBox.Name, currentTextBox.Text);
                    else
                        settingDic[currentTextBox.Name] = currentTextBox.Text;
                }
            }
            else if (SourceControl.GetType().Name == "ComboBox")
            {
                //ComboBoxItem
                currentComboBox = null;
                currentComboBox = ((ComboBox)SourceControl);
                if (!string.IsNullOrEmpty(currentComboBox.Name))
                {
                    string nameStr = ((ComboBoxItem)currentComboBox.SelectedItem).Name;
                    if (!settingDic.ContainsKey(currentComboBox.Name))
                        settingDic.Add(currentComboBox.Name, nameStr);
                    else
                        settingDic[currentComboBox.Name] = nameStr;

                }
            }
            else if (SourceControl.GetType().Name == "ListView")
            {
                //currentListView
                //currentListView = null;
                //currentListView = ((ListView)SourceControl);
                //if (currentListView.Items.Count > 0)
                //{
                //    List<ListItem> dataSource = currentListView.ItemsSource as List<ListItem>;
                //    foreach (var item in dataSource)
                //    {
                //        MessageBox.Show(item.FileName + ":" + item.Time);
                //    }
                //}
            }
        }

        private void InitShow(object SourceControl)
        {
            if (SourceControl.GetType().Name == "StackPanel")
            {
                foreach (var item in ((StackPanel)SourceControl).Children)
                {
                    InitShow(item);
                }
            }
            else if (SourceControl.GetType().Name == "Grid")
            {
                foreach (var item in ((Grid)SourceControl).Children)
                {
                    InitShow(item);
                }
            }
            else if (SourceControl.GetType().Name == "CheckBox")
            {
                currentCheckBox = null;
                currentCheckBox = ((CheckBox)SourceControl);
                if (!string.IsNullOrEmpty(currentCheckBox.Name)
                    && initDic.ContainsKey(currentCheckBox.Name))
                {
                    bool temp = false;
                    bool.TryParse(initDic[currentCheckBox.Name], out temp);
                    ((CheckBox)SourceControl).IsChecked = temp;
                }
            }
            else if (SourceControl.GetType().Name == "TextBox")
            {
                currentTextBox = null;
                currentTextBox = ((TextBox)SourceControl);
                if (!string.IsNullOrEmpty(currentTextBox.Name)
                    && initDic.ContainsKey(currentTextBox.Name))
                {
                    ((TextBox)SourceControl).Text = initDic[currentTextBox.Name];
                }
            }
            else if (SourceControl.GetType().Name == "ComboBox")
            {
                //ComboBoxItem
                currentComboBox = null;
                currentComboBox = ((ComboBox)SourceControl);
                if (!string.IsNullOrEmpty(currentComboBox.Name)
                    && initDic.ContainsKey(currentComboBox.Name))
                {
                    for (int i = 0; i < currentComboBox.Items.Count; i++)
                    {
                        if ((currentComboBox.Items[i] as ComboBoxItem).Name.Equals(initDic[currentComboBox.Name]))
                        {
                            ((ComboBox)SourceControl).SelectedIndex = i;
                            ((ComboBox)SourceControl).UpdateLayout();
                        }
                    }

                }
            }

            listView.ItemsSource = GlobalVars.FileList as List<ListItem>;
        }
        public delegate void SubmitDelegate(Dictionary<string, string> dataDic);
        public event SubmitDelegate Submit;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<ListItem> sources = GlobalVars.FileList as List<ListItem>;
            if (sources == null)
                sources = new List<ListItem>();
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            //openFileDialog.InitialDirectory = "c:\\";//注意这里写路径时要用c:\\而不是c:\
            openFileDialog.Filter = "ppt文件|*.pptx|所有文件|*.*";
            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var item in openFileDialog.FileNames)
                {
                    ListItem lt = new ListItem();
                    lt.FilePath = item;
                    lt.FileName = item.Substring(item.LastIndexOf('\\') + 1, item.Length - item.LastIndexOf('\\') - 1);
                    lt.Time = "05:00";
                    sources.Add(lt);
                    //listView.Items.Add(lt);
                }
                listView.ItemsSource = null;
                listView.ItemsSource = sources;
                GlobalVars.FileList = sources;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = null;
            GlobalVars.FileList = null;
        }
    }

    public class ListItem
    {

        public string FilePath
        {
            get; set;
        }
        public string FileName
        {
            get; set;
        }
        public string Time
        {
            get; set;
        }
    }
}
