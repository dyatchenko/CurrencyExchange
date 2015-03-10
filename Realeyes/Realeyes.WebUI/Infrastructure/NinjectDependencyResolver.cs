namespace Realeyes.WebUI.Infrastructure
{
    namespace ServiceBrokerListener.WebUI.Infrastructure
    {
        using System.Web.Mvc;
        using System;
        using System.Collections.Generic;

        using Ninject;

        using Realeyes.WebUI.Abstract;
        using Realeyes.WebUI.Models;

        public class NinjectDependencyResolver : IDependencyResolver
        {
            private readonly IKernel kernel;

            public NinjectDependencyResolver(IKernel kernelParam)
            {
                kernel = kernelParam;
                AddBindings();
            }

            public object GetService(Type serviceType)
            {
                return kernel.TryGet(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return kernel.GetAll(serviceType);
            }

            private void AddBindings()
            {
                kernel.Bind<IEcbDataSource>().To<EcbDataSource>().InSingletonScope();
                kernel.Bind<ICurrencyExchangeRepository>()
                    .To<CurrencyExchangeRepository>()
                    .InSingletonScope();
            }
        }
    }
}