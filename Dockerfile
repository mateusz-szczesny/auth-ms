FROM mcr.microsoft.com/dotnet/core/sdk:3.0
COPY . /app
WORKDIR /app
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
ENV PATH="${PATH}:/root/.dotnet/tools"
EXPOSE 80/tcp
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh