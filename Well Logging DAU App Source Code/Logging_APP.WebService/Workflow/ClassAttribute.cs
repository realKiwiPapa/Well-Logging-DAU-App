using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging_App.WebService.Workflow
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class ClassAttribute : Attribute
    {
        private Type _type;
        public Type Type { get { return _type; } }

        public ClassAttribute(Type type)
        {
            if (type.BaseType != typeof(Controller)) throw new TypeAccessException();
            _type = type;
        }
    }

}