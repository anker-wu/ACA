var input = "";
while (!WScript.StdIn.AtEndOfStream)
{
   input += WScript.StdIn.ReadAll();
}
var output = input.replace(/Type\.registerNamespace\(\"AjaxControlToolkit.HTMLEditor\"\);/ig,"");
WScript.StdOut.Write("Type.registerNamespace(\"AjaxControlToolkit.HTMLEditor\");"+output);
WScript.StdOut.Close();
