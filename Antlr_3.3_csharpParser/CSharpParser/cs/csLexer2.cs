using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace CSharpParser
{
    partial class csLexer 
    {
        public event System.Action<csParserException> OnDisplayRecognitionError;
        public List<string> _SyntaxErrors = new List<string>();
        public override void Reset() {
            base.Reset();
            _SyntaxErrors.Clear();
        }
        public override void DisplayRecognitionError(string[] tokenNames, RecognitionException e) {
            if (OnDisplayRecognitionError != null) {
                csParserException ce = new csParserException();
                ce._tokenNames = tokenNames;
                ce._source = this;
                ce._exception = e;
                ce._errorStr = GetErrorHeader(e) + " " + GetErrorMessage(e, tokenNames);
                OnDisplayRecognitionError(ce);
            }
            //base.DisplayRecognitionError(tokenNames, e);
            string hdr = GetErrorHeader(e);
            string msg = GetErrorMessage(e, tokenNames);
            _SyntaxErrors.Add(hdr + " " + msg);
            EmitErrorMessage(hdr + " " + msg);
        }
        /** <summary>What is the error header, normally line/character position information?</summary> */
        public override string GetErrorHeader(RecognitionException e) {
            string prefix = SourceName ?? string.Empty;
            if (prefix.Length > 0)
                prefix += ' ';

            return string.Format("{0}line {1}:{2}", prefix, e.Line, e.CharPositionInLine + 1);
        }
        public override string GetErrorMessage(RecognitionException e, string[] tokenNames) {
            string msg = e.Message;
            if (e is UnwantedTokenException) {
                UnwantedTokenException ute = (UnwantedTokenException)e;
                string tokenName = "<unknown>";
                if (ute.Expecting == TokenTypes.EndOfFile) {
                    tokenName = "EndOfFile";
                }
                else {
                    tokenName = tokenNames[ute.Expecting];
                }
                msg = "extraneous input " + GetTokenErrorDisplay(ute.UnexpectedToken) +
                    " expecting " + tokenName;
            }
            else if (e is MissingTokenException) {
                MissingTokenException mte = (MissingTokenException)e;
                string tokenName = "<unknown>";
                if (mte.Expecting == TokenTypes.EndOfFile) {
                    tokenName = "EndOfFile";
                }
                else {
                    tokenName = tokenNames[mte.Expecting];
                }
                msg = "missing " + tokenName + " at " + GetTokenErrorDisplay(e.Token);
            }
            else if (e is MismatchedTokenException) {
                MismatchedTokenException mte = (MismatchedTokenException)e;
                string tokenName = "<unknown>";
                if (mte.Expecting == TokenTypes.EndOfFile) {
                    tokenName = "EndOfFile";
                }
                else {
                    tokenName = tokenNames[mte.Expecting];
                }
                msg = "mismatched input " + GetTokenErrorDisplay(e.Token) +
                    " expecting " + tokenName;
            }
            else if (e is MismatchedTreeNodeException) {
                MismatchedTreeNodeException mtne = (MismatchedTreeNodeException)e;
                string tokenName = "<unknown>";
                if (mtne.Expecting == TokenTypes.EndOfFile) {
                    tokenName = "EndOfFile";
                }
                else {
                    tokenName = tokenNames[mtne.Expecting];
                }
                // workaround for a .NET framework bug (NullReferenceException)
                string nodeText = (mtne.Node != null) ? mtne.Node.ToString() ?? string.Empty : string.Empty;
                msg = "mismatched tree node: " + nodeText + " expecting " + tokenName;
            }
            else if (e is NoViableAltException) {
                //NoViableAltException nvae = (NoViableAltException)e;
                // for development, can add "decision=<<"+nvae.grammarDecisionDescription+">>"
                // and "(decision="+nvae.decisionNumber+") and
                // "state "+nvae.stateNumber
                msg = "no viable alternative at input " + GetTokenErrorDisplay(e.Token);
            }
            else if (e is EarlyExitException) {
                //EarlyExitException eee = (EarlyExitException)e;
                // for development, can add "(decision="+eee.decisionNumber+")"
                msg = "required (...)+ loop did not match anything at input " +
                    GetTokenErrorDisplay(e.Token);
            }
            else if (e is MismatchedSetException) {
                MismatchedSetException mse = (MismatchedSetException)e;
                msg = "mismatched input " + GetTokenErrorDisplay(e.Token) +
                    " expecting set " + mse.Expecting;
            }
            else if (e is MismatchedNotSetException) {
                MismatchedNotSetException mse = (MismatchedNotSetException)e;
                msg = "mismatched input " + GetTokenErrorDisplay(e.Token) +
                    " expecting set " + mse.Expecting;
            }
            else if (e is FailedPredicateException) {
                FailedPredicateException fpe = (FailedPredicateException)e;
                msg = "rule " + fpe.RuleName + " failed predicate: {" +
                    fpe.PredicateText + "}?";
            }
            return msg;
        }
    }
}
