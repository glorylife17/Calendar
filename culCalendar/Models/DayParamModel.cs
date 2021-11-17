using System;
using culCalendar.Enums;

namespace culCalendar.Models
{
    public class DayParamModel
    {
        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 循環類型
        /// </summary>
        public RecurringType RecurringType { get; set; } = RecurringType.None;

        /// <summary>
        /// 間隔次數
        /// </summary>
        public int Period { get; set; } = 1;

        /// <summary>
        /// 包含 Weekly:星期 / Monthly:日 / Yearly: 日期
        /// </summary>
        public string[] Days { get; set; } = new string[0];

        /// <summary>
        /// 避開例假日
        /// </summary>
        public bool IsAvoidHoliday { get; set; } = false;

        /// <summary>
        /// 避開日期至平日類型
        /// </summary>
        public AvoidType AvoidType { get; set; } = AvoidType.Ignore;

        /// <summary>
        /// 假日資料
        /// </summary>
        public DateTime[] Holidays { get; set; } = new DateTime[0];

        /// <summary>
        /// 避開無此日
        /// </summary>
        public bool IsIncludeNoday { get; set; } = false;
    }

}
