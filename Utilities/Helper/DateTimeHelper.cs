﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Helper
{
    public static class DateTimeHelper
    {
        //public static DateTime VnNow { get { return DateTime.UtcNow.AddHours(7); } }

        //public static string ToRoundedSecondsTimeString(this TimeSpan? time)
        //{
        //    if (time == null)
        //    {
        //        return null;
        //    }

        //    var rounded = Math.Round(time.Value.TotalSeconds);
        //    return TimeSpan.FromSeconds(rounded).ToString();
        //}

        //public static bool IsToday(this DateTime date)
        //{
        //    return date.Date.Equals(DateTimeHelper.VnNow.Date);
        //}
        
        public static bool FailDateTime(DateTime endTime, TimeSpan minConflict = default(TimeSpan))
        {
            return !(endTime > DateTime.UtcNow + minConflict);
        }

        public static bool ConflictDateTime(DateTime startTime, DateTime? endTime,
            TimeSpan minConflict = default(TimeSpan))
        {
            return !(endTime > startTime + minConflict);
        }

        public static bool DuplicateDateTime(DateTime startTime1, DateTime endTime1, DateTime startTime2,
            DateTime endTime2)
        {
            return !(endTime1 > startTime2 && startTime1 < endTime2);
        }
    }
}
