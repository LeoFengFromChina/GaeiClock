using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Logic.Models
{
    /// <summary>
    /// 表示时间范围
    /// </summary>
    public struct DateRange
    {
        public DateRangeType Type { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public static bool operator ==(DateRange dr1, DateRange dr2)
        {
            if (dr1.Type == DateRangeType.CustomRange && dr1.Type == dr2.Type)
            {
                return dr1.StartDate == dr2.StartDate && dr1.EndDate == dr2.EndDate;
            }
            return dr1.Type == dr2.Type && dr1.StartDate == dr2.StartDate && dr1.EndDate == dr2.EndDate;
        }

        public static bool operator !=(DateRange dr1, DateRange dr2)
        {
            return !(dr1 == dr2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != typeof(DateRange))
            {
                return false;
            }
            var dr2 = (DateRange)obj;
            return this == dr2;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum DateRangeType
    {
        /// <summary>
        /// 自定义范围
        /// </summary>
        CustomRange = 0,

        /// <summary>
        /// 今天
        /// </summary>
        Today = 1,

        /// <summary>
        /// 本周
        /// </summary>
        ThisWeek = 2,

        /// <summary>
        /// 本月
        /// </summary>
        ThisMonth = 3,

        /// <summary>
        /// 本年
        /// </summary>
        ThisYear = 4
    }
}
