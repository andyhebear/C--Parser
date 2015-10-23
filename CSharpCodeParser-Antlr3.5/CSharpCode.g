grammar CSharpCode;

options
{
  language=CSharp2;
}

@parser::namespace { Gplusnasite.CSharpParser }
@lexer::namespace { Gplusnasite.CSharpParser }

@parser::members
{
    public AccessType Accessor = AccessType.Private;
}

@lexer::members
{
    public AccessType Accessor = AccessType.Private;
}

SPACE
: (' ' | '\t')+
;

COMMENT
  : ('//' ~('\n'|'\r')* '\r'? '\n') {Skip();}
  | ('/*' ( options {greedy=false;} : . )* '*/') {Skip();}
  ;
  
fragment
HEX_DIGIT : ('0'..'9'|'a'..'f'|'A'..'F') ;

fragment
UNICODE_ESC
    :   '\\' 'u' HEX_DIGIT HEX_DIGIT HEX_DIGIT HEX_DIGIT
    ;
    
fragment
ESC_SEQ
    :   '\\' ('b'|'t'|'n'|'f'|'r'|'\"'|'\''|'\\')
    |   UNICODE_ESC
    ;
    
STRING
    :  ('"' (ESC_SEQ | ~('\\'|'"'))* '"') | '\'' (ESC_SEQ | ~('\\'|'\''))* '\''
    ;
    
NAME
    :('a'..'z'|'A'..'Z'|'_')('a'..'z'|'A'..'Z'|'0'..'9'|'_')*
    ;

fragment
using_namespace returns [string result]
: NAME
{
    $result = $NAME.text;
}
;

using_namespaces returns [Namespaces result]
    @init
    {
        $result = new Namespaces();
    }
    : ('using' SPACE (namespace1 = using_namespace)
    {
        $result.Names.Add($namespace1.result);
        $result.FullName += $namespace1.result;
    }
    ('.' (namespace2 = using_namespace)
    {
        $result.Names.Add($namespace2.result);
        $result.FullName += ("." + $namespace2.result);
    }
    )* ';')
;

access_type returns [AccessType result]
    :
    ('public' ':')
    {
        Accessor = AccessType.Public;
        $result = AccessType.Public;
    }
    |('protected' ':')
    {
        Accessor = AccessType.Protected;
        $result = AccessType.Protected;
    }
    |('private' ':')
    {
        Accessor = AccessType.Private;
        $result = AccessType.Private;
    }
    ;

    
DEFAULT_VALUE
    :  ('a'..'z'|'A'..'Z'|'_'|'@') ('a'..'z'|'A'..'Z'|'0'..'9'|'_'|'@')*
    ;

variable_value
    :'=' (~(';')+) ';'
    ;
        
variable returns [Variable result]
    @init
    {
        $result = new Variable();
    }
    :(access_type? (type = NAME) SPACE (name = NAME) ';')
    {
    	$result.Accessor = Accessor;
    	$result.Type = $type.text;
    	$result.Name = $name.text;
    }
    ;
    
parameter returns [Variable result]
    @init
    {
        $result = new Variable();
    }
    :(type = NAME) SPACE (name = NAME)
    {
    	$result.Accessor = AccessType.None;
    	$result.Type = $type.text;
    	$result.Name = $name.text;
    }
    ;
    
parameters returns [List<Variable> result]
    @init
    {
        $result = new List<Variable>();
    }
    :((parameter1 = parameter)
    {
    	if ($parameter1.result != null)
    	{
    	    $result.Add($parameter1.result);
    	}
    }
     (SPACE? (',' SPACE? parameter2 = parameter)
     {
    	if ($parameter2.result != null)
    	{
    	    $result.Add($parameter2.result);
    	}
    })*
     
     )?
    ;
    
function_value
    :
    '{' ( options {greedy=false;} : . )* '}'
    ;

function returns [Function result]
    @init
    {
        $result = new Function();
    }
    :access_type? (return_type = NAME) SPACE (name = NAME) '(' SPACE? parameters SPACE? ')' (';' | function_value)
    {
    	$result.Accessor = Accessor;
    	$result.ReturnType = $return_type.text;
    	$result.Name = $name.text;
    	$result.Parameters = $parameters.result;    	
    }
    ;
    
class returns [Class result]
    @init
    {
        $result = new Class();
    }
    :access_type? 'class' NAME
    {
        $result.Name = $NAME.text;
    }
    '{'
    
    (variable
    {
    	$result.Variables.Add($variable.result);
    }
    | function
    {
    	$result.Functions.Add($function.result);
    }
    )*
    
    '}' ';'
    ;
    
cpp_code returns [CSharpCode result]
    @init
    {
        $result = new CSharpCode();
    }
    :
    (
    using_namespaces
    {
    	$result.UsingNamespaces.Add($using_namespaces.result);
    }
    | class
    {
    	$result.Classes.Add($class.result);
    }
    | variable
    {
    	$result.GlobalVariables.Add($variable.result);
    }
    | function
    {
    	$result.GlobalFunctions.Add($function.result);
    }
    )*
    ;
    
public parse returns [CSharpCode result]
  :  cpp_code EOF
  {
  	$result = $cpp_code.result;
  };
  