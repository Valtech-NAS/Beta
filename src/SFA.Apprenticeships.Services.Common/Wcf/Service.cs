﻿using System;
using System.ServiceModel;

namespace SFA.Apprenticeships.Services.Common.Wcf
{
    public class Service<T>
    {
        public static void Use(Action<T> action)
        {
            Use("*", action);
        }

        public static void Use(string endpointConfigurationName, string endpointAddress, Action<T> action)
        {
            var factory = new ChannelFactory<T>(endpointConfigurationName, new EndpointAddress(endpointAddress));
            CallServiceAction(action, factory);
        }

        public static void Use(string endpointConfigurationName, Action<T> action)
        {
            var factory = new ChannelFactory<T>(endpointConfigurationName);
            CallServiceAction(action, factory);
        }

        private static void CallServiceAction(Action<T> action, ChannelFactory<T> factory)
        {
            var client = factory.CreateChannel();
            var success = false;

            try
            {
                //Logger.Debug("Calling service {0}", factory.Endpoint.Address);

                action(client);
                ((IClientChannel)client).Close();
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
                    ((IClientChannel)client).Abort();
                    factory.Abort();
                }
            }
        }
    }
}
