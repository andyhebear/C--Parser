// ArgumentSymbol.cs
//

namespace Symbol
{
    /** Represents a method argument definition (name,type) in symbol table */
    public class ArgumentSymbol<AST> : Symbol<AST>
    {
        public ArgumentSymbol(string name)
            : base(name)
        {
        }
        // format: SomeClass.Function(p1,p2)_p2 
        #region public string QualifiedName
        public override string QualifiedName
        {
            get
            {
                return string.Format("{0}_{1}", DefinedScope.QualifiedName, Name);
            }
        }
        #endregion
    }
}
