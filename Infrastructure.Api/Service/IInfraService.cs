using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Api.Service
{
    public interface IInfraService
    {
        Task<ActionResult<string>> GetInformation();
    }
}
