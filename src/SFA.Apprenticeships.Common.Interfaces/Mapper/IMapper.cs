using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Common.Interfaces.Mapper
{
    public interface IMapper
    {
        object Map(object source, Type sourceType, Type destinationType);
        TDestination Map<TSource, TDestination>(TSource sourceObject);
    }
}
