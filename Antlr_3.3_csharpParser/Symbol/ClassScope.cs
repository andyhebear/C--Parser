using System;

namespace Symbol
{
    public class ClassScope<AST> : Scope<AST>, ISymbolType
    {
        Scope<AST> Members;
        string TypeName = string.Empty;

        public ClassScope(Scope<AST> super, Scope<AST> parent, string name)
            : base(parent, name)
        {
            Members = new Scope<AST>(super, name);
            EnclosingScope = parent;
            ScopeName = name;
        }

        public new Symbol<AST> Resolve(string name, bool search_global, int line = -1)
        {
            // Check the enclosing method etc.
            Symbol<AST> rt = base.Resolve(name, false);

            if (rt != null)
                return rt;

            if (Members != null)
                rt = Members.Resolve(name, false);

            if (rt != null)
                return rt;

            // otherwise check global scope
            rt = base.Resolve(name, true);

            return rt;
        }
        string ISymbolType.Name
        {
            get { return TypeName; }
        }
        //public override void Define(Symbol sym)
    }
}