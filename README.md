# FarmAdvisor
### Running the app
-> install dotnet
-> install docker
-> run the following in the root directory of the project
```
docker compose up
```
-> wait for the above process to download and start running (wait about 5 minutes after it finishes the download)
-> open another terminal in the root directory of the project and run the following 
```
cd src
dotnet tool install -g dotnet-ef
dotnet ef migrations add CreateTables
dotnet ef database update
dotnet run
```
-> visit https://localhost:7086/swagger/index.html to get the endpoints of the running REST API
### Notes
-> The SignUp end point does't send any Verification code to the phone provided, Use 123456 instead