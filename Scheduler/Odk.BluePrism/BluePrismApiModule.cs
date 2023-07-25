using Ninject.Modules;
using Ninject.Web.Common;

namespace Odk.BluePrism
{
    public class BluePrismApiModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBluePrism>().To<BluePrism>().InRequestScope();
        }
    }
}
