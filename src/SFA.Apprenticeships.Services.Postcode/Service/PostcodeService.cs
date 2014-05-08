using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using SFA.Apprenticeships.Services.Common.Services;
using SFA.Apprenticeships.Services.Postcode.Abstract;
using SFA.Apprenticeships.Services.Postcode.Entities;

namespace SFA.Apprenticeships.Services.Postcode.Service
{
    public class PostcodeService : RestService, IPostcodeService
    {
        /// <summary>
        /// <add key="PostcodeServiceEndpoint" value="http://api.postcodes.io" />
        /// </summary>
        public PostcodeService(string baseUrl)
            : base(baseUrl)
        {
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

            var request = Create("postcodes?q={postcode}", new[] { new KeyValuePair<string, string>("postcode", postcode) });
            var postcodeInfo = Execute<PostcodeInfoResult>(request);

            if (postcodeInfo.Data != null && postcodeInfo.Data.Result != null)
            {
                return postcodeInfo.Data.Result;
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
            var postcodeInfo = Execute<PostcodeInfoResult>(Create("random/postcodes"));

            if (postcodeInfo.Data == null)
            {
                throw new ApplicationException("No postcode information returned.");
            }

            return postcodeInfo.Data.Result.First();
        }
    }
}
