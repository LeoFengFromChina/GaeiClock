using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Logic.Enums
{
    /// <summary>
    /// 笔记导出类型枚举
    /// </summary>
    public enum ExportTypes : byte
    {
        /// <summary>
        /// 记事本
        /// </summary>
        PlainText = 0,

        /// <summary>
        /// Word文档
        /// </summary>
        Word = 1,
        
        /// <summary>
        /// Html文件
        /// </summary>
        Html = 2
    }
}
