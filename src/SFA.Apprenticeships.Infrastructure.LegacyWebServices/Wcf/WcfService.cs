namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Wcf
{
    using System;
    using System.Configuration;
    using System.ServiceModel;
    using System.ServiceModel.Configuration;
    using Application.Interfaces.Logging;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Configuration;

    public class WcfService<T> : IWcfService<T>
    {
        private readonly ILogService _logger;

        // ReSharper disable StaticFieldInGenericType
        private static Configuration _configuration;
        private static readonly object Lock = new object();
        // ReSharper restore StaticFieldInGenericType

        public WcfService(IConfigurationManager configurationManager, ILogService logger)
        {
            _logger = logger;

            if (_configuration != null) return;

            lock (Lock)
            {
                if (_configuration != null) return;

                var configMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = configurationManager.ConfigurationFilePath
                };

                _configuration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            }
        }

        public void Use(Action<T> action)
        {
            Use("*", action);
        }

        public void Use(string endpointConfigurationName, string endpointAddress, Action<T> action)
        {
            var factory = new ConfigurationChannelFactory<T>(endpointConfigurationName, _configuration, new EndpointAddress(endpointAddress));

            CallServiceAction(action, factory);
        }

        public void Use(string endpointConfigurationName, Action<T> action)
        {
            var configChannelFactory = new ConfigurationChannelFactory<T>(endpointConfigurationName, _configuration, null);

            CallServiceAction(action, configChannelFactory);
        }

        protected virtual void CallServiceAction(Action<T> action, ChannelFactory<T> factory)
        {
            var client = factory.CreateChannel();
            var success = false;

            try
            {
                _logger.Debug("Calling service {0}", factory.Endpoint.Address);

                action(client);
                ((IClientChannel)client).Close();
                factory.Close();
                success = true;

                _logger.Debug("Call succeeded and client is now closed");
            }
            catch (ServerTooBusyException e)
            {
                throw new BoundaryException(ErrorCodes.WebServiceFailed, e);
            }
            catch (CommunicationException e)
            {
                throw new BoundaryException(ErrorCodes.WebServiceFailed, e);
            }
            catch (TimeoutException e)
            {
                throw new BoundaryException(ErrorCodes.WebServiceFailed, e);
            }
            finally
            {
                if (!success) AbortClient(factory, client);
            }
        }

        private void AbortClient(ICommunicationObject factory, T client)
        {
            try
            {
                ((IClientChannel) client).Abort();
            }
            catch (Exception e)
            {
                _logger.Info("Failed to abort client", e);
            }

            try
            {
                factory.Abort();
            }
            catch (Exception e)
            {
                _logger.Info("Failed to abort factory", e);
            }
        }
    }
}
