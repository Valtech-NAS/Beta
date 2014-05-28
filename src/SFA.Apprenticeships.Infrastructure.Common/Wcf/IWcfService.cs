namespace SFA.Apprenticeships.Infrastructure.Common.Wcf
{
    using System;

    //TODO: ???? Where/too techy here - Infrastrcucture web common?
    public interface IWcfService<T>
    {
        void Use(Action<T> action);
        void Use(string endpointConfigurationName, string endpointAddress, Action<T> action);
        void Use(string endpointConfigurationName, Action<T> action);
    }
}
