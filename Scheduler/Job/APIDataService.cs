using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.Job
{
    /// <summary>
    /// 启动项目时，调用
    /// </summary>
    public class APIDataService : BackgroundService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        public APIDataService(ISchedulerFactory schedulerFactory)
        {
            this._schedulerFactory = schedulerFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //需要执行的任务
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
                }
                catch (Exception ex)
                {
                    LogHelper.LogExceptionMessage(ex);
                }
                await Task.Delay(1000, stoppingToken);//等待1秒
            }
        }
    }
}
