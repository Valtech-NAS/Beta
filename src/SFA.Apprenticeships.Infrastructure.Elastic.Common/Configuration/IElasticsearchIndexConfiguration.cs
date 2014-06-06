namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration
{
    using System;

    public interface IElasticsearchIndexConfiguration
    {
        /// <summary>
        /// 	Index name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 	.Net type used in mappging
        /// </summary>
        Type MappingType { get; set; }
    }
}
