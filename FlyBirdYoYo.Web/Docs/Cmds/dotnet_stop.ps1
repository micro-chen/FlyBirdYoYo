# pleas ensure the system powershell by administrator : Set-ExecutionPolicy remotesigned 
echo  'begin to stop dotnet process...'
Get-Process | Where-Object {$_.Name -eq "dotnet"}|ForEach-Object {$_.Kill()}
echo  'succes stop dotnet process!'
