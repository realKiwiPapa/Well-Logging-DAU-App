using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging_App.WebService.Workflow
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple=false,Inherited=false)]
    public sealed class PropertyAttribute : System.Attribute
    {
        private string _name;
        public string Name { get { return _name; } }

        public PropertyAttribute(string name)
        {
            _name = name;
        }
    }

}