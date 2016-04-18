mkdir .\bin\Release\StateFunding
move .\bin\Release\assets .\bin\Release\StateFunding
move .\bin\Release\data .\bin\Release\StateFunding
move .\bin\Release\StateFunding.version .\bin\Release\StateFunding
move .\bin\Release\license.txt .\bin\Release\StateFunding
move .\bin\Release\StateFunding.dll .\bin\Release\StateFunding
"C:\Program Files\7-Zip\7z" a StateFunding.zip .\bin\Release\StateFunding 
