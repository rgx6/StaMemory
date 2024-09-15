del publish.zip
del /q .\api\bin\Release\net8.0\linux-arm64\publish\* 
dotnet publish -c Release -r linux-arm64 --no-self-contained .\api
7za a publish.zip .\api\bin\Release\net8.0\linux-arm64\publish\*
aws lambda update-function-code --function-name StaMemory --zip-file fileb://publish.zip
