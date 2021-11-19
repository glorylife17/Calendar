using System;
using System.Collections.Generic;
using System.Linq;
using culCalendar.Abstracts;
using Ical.Net.DataTypes;

namespace culCalendar.Classes
{
    public class RecurringDailyClass: ARecurringDays
    {
        public override List<DateTimeOffset> GetDays()
        {
            var rPattern = new RecurrencePattern();
            rPattern.Frequency = Ical.Net.FrequencyType.Daily;
            rPattern.Interval = _plan.Period;
            rPattern.Until = _plan.EndDate;

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
