using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SmokeNote.Logic.Models
{
    public class FontFamilyList : ObservableCollection<string>
    {
        public FontFamilyList()
        {
            this.Add("宋体");
            this.Add("新宋体");
            this.Add("仿宋");
            this.Add("楷体");
            this.Add("黑体");
            this.Add("微软雅黑");
        }
    }
}
