FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos de projeto respeitando a estrutura de pastas da solução
COPY ["Fiap.Catalog.Api/Fiap.Catalog.Api.csproj", "Fiap.Catalog.Api/"]
COPY ["Fiap.Catalog.Domain/Fiap.Catalog.Domain.csproj", "Fiap.Catalog.Domain/"]
COPY ["Fiap.Catalog.Infra/Fiap.Catalog.Infra.csproj", "Fiap.Catalog.Infra/"]

# Restaura as dependências a partir do projeto principal
RUN dotnet restore "Fiap.Catalog.Api/Fiap.Catalog.Api.csproj"

# Copia todo o código fonte
COPY . .

# Define o diretório de trabalho no projeto principal e compila
WORKDIR "/src/Fiap.Catalog.Api"
RUN dotnet build "Fiap.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Fiap.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_URLS="http://+:8082"
ENV ASPNETCORE_ENVIRONMENT="Development"

# Dá permissão de root temporária para criar a pasta e copiar o banco existente
USER root
RUN mkdir -p /app/database

# Sobrescreve a String de Conexão para apontar para a pasta do container
ENV ConnectionStrings__DefaultConnection="Data Source=/app/database/catalog_data_base.db"

# Copia o arquivo .db que está no seu projeto Infra para dentro da pasta criada
COPY Fiap.Catalog.Infra/catalog_data_base.db /app/database/catalog_data_base.db

# Ajusta as permissões para o usuário padrão da aplicação conseguir escrever no banco
RUN chown -R app:app /app/database

# Retorna para o usuário seguro do container
USER app

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fiap.Catalog.Api.dll"]