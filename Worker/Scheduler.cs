using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace Worker
{

    public class Scheduler
    {
        public TimeSpan RepeatInterval { get; private set; }
        private ITrigger trigger;
        private ISchedulerFactory schedFact;
        private IScheduler sched;
        private IJobDetail jobDetail;
        public bool Running { get; private set; }
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

            Running = false;
        }

        public void StartUpdating()
        {
            if (Running == false)
            {
                trigger = new SimpleTriggerImpl("Feed parsing", null, DateTime.Now, null, SimpleTriggerImpl.RepeatIndefinitely, RepeatInterval);
                jobDetail = new JobDetailImpl("job", typeof(UpdateJob));
                jobDetail.JobDataMap["service"] = service;

                sched.ScheduleJob(jobDetail, trigger);
                Running = true;
            }
        }

        public void Stop()
        {
            sched.Clear();
            Running = false;
        }
    }

    public class UpdateJob : IJob{

        public void Execute(IJobExecutionContext context)
        {
            ParsingService service = (ParsingService)context.JobDetail.JobDataMap["service"];

            service.startParse();
        }
    }
}
