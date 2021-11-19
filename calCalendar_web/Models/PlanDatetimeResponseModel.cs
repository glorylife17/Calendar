using System;
namespace calCalendar_web.Models
{
    public class PlanDatetimeResponseModel
    {
        /// <summary>
        /// 索引
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 符合計畫內的工作日期群
        /// </summary>
        public DateTimeOffset[] Days {get;set;}
    }
}
