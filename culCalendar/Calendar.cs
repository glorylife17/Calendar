using System;
using System.Collections.Generic;
using System.Linq;
using culCalendar.Enums;
using culCalendar.Models;

namespace culCalendar
{
    public class Calendar
    {
        public Calendar()
        {
        }

        private static DayParamModel _plan;

        private static DateTime currentDate;
        public DateTime CurrentDate
        {
            get { return currentDate == DateTime.MinValue ? DateTime.Now : currentDate; }
            set { currentDate = value; }
        }

        private DateTime minCurrentMonthDate => new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
        private DateTime maxCurrentMonthDate => minCurrentMonthDate.AddMonths(1).AddDays(-1);


        public List<DateTime> getWorkdays(DayParamModel plan)
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

            return new List<DateTime>();
        }

        private List<DateTime> getWeeklyDays()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 取得沒循環的日子
        /// </summary>
        /// <returns></returns>
        private List<DateTime> getNoRecurringDays()
        {
            var days = getCurrentMonthPlanDays();
            return days;
        }

        /// <summary>
        /// 計算每日規則天
        /// </summary>
        /// <returns></returns>
        private List<DateTime> getDailyDays()
        {
            var monthDays = getCurrentMonthPlanDays();
            if (!monthDays.Any())
                throw new Exception("計畫時間不在該區間");


            var recurringDays = new List<DateTime>();
            var firstDay = getRecurringFirstDay();
            var maxDay = monthDays.Last().AddDays(1);

            var addDay = 0;
            while (firstDay < maxDay)
            {
                recurringDays.Add(firstDay.AddDays(addDay));
                firstDay = firstDay.AddDays(addDay + _plan.Period);
            }

            var result = monthDays.Intersect(recurringDays).ToList();

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
        private List<DateTime> filterSpecialDates(List<DateTime> dates)
        {
            var result = new List<DateTime>();

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
        /// 取得循環日在該月的第一天
        /// </summary>
        /// <returns></returns>
        private DateTime getRecurringFirstDay()
        {
            var intialDate = Convert.ToDateTime($"{CurrentDate.ToString("yyyy-MM-01")}");
            var days = (intialDate - _plan.StartDate).TotalDays;

            var workdays = days % _plan.Period;

            var recurringDay = intialDate;

            if (workdays > 0)
                recurringDay = intialDate.AddDays(_plan.Period - workdays);

            return recurringDay;
        }

        /// <summary>
        /// 計算該月是否是閏年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private bool isLeapYear(int year)
        {
            return (year % 400 == 0) || ((year % 4 == 0) && (year % 100 != 0));
        }

        /// <summary>
        /// 過濾星期六與星期日
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        private List<DateTime> fillterWeekends(List<DateTime> dates, int flagDay = 0)
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
        private List<DateTime> fillterHoliDays(List<DateTime> dates, int flagDay = 0)
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
        private DateTime getWorkDays(DateTime item, int addDay)
        {
            var result = item;
            if (_plan.Holidays.Contains(item)
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
        private List<DateTime> getCurrentMonthPlanDays()
        {
            var min = (minCurrentMonthDate < _plan.StartDate) ? _plan.StartDate : minCurrentMonthDate;
            var max = (maxCurrentMonthDate < _plan.EndDate) ? maxCurrentMonthDate : _plan.EndDate;
            var intervalDays = (max - min).TotalDays + 1;

            var dates = new List<DateTime>();
            for (int i = 0; i < intervalDays; i++)
            {
                dates.Add(min.AddDays(i));
            }

            return dates;
        }
    }
}
