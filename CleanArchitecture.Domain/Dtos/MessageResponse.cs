using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Dtos
{
    public sealed record MessageResponse(
        string Message)
    {
    }//tek tek featureslarda CarCommandMessageResponse gibi classlar oluşturmak yerine tek bir class oluşturup Commandda çağırdık.
}
