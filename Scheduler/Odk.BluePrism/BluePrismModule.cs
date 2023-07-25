using Ninject.Modules;

namespace Odk.BluePrism
{
    public class BluePrismModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBluePrism>().To<BluePrism>().InSingletonScope();
        }
    }
}
