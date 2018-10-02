using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Api
{
    public interface IInfraRepository
    {
        Task<string> GetInformation();
    }
}
