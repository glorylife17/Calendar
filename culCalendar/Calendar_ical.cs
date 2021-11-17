using System;
using System.Collections.Generic;
using System.Linq;
using culCalendar.Enums;
using culCalendar.Models;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace culCalendar
{
    public class Calendar_ical
    {
        private static DayParamModel _plan;
        private static DateTime currentDate;
        public DateTime CurrentDate
        {
            get { return currentDate == DateTime.MinValue ? DateTime.Now : currentDate; }
            set { currentDate = value; }
        }

        private DateTime minCurrentMonthDate => new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
        private DateTime maxCurrentMonthDate => minCurrentMonthDate.AddMonths(1);


        public List<DateTimeOffset> getWorkdays(DayParamModel plan)
        {
            _plan = plan;

            if (NoSchedulePlan())
                throw new Exception("計畫資訊不完整");

            switch (plan.RecurringType)
            {
                case RecurringType.None:
                    return getNoRecurringDays();
                case RecurringType.DAILY:
                    return getDailyDays();
            }

            return new List<DateTimeOffset>();
        }

        /// <summary>
        /// 取得沒循環的日子
        /// </summary>
        /// <returns></returns>
        private List<DateTimeOffset> getNoRecurringDays()
        {
            var days = getCurrentMonthPlanDays();
            return days;
        }

        public List<DateTimeOffset> getDailyDays()
        {
            var monthDays = getCurrentMonthPlanDays();
            if (!monthDays.Any())
                return new List<DateTimeOffset>();

            var rPattern = new RecurrencePattern();
            rPattern.Frequency = Ical.Net.FrequencyType.Daily;
            rPattern.Interval = _plan.Period;
            rPattern.Until = _plan.EndDate;

            var recComp = new Ical.Net.CalendarComponents.RecurringComponent();
            recComp.RecurrenceRules.Add(rPattern);
            recComp.Start = new Ical.Net.DataTypes.CalDateTime(_plan.StartDate);

            var occurences = recComp.GetOccurrences(minCurrentMonthDate, maxCurrentMonthDate);
            var result = occurences.Select(x => x.Period.StartTime.AsDateTimeOffset).ToList();

            if (_plan.IsAvoidHoliday)
            {
                result = filterSpecialDates(result);
            }


            return result;
        }

        /// <summary>
        /// 過濾特殊日期
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        private List<DateTimeOffset> filterSpecialDates(List<DateTimeOffset> dates)
        {
            var result = new List<DateTimeOffset>();

            switch (_plan.AvoidType)
            {
                case AvoidType.Ignore:
                    result = fillterWeekends(dates);
                    result = fillterHoliDays(dates);
                    break;
                case AvoidType.Next:
                    result = fillterWeekends(dates, 1);
                    result = fillterHoliDays(dates, 1);
                    break;
                case AvoidType.Previous:
                    result = fillterWeekends(dates, -1);
                    result = fillterHoliDays(dates, -1);
                    break;
            }

            return dates.OrderBy(x => x).ToList();
        }

        /// <summary>
        /// 過濾星期六與星期日
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        private List<DateTimeOffset> fillterWeekends(List<DateTimeOffset> dates, int flagDay = 0)
        {
            var result = dates;

            var items = dates.Where(x =>
                   x.DayOfWeek == DayOfWeek.Saturday
                || x.DayOfWeek == DayOfWeek.Sunday
                ).ToList();

            foreach (var item in items)
            {
                result.Remove(item);

                if (flagDay != 0)
                {
                    var workDay = getWorkDays(item.AddDays(flagDay), flagDay);
                    if (!dates.Contains(workDay))
                        result.Add(workDay);
                }
            }

            return result;
        }

        /// <summary>
        /// 過濾假日
        /// </summary>
        /// <param name="dates"></param>
        /// <param name="flagDay"></param>
        /// <returns></returns>
        private List<DateTimeOffset> fillterHoliDays(List<DateTimeOffset> dates, int flagDay = 0)
        {
            var result = dates;

            if (_plan.Holidays != null && _plan.Holidays.Any())
            {
                var items = _plan.Holidays.Where(x => result.Contains(x)).ToList();

                foreach (var item in items)
                {
                    result.Remove(item);

                    if (flagDay != 0)
                    {
                        var workDay = getWorkDays(item.AddDays(flagDay), flagDay);
                        if (!dates.Contains(workDay))
                            result.Add(workDay);
                    }
                }

            }

            return result;
        }


        /// <summary>
        /// 計算可工作日
        /// </summary>
        /// <param name="item"></param>
        /// <param name="addDay"></param>
        /// <returns></returns>
        private DateTimeOffset getWorkDays(DateTimeOffset item, int addDay)
        {
            var result = item;
            var holidays = _plan.Holidays.Select(x => (DateTimeOffset)x).ToList();

            if (holidays.Contains(item)
                || item.DayOfWeek == DayOfWeek.Saturday
                || item.DayOfWeek == DayOfWeek.Sunday
                )
            {
                result = getWorkDays(item.AddDays(addDay), addDay);
            }

            return result;
        }


        /// <summary>
        /// 是否有計畫
        /// </summary>
        /// <returns></returns>
        private bool NoSchedulePlan()
        {
            var res = false;

            if (_plan == null)
                return true;

            var errorMinDate = new DateTime(1911, 1, 1);
            if (_plan.StartDate < errorMinDate || _plan.EndDate < errorMinDate)
                return true;

            return res;
        }

        /// <summary>
        /// 取得該月份與計畫交集日期群
        /// </summary>
        /// <returns></returns>
        private List<DateTimeOffset> getCurrentMonthPlanDays()
        {
            var min = (minCurrentMonthDate < _plan.StartDate) ? _plan.StartDate : minCurrentMonthDate;
            var max = (maxCurrentMonthDate < _plan.EndDate) ? maxCurrentMonthDate : _plan.EndDate;
            var intervalDays = (max - min).TotalDays + 1;

            var dates = new List<DateTimeOffset>();
            for (int i = 0; i < intervalDays; i++)
            {
                dates.Add(min.AddDays(i));
            }

            return dates;
        }

    }
}
