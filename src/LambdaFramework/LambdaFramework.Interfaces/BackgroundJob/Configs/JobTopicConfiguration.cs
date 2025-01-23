using MsgQueue.Client.Service;

namespace Command.Job;

public partial class JobTopicConfiguration: ITopicConfig
{
    public string Id { get; set; }
    public JobType Type { get; set; }
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public string AssemblyName { get; set; }
    public string TypeName { get; set; }

    // Background Job specific
    public string QueueName { get; set; }

    public int MaxThreads { get; set; } = 1;
    public int EmptyQueueWaitTimeSeconds { get; set; } = 60;

    public int DelayInMilliSecondForEveryRun { get; set; } = 10000; //can be 0


    // Scheduled Task specific
    public string CronExpression { get; set; }

    public int PollTimeInMilliSecond { get; internal set; } // 1000 *60 =  1 min



    //ITopic/Job Config

    public string TopicId { get; set; }
    public string? TopicName { get; set; }

    public int MaxTasks { get; set; }
    public int WaitTimeInSecondsIfEmpty { get; set; }

    public string? Comments { get; set; }

    public string? CommandName { get; set; }
    public string? ActionName { get; set; }
    public string? TenantName { get; set; }
    public string? CommandVersion { get; set; }

    public bool? Active { get; set; }
    // not needed
    public DateTime? StartTimeUtc { get; set; }
    public DateTime? EndTimeUtc { get; set; }
    //Command 

   





    // Pause time- Planned Downtimes
    public DateTime? OneTimeStartTime { get; set; }
    public DateTime? OneTimeEndTime { get; set; }
    public int? RecurringDay { get; set; }
    public string? RecurringStartTime { get; set; }
    public string? RecurringEndTime { get; set; }


    TimeSpan TimeSpanFromString(string timeStr)
    {
        if (string.IsNullOrWhiteSpace(timeStr)) return TimeSpan.Zero;

        string[] parts = timeStr.Split(':');
        if (parts.Length != 2)
            throw new ArgumentException("Invalid time format. Use HH:MM format (e.g. '13:45')");

        if (!int.TryParse(parts[0], out int hours) || !int.TryParse(parts[1], out int minutes))
            throw new ArgumentException("Hours and minutes must be valid numbers");

        if (hours < 0 || hours > 23)
            throw new ArgumentException("Hours must be between 0 and 23");

        if (minutes < 0 || minutes > 59)
            throw new ArgumentException("Minutes must be between 0 and 59");

        return TimeSpan.FromMinutes(hours * 60 + minutes);

    }


    public bool IsInPauseWindow(DateTime currentTime)
    {
        if (OneTimeStartTime.HasValue && OneTimeEndTime.HasValue)
        {
            if (currentTime >= OneTimeStartTime && currentTime <= OneTimeEndTime)
                return true;
        }

        if (RecurringDay.HasValue && !string.IsNullOrEmpty(RecurringStartTime) && !string.IsNullOrEmpty(RecurringEndTime))
        {
            int rDay = RecurringDay ?? -1;
            var ts_start = TimeSpanFromString(RecurringStartTime);
            var ts_end = TimeSpanFromString(RecurringEndTime);

            return currentTime.DayOfWeek == (DayOfWeek) rDay &&
                   currentTime.TimeOfDay >= ts_start &&
                   currentTime.TimeOfDay <= ts_end;
        }

        return false;
    }




 

}
