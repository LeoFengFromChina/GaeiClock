using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Framework.Common.Helpers
{
    public static class RichTextHelper
    {
        public static string GetPlainText(string rtf)
        {
            if (string.IsNullOrWhiteSpace(rtf))
            {
                return null;
            }

            var rtb = new System.Windows.Controls.RichTextBox();
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    sw.Write(rtf);
                    sw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    textRange.Load(ms, DataFormats.Rtf);
                    return textRange.Text;
                }
            }
        }
    }
}
