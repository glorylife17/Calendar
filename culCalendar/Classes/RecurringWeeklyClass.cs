using System;
using System.Collections.Generic;
using System.Linq;
using culCalendar.Abstracts;
using Ical.Net.DataTypes;

namespace culCalendar.Classes
{
    public class RecurringWeeklyClass : ARecurringDays
    {

        public override List<DateTimeOffset> GetDays()
        {
            if (_plan.Days == null || !_plan.Days.Any())
                throw new Exception("計畫資訊沒有設定星期");

            var rPattern = new RecurrencePattern();
            rPattern.Frequency = Ical.Net.FrequencyType.Weekly;
            rPattern.Interval = _plan.Period;
            rPattern.Until = _plan.EndDate;
            rPattern.FirstDayOfWeek = DayOfWeek.Sunday;

            foreach (var day in _plan.Days)
            {
                rPattern.ByDay.Add(new WeekDay((DayOfWeek)Convert.ToInt32(day)));
            }

            var recComp = new Ical.Net.CalendarComponents.RecurringComponent();
            recComp.RecurrenceRules.Add(rPattern);
            recComp.Start = new Ical.Net.DataTypes.CalDateTime(_plan.StartDate);

            var occurences = recComp.GetOccurrences(_minSearchDate, _maxSearchDate);
            var result = occurences.Select(x => x.Period.StartTime.AsDateTimeOffset).ToList();
            if (_plan.IsAvoidHoliday)
            {
                result = filterSpecialDates(result);
            }

            result = fillterOtherDays(result);

            return result;
        }
    }
}
