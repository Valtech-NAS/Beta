using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using RestSharp.Deserializers;
using SFA.Apprenticeships.Services.Common.Configuration;
using SFA.Apprenticeships.Services.Postcode.Abstract;
using SFA.Apprenticeships.Services.Postcode.Entities;

namespace SFA.Apprenticeships.Services.Postcode.Service
{
    public class PostcodeService : IPostcodeService
    {
        private readonly string _postcodeServiceEndpoint;
        private IRestClient _client;

        /// <summary>
        /// <add key="PostcodeServiceEndpoint" value="http://api.postcodes.io" />
        /// </summary>
        public PostcodeService(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException("baseUrl");
            }

            _postcodeServiceEndpoint = baseUrl;
        }

        public IRestClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new RestClient { BaseUrl = _postcodeServiceEndpoint };
                    (_client as RestClient).AddHandler("application/json", new JsonDeserializer());
                }

                return _client;
            }

            set { _client = value; }
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

            IRestRequest request = new RestRequest("postcodes?q={postcode}") { RequestFormat = DataFormat.Json };
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
            IRestRequest request = new RestRequest("random/postcodes") {RequestFormat = DataFormat.Json};
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var postcodeInfo = Execute<PostcodeInfoResult>(request);

            if (postcodeInfo == null)
            {
                throw new ApplicationException("No postcode information returned.");
            }

            return postcodeInfo.Result.First();
        }

        protected virtual T Execute<T>(IRestRequest request) where T : new()
        {           
            var response = _client.Execute<T>(request);

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
