using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gplusnasite.CSharpParser
{
    public class CSharpCode
    {
        public CSharpCode()
        {
            UsingNamespaces = new List<Namespaces>();
            GlobalVariables = new List<Variable>();
            GlobalFunctions = new List<Function>();
            Classes = new List<Class>();
        }

        #region Property
        public string FileName { get; set; }
        public List<Namespaces> UsingNamespaces { get; set; }
        public List<Variable> GlobalVariables { get; set; }
        public List<Function> GlobalFunctions { get; set; }
        public List<Class> Classes { get; set; }
        #endregion
    }
}
