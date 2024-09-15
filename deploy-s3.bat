cd client
rmdir /s /q .\dist
call npm run build
aws s3 sync .\dist\ s3://stamemory.rgx6.com --delete
cd ..
