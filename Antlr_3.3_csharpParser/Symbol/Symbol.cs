/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/

namespace Symbol
{
    /// <summary> A generic programming language symbol.</summary>
    public class Symbol<AST>
    {
        // All symbols at least have a name
        public string Name { get; set; }
        public ISymbolType Type;
        public IScope<AST> DefinedScope;
        public AST Def;    // points at ID node in tree

        public Symbol()
        {
        }
        public Symbol(string name)
        {
            Name = name;
        }

        #region public string QualifiedName
        public virtual string QualifiedName
        {
            get
            {
                // Use def if it's defined, else use our Name
                string name = Def.ToString();
                if (name == "")
                    name = Name;
                return string.Format("{0}.{1}", DefinedScope.QualifiedName, name);
            }
        }
        #endregion
        public override string ToString()
        {
            if (Type != null)
                return string.Format("<{0}:{1}~{2}>", Name, Type, DefinedScope);
            return Name;
        }
    }
}