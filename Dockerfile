# FROM mcr.microsoft.com/dotnet/core/sdk:3.0
# COPY . /app
# WORKDIR /app
# RUN ["dotnet", "restore"]
# RUN ["dotnet", "build"]
# EXPOSE 80/tcp
# RUN chmod +x ./entrypoint.sh
# CMD /bin/bash ./entrypoint.sh

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

EXPOSE 80/tcp

WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Auth.dll"]
