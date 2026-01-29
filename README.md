# 🔌 Plugins Dataverse 

Projeto desenvolvido durante treinamento em Power Platform / Dataverse, com foco em criação de **Plugins em C#** para automação de regras de negócio no Dynamics 365.

## 🚀 Funcionalidades

### ✅ Tarefa 4 — Total de Pedidos por Conta
- Ao criar um Pedido de Máquina vinculado a uma Conta:
  - O plugin soma todos os pedidos daquela conta
  - Atualiza automaticamente o campo **Total de Pedidos** na tabela Conta

### ✅ Tarefa 5 — Sincronização de Telefone
- Ao atualizar o telefone (`telephone1`) da Conta:
  - O plugin atualiza automaticamente o telefone do **Contato Primário** associado

## 🛠 Tecnologias

- C#
- .NET Framework
- Microsoft Dataverse SDK
- Plugin Registration Tool
- Dynamics 365 / Power Platform

## 💡 Conceitos aplicados

- IPlugin
- PostOperation Steps
- Pre Image
- QueryExpression
- Retrieve / Update via IOrganizationService
- Regras de negócio no servidor (Server-side)

---

Projeto com fins educacionais e de aprendizado prático em Dataverse e Power Platform.
