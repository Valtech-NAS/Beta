using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using RestSharp.Deserializers;
using SFA.Apprenticeships.Services.Common.ConfigurationUtilities;
using SFA.Apprenticeships.Services.Postcode.Abstract;
using SFA.Apprenticeships.Services.Postcode.Entities;

namespace SFA.Apprenticeships.Services.Postcode.Service
{
    public class PostcodeService : IPostcodeService
    {
        private readonly string _postcodeServiceEndpoint;

        /// <summary>
        /// <add key="PostcodeServiceEndpoint" value="http://api.postcodes.io" />
        /// </summary>
        public PostcodeService(IConfigurationManager configManager)
        {
            if (configManager == null)
            {
                throw new ArgumentNullException("configManager");
            }

            _postcodeServiceEndpoint = configManager.GetAppSetting("PostcodeServiceEndpoint");
        }

        public string GetPostcodeFromLatLong(string latitudeLongitude)
        {
            throw new NotImplementedException();
        }

        public bool ValidatePostcode(string postcode)
        {
            throw new NotImplementedException();
        }

        public IList<PostcodeInfo> GetPartialMatches(string postcode)
        {
            if (string.IsNullOrEmpty(postcode))
            {
                throw new ArgumentNullException("postcode");
            }

            var request = new RestRequest("postcodes?q={postcode}") {RequestFormat = DataFormat.Json};
            request.AddUrlSegment("postcode", postcode);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var postcodeInfo = Execute<PostcodeInfoResult>(request);
            if (postcodeInfo != null && postcodeInfo.Result != null)
            {
                return postcodeInfo.Result;
            }

            return new List<PostcodeInfo>();
        }

        public PostcodeInfo GetPostcodeInfo(string postcode)
        {
            var result = GetPartialMatches(postcode);

            return result.FirstOrDefault();
        }

        public PostcodeInfo GetRandomPostcode()
        {
            var request = new RestRequest("random/postcodes") {RequestFormat = DataFormat.Json};

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var postcodeInfo = Execute<PostcodeInfoResult>(request);

            if (postcodeInfo == null)
            {
                throw new ApplicationException("No postcode information returned.");
            }

            return postcodeInfo.Result.First();
        }

        private T Execute<T>(IRestRequest request) where T : new()
        {
            var client = new RestClient { BaseUrl = _postcodeServiceEndpoint };

            client.AddHandler("application/json", new JsonDeserializer());
            
            var response = client.Execute<T>(request);

            /* Restsharp derserializer doesnt appear to handle property names that differ from response */
           // var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);

            if (response.ErrorException != null)
            {
                throw new ApplicationException(
                    "Error retrieving response. Check inner details for more info.",
                    response.ErrorException);
            }

            return response.Data;
        }
    }
}
