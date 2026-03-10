using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuartzColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "qrtz_triggers",
                newName: "priority");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "qrtz_triggers",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "TriggerType",
                table: "qrtz_triggers",
                newName: "trigger_type");

            migrationBuilder.RenameColumn(
                name: "TriggerState",
                table: "qrtz_triggers",
                newName: "trigger_state");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "qrtz_triggers",
                newName: "start_time");

            migrationBuilder.RenameColumn(
                name: "PrevFireTime",
                table: "qrtz_triggers",
                newName: "prev_fire_time");

            migrationBuilder.RenameColumn(
                name: "NextFireTime",
                table: "qrtz_triggers",
                newName: "next_fire_time");

            migrationBuilder.RenameColumn(
                name: "MisfireInstr",
                table: "qrtz_triggers",
                newName: "misfire_instr");

            migrationBuilder.RenameColumn(
                name: "JobName",
                table: "qrtz_triggers",
                newName: "job_name");

            migrationBuilder.RenameColumn(
                name: "JobGroup",
                table: "qrtz_triggers",
                newName: "job_group");

            migrationBuilder.RenameColumn(
                name: "JobData",
                table: "qrtz_triggers",
                newName: "job_data");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "qrtz_triggers",
                newName: "end_time");

            migrationBuilder.RenameColumn(
                name: "CalendarName",
                table: "qrtz_triggers",
                newName: "calendar_name");

            migrationBuilder.RenameColumn(
                name: "TriggerGroup",
                table: "qrtz_triggers",
                newName: "trigger_group");

            migrationBuilder.RenameColumn(
                name: "TriggerName",
                table: "qrtz_triggers",
                newName: "trigger_name");

            migrationBuilder.RenameColumn(
                name: "SchedName",
                table: "qrtz_triggers",
                newName: "sched_name");

            migrationBuilder.RenameIndex(
                name: "IX_qrtz_triggers_SchedName_JobName_JobGroup",
                table: "qrtz_triggers",
                newName: "IX_qrtz_triggers_sched_name_job_name_job_group");

            migrationBuilder.RenameColumn(
                name: "TimesTriggered",
                table: "qrtz_simple_triggers",
                newName: "times_triggered");

            migrationBuilder.RenameColumn(
                name: "RepeatInterval",
                table: "qrtz_simple_triggers",
                newName: "repeat_interval");

            migrationBuilder.RenameColumn(
                name: "RepeatCount",
                table: "qrtz_simple_triggers",
                newName: "repeat_count");

            migrationBuilder.RenameColumn(
                name: "TriggerGroup",
                table: "qrtz_simple_triggers",
                newName: "trigger_group");

            migrationBuilder.RenameColumn(
                name: "TriggerName",
                table: "qrtz_simple_triggers",
                newName: "trigger_name");

            migrationBuilder.RenameColumn(
                name: "SchedName",
                table: "qrtz_simple_triggers",
                newName: "sched_name");

            migrationBuilder.RenameColumn(
                name: "LastCheckinTime",
                table: "qrtz_scheduler_state",
                newName: "last_checkin_time");

            migrationBuilder.RenameColumn(
                name: "CheckinInterval",
                table: "qrtz_scheduler_state",
                newName: "checkin_interval");

            migrationBuilder.RenameColumn(
                name: "InstanceName",
                table: "qrtz_scheduler_state",
                newName: "instance_name");

            migrationBuilder.RenameColumn(
                name: "SchedName",
                table: "qrtz_scheduler_state",
                newName: "sched_name");

            migrationBuilder.RenameColumn(
                name: "TriggerGroup",
                table: "qrtz_paused_trigger_grps",
                newName: "trigger_group");

            migrationBuilder.RenameColumn(
                name: "SchedName",
                table: "qrtz_paused_trigger_grps",
                newName: "sched_name");

            migrationBuilder.RenameColumn(
                name: "LockName",
                table: "qrtz_locks",
                newName: "lock_name");

            migrationBuilder.RenameColumn(
                name: "SchedName",
                table: "qrtz_locks",
                newName: "sched_name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "qrtz_job_details",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "RequestsRecovery",
                table: "qrtz_job_details",
                newName: "requests_recovery");

            migrationBuilder.RenameColumn(
                name: "JobData",
                table: "qrtz_job_details",
                newName: "job_data");

            migrationBuilder.RenameColumn(
                name: "JobClassName",
                table: "qrtz_job_details",
                newName: "job_class_name");

            migrationBuilder.RenameColumn(
                name: "IsUpdateData",
                table: "qrtz_job_details",
                newName: "is_update_data");

            migrationBuilder.RenameColumn(
                name: "IsNonconcurrent",
                table: "qrtz_job_details",
                newName: "is_nonconcurrent");

            migrationBuilder.RenameColumn(
                name: "IsDurable",
                table: "qrtz_job_details",
                newName: "is_durable");

            migrationBuilder.RenameColumn(
                name: "JobGroup",
                table: "qrtz_job_details",
                newName: "job_group");

            migrationBuilder.RenameColumn(
                name: "JobName",
                table: "qrtz_job_details",
                newName: "job_name");

            migrationBuilder.RenameColumn(
                name: "SchedName",
                table: "qrtz_job_details",
                newName: "sched_name");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "qrtz_fired_triggers",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "qrtz_fired_triggers",
                newName: "priority");

            migrationBuilder.RenameColumn(
                name: "TriggerName",
                table: "qrtz_fired_triggers",
                newName: "trigger_name");

            migrationBuilder.RenameColumn(
                name: "TriggerGroup",
                table: "qrtz_fired_triggers",
                newName: "trigger_group");

            migrationBuilder.RenameColumn(
                name: "SchedTime",
                table: "qrtz_fired_triggers",
                newName: "sched_time");

            migrationBuilder.RenameColumn(
                name: "RequestsRecovery",
                table: "qrtz_fired_triggers",
                newName: "requests_recovery");

            migrationBuilder.RenameColumn(
                name: "JobName",
                table: "qrtz_fired_triggers",
                newName: "job_name");

            migrationBuilder.RenameColumn(
                name: "JobGroup",
                table: "qrtz_fired_triggers",
                newName: "job_group");

            migrationBuilder.RenameColumn(
                name: "IsNonconcurrent",
                table: "qrtz_fired_triggers",
                newName: "is_nonconcurrent");

            migrationBuilder.RenameColumn(
                name: "InstanceName",
                table: "qrtz_fired_triggers",
                newName: "instance_name");

            migrationBuilder.RenameColumn(
                name: "FiredTime",
                table: "qrtz_fired_triggers",
                newName: "fired_time");

            migrationBuilder.RenameColumn(
                name: "EntryId",
                table: "qrtz_fired_triggers",
                newName: "entry_id");

            migrationBuilder.RenameColumn(
                name: "SchedName",
                table: "qrtz_fired_triggers",
                newName: "sched_name");

            migrationBuilder.RenameColumn(
                name: "TimeZoneId",
                table: "qrtz_cron_triggers",
                newName: "time_zone_id");

            migrationBuilder.RenameColumn(
                name: "CronExpression",
                table: "qrtz_cron_triggers",
                newName: "cron_expression");

            migrationBuilder.RenameColumn(
                name: "TriggerGroup",
                table: "qrtz_cron_triggers",
                newName: "trigger_group");

            migrationBuilder.RenameColumn(
                name: "TriggerName",
                table: "qrtz_cron_triggers",
                newName: "trigger_name");

            migrationBuilder.RenameColumn(
                name: "SchedName",
                table: "qrtz_cron_triggers",
                newName: "sched_name");

            migrationBuilder.RenameColumn(
                name: "Calendar",
                table: "qrtz_calendars",
                newName: "calendar");

            migrationBuilder.RenameColumn(
                name: "CalendarName",
                table: "qrtz_calendars",
                newName: "calendar_name");

            migrationBuilder.RenameColumn(
                name: "SchedName",
                table: "qrtz_calendars",
                newName: "sched_name");

            migrationBuilder.RenameColumn(
                name: "BlobData",
                table: "qrtz_blob_triggers",
                newName: "blob_data");

            migrationBuilder.RenameColumn(
                name: "TriggerGroup",
                table: "qrtz_blob_triggers",
                newName: "trigger_group");

            migrationBuilder.RenameColumn(
                name: "TriggerName",
                table: "qrtz_blob_triggers",
                newName: "trigger_name");

            migrationBuilder.RenameColumn(
                name: "SchedName",
                table: "qrtz_blob_triggers",
                newName: "sched_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "priority",
                table: "qrtz_triggers",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "qrtz_triggers",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "trigger_type",
                table: "qrtz_triggers",
                newName: "TriggerType");

            migrationBuilder.RenameColumn(
                name: "trigger_state",
                table: "qrtz_triggers",
                newName: "TriggerState");

            migrationBuilder.RenameColumn(
                name: "start_time",
                table: "qrtz_triggers",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "prev_fire_time",
                table: "qrtz_triggers",
                newName: "PrevFireTime");

            migrationBuilder.RenameColumn(
                name: "next_fire_time",
                table: "qrtz_triggers",
                newName: "NextFireTime");

            migrationBuilder.RenameColumn(
                name: "misfire_instr",
                table: "qrtz_triggers",
                newName: "MisfireInstr");

            migrationBuilder.RenameColumn(
                name: "job_name",
                table: "qrtz_triggers",
                newName: "JobName");

            migrationBuilder.RenameColumn(
                name: "job_group",
                table: "qrtz_triggers",
                newName: "JobGroup");

            migrationBuilder.RenameColumn(
                name: "job_data",
                table: "qrtz_triggers",
                newName: "JobData");

            migrationBuilder.RenameColumn(
                name: "end_time",
                table: "qrtz_triggers",
                newName: "EndTime");

            migrationBuilder.RenameColumn(
                name: "calendar_name",
                table: "qrtz_triggers",
                newName: "CalendarName");

            migrationBuilder.RenameColumn(
                name: "trigger_group",
                table: "qrtz_triggers",
                newName: "TriggerGroup");

            migrationBuilder.RenameColumn(
                name: "trigger_name",
                table: "qrtz_triggers",
                newName: "TriggerName");

            migrationBuilder.RenameColumn(
                name: "sched_name",
                table: "qrtz_triggers",
                newName: "SchedName");

            migrationBuilder.RenameIndex(
                name: "IX_qrtz_triggers_sched_name_job_name_job_group",
                table: "qrtz_triggers",
                newName: "IX_qrtz_triggers_SchedName_JobName_JobGroup");

            migrationBuilder.RenameColumn(
                name: "times_triggered",
                table: "qrtz_simple_triggers",
                newName: "TimesTriggered");

            migrationBuilder.RenameColumn(
                name: "repeat_interval",
                table: "qrtz_simple_triggers",
                newName: "RepeatInterval");

            migrationBuilder.RenameColumn(
                name: "repeat_count",
                table: "qrtz_simple_triggers",
                newName: "RepeatCount");

            migrationBuilder.RenameColumn(
                name: "trigger_group",
                table: "qrtz_simple_triggers",
                newName: "TriggerGroup");

            migrationBuilder.RenameColumn(
                name: "trigger_name",
                table: "qrtz_simple_triggers",
                newName: "TriggerName");

            migrationBuilder.RenameColumn(
                name: "sched_name",
                table: "qrtz_simple_triggers",
                newName: "SchedName");

            migrationBuilder.RenameColumn(
                name: "last_checkin_time",
                table: "qrtz_scheduler_state",
                newName: "LastCheckinTime");

            migrationBuilder.RenameColumn(
                name: "checkin_interval",
                table: "qrtz_scheduler_state",
                newName: "CheckinInterval");

            migrationBuilder.RenameColumn(
                name: "instance_name",
                table: "qrtz_scheduler_state",
                newName: "InstanceName");

            migrationBuilder.RenameColumn(
                name: "sched_name",
                table: "qrtz_scheduler_state",
                newName: "SchedName");

            migrationBuilder.RenameColumn(
                name: "trigger_group",
                table: "qrtz_paused_trigger_grps",
                newName: "TriggerGroup");

            migrationBuilder.RenameColumn(
                name: "sched_name",
                table: "qrtz_paused_trigger_grps",
                newName: "SchedName");

            migrationBuilder.RenameColumn(
                name: "lock_name",
                table: "qrtz_locks",
                newName: "LockName");

            migrationBuilder.RenameColumn(
                name: "sched_name",
                table: "qrtz_locks",
                newName: "SchedName");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "qrtz_job_details",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "requests_recovery",
                table: "qrtz_job_details",
                newName: "RequestsRecovery");

            migrationBuilder.RenameColumn(
                name: "job_data",
                table: "qrtz_job_details",
                newName: "JobData");

            migrationBuilder.RenameColumn(
                name: "job_class_name",
                table: "qrtz_job_details",
                newName: "JobClassName");

            migrationBuilder.RenameColumn(
                name: "is_update_data",
                table: "qrtz_job_details",
                newName: "IsUpdateData");

            migrationBuilder.RenameColumn(
                name: "is_nonconcurrent",
                table: "qrtz_job_details",
                newName: "IsNonconcurrent");

            migrationBuilder.RenameColumn(
                name: "is_durable",
                table: "qrtz_job_details",
                newName: "IsDurable");

            migrationBuilder.RenameColumn(
                name: "job_group",
                table: "qrtz_job_details",
                newName: "JobGroup");

            migrationBuilder.RenameColumn(
                name: "job_name",
                table: "qrtz_job_details",
                newName: "JobName");

            migrationBuilder.RenameColumn(
                name: "sched_name",
                table: "qrtz_job_details",
                newName: "SchedName");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "qrtz_fired_triggers",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "priority",
                table: "qrtz_fired_triggers",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "trigger_name",
                table: "qrtz_fired_triggers",
                newName: "TriggerName");

            migrationBuilder.RenameColumn(
                name: "trigger_group",
                table: "qrtz_fired_triggers",
                newName: "TriggerGroup");

            migrationBuilder.RenameColumn(
                name: "sched_time",
                table: "qrtz_fired_triggers",
                newName: "SchedTime");

            migrationBuilder.RenameColumn(
                name: "requests_recovery",
                table: "qrtz_fired_triggers",
                newName: "RequestsRecovery");

            migrationBuilder.RenameColumn(
                name: "job_name",
                table: "qrtz_fired_triggers",
                newName: "JobName");

            migrationBuilder.RenameColumn(
                name: "job_group",
                table: "qrtz_fired_triggers",
                newName: "JobGroup");

            migrationBuilder.RenameColumn(
                name: "is_nonconcurrent",
                table: "qrtz_fired_triggers",
                newName: "IsNonconcurrent");

            migrationBuilder.RenameColumn(
                name: "instance_name",
                table: "qrtz_fired_triggers",
                newName: "InstanceName");

            migrationBuilder.RenameColumn(
                name: "fired_time",
                table: "qrtz_fired_triggers",
                newName: "FiredTime");

            migrationBuilder.RenameColumn(
                name: "entry_id",
                table: "qrtz_fired_triggers",
                newName: "EntryId");

            migrationBuilder.RenameColumn(
                name: "sched_name",
                table: "qrtz_fired_triggers",
                newName: "SchedName");

            migrationBuilder.RenameColumn(
                name: "time_zone_id",
                table: "qrtz_cron_triggers",
                newName: "TimeZoneId");

            migrationBuilder.RenameColumn(
                name: "cron_expression",
                table: "qrtz_cron_triggers",
                newName: "CronExpression");

            migrationBuilder.RenameColumn(
                name: "trigger_group",
                table: "qrtz_cron_triggers",
                newName: "TriggerGroup");

            migrationBuilder.RenameColumn(
                name: "trigger_name",
                table: "qrtz_cron_triggers",
                newName: "TriggerName");

            migrationBuilder.RenameColumn(
                name: "sched_name",
                table: "qrtz_cron_triggers",
                newName: "SchedName");

            migrationBuilder.RenameColumn(
                name: "calendar",
                table: "qrtz_calendars",
                newName: "Calendar");

            migrationBuilder.RenameColumn(
                name: "calendar_name",
                table: "qrtz_calendars",
                newName: "CalendarName");

            migrationBuilder.RenameColumn(
                name: "sched_name",
                table: "qrtz_calendars",
                newName: "SchedName");

            migrationBuilder.RenameColumn(
                name: "blob_data",
                table: "qrtz_blob_triggers",
                newName: "BlobData");

            migrationBuilder.RenameColumn(
                name: "trigger_group",
                table: "qrtz_blob_triggers",
                newName: "TriggerGroup");

            migrationBuilder.RenameColumn(
                name: "trigger_name",
                table: "qrtz_blob_triggers",
                newName: "TriggerName");

            migrationBuilder.RenameColumn(
                name: "sched_name",
                table: "qrtz_blob_triggers",
                newName: "SchedName");
        }
    }
}
