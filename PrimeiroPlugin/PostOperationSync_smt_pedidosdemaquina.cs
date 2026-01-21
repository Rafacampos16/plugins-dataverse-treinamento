using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace PluginsTreinamento
{
    public class PostOperationSync_smt_pedidosdemaquina : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // 1) Contexto e Service
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            // 2) Garantir que é CREATE
            if (context.MessageName.ToLower() != "create")
                return;

            // 3) Pegar o Target (registro que está sendo criado)
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
                return;

            Entity pedido = (Entity)context.InputParameters["Target"];

            // 4) Pegar Conta associada ao pedido (ajuste o nome do lookup!)
            if (!pedido.Contains("smt_conta")) return; // <-- ajuste esse nome

            EntityReference contaRef = pedido.GetAttributeValue<EntityReference>("smt_conta");

            // 5) Buscar todos pedidos da conta
            QueryExpression query = new QueryExpression("smt_pedidosdemaquina");
            query.ColumnSet = new ColumnSet("smt_preco"); // <-- ajuste esse nome (campo de preço)
            query.Criteria.AddCondition("smt_conta", ConditionOperator.Equal, contaRef.Id); // <-- ajuste se necessário

            EntityCollection pedidos = service.RetrieveMultiple(query);

            // 6) Somar
            decimal total = 0;
            foreach (var p in pedidos.Entities)
            {
                if (p.Contains("smt_preco"))
                {
                    Money valor = p.GetAttributeValue<Money>("smt_preco");
                    total += valor.Value;
                }
            }

            // 7) Atualizar a conta com o total
            Entity contaUpdate = new Entity("account", contaRef.Id);
            contaUpdate["smt_totaldepedidos"] = new Money(total); // <-- ajuste nome do campo da conta

            service.Update(contaUpdate);
        }
    }
}
