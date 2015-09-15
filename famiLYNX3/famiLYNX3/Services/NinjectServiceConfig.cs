using famiLYNX3.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace famiLYNX3.Services {
    public class NinjectServiceConfig {
        public static void RegisterServices(IKernel kernel) {
            NinjectInfrastructureConfig.RegisterInfrastructure(kernel);
            kernel.Bind<AllServices>().ToSelf();
        }
    }
}