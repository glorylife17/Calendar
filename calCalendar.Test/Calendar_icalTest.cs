using System;
using System.Collections.Generic;
using System.Linq;
using culCalendar;
using culCalendar.Enums;
using culCalendar.Models;
using NUnit.Framework;

namespace calCalendar.Test
{
    public class Calendar_icalicalTest
    {
        private Calendar_ical _func() => new Calendar_ical();
        private DateTime nowDate = new DateTime(2021, 11, 11);


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Calendar_計畫月循環時間區間20210101至20250101每月31日無此日遇假日往後移至工作日＿回傳()
        {
            var testDay = new DateTime(2022, 5, 1);
            _func().CurrentDate = testDay;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.MONTHLY,
                StartDate = new DateTime(2021, 01, 01),
                EndDate = new DateTime(2025, 01, 01),
                Period = 1,
                IsAvoidHoliday = true,
                Days = new string[] { "30" },
                IsIncludeNoday=true,
                AvoidType = AvoidType.Next
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(2022,05,02),
                new DateTime(2022,05,30),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計算日循環跨月問題_回傳排除某一天1105()
        {
            _func().CurrentDate = nowDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 3,
                IsAvoidHoliday = true,
                AvoidType = AvoidType.Next,
                RemoveDays = new DateTime[] { new DateTime(2021,11,15)}
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(2021,11,01),
                new DateTime(2021,11,03),
                new DateTime(2021,11,08),
                new DateTime(2021,11,09),
                new DateTime(2021,11,12),
                //new DateTime(2021,11,15),
                new DateTime(2021,11,18),
                new DateTime(2021,11,22),
                new DateTime(2021,11,24),
                new DateTime(2021,11,29),
                new DateTime(2021,11,30),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計算日循環跨月問題_回傳1031會移到1101()
        {
            _func().CurrentDate = nowDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.DAILY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 3,
                IsAvoidHoliday = true,
                AvoidType = AvoidType.Next
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(2021,11,01),
                new DateTime(2021,11,03),
                new DateTime(2021,11,08),
                new DateTime(2021,11,09),
                new DateTime(2021,11,12),
                new DateTime(2021,11,15),
                new DateTime(2021,11,18),
                new DateTime(2021,11,22),
                new DateTime(2021,11,24),
                new DateTime(2021,11,29),
                new DateTime(2021,11,30),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫年循環時間區間20200101至20251231每隔二年20200306日_回傳20200303有這天()
        {
            var testDate = new DateTime(2022, 3, 1);
            _func().CurrentDate = testDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.YEARLY,
                StartDate = new DateTime(2020, 01, 01),
                EndDate = new DateTime(2025, 12, 31),
                Period = 1,
                Days = new string[] { "1/8", "2/1", "2/10", "2/29", "3/6" },
                IsIncludeNoday = true,
                IsAvoidHoliday = true,
                AvoidType = AvoidType.Previous,
                Holidays = new DateTime[] { new DateTime(2022,3,4)}
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(testDate.Year,3,3)
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫年循環時間區間20200101至20251231每隔二年_回傳20200305有這天()
        {
            var testDate = new DateTime(2022, 3, 1);
            _func().CurrentDate = testDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.YEARLY,
                StartDate = new DateTime(2020, 01, 01),
                EndDate = new DateTime(2025, 12, 31),
                Period = 1,
                Days = new string[] { "1/8", "2/1", "2/10", "2/29", "3/5" },
                IsIncludeNoday = true
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(testDate.Year,3,5)
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫年循環時間區間20200101至20251231每隔二年_回傳20220229移到0228有這天()
        {
            var testDate = new DateTime(2022, 2, 1);
            _func().CurrentDate = testDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.YEARLY,
                StartDate = new DateTime(2020, 01, 01),
                EndDate = new DateTime(2025, 12, 31),
                Period = 1,
                Days = new string[] { "1/8","2/1", "2/10", "2/29","3/5" },
                IsIncludeNoday = true
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(testDate.Year,2,1),
                new DateTime(testDate.Year,2,10),
                new DateTime(testDate.Year,2,28),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫年循環時間區間20200101至20251231每隔二年_回傳跳過20220229有這天()
        {
            var testDate = new DateTime(2022, 2, 1);
            _func().CurrentDate = testDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.YEARLY,
                StartDate = new DateTime(2020, 01, 01),
                EndDate = new DateTime(2025, 12, 31),
                Period = 1,
                Days = new string[] { "2/1", "2/10", "2/29" },
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(testDate.Year,2,1),
                new DateTime(testDate.Year,2,10),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫年循環時間區間20200101至20251231每隔二年_回傳20220229有這天()
        {
            var testDate = new DateTime(2024, 2, 1);
            _func().CurrentDate = testDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.YEARLY,
                StartDate = new DateTime(2020, 01, 01),
                EndDate = new DateTime(2025, 12, 31),
                Period = 1,
                Days = new string[] { "2/1", "2/10", "2/29" },
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(testDate.Year,2,1),
                new DateTime(testDate.Year,2,10),
                new DateTime(testDate.Year,2,29),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫年循環時間區間20200101至20251231每隔二年_Exception()
        {
            _func().CurrentDate = nowDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.YEARLY,
                StartDate = new DateTime(2020, 10, 01),
                EndDate = new DateTime(2025, 12, 31),
                Period = 2,
            };

            var res = Assert.Throws<Exception>(() => _func().getWorkdays(param));
            StringAssert.Contains("沒有設定日期", res.Message);
        }

        [Test]
        public void Calendar_ical計畫月循環時間區間20210101至20211231每隔一月30日_回傳2月沒這天()
        {
            var testDate = new DateTime(2021, 2, 5);
            _func().CurrentDate = testDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.MONTHLY,
                StartDate = new DateTime(2021, 01, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 1,
                Days = new string[] { "3", "10", "30" },
                IsAvoidHoliday = true,
                IsIncludeNoday = false,
                AvoidType = AvoidType.Previous,
                Holidays = new DateTime[] { new DateTime(2021, 02, 28) }
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(2021,02,03),
                new DateTime(2021,02,10),
                //new DateTime(2021,02,28),   //假設0228為假日 0227(六) 往後算一天但遇到六日
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫月循環時間區間20210101至20211231每隔一月30日假設假日0228_回傳有0226()
        {
            var testDate = new DateTime(2021, 2, 5);
            _func().CurrentDate = testDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.MONTHLY,
                StartDate = new DateTime(2021, 01, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 1,
                Days = new string[] { "3", "10", "30" },
                IsAvoidHoliday = true,
                IsIncludeNoday = true,
                AvoidType = AvoidType.Previous,
                Holidays = new DateTime[] { new DateTime(2021, 02, 28) }
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(2021,02,03),
                new DateTime(2021,02,10),
                new DateTime(2021,02,26),   
                //new DateTime(2021,02,28),   //假設0228為假日 0227(六) 往後算一天但遇到六日
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫月循環時間區間20210101至20211231每隔一月30日_回傳有0228()
        {
            var testDate = new DateTime(2021, 2, 5);
            _func().CurrentDate = testDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.MONTHLY,
                StartDate = new DateTime(2021, 01, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 1,
                Days = new string[] { "3", "10", "30" },
                IsIncludeNoday = true
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(2021,02,03),
                new DateTime(2021,02,10),
                new DateTime(2021,02,28),   //假設1126為假日  往後算一天但遇到六日
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫月循環時間區間20211001至20211218每隔一月3日_回傳有1103()
        {
            _func().CurrentDate = nowDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.MONTHLY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 1,
                Days = new string[] { "3", "10","26" }
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(2021,11,03),
                new DateTime(2021,11,10),
                new DateTime(2021,11,26),   //假設1126為假日  往後算一天但遇到六日
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫月循環時間區間20211001至20211218每隔一月沒設定日_Exception()
        {
            _func().CurrentDate = nowDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.MONTHLY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 1,
            };

            var res = Assert.Throws<Exception>(() => _func().getWorkdays(param));
            StringAssert.Contains("沒有設定日子", res.Message);
        }

        [Test]
        public void Calendar_ical計畫周循環時間區間20211001至20211218每隔二週星期一三五遇到假日為1126＿回傳往後1129()
        {
            _func().CurrentDate = nowDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.WEEKLY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 2,
                Days = new string[] { "1", "3", "5" },
                IsAvoidHoliday = true,
                AvoidType = AvoidType.Next,
                Holidays = new DateTime[] { new DateTime(2021, 11, 26) }
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(2021,11,08),
                new DateTime(2021,11,10),
                new DateTime(2021,11,12),
                new DateTime(2021,11,22),
                new DateTime(2021,11,24),
                //new DateTime(2021,11,26),   //假設1126為假日  往後算一天但遇到六日
                new DateTime(2021,11,29),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫周循環時間區間20211001至20211218每隔二週星期一三五遇到假日為1126＿回傳往前1125 ()
        {
            _func().CurrentDate = nowDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.WEEKLY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 2,
                Days = new string[] { "1", "3", "5" },
                IsAvoidHoliday = true,
                AvoidType = AvoidType.Previous,
                Holidays = new DateTime[] { new DateTime(2021, 11, 26) }
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(2021,11,08),
                new DateTime(2021,11,10),
                new DateTime(2021,11,12),
                new DateTime(2021,11,22),
                new DateTime(2021,11,24),
                new DateTime(2021,11,25),
                //new DateTime(2021,11,26),   //假設1126為假日  往前算一天
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫周循環時間區間20211001至20211218每隔二週星期一三六＿回傳共6天()
        {
            _func().CurrentDate = nowDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.WEEKLY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 2,
                Days= new string[] { "0","3","6"}
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(2021,11,07),
                new DateTime(2021,11,10),
                new DateTime(2021,11,13),
                new DateTime(2021,11,21),
                new DateTime(2021,11,24),
                new DateTime(2021,11,27),
            };

            Assert.AreEqual(expect.Count, res.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i], res[i]);
            }
        }

        [Test]
        public void Calendar_ical計畫周循環時間區間20211001至20211218每隔一週沒設定週期_Exception()
        {
            _func().CurrentDate = nowDate;

            var param = new DayParamModel
            {
                RecurringType = RecurringType.WEEKLY,
                StartDate = new DateTime(2021, 10, 01),
                EndDate = new DateTime(2021, 12, 31),
                Period = 1,
            };

            var res = Assert.Throws<Exception>(() => _func().getWorkdays(param));
            StringAssert.Contains("沒有設定星期",res.Message);
        }

        [Test]
        public void Calendar_ical計畫日循環時間區間20211001至20211218每隔三天假設假日為20211125與26＿回傳六日及假日往前算24為工作天不再多計算()
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
                Holidays = new DateTime[] { new DateTime(2021, 11, 25), new DateTime(2021, 11, 26) },
                AvoidType = AvoidType.Previous
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
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
        public void Calendar_ical計畫日循環時間區間20211001至20211218每隔三天假設假日為20211126＿回傳遇到六日及假日往前算()
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
                Holidays = new DateTime[] { new DateTime(2021, 11, 26) },
                AvoidType = AvoidType.Previous
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
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
        public void Calendar_ical計畫日循環時間區間20211001至20211218每隔三天＿回傳遇到六日往前算()
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
            var expect = new List<DateTimeOffset>()
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
        public void Calendar_ical計畫日循環時間區間20211001至20211218每隔三天＿回傳遇到六日往後算()
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
                Holidays = new DateTime[] { },
                AvoidType = AvoidType.Next
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
            {
                new DateTime(2021,11,01),
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
        public void Calendar_ical計畫日循環時間區間20211001至20211218每隔三天假設1127為假日＿回傳忽略假日()
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
                AvoidType = AvoidType.Ignore
            };

            var res = _func().getWorkdays(param);
            var expect = new List<DateTimeOffset>()
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
        public void Calendar_ical計畫日循環時間區間20211001至20211119每隔三天＿回傳共6天()
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
            var expect = new List<DateTimeOffset>()
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
        public void Calendar_ical計畫日循環時間區間20211001至20211231每隔三天＿回傳共10天()
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
            var expect = new List<DateTimeOffset>()
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
        public void Calendar_ical計畫日循環時間區間20211001至20211231每隔三天20251103＿沒有天數()
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

            var expect = 0;
            var res =  _func().getWorkdays(param);
            Assert.AreEqual(expect, res.Count);
        }

        [Test]
        public void Calendar_ical計畫日循環時間區間1001至1231每隔三天1103＿回傳有工作()
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

            var days = _func().getWorkdays(param);
            var expect = testDay;

            Assert.IsTrue(days.Contains(expect));
        }

        [Test]
        public void Calendar_ical計畫日循環時間區間0101至1201避開假日1113六_回傳沒這天()
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
        public void Calendar_ical計畫無循環回傳該月計畫中的時間區間1104至1107_回傳4天()
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
        public void Calendar_ical計畫無循環回傳11月天數_回傳30天()
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
        public void Calendar_ical計畫沒有結束日期_Exception()
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
        public void Calendar_ical計劃內容沒有開始日期_Exception()
        {
            var param = new DayParamModel
            {
                RecurringType = RecurringType.None,
            };
            var res = Assert.Throws<Exception>(() => _func().getWorkdays(param));

            StringAssert.Contains("計畫資訊不", res.Message);
        }

        [Test]
        public void Calendar_ical沒有計劃內容_Exception()
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
