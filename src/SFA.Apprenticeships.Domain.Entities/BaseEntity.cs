namespace SFA.Apprenticeships.Domain.Entities
{
    using System;

    /// <summary>
    /// Base type for persistent domain entities
    /// </summary>
    public abstract class BaseEntity
    {
        //todo: temporarily a Guid... change to ObjectId? http://stackoverflow.com/questions/21726985/net-layered-architecture-mongodb-what-to-use-as-id
        public Guid Id { get; set; } 
    }
}
