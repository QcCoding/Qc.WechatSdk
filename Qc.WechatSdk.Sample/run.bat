taskkill /F /T /FI "WINDOWTITLE eq Qc.WechatSdk.Sample" /IM dotnet.exe
start "Qc.WechatSdk.Sample" dotnet run
exit