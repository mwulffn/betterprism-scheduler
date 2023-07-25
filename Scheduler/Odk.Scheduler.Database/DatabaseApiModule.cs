using Ninject.Modules;
using Odk.Scheduler.Database.Repositories;
using PetaPoco;
using Ninject.Web.Common;

namespace Odk.Scheduler.Database
{
    public class DatabaseApiModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabase>().To<PetaPoco.Database>().InRequestScope().WithConstructorArgument("connectionStringName", "SchedulerContext").WithConstructorArgument<IMapper>(a => null);
            Bind<ITaskRepository>().To<TaskRepository>().InRequestScope();
            Bind<IWorkblockRepository>().To<WorkblockRepository>().InRequestScope();
            Bind<ISessionRepository>().To<SessionRepository>().InRequestScope();
            Bind<IScreenshotRepository>().To<ScreenshotRepository>().InRequestScope();
        }
    }
}