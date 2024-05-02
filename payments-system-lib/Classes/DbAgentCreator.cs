using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using payments_system_lib.Classes.Users;
using payments_system_lib.Interfaces;

namespace payments_system_lib.Classes
{
    public abstract class DbAgentCreator<T> 
        where T : IDbAgent
    {
        public abstract T TryGetFromDb();

        public abstract T CreateNew();
        public abstract List<T2> GetAll<T2>() where T2 : T;
    }
}
