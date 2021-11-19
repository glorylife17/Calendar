using System;
using System.Collections.Generic;
using System.Linq;
using culCalendar.Enums;
using culCalendar.Models;

namespace culCalendar.Abstracts
{
    public abstract class ARecurringDays
    {
        protected DateTime _minSearchDate = DateTime.MinValue;
        protected DateTime _maxSearchDate = DateTime.MaxValue;
        protected DateTime _currentDate = DateTime.MinValue;
        protected DateTime _minCurrentDate = DateTime.MaxValue;
        protected DateTime _maxCurrentDate = DateTime.MinValue;

        protected DayParamModel _plan { get; set; }

        public void SetSearchRange(DateTime currentDate)
        {
            _currentDate = currentDate;

            _minCurrentDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            _maxCurrentDate = _minCurrentDate.AddMonths(1);

            _minSearchDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);
            _maxSearchDate = _minSearchDate.AddMonths(3);
        }

        public void SetPlan(DayParamModel plans)
        {
            _plan = plans;
        }

        public abstract List<DateTimeOffset> GetDays();

        /// <summary>
        /// 過濾特殊日期
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        protected List<DateTimeOffset> filterSpecialDates(List<DateTimeOffset> dates)
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
        /// 過濾非本月的日子
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        protected List<DateTimeOffset> fillterOtherDays(List<DateTimeOffset> dates)
        {
            var result = dates.Where(x => _minCurrentDate <= x && x < _maxCurrentDate).ToList();
            return result;
        }

        /// <summary>
        /// 過濾星期六與星期日
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        protected List<DateTimeOffset> fillterWeekends(List<DateTimeOffset> dates, int flagDay = 0)
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
        protected List<DateTimeOffset> fillterHoliDays(List<DateTimeOffset> dates, int flagDay = 0)
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
        protected DateTimeOffset getWorkDays(DateTimeOffset item, int addDay)
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

    }
}
