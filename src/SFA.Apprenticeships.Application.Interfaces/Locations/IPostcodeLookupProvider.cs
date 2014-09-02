namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using Domain.Entities.Locations;

    public interface IPostcodeLookupProvider
    {
        Location GetLocation(string postcode);
    }
}
