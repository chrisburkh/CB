using MastertData.Models;
using Persons.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persons.Api.Repository
{
    public interface IClassRepository
    {

        IEnumerable<Class> GetAll();

        Task<Class> Get(string Id);
        Task<Class> Write(Class Entity);

        Task<string> Delete(string id);
    }
}
