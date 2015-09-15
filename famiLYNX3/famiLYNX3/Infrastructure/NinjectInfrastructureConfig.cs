using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace famiLYNX3.Infrastructure {
    public class NinjectInfrastructureConfig {
        public static void RegisterInfrastructure(IKernel kernel) {
            kernel.Bind<IRepository>().To<Repository>();
        }
    }
}