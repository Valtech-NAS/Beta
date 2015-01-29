﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Wcf
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
            catch (ServerTooBusyException ex)
            {
                Logger.Info("WCF ServerTooBusyException", (Exception)ex);
                throw;
            }
            catch (CommunicationException ex)
            {
                Logger.Info("WCF CommunicationException", (Exception)ex);
                throw;
            }
            catch (TimeoutException ex)
            {
                Logger.Info("WCF TimeoutException", (Exception)ex);
                throw;
            }
            catch (Exception exception)
            {
                Logger.Info("Non-WCF Exception", exception);
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
                        Logger.Info("Failed to abort client", ex);
                    }
                    try
                    {
                        factory.Abort();
                    }
                    catch (Exception ex)
                    {
                        Logger.Info("Failed to abort factory", ex);
                    }
                }
            }
        }
    }
}
