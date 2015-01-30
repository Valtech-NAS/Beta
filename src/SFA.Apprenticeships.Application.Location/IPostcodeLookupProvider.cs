namespace SFA.Apprenticeships.Application.Location
{
    using Domain.Entities.Locations;

    public interface IPostcodeLookupProvider
    {
        Location GetLocation(string postcode);
    }
}
