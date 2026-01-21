using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeiroPlugin
{
    public class PreOperationSync_smt_pedidodemaquina : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context =

           (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext
           ));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service =
           serviceFactory.CreateOrganizationService(context.UserId);
            //Verifica se o gatilho é do tipo “create “e se a tabela é do tipo “smt_pedidodemaquina”
            if (context.MessageName.ToLower() == "create" &&
           context.PrimaryEntityName == "smt_pedidosdemaquina")
            {
                //Verifica se o contexto do formulário e dos campos
                if (context.InputParameters.Contains("Target") &&
               context.InputParameters["Target"] is Entity)
                {
                    Entity targetMaquinas =
                   (Entity)context.InputParameters["Target"];
                    //Armazena o nome do máquina
                    string MaquinaName = string.Empty;
                    //Verifica se no formulário existe o nome
                    if (targetMaquinas.Contains("smt_nomedamaquina"))
                    {
                        //Armazena o valor do campo nome do máquina
                        MaquinaName =
                       targetMaquinas["smt_nomedamaquina"].ToString();
                        //Faz a requisição passando a variável com o valor do nomeda máquina
                        string xml = string.Format(@"<fetch version='1.0' outputformat='xml-platform'
                                                 mapping='logical' distinct='true'>
                                                <entity name='smt_pedidosdemaquina'>
                                                 <attribute name=
                                                'smt_nomedamaquina' />
                                                 <filter type='and'>
                                                 <condition attribute='smt_nomedamaquina'
                                                operator='eq' value='{0}'/>
                                                 </filter>
                                                </entity>
                                                </fetch>",
                                                     MaquinaName
                       );
                        EntityCollection resultado = service.RetrieveMultiple(new
                       FetchExpression(xml));
                        //Caso já exista uma máquina cadastrado ele irá retornar um erro que já existe uma máquina
                        // cadastrado e não irá realizar o cadastro atual.
                        if (resultado.Entities.Count > 0)
                        {
                            throw new InvalidPluginExecutionException("Este dispotivo já possui cadastro.");
                        }
                    }
                }
            }
        }

    }
}
