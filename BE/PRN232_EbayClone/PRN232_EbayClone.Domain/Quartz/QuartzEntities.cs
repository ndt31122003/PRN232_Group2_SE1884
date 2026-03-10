namespace PRN232_EbayClone.Domain.Quartz;

public class QrtzJobDetail
{
    public string SchedName { get; set; } = null!;
    public string JobName { get; set; } = null!;
    public string JobGroup { get; set; } = null!;
    public string? Description { get; set; }
    public string JobClassName { get; set; } = null!;
    public string IsDurable { get; set; } = null!;
    public string IsNonconcurrent { get; set; } = null!;
    public string IsUpdateData { get; set; } = null!;
    public string? RequestsRecovery { get; set; }
    public byte[]? JobData { get; set; }
}

public class QrtzTrigger
{
    public string SchedName { get; set; } = null!;
    public string TriggerName { get; set; } = null!;
    public string TriggerGroup { get; set; } = null!;
    public string JobName { get; set; } = null!;
    public string JobGroup { get; set; } = null!;
    public string? Description { get; set; }
    public long? NextFireTime { get; set; }
    public long? PrevFireTime { get; set; }
    public int? Priority { get; set; }
    public string TriggerState { get; set; } = null!;
    public string TriggerType { get; set; } = null!;
    public long StartTime { get; set; }
    public long? EndTime { get; set; }
    public string? CalendarName { get; set; }
    public short? MisfireInstr { get; set; }
    public byte[]? JobData { get; set; }
}

public class QrtzSimpleTrigger
{
    public string SchedName { get; set; } = null!;
    public string TriggerName { get; set; } = null!;
    public string TriggerGroup { get; set; } = null!;
    public long RepeatCount { get; set; }
    public long RepeatInterval { get; set; }
    public long TimesTriggered { get; set; }
}

public class QrtzCronTrigger
{
    public string SchedName { get; set; } = null!;
    public string TriggerName { get; set; } = null!;
    public string TriggerGroup { get; set; } = null!;
    public string CronExpression { get; set; } = null!;
    public string? TimeZoneId { get; set; }
}

public class QrtzBlobTrigger
{
    public string SchedName { get; set; } = null!;
    public string TriggerName { get; set; } = null!;
    public string TriggerGroup { get; set; } = null!;
    public byte[]? BlobData { get; set; }
}

public class QrtzCalendar
{
    public string SchedName { get; set; } = null!;
    public string CalendarName { get; set; } = null!;
    public byte[] Calendar { get; set; } = null!;
}

public class QrtzPausedTriggerGrp
{
    public string SchedName { get; set; } = null!;
    public string TriggerGroup { get; set; } = null!;
}

public class QrtzFiredTrigger
{
    public string SchedName { get; set; } = null!;
    public string EntryId { get; set; } = null!;
    public string TriggerName { get; set; } = null!;
    public string TriggerGroup { get; set; } = null!;
    public string InstanceName { get; set; } = null!;
    public long FiredTime { get; set; }
    public long SchedTime { get; set; }
    public int Priority { get; set; }
    public string State { get; set; } = null!;
    public string? JobName { get; set; }
    public string? JobGroup { get; set; }
    public string? IsNonconcurrent { get; set; }
    public string? RequestsRecovery { get; set; }
}

public class QrtzSchedulerState
{
    public string SchedName { get; set; } = null!;
    public string InstanceName { get; set; } = null!;
    public long LastCheckinTime { get; set; }
    public long CheckinInterval { get; set; }
}

public class QrtzLock
{
    public string SchedName { get; set; } = null!;
    public string LockName { get; set; } = null!;
}
