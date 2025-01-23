namespace Command.Job;

public class GloabalLocalServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _globalDependecy;
        private readonly IServiceProvider _localDependency;

        public GloabalLocalServiceProvider(IServiceProvider primary, IServiceProvider fallback)
        {
            _globalDependecy = primary;
            _localDependency = fallback;
        }

        public object GetService(Type serviceType)
        {
            var service = _globalDependecy.GetService(serviceType);
            return service ?? _localDependency.GetService(serviceType);
        }
        
    }

