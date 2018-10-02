using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBAdmin.Service
{
    public interface IDatabaseService
    {
        Task ResetDatabase();
    }
}
