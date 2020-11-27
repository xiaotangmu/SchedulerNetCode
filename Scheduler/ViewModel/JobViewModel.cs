using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.ViewModel
{
    public class JobViewModel
    {
        public string JobName { get; set; }
        public string GroupName { get; set; }
        public string Cron { get; set; }
        public int Type { get; set; }
    }
}
