namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Wcf
{
    using System;
    using System.Configuration;
    using System.ServiceModel;
    using System.ServiceModel.Configuration;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Configuration;

    public class WcfService<T> : IWcfService<T>
    {
        private readonly ILogService _logger;

        private static Configuration _configuration;
        private static readonly object _lock = new object();

        public WcfService(IConfigurationManager configurationManager, ILogService logger)
        {
            _logger = logger;
            if (_configuration != null) return;

            lock (_lock)
            {
                if (_configuration != null)
                {
                    return;
                }

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
            catch (ServerTooBusyException ex)
            {
                _logger.Info("WCF ServerTooBusyException", (Exception)ex);
                throw;
            }
            catch (CommunicationException ex)
            {
                _logger.Info("WCF CommunicationException", (Exception)ex);
                throw;
            }
            catch (TimeoutException ex)
            {
                _logger.Info("WCF TimeoutException", (Exception)ex);
                throw;
            }
            catch (Exception exception)
            {
                _logger.Info("Non-WCF Exception", exception);
                throw;
            }
            finally
            {
                if (!success)
                {
                    try
                    {
                        ((IClientChannel)client).Abort();
                    }
                    catch (Exception ex)
                    {
                        _logger.Info("Failed to abort client", ex);
                    }
                    try
                    {
                        factory.Abort();
                    }
                    catch (Exception ex)
                    {
                        _logger.Info("Failed to abort factory", ex);
                    }
                }
            }
        }
    }
}
