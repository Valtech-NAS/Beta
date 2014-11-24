namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Wcf
{
    using System;
    using System.Configuration;
    using System.ServiceModel;
    using System.ServiceModel.Configuration;
    using Domain.Interfaces.Configuration;
    using NLog;

    public class WcfService<T> : IWcfService<T>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Configuration _configuration;
        private static readonly object _lock = new object();

        public WcfService(IConfigurationManager configurationManager)
        {
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
                Logger.Debug("Calling service {0}", factory.Endpoint.Address);

                action(client);
                ((IClientChannel)client).Close();
                factory.Close();
                success = true;
                Logger.Debug("Call succeeded and client is now closed");
            }
            catch (CommunicationException ex)
            {
                Logger.Fatal("WCF CommunicationException ", ex);
                throw; // handle WCF CommunicationException
            }
            catch (TimeoutException ex)
            {
                Logger.Fatal("WCF TimeoutException ", ex);
                throw; // handle WCF TimeoutException
            }
            catch (Exception exception)
            {
                Logger.Error("Non-WCF TimeoutException ", exception);
                throw; // handle non-WCF Exception
            }
            finally
            {
                if (!success)
                {
                    Logger.Error(string.Format("WCF failed and client was aborted. StackTrace = {0}", Environment.StackTrace));
                    ((IClientChannel)client).Abort();
                    factory.Abort();
                }
            }
        }
    }
}
