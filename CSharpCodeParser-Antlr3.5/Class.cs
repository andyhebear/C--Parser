using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gplusnasite.CSharpParser
{
    public class Class
    {
        public Class()
        {
            Functions = new List<Function>();
            Variables = new List<Variable>();
        }

        #region Property
        public string Name { get; set; }
        public List<Function> Functions { get; set; }
        public List<Variable> Variables { get; set; }
        #endregion

        public override string ToString()
        {
            string result = "";
            result += ("class " + Name);
            result += "\r\n{";

            foreach (Function functionInfo in Functions)
            {
                result += "\r\n";
                result += ("    " + functionInfo.ToString());
            }

            foreach (Variable variableInfo in Variables)
            {
                result += "\r\n";
                result += ("    " + variableInfo.ToString());
            }

            result += "\r\n};";

            return result;
        }
    }
}
