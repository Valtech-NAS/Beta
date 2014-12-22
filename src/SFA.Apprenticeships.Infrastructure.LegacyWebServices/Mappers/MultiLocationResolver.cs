namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class MultiLocationResolver : ValueResolver<string, bool>
    {
        protected override bool ResolveCore(string source)
        {
            if (source == null)
            {
                return false;
            }
           
            switch (source)
            {
                case "National":              
                case "Standard":
                    return false;
                case "MultipleLocation":
                    return true;
                default:
                    throw new ArgumentException(
                        string.Format(
                            "The vacancy location is not valid: {0}, it must be either National or Standard to map correctly",
                            source));
            }
        }
    }
}
