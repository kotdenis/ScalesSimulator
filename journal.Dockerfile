FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./Scales.Journal.Api/Scales.Journal.Api.csproj ./
COPY ./Scales.Journal.Core/Scales.Journal.Core.csproj ./
COPY ./Scales.Journal.Domain/Scales.Journal.Domain.csproj ./
COPY ./Scales.ReferenceBook.Api/Scales.ReferenceBook.Api.csproj ./
COPY ./Scales.ReferenceBook.Core/Scales.ReferenceBook.Core.csproj ./
COPY ./Scales.ReferenceBook.Domain/Scales.ReferenceBook.Domain.csproj ./
COPY ./SharedLibrary/SharedLibrary.csproj ./
RUN dotnet restore Scales.Journal.Api.csproj
COPY . .

RUN dotnet build ./Scales.Journal.Api/Scales.Journal.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ./Scales.Journal.Api/Scales.Journal.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scales.Journal.Api.dll"]