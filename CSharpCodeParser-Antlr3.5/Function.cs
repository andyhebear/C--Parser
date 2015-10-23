using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gplusnasite.CSharpParser
{
    public class Function
    {
        public Function()
        {
            Accessor = AccessType.Private;
            Parameters = new List<Variable>();
        }

        #region Property
        public AccessType Accessor { get; set; }
        public string ReturnType { get; set; }
        public string Name { get; set; }
        public List<Variable> Parameters { get; set; }
        public bool IsStatic { get; set; }
        public bool IsConst { get; set; }
        #endregion

        public override string ToString()
        {
            string result = "";

            result += (Accessor.ToString() + " ");
            result += (ReturnType + " " + Name + "(");

            for (int count = 0; count < Parameters.Count; ++count)
            {
                if (count > 0)
                {
                    result += ", ";
                }
                result += Parameters[count].ToString();
            }
            result += ")";

            return result;
        }
    }
}
