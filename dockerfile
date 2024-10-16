FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY *.sln ./

COPY Movies.Api/Movies.Api.csproj ./Movies.Api/
COPY Movies.Application/Movies.Application.csproj ./Movies.Application/
COPY Movies.Contracts/Movies.Contracts.csproj ./Movies.Contracts/

RUN dotnet restore ./movies.sln --verbosity detailed

COPY . ./

RUN dotnet build


RUN ["dotnet", "publish", "-c", "Release", "-o", "out"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build-env /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Movies.Api.dll"]
