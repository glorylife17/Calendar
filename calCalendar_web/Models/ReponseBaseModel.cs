using System;
using calCalendar_web.Enums;

namespace calCalendar_web.Models
{
    public class ReponseBaseModel<T>
    {
        public ResponseStatus status { get; set; }

        public string msg { get; set; }

        public T[] list { get; set; }
    }
}
