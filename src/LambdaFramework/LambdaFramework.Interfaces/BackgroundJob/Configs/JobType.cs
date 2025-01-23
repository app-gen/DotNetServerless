using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command.Job;

public enum JobType
{
    BackgroundJob = 1, // Any multithreaded job -- supports pause between runs
    ScheduledTask = 2, // Any scheduled job 
    MsgQueueClient =3  // pick a msg and send it to plugin for process, pass IMessageEntry and Look for errors
        ,
    IQuartzJob =4
}
