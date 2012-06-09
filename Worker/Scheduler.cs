using System;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace Worker
{
    public class Scheduler
    {
        public enum RunningState { Runing, Waiting }

        public TimeSpan RepeatInterval { get; private set; }

        private ITrigger trigger;
        private ISchedulerFactory schedFact;
        private IScheduler sched;
        private IJobDetail jobDetail;

        public RunningState State { get; private set; }

        private ParsingService service;

        public Scheduler(int repeatInterval, ParsingService service)
        {
            this.service = service;
            RepeatInterval = TimeSpan.FromSeconds(repeatInterval);
            // construct a scheduler factory
            schedFact = new StdSchedulerFactory();

            // get a scheduler
            sched = schedFact.GetScheduler();
            sched.Start();
            State = RunningState.Waiting;
        }

        public void StartUpdating()
        {
            if (State == RunningState.Waiting)
            {
                trigger = new SimpleTriggerImpl("Feed parsing", null, DateTime.Now, null, SimpleTriggerImpl.RepeatIndefinitely, RepeatInterval);
                jobDetail = new JobDetailImpl("job", typeof(UpdateJob));
                jobDetail.JobDataMap["service"] = service;
                sched.ScheduleJob(jobDetail, trigger);
                State = RunningState.Runing;
            }
        }

        public void Stop()
        {
            sched.Clear();
            State = RunningState.Waiting;
        }
    }

    public class UpdateJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ParsingService service = (ParsingService)context.JobDetail.JobDataMap["service"];
            service.startParse();
        }
    }
}