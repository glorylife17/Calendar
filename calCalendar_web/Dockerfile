#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY calCalendar_web/calCalendar_web.csproj .
RUN dotnet restore calCalendar_web.csproj
COPY . .
RUN dotnet publish -c Release -o publish calCalendar_web/calCalendar_web.csproj

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
RUN apk add tzdata
WORKDIR /app
COPY --from=build /src/publish .

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "calCalendar_web.dll"]