// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SFA.Apprenticeships.Web.Candidate.DependencyResolution {
    using Application.Interfaces.Logging;
    using Candidate.IoC;
    using Common.IoC;
    using Common.Providers;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Address.IoC;
    using Infrastructure.Caching.Azure.IoC;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.LocationLookup.IoC;
    using Infrastructure.Logging.IoC;
    using Infrastructure.Postcode.IoC;
    using Infrastructure.RabbitMq.IoC;
    using Infrastructure.Repositories.Applications.IoC;
    using Infrastructure.Repositories.Authentication.IoC;
    using Infrastructure.Repositories.Candidates.IoC;
    using Infrastructure.Repositories.Users.IoC;
    using Infrastructure.UserDirectory.IoC;
    using Infrastructure.VacancySearch.IoC;
    using StructureMap;
	
    public static class IoC {
        public static IContainer Initialize()
        {
            var config = new ConfigurationManager();
            var useCacheSetting = config.TryGetAppSetting("UseCaching");
            bool useCache;
            bool.TryParse(useCacheSetting, out useCache);

            return new Container(delegate(ConfigurationExpression x)
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();

                // service layer
                x.AddRegistry<AzureCacheRegistry>();
                //x.AddRegistry<MemoryCacheRegistry>();

                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(useCache));
                x.AddRegistry<PostcodeRegistry>();
                // TODO: DEBT: AG: if Rabbit is incorrectly configured, website fails to start properly. Need to more lazily initialise RabbitMQ.
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<AuthenticationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<UserDirectoryRegistry>();
                x.AddRegistry<AddressRegistry>();

                // web layer
                x.AddRegistry<WebCommonRegistry>();
                x.AddRegistry<CandidateWebRegistry>();

                x.For<IUserDataProvider>().Use<CookieUserDataProvider>();
                x.For<IEuCookieDirectiveProvider>().Use<EuCookieDirectiveProvider>();
                x.For<ICookieDetectionProvider>().Use<CookieDetectionProvider>();
                x.For<IDismissPlannedOutageMessageCookieProvider>().Use<DismissPlannedOutageMessageCookieProvider>();

                x.Policies.SetAllProperties(y => y.OfType<IConfigurationManager>());
                x.Policies.SetAllProperties(y => y.OfType<IFeatureToggle>());
                x.Policies.SetAllProperties(y => y.OfType<ICookieDetectionProvider>());
                x.Policies.SetAllProperties(y => y.OfType<IEuCookieDirectiveProvider>());
                x.Policies.SetAllProperties(y => y.OfType<IDismissPlannedOutageMessageCookieProvider>());
                x.Policies.SetAllProperties(y => y.OfType<ILogService>());
            });
        }
    }
}