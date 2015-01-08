﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Apprenticeships
{
    using System;
    using AutoMapper;

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