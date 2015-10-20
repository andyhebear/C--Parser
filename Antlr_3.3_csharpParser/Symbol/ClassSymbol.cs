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
    public class ClassSymbol<AST> : ScopedSymbol<AST>, ISymbolType
    {
        /** This is the superclass not enclosingScope field. We still record the enclosing scope so we can push in and 
         * pop out of class definitions.
         */
        public ClassSymbol<AST> BaseClass;

        public ClassSymbol()
        {
        }
        public ClassSymbol(string name, IScope<AST> enclosingScope, ClassSymbol<AST> base_class = null)
            : base(name, enclosingScope)
        {
            BaseClass = base_class;
        }

        public override Symbol<AST> ResolveSymbol(string name, bool search_global)
        {
            Symbol<AST> rt = null;

            // check local etc
            rt = base.ResolveSymbol(name, false);
            if (rt != null)
                return rt;

            rt = ResolveMember(name);
            if (rt != null)
                return rt;

            // try global now
            return base.ResolveSymbol(name, search_global);
        }

        public Symbol<AST> ResolveMember(string name, int line = -1)
        {
            if (Members.ContainsKey(name))
                return Members[name];

            // if not here, check just the base_class chain
            Symbol<AST> rt = BaseClass != null ? BaseClass.ResolveMember(name) : null;

            if (rt == null && line > 0)
            {
                ConsoleColor restore = Console.ForegroundColor;
                Console.ForegroundColor = System.ConsoleColor.Red;
                Console.Error.WriteLine("line {0}: member {1} is undefined", line, name);
                Console.ForegroundColor = restore;
            }
            return rt;
        }

        #region public Dictionary<string, Symbol<AST>> Members
        /** List of all fields and methods */
        public Dictionary<string, Symbol<AST>> members = new Dictionary<string, Symbol<AST>>();
        public override Dictionary<string, Symbol<AST>> Members
        { get { return members; } }
        #endregion
        public override string ToString()
        {
            // key,key,key,key
            string keys = string.Empty;
            foreach (string s in members.Keys)
                keys += s + ",";
            keys = keys.TrimEnd(new char[] { ',' });

            return string.Format("class {0}:{{{1}}}", Name, keys);
        }
    }
}