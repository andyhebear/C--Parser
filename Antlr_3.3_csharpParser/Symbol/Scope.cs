/***    
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using System;
using System.Collections.Generic;

namespace Symbol
{
    public interface IScope<AST>
    {
        string ScopeName { get; }
        string QualifiedName { get; }
        IScope<AST> EnclosingScope { get; set; }
        Symbol<AST> Resolve(string name, bool search_global, int line = -1);
        void Define(Symbol<AST> sym);
    }

    public class Scope<AST> : IScope<AST>
    {
        public IScope<AST> EnclosingScope { get; set; } // null if global (outermost) scope
        public string ScopeName { get; protected set; }
        #region public override string QualifiedName
        public virtual string QualifiedName
        {
            get
            {
                if (ScopeName == "global")
                    return string.Empty;

                return EnclosingScope.QualifiedName + ScopeName;
            }
        }
        #endregion
        Dictionary<string, Symbol<AST>> symbols = new Dictionary<string, Symbol<AST>>();

        public Scope(IScope<AST> parent, string name)
        {
            EnclosingScope = parent;
            ScopeName = name;
        }

        public Symbol<AST> Resolve(string name, bool search_global, int line = -1)
        {
            if (EnclosingScope == null && !search_global)
                return null;

            if (symbols.ContainsKey(name))
                return symbols[name];

            Symbol<AST> rt = null;
            // if not here, check any enclosing scope
            if (EnclosingScope != null)
                rt = EnclosingScope.Resolve(name, search_global);

            if (rt == null && line > 0)
            {
                ConsoleColor restore = Console.ForegroundColor;
                Console.ForegroundColor = System.ConsoleColor.Red;
                Console.Error.WriteLine("line {0}: {1} is undefined", line, name);
                Console.ForegroundColor = restore;
            }
            return rt; // not found
        }

        public void Define(Symbol<AST> sym)
        {
            symbols[sym.Name] = sym;
            sym.DefinedScope = this; // track the scope in each symbol
        }

        /// <summary> [key,key,key,key] </summary>
        public override string ToString()
        {
            string rt = string.Empty;

            foreach (string s in symbols.Keys)
                rt += s + ",";
            rt = rt.TrimEnd(new char[] { ',' });
            return ScopeName + ":[" + rt + "]";
        }
    }
}