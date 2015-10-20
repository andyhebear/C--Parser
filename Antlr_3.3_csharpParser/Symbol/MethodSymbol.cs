/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using System.Collections.Generic;

namespace Symbol
{
    /// <summary> This basically adds nothing to ScopedSymbol </summary>
    public class MethodSymbol<AST> : ScopedSymbol<AST>
    {
        public override Dictionary<string, Symbol<AST>> Members { get { return ordered_args; } }
        Dictionary<string, Symbol<AST>> ordered_args = new Dictionary<string, Symbol<AST>>();

        public MethodSymbol()
        {
        }
        public MethodSymbol(string name, IScope<AST> parent)
            : base(name, parent)
        {
        }

        #region protected virtual string Args
        protected virtual string Args
        {
            get
            {
                string args = string.Empty;

                foreach (string s in ordered_args.Keys)
                    args += s + ",";
                args = args.TrimEnd(new char[] { ',' });
                return args;
            }
        }
        #endregion
        #region public override string QualifiedName
        public override string QualifiedName
        {
            get
            {
                if (EnclosingScope.QualifiedName != string.Empty)
                    return string.Format("{0}.{1}", EnclosingScope.QualifiedName, ToString());
                else
                    return ToString();
            }
        }
        #endregion
        public override string ToString()
        {
            return string.Format("{0}({1})", Name, Args);
        }
    }
}