using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace CSharpParser
{
    public class csParserException : Exception
    {
        public string[] _tokenNames;
        public BaseRecognizer _source;
        public RecognitionException _exception;
        public string _errorStr;
    }
}
