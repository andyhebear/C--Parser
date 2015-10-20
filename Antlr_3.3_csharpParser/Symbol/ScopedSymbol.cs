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
    /// <summary> A symbol with an enclosing scope, like a method, struct, or class.</summary>
    public abstract class ScopedSymbol<AST> : Symbol<AST>, IScope<AST>
    {
        public ScopedSymbol()
        {
        }
        public ScopedSymbol(string name, IScope<AST> enclosing_scope)
            : base(name)
        {
            EnclosingScope = enclosing_scope;
        }

        public Symbol<AST> Resolve(string name, bool search_global, int line = -1)
        {
            Symbol<AST> rt;
            rt = ResolveSymbol(name, search_global);

            if (rt == null && line > 0)
            {
                ConsoleColor restore = Console.ForegroundColor;
                Console.ForegroundColor = System.ConsoleColor.Red;
                Console.Error.WriteLine("line {0}: {1} is undefined", line, name);
                Console.ForegroundColor = restore;
            }

            return rt;
        }

        /// <summary> ClassSymbol and MethodSymbol overload this.</summary>
        /// <param name="name">the symbol to resolve</param>
        /// <param name="search_global">search the global namespace?</param>
        /// <returns>Symbol</returns>
        public virtual Symbol<AST> ResolveSymbol(string name, bool search_global)
        {
            if (EnclosingScope == null && !search_global)
                return null;

            if (Members.ContainsKey(name))
                return Members[name];
            // if not here, check any parent scope
            if (EnclosingScope != null)
                return EnclosingScope.Resolve(name, search_global);
            return null; // not found
             
        }
        public void Define(Symbol<AST> sym)
        {
            Members[sym.Name] = sym;
            sym.DefinedScope = this; // track the scope in each symbol
        }

        public IScope<AST> EnclosingScope
        { get; set; }

        public string ScopeName
        { get { return Name; } }

        #region public override string QualifiedName
        public override string QualifiedName
        {
            get
            {
                if (EnclosingScope.QualifiedName != string.Empty)
                    return string.Format("{0}.{1}", EnclosingScope.QualifiedName, Name);
                else
                    return Name;
            }
        }
        #endregion

        /** Indicate how subclasses store scope members. Allows us to
         *  factor out common code in this class.
         */
        public abstract Dictionary<string, Symbol<AST>> Members { get; }
    }
}