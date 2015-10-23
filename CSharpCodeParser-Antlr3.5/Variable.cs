using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gplusnasite.CSharpParser
{
    public class Variable
    {
        public Variable()
        {
            Accessor = AccessType.Private;
        }
        #region Property
        public AccessType Accessor { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }
        public bool IsStatic { get; set; }
        public bool IsMutable { get; set; }
        #endregion

        public override string ToString()
        {
            string result = "";

            result += (Accessor.ToString() + " ");
            result += (Type + " " + Name);

            return result;
        }
    }
}
