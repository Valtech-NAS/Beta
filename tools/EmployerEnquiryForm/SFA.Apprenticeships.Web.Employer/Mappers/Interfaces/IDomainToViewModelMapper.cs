using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Web.Employer.Mappers.Interfaces
{
    interface IDomainToViewModelMapper<TSource, TDestination>
    {
        TDestination ConvertToViewModel(TSource domain);        
    }
}
