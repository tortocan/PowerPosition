#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PowerPosition.csproj", "."]
RUN dotnet restore "./PowerPosition.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PowerPosition.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PowerPosition.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
# Install Cron
RUN apt-get update -qq && apt-get -y install cron dos2unix -qq --force-yes

# Add export environment variable script and schedule
COPY *.sh ./
COPY schedule /etc/cron.d/schedule
RUN dos2unix export_env.sh \
    && dos2unix run_app.sh \
    && dos2unix /etc/cron.d/schedule \
    && chmod +x export_env.sh run_app.sh \
    && chmod 0644 /etc/cron.d/schedule

# Create log file
RUN touch /var/log/cron.log
RUN chmod 0666 /var/log/cron.log

# Volume required for tail command
VOLUME /var/log


COPY --from=publish /app/publish .
# Run Cron
CMD /app/export_env.sh && /usr/sbin/cron && tail -f /var/log/cron.log