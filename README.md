# My ASP.NET Web API

Este projeto é uma Web API construída com ASP.NET Core. Este guia fornecerá as etapas necessárias para configurar e executar a aplicação localmente.

## Pré-requisitos

Antes de começar, você precisará ter instalado em sua máquina:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (opcional, dependendo da configuração do banco de dados)

## Configuração do Projeto

### 1. Clone o repositório

Clone este repositório para sua máquina local usando o seguinte comando:

```bash
git clone https://github.com/Santana-Fernando/Server.git
```

### 2. Configurar string de conexção

Insira seus dados na conections string

```bash
"DefaultConnection": "Data Source=host;User ID=usuario;Password=senha;Database=TaskRegister;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
```

### 3. Rode as migations

Ferramentas > Gerenciador de pacotes NuGet > Console do gerendiado de pacotes > e rode o comando

```bash
Update-Database
```
### 3. Rode o projeto
botão direito no projeto Presentation > Definir como projeto de inicialização > Iniciar


### Extra. Rode o projeto pelo Docker

Ajuste a string de conexão

```bash
"DockerConnection": "Server=sqlserver;Database=TaskRegister;User ID=usuario;Password=senha;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"
```

Faça a mudança no Infra.IoC > DependenceInjection
```bash
services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DockerConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
```

Coloque as credenciais no arquivo docker-compose.override.yml
```bash
- ConnectionStrings__DockerConnection=Server=sqlserver;Database=TaskRegister;User ID=sa;Password=Fern@nd01331;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;
```

Abra o terminal na pasta do projeto e rode o comando
```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```
