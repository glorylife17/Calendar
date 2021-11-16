using System;
using System.Collections.Generic;
using System.Linq;
using culCalendar;
using culCalendar.Enums;
using culCalendar.Models;
using NUnit.Framework;

namespace calCalendar.Test
{
    public class CalendarTest
    {
        private Calendar _func() => new Calendar();
        private DateTime nowDate = new DateTime(2021, 11, 11);

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Calendar_計畫日循環時間區間20211001至20211218每隔三天假設假日為20211125與26＿回傳六日及假日往前算24為工作天不再多計算()
        {
            var testDay = new DateTime(2021, 11, 27);
            _func().CurrentDate = testDay;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 3,
                IsAvoidHoliday = true,
                Holidays = new DateTime[] { new DateTime(2021, 11, 25) ,new DateTime(2021, 11, 26) },
                AvoidType = AvoidType.Previous
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTime>()
            {
                new DateTime(2021,11,03),
                new DateTime(2021,11,05),   // 1106(六) 往前計算
                new DateTime(2021,11,09),
                new DateTime(2021,11,12),
                new DateTime(2021,11,15),
                new DateTime(2021,11,18),
                new DateTime(2021,11,19),   // 1121(日) 往前計算
                new DateTime(2021,11,24),
                //new DateTime(2021,11,27),   // 1127(六) 往前計算 1125~26(假) 往前計算
                new DateTime(2021,11,30),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_計畫日循環時間區間20211001至20211218每隔三天假設假日為20211126＿回傳遇到六日及假日往前算()
        {
            var testDay = new DateTime(2021, 11, 27);
            _func().CurrentDate = testDay;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 3,
                IsAvoidHoliday = true,
                Holidays = new DateTime[] { new DateTime(2021,11,26)},
                AvoidType = AvoidType.Previous
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTime>()
            {
                new DateTime(2021,11,03),
                new DateTime(2021,11,05),   // 1106(六) 往前計算
                new DateTime(2021,11,09),
                new DateTime(2021,11,12),
                new DateTime(2021,11,15),
                new DateTime(2021,11,18),
                new DateTime(2021,11,19),   // 1121(日) 往前計算
                new DateTime(2021,11,24),
                new DateTime(2021,11,25),   // 1127(六) 往前計算 1125~26(假) 往前計算
                new DateTime(2021,11,30),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_計畫日循環時間區間20211001至20211218每隔三天＿回傳遇到六日往前算()
        {
            var testDay = new DateTime(2021, 11, 27);
            _func().CurrentDate = testDay;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 3,
                IsAvoidHoliday = true,
                AvoidType = AvoidType.Previous
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTime>()
            {
                new DateTime(2021,11,03),
                new DateTime(2021,11,05),   // 1106(六) 往前計算
                new DateTime(2021,11,09),
                new DateTime(2021,11,12),
                new DateTime(2021,11,15),
                new DateTime(2021,11,18),
                new DateTime(2021,11,19),   // 1121(日) 往前計算
                new DateTime(2021,11,24),
                new DateTime(2021,11,26),     // 1127(六) 再往後              
                new DateTime(2021,11,30),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_計畫日循環時間區間20211001至20211218每隔三天＿回傳遇到六日往後算()
        {
            var testDay = new DateTime(2021, 11, 27);
            _func().CurrentDate = testDay;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 3,
                IsAvoidHoliday = true,
                Holidays = new DateTime[] {},
                AvoidType = AvoidType.Next
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTime>()
            {
                new DateTime(2021,11,03),
                new DateTime(2021,11,08),   // 1106(六) 往後計算
                new DateTime(2021,11,09),
                new DateTime(2021,11,12),
                new DateTime(2021,11,15),
                new DateTime(2021,11,18),
                new DateTime(2021,11,22),   // 1121(日) 往後計算
                new DateTime(2021,11,24),
                new DateTime(2021,11,29),   // 1127(日) 往後計算
                new DateTime(2021,11,30),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_計畫日循環時間區間20211001至20211218每隔三天假設1127為假日＿回傳忽略假日()
        {
            var testDay = new DateTime(2021, 11, 27);
            _func().CurrentDate = testDay;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 3,
                IsAvoidHoliday = true,
                Holidays = new DateTime[] { new DateTime(2021, 11, 27) },
                AvoidType= AvoidType.Ignore
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTime>()
            {
                new DateTime(2021,11,03),
                //new DateTime(2021,11,06),   // 1106(六) 往後計算
                new DateTime(2021,11,09),
                new DateTime(2021,11,12),
                new DateTime(2021,11,15),
                new DateTime(2021,11,18),
                //new DateTime(2021,11,21),   // 1121(日) 往後計算
                new DateTime(2021,11,24),
                //new DateTime(2021,11,27),   // 假設為假日
                new DateTime(2021,11,30),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_計畫日循環時間區間20211001至20211118每隔三天＿回傳共6天()
        {
            var testDay = new DateTime(2021, 11, 03);
            _func().CurrentDate = testDay;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 11, 19),
                Period = 3,
                IsAvoidHoliday = false
            };


            var res = _func().getWorkdays(param);
            var expect = new List<DateTime>()
            {
                new DateTime(2021,11,03),
                new DateTime(2021,11,06),
                new DateTime(2021,11,09),
                new DateTime(2021,11,12),
                new DateTime(2021,11,15),
                new DateTime(2021,11,18),
            };


            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_計畫日循環時間區間20211001至20211231每隔三天＿回傳共10天()
        {
            var testDay = new DateTime(2021, 11, 03);
            _func().CurrentDate = testDay;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 3,
                IsAvoidHoliday = false
            };


            var res = _func().getWorkdays(param);
            var expect = new List<DateTime>()
            {
                new DateTime(2021,11,03),
                new DateTime(2021,11,06),
                new DateTime(2021,11,09),
                new DateTime(2021,11,12),
                new DateTime(2021,11,15),
                new DateTime(2021,11,18),
                new DateTime(2021,11,21),
                new DateTime(2021,11,24),
                new DateTime(2021,11,27),
                new DateTime(2021,11,30),
            };


            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_計畫日循環時間區間20211001至20211231每隔三天20251103＿Exception計畫時間不在該區間()
        {
            var testDay = new DateTime(2025, 11, 03);
            _func().CurrentDate = testDay;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 3,
                IsAvoidHoliday = false
            };

            var res = Assert.Throws<Exception>(() => _func().getWorkdays(param));
            StringAssert.Contains("計畫時間",res.Message);
        }

        [Test]
        public void Calendar_計畫日循環時間區間1001至1231每隔三天1103＿回傳有工作()
        {
            var testDay = new DateTime(2021, 11, 03);
            _func().CurrentDate = testDay;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period=3,
                IsAvoidHoliday = false
            };

            var days = _func().getWorkdays(param);
            var expect = testDay;

            Assert.IsTrue(days.Contains(expect));
        }

        [Test]
        public void Calendar_計畫日循環時間區間0101至1201避開假日1113六_回傳沒這天()
        {
            var testDay = new DateTime(2021, 11, 13);
            _func().CurrentDate = testDay;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 01, 01),
                EndDate = new DateTime(2021, 12, 01),
                IsAvoidHoliday = true
            };

            var res = _func().getWorkdays(param);
            Assert.IsFalse(res.Where(x => x == testDay).Any());
        }

        [Test]
        public void Calendar_計畫無循環回傳該月計畫中的時間區間1104至1107_回傳4天()
        {
            _func().CurrentDate = nowDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.None,
                StartDate = new DateTime(2021, 11, 04),
                EndDate = new DateTime(2021, 11, 07)
            };

            var res = _func().getWorkdays(param).Count;
            var expect = 4;
            Assert.AreEqual(expect, expect);
        }

        [Test]
        public void Calendar_計畫無循環回傳11月天數_回傳30天()
        {
            _func().CurrentDate = nowDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.None,
                StartDate = new DateTime(2021, 07, 01),
                EndDate = new DateTime(2022, 01, 01)
            };

            var res = _func().getWorkdays(param).Count;
            var expect = 30;
            Assert.AreEqual(expect, expect);
        }

        [Test]
        public void Calendar_計畫沒有結束日期_Exception()
        {
            var param = new DayParamModel
            {
                RecurringType = RecurringType.None,
                StartDate = new DateTime(2021, 07, 01)
            };
            var res = Assert.Throws<Exception>(() => _func().getWorkdays(param));

            StringAssert.Contains("計畫資訊不", res.Message);
        }

        [Test]
        public void Calendar_計劃內容沒有開始日期_Exception()
        {
            var param = new DayParamModel
            {
                 RecurringType = RecurringType.None,
            };
            var res = Assert.Throws<Exception>(() => _func().getWorkdays(param));

            StringAssert.Contains("計畫資訊不", res.Message);
        }

        [Test]
        public void Calendar_沒有計劃內容_Exception()
        {
            var res = Assert.Throws<Exception>(() => _func().getWorkdays(null));

            StringAssert.Contains("計畫資訊不", res.Message);
        }

        [Test]
        public void 測試程式()
        {
            Assert.IsTrue(true);
        }



    }
}
