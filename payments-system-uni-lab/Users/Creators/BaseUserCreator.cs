using payments_system_uni_lab.Objects;
using payments_system_uni_lab.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace payments_system_uni_lab.Users.Creators
{
    public abstract class BaseUserCreator
    {
        public abstract BaseUser TryGetFromDb(BaseUserArgs args);

        public abstract BaseUser CreateNew(BaseUserArgs args);

        public abstract bool IsValidArgs(BaseUserArgs args);
    }

    public abstract class BaseUserArgs
    {
    }
}
