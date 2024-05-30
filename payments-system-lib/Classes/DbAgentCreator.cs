using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace payments_system_lib.Classes
{
    public class InvalidParamException : Exception
    {
        public InvalidParamException(string paramName)
            : base($"Invalid param: {paramName}")
        { }
    }

    public abstract class DbAgentCreator<T> 
        where T : class
    {
        public abstract Task<T> TryGetFromDb();

        public abstract Task<T> CreateNew();
        public abstract Task<List<T2>> GetAll<T2>() where T2 : T;

        public abstract Task Save(T toSave);

        public abstract Task Destroy(T toDestroy);
    }
}
