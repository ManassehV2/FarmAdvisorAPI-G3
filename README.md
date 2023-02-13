# FarmAdvisor

### Running the app

-> install dotnet

-> install docker

-> run the following in the root directory of the project

```
docker compose up
```

-> open another terminal in the root directory of the project and run the following 

```
cd src
dotnet tool install -g dotnet-ef
dotnet ef migrations add CreateTables
dotnet ef database update
dotnet run
```