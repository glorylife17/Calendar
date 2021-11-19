using System;
using System.Collections.Generic;
using System.Linq;
using culCalendar.Abstracts;
using Ical.Net.DataTypes;

namespace culCalendar.Classes
{
    public class RecurringYearlyClass : ARecurringDays
    {
        public override List<DateTimeOffset> GetDays()
        {
            if (_plan.Days == null || !_plan.Days.Any())
                throw new Exception("計畫資訊沒有設定日期");

            var rPattern = new RecurrencePattern();
            rPattern.Frequency = Ical.Net.FrequencyType.Yearly;
            rPattern.Interval = _plan.Period;
            rPattern.Until = _plan.EndDate;
            rPattern.FirstDayOfWeek = DayOfWeek.Sunday;

            foreach (var day in _plan.Days)
            {
                if (DateTime.TryParse($"{_currentDate.Year}/{day}", out DateTime cDate))
                {
                    rPattern.ByYearDay.Add(cDate.DayOfYear);
                }
            }

            var recComp = new Ical.Net.CalendarComponents.RecurringComponent();
            recComp.RecurrenceRules.Add(rPattern);
            recComp.Start = new Ical.Net.DataTypes.CalDateTime(_plan.StartDate);

            var occurences = recComp.GetOccurrences(_minSearchDate, _maxSearchDate);
            var result = occurences.Select(x => x.Period.StartTime.AsDateTimeOffset).ToList();

            if (_plan.IsIncludeNoday)
            {
                result = addNoDates(result);
            }

            if (_plan.IsAvoidHoliday)
            {
                result = filterSpecialDates(result);
            }

            result = fillterOtherDays(result);

            return result;
        }

        private List<DateTimeOffset> addNoDates(List<DateTimeOffset> dates)
        {
            foreach (var day in _plan.Days)
            {
                if (!DateTime.TryParse($"{_currentDate.Year}/{day}", out DateTime cDate))
                {
                    var month = Convert.ToInt32($"{_currentDate.Year}" + Convert.ToInt32(day.Split('/')[0]).ToString("00"));

                    var minCurrentMonthDate = new DateTime(_currentDate.Year, _currentDate.Month, 1);
                    var maxCurrentMonthDate = new DateTime(_currentDate.Year, _currentDate.Month, 1).AddMonths(1);

                    if (Convert.ToInt32(minCurrentMonthDate.ToString("yyyyMM")) <= month && month < Convert.ToInt32(maxCurrentMonthDate.ToString("yyyyMM")))
                    {
                        var lastDay = maxCurrentMonthDate.AddDays(-1);
                        if (!dates.Contains(lastDay))
                            dates.Add(lastDay);
                    }
                }
            }

            return dates;
        }
    }
}
