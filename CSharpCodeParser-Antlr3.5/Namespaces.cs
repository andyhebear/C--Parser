using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gplusnasite.CSharpParser
{
    public class Namespaces
    {
        public Namespaces()
        {
            Names = new List<string>();
        }

        #region Property
        public string FullName { get; set; }
        public List<string> Names { get; set; }

        public override string ToString()
        {
            return FullName;
        }
        #endregion
    }
}
