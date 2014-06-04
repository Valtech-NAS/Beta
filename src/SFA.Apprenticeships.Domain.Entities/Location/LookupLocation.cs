namespace SFA.Apprenticeships.Domain.Entities.Location
{
    using System;

    //todo: may not need LookupLocation if can populate Location.Name instead
    public class LookupLocation : Location
    {
        public string Type { get; set; }
    }
}
