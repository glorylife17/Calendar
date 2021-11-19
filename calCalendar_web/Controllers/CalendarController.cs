using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using calCalendar_web.Enums;
using calCalendar_web.Models;
using culCalendar;
using culCalendar.Models;
using Microsoft.AspNetCore.Mvc;

namespace calCalendar_web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        readonly Calendar_ical _calenndar;

        public CalendarController(Calendar_ical calenndar)
        {
            _calenndar = calenndar;
        }

        /// <summary>
        /// 取得當月該相關計畫的日期群
        /// </summary>
        /// <param name="plans">計畫群</param>
        /// <param name="searchDate">查詢日期</param>
        /// <returns></returns>
        [HttpPost("GetMonthWorkDays")]
        public async Task<ReponseBaseModel<PlanDatetimeResponseModel>> GetMonthWorkDays(List<PlanPayloadModel> plans, DateTime searchDate)
        {
            try
            {
                var res = new List<PlanDatetimeResponseModel>();

                _calenndar.CurrentDate = searchDate;
                foreach (var plan in plans)
                {
                   
                    var days = _calenndar.getWorkdays(plan.ToDayParam()) ;

                    res.Add(new PlanDatetimeResponseModel
                    {
                        Id = plan.Id,
                        Days = days.ToArray()
                    });
                }


                return new ReponseBaseModel<PlanDatetimeResponseModel>
                {
                    status =  ResponseStatus.Success,
                    list = res.ToArray()
                };
            }
            catch(Exception se)
            {

                return new ReponseBaseModel<PlanDatetimeResponseModel>
                {
                    status = ResponseStatus.Error,
                    msg = se.Message
                };
            }
        }
    }
}
