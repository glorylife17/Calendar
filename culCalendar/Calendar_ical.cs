using System;
using System.Collections.Generic;
using System.Linq;
using culCalendar.Abstracts;
using culCalendar.Classes;
using culCalendar.Enums;
using culCalendar.Models;

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

        private DateTime minSearchMonthDate => new DateTime(CurrentDate.Year, CurrentDate.Month, 1).AddMonths(-1);
        private DateTime maxSearchMonthDate => minSearchMonthDate.AddMonths(3);
    
        public List<DateTimeOffset> getWorkdays(DayParamModel plan)
        {
            _plan = plan;

            if (NoSchedulePlan())
                throw new Exception("計畫資訊不完整");

            var monthDays = getCurrentMonthPlanDays();
            if (!monthDays.Any())
                return new List<DateTimeOffset>();

            switch (plan.RecurringType)
            {
                case RecurringType.None:
                    return monthDays;

                default:
                    return getRecurringDays();
            }
        }

        private List<DateTimeOffset> getRecurringDays()
        {
            var recurringDays = RecurringFactory(_plan.RecurringType);

            if (recurringDays != null)
            {
                recurringDays.SetPlan(_plan);
                recurringDays.SetSearchRange(CurrentDate);

                return recurringDays.GetDays();
            }
            return new List<DateTimeOffset>();
        }

        private ARecurringDays RecurringFactory(RecurringType type)
        {
          
            switch (type)
            {
                case RecurringType.DAILY:
                    return new RecurringDailyClass();

                case RecurringType.WEEKLY:
                    return new RecurringWeeklyClass();

                case RecurringType.MONTHLY:
                    return new RecurringMonthlyClass();

                case RecurringType.YEARLY:
                    return new RecurringYearlyClass();
            }

            return null;
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
            var min = (minSearchMonthDate < _plan.StartDate) ? _plan.StartDate : minSearchMonthDate;
            var max = (maxSearchMonthDate < _plan.EndDate) ? maxSearchMonthDate : _plan.EndDate;
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
