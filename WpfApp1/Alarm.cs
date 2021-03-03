using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmClock
{
    public class Alarm
    {
        public DateTime DateAndTime { get; set; }
        public string Status { get; set; } = "Waiting";
    }
}
