namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Wcf
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Configuration;
    using Common.Configuration;

    public class WcfService<T> : IWcfService<T>
    {
        private readonly System.Configuration.Configuration _configuration;

        public WcfService(IConfigurationManager configurationManager)
        {
            _configuration = configurationManager.Configuration;
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
                //Logger.Debug("Calling service {0}", factory.Endpoint.Address);

                action(client);
                ((IClientChannel) client).Close();
                factory.Close();
                success = true;
            }
            catch (CommunicationException)
            {
                throw; // handle WCF CommunicationException
            }
            catch (TimeoutException)
            {
                throw; // handle WCF TimeoutException
            }
            catch (Exception)
            {
                throw; // handle non-WCF Exception
            }
            finally
            {
                if (!success)
                {
                    ((IClientChannel) client).Abort();
                    factory.Abort();
                }
            }
        }
    }
}
