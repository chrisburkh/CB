using Persons.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persons.Api.Repository
{
    public interface IPersonsRepository<T>
    {

        Task<IEnumerable<T>> GetAll();

        Task<T> Get(string Id);
        Task<T> Write(T Entity);

        Task<string> Delete(string id);
    }
}
