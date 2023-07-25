using System;
using Topshelf;
using Odk.BluePrism;
using Ninject;
using Odk.Scheduler.Migrations;
using Odk.Scheduler.Database;

namespace Odk.Scheduler
{
    class Program
    {
        public static IKernel NinjectKernel { get; set; }
        public static IKernel NinjectApiKernel { get; set; }

        static void Main(string[] args)
        {
            NinjectKernel = new Ninject.StandardKernel(new Ninject.Modules.INinjectModule[] {
                new BluePrismModule(),
                new DatabaseModule()
                });

            NinjectApiKernel = new Ninject.StandardKernel(new Ninject.Modules.INinjectModule[] {
                new BluePrismApiModule(),
                new DatabaseApiModule()
                });

            var migrationRunner = new MigrationRunner(System.Configuration.ConfigurationManager.ConnectionStrings["SchedulerContext"].ConnectionString);
            migrationRunner.Up();           

            var rc = HostFactory.Run(x =>
            {
                x.Service<Scheduler>(s =>
                {
                    s.ConstructUsing(name => NinjectKernel.Get<Scheduler>());
                    s.WhenStarted(scheduler => scheduler.Start());
                    s.WhenStopped(scheduler => scheduler.Stop());
                });

                x.SetDescription("Better Prism Scheduler");
                x.SetDisplayName("BPScheduler");
                x.SetServiceName("BPScheduler");
                x.UseNLog();
                x.OnException(ex => { NLog.LogManager.GetCurrentClassLogger()?.Error(ex, "Unexpected crash"); });
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}