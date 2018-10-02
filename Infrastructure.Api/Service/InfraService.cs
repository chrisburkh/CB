using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Api.Service
{
    public class InfraService : IInfraService
    {
        private IInfraRepository _infraRepository;

        public InfraService(IInfraRepository infraRepository)
        {
            _infraRepository = infraRepository;
        }

        public async Task<ActionResult<string>> GetInformation()
        {
            return await _infraRepository.GetInformation();
        }
    }
}
