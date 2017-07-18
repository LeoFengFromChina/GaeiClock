using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SmokeNote.Logic.Models
{
    public class FontSizeList : ObservableCollection<double>
    {
        public FontSizeList()
        {
            this.Add(9);
            this.Add(12);
            this.Add(14);
            this.Add(16);
            this.Add(18);
            this.Add(22);
            this.Add(26);
            this.Add(30);
        }
    }
}
