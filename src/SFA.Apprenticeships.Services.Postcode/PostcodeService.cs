namespace SFA.Apprenticeships.Services.Postcode
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Application.Interfaces.Postcode;
    using SFA.Apprenticeships.Domain.Entities.Postcode;

    public class PostcodeService : IPostcodeService
    {
        private readonly IPostcodeService _infrasructureService;

        /// <summary>
        /// <add key="PostcodeServiceEndpoint" value="http://api.postcodes.io" />
        /// </summary>
        public PostcodeService(IPostcodeService infrasructureService)
        {
            _infrasructureService = infrasructureService;
        }

        public string GetPostcodeFromLatLong(string latitudeLongitude)
        {
            return _infrasructureService.GetPostcodeFromLatLong(latitudeLongitude);
        }

        public bool ValidatePostcode(string postcode)
        {
            return _infrasructureService.ValidatePostcode(postcode);
        }

        public IList<PostcodeInfo> GetPartialMatches(string postcode)
        {
            return _infrasructureService.GetPartialMatches(postcode);
        }

        public PostcodeInfo GetPostcodeInfo(string postcode)
        {
            return _infrasructureService.GetPostcodeInfo(postcode);
        }

        public PostcodeInfo GetRandomPostcode()
        {
            return _infrasructureService.GetRandomPostcode();
        }
    }
}
