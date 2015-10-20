using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace CSharpParser
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine("---------------");
            Console.Error.WriteLine("test.cs");


            string file_name = "test.cs";
            CommonTokenStream tokens = CreateLexer<csLexer>(file_name);
            csParser p = new csParser(tokens);
            TreeAdaptor csAdaptor = new TreeAdaptor();
            p.TreeAdaptor = csAdaptor;
            csParser.compilation_unit_return parser_rt;
            CommonTree tree = null;
            p.OnDisplayRecognitionError += (err => {
                Console.WriteLine(err._errorStr);
            });
            parser_rt = p.compilation_unit();

            #region Error Checking
            int count = 0;
            tree = (CommonTree)parser_rt.Tree;
            // Check if we didn't get an AST
            // This often happens if your grammar and tree grammar don't match
            if (tree == null) {
                if (tokens.Count > 0) {
                    Console.Error.WriteLine("No Tree returned from parsing! (Your rule did not parse correctly)");
                }
                else {
                    // the file was empty, this is not an error.
                    // Clear archive attribute
                    File.SetAttributes(file_name, File.GetAttributes(file_name) & ~FileAttributes.Archive);
                    ++count;
                }
                return ;
            }
            #endregion
            if (p.NumberOfSyntaxErrors > 0) {
                List<string> strs = p.ToStrings(tokens.GetTokens());
                foreach (var v in strs) {
                  //  Console.WriteLine(v);
                }
            }
            //Console.WriteLine(tree.ToStringTree());
            // Get the AST stream
            CommonTreeNodeStream nodes = new CommonTreeNodeStream(csAdaptor, tree);
            // Add the tokens for DumpNodes, otherwise there are no token names to print out.
            nodes.TokenStream = tokens;

        
          Console.WriteLine(nodes.ToTokenTypeString());
          
            // Dump the tree nodes if -n is passed on the command line.
            DumpNodes(nodes);

            //SymbolTable symtab = new SymbolTable(); 	// init symbol table
            //Def def = new Def(nodes, symtab);       	// create Def phase
            //def.Downup(tree);                          	// Do pass 1

            //System.Console.WriteLine("globals: \n" + symtab.globals + "\n\n");
            //System.ConsoleColor restore = System.Console.ForegroundColor;
            //System.Console.ForegroundColor = ConsoleColor.Yellow;
            //System.Console.WriteLine("Ref:");
            //System.Console.ForegroundColor = restore;

            //nodes.Reset(); // rewind AST node stream to root

            //Ref @ref = new Ref(nodes);               	// create Ref phase
            //@ref.Downup(tree);                          	// Do pass 2
	

            Application.Run(new Form1());
        }

        /// <summary> CreateLexer </summary>
        public static CommonTokenStream CreateLexer<L>(string file_name)
            where L : Lexer, new() {
            string inputFileName = file_name;
            CommonTokenStream tokens = null;

            if (!Path.IsPathRooted(inputFileName))
                inputFileName = Path.Combine(Environment.CurrentDirectory, inputFileName);

            Console.WriteLine(inputFileName);
            ICharStream input = new ANTLRFileStream(inputFileName);
            L lex = new L();
            lex.CharStream = input;

            tokens = new CommonTokenStream(lex);
            return tokens;
        }
        /// <summary> DumpNodes
        /// The CommonTreeNodeStream has a tree in "flat form".  The UP and DOWN tokens represent the branches of the
        /// tree.  Dump these out in tree form to the console.
        /// </summary>
        static void DumpNodes(CommonTreeNodeStream nodes) {
            // Dump out nodes if -n on command line
            //if (Util.Args.IsFlagSet("-n")) {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Nodes");
                int spaces = 0;
                string str_spaces = "                                                                                       ";
                object o_prev = string.Empty;
                //for (int n = 0; n < nodes.Count; ++n)
                object o = nodes.NextElement();
                while (!nodes.IsEndOfFile(o)) {
                    //object o = nodes.Get(n);
                    //object o = nodes[n];

                    if (o.ToString() == "DOWN") {
                        spaces += 2;
                        if (o_prev.ToString() != "UP" && o_prev.ToString() != "DOWN")
                            Console.Write("\r\n{0} {1}", str_spaces.Substring(0, spaces), o_prev);
                    }
                    else if (o.ToString() == "UP") {
                        spaces -= 2;
                        if (o_prev.ToString() != "UP" && o_prev.ToString() != "DOWN")
                            Console.Write(" {0}\r\n{1}", o_prev, str_spaces.Substring(0, spaces));
                    }
                    else if (o_prev.ToString() != "UP" && o_prev.ToString() != "DOWN")
                        Console.Write(" {0}", o_prev.ToString());

                    o_prev = o;
                    o = nodes.NextElement();
                }
                if (o_prev.ToString() != "UP" && o_prev.ToString() != "DOWN")
                    Console.WriteLine(" {0}", o_prev.ToString());
                Console.ResetColor();
            //}
        }
        public class TreeAdaptor : CommonTreeAdaptor
        {
            [DebuggerStepThrough()]
            //public override object Create(IToken token) {
            //    return new CSharpNode(token);
           // }
            //public override object DupNode(object t) {
           //     if (t == null)
            //        return null;
            //    return Create(((CSharpNode)t).Token);
            //}
            public override object ErrorNode(ITokenStream input, IToken start, IToken stop, RecognitionException e) {
               // CSharpNode t = new CSharpNode(start);
                Console.WriteLine("returning error node '"  + "' @index=" + input.Index);
                return base.ErrorNode(input,start,stop,e);
            }
        }
    }
}
