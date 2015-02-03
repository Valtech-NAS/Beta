namespace SFA.Apprenticeships.Infrastructure.Logging.IoC
{
    using System;
    using Application.Interfaces.Logging;
    using StructureMap.Configuration.DSL;

    public class LoggingRegistry : Registry
    {
        public LoggingRegistry()
        {
            For<ILogService>().AlwaysUnique().Use<NLogLogService>().Ctor<Type>().Is(c => c.ParentType ?? c.RootType);
        }
    }
}
