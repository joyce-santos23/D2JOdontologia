
#TheGrad

Este repositório contém o código-fonte de uma aplicação para organizar formaturas, desenvolvida com .NET 8 e utilizando SQL Server como banco de dados. A aplicação está hospedada no Azure e oferece APIs para gerenciamento de participantes, eventos e pagamentos.

#Descrição do Projeto
TheGrad foi desenvolvido para ajudar na organização de formaturas, oferecendo funcionalidades para gerenciar participantes, eventos, tarefas e pagamentos. A aplicação está dividida em duas APIs separadas:
TheGrad API: Gerencia participantes, eventos e tarefas.
TheGrad Payments API: Gerencia os pagamentos relacionados à formatura.

#Tecnologias Utilizadas
.NET 8: Framework para construção de aplicações web robustas e escaláveis.
SQL Server: Sistema de gerenciamento de banco de dados relacional.
Azure: Plataforma de nuvem utilizada para hospedagem e gerenciamento da aplicação.
Azure App Service: Serviço de hospedagem para as APIs.
Azure SQL Database: Banco de dados gerenciado para a aplicação.

#Pré-requisitos
Antes de começar, certifique-se de ter os seguintes itens instalados:
.NET 8 SDK
SQL Server
Azure CLI
Uma conta no Azure

#Estrutura do Projeto
thegrad/
├── TheGrad_API/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── appsettings.json
├── TheGrad_Payments_API/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── appsettings.json
└── README.md

#Endpoints da API
#TheGrad API
GET /participantes: Listar todos os participantes.
POST /participantes: Adicionar um novo participante.
PUT /participantes/{id}: Atualizar os dados de um participante.
DELETE /participantes/{id}: Deletar um participante.
#TheGrad Payments API
GET /pagamentos: Listar todos os pagamentos.
POST /pagamentos: Registrar um novo pagamento.
PUT /pagamentos/{id}: Atualizar o status de um pagamento.

