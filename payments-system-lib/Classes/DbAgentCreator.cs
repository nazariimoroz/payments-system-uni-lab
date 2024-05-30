using System;
using System.Collections.Generic;

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
        public abstract T TryGetFromDb();

        public abstract T CreateNew();
        public abstract List<T2> GetAll<T2>() where T2 : T;

        public abstract void Save(T toSave);

        public abstract void Destroy(T toDestroy);
    }
}
