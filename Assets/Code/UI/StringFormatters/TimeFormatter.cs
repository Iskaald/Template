using System;

namespace Core.UI
{
    public static class TimeFormatter
    {
        public static string FormatTime (float seconds)
		{
            var time = TimeSpan.FromSeconds(seconds);
            
            switch (seconds)
            {
                case < 60:
                    return $"{seconds:F0} sec";
                case < 3600:
                    return time.Seconds > 0 ? $"{time.Minutes} min {time.Seconds} sec" : $"{time.Minutes} min";
                case < 86400:
                    return time.Minutes > 0 || time.Seconds > 0
                        ? $"{time.Hours} hr {time.Minutes} min" 
                        : $"{time.Hours} hr";
                case < 31536000:
                    return time.Hours > 0 || time.Minutes > 0 || time.Seconds > 0
                        ? $"{time.Days} day{(time.Days > 1 ? "s" : "")}, {time.Hours} hr" 
                        : $"{time.Days} day{(time.Days > 1 ? "s" : "")}";
            }

            var years = time.Days / 365;
            var days = time.Days % 365;
            return days > 0
                ? $"{years} year{(years > 1 ? "s" : "")}, {days} day{(days > 1 ? "s" : "")}" 
                : $"{years} year{(years > 1 ? "s" : "")}";
        }
    }
}