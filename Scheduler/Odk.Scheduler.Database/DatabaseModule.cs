using Ninject.Modules;
using Odk.Scheduler.Database.Repositories;
using PetaPoco;

namespace Odk.Scheduler.Database
{
    public class DatabaseModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabase>().To<PetaPoco.Database>().InThreadScope().WithConstructorArgument("connectionStringName", "SchedulerContext").WithConstructorArgument<IMapper>(a => null);
            Bind<ITaskRepository>().To<TaskRepository>().InThreadScope();
            Bind<IWorkblockRepository>().To<WorkblockRepository>().InThreadScope();
            Bind<ISessionRepository>().To<SessionRepository>().InThreadScope();
            Bind<IScreenshotRepository>().To<ScreenshotRepository>().InThreadScope();
        }
    }
}