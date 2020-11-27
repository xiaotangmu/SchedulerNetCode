using Microsoft.AspNetCore.Mvc;
using Quartz;
using Scheduler.Job;
using Scheduler.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        public TestController(ISchedulerFactory schedulerFactory)
        {
            this._schedulerFactory = schedulerFactory;
        }
        [HttpGet]
        public async Task<string[]> Get()
        {　　　　　　　
            //1、通过调度工厂获得调度器
            _scheduler = await _schedulerFactory.GetScheduler();　　　　　　　
            //2、开启调度器
            await _scheduler.Start();
            //3、创建一个触发器
            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())//每两秒执行一次
                            .Build();
            //4、创建任务
            var jobDetail = JobBuilder.Create<MyJob>()
                            .WithIdentity("job", "group")
                            .Build();
            //5、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);
            return await Task.FromResult(new string[] { "value1", "value2" });
        }

        [HttpPost]
        public async Task<string[]> SetJob(JobViewModel model)
        {
            //1、通过调度工厂获得调度器
            _scheduler = await _schedulerFactory.GetScheduler();
            //2、开启调度器
            await _scheduler.Start();
            //3、创建一个触发器
            var trigger = TriggerBuilder.Create()
                            .WithCronSchedule("0 * 16 27 * ?")// CronTrigger:Cron表达式包含7个字段，秒 分 时 月内日期 月 周内日期 年(可选)。
                            .Build();
            //4、创建任务
            var jobDetail = JobBuilder.Create<MyJob>()
                            .WithIdentity(model.JobName, model.GroupName)
                            .Build();
            //5、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);
            return await Task.FromResult(new string[] { "value1", "value2" });
        }
    }
}
