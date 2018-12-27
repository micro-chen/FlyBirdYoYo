# pleas ensure the system powershell by administrator : Set-ExecutionPolicy remotesigned 
echo  'begin to stop wechat  dev tool process...'
Get-Process | Where-Object {$_.Name -eq "wechatdevtools"}|ForEach-Object {$_.Kill()}
echo  'succes stop wechat  dev tool  process!'
