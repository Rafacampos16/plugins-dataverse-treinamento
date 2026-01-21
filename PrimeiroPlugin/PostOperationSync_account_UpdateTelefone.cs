using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeiroPlugin
{
    public class PostOperationSync_account_UpdateTelefone : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            if (context.MessageName.ToLower() != "update" || context.PrimaryEntityName != "account")
                return;

            // Target (conta)
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
                return;

            var target = (Entity)context.InputParameters["Target"];

            // Só roda se telephone1 veio no update (reforço do Filtering Attributes)
            if (!target.Contains("telephone1"))
                return;

            string novoTelefone = target.GetAttributeValue<string>("telephone1");
            if (string.IsNullOrWhiteSpace(novoTelefone))
                return;

            // Pegar o contato primário via PreImage
            if (!context.PreEntityImages.Contains("PreImage"))
                return;

            var pre = context.PreEntityImages["PreImage"];

            var contatoRef = pre.GetAttributeValue<EntityReference>("primarycontactid");
            if (contatoRef == null)
                return;

            // 1) Retrieve(GUID) do contato (como pede o roteiro)
            var contato = service.Retrieve("contact", contatoRef.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet("telephone1"));

            // 2) Atualizar telephone1 do contato
            contato["telephone1"] = novoTelefone;
            service.Update(contato);
        }
    }
}