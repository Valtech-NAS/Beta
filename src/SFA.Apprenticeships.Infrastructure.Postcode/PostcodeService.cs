namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Apprenticeships.Application.Interfaces.Postcode;
    using SFA.Apprenticeships.Infrastructure.Common.Mappers;
    using SFA.Apprenticeships.Infrastructure.Common.Rest;
    using SFA.Apprenticeships.Infrastructure.Postcode.Entities;
    using PostcodeInfo = SFA.Apprenticeships.Domain.Entities.Postcode.PostcodeInfo;

    public class PostcodeService : RestService, IPostcodeService
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// <add key="PostcodeServiceEndpoint" value="http://api.postcodes.io" />
        /// </summary>
        public PostcodeService(string baseUrl, IMapper mapper)
            : base(baseUrl)
        {
            _mapper = mapper;
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
                return _mapper.Map<IList<Entities.PostcodeInfo>, IList<PostcodeInfo>>(postcodeInfo.Data.Result);
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

            return _mapper.Map<Entities.PostcodeInfo, PostcodeInfo>(postcodeInfo.Data.Result.First());
        }
    }

}
