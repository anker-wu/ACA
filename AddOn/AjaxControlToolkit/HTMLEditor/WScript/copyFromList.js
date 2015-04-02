var fso = WScript.CreateObject("Scripting.FileSystemObject")

var input = "";
while (!WScript.StdIn.AtEndOfStream)
{
   input += WScript.StdIn.ReadAll();
}

var list = input.split("\r\n");

for(var i=0; i < list.length; i++)
{
   var file = list[i];

   if(file != "" && !(/^\/\//.test(file)))
   {
      var stream = fso.OpenTextFile(file, 1);
      var content = stream.ReadAll();
      stream.Close();
      WScript.StdOut.Write(content);
   }
}

WScript.StdOut.Close();
