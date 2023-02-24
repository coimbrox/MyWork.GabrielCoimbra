using Microsoft.Xrm.Sdk;
using MyWork.GabrielCoimbra.Controller;
using MyWork.GabrielCoimbra.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynacoop2023.AlfaPeople.MyWorkII
{
    public class OpportunityManager : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            //sempre que precisar conectar em outro Dynamics
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            //dentro do contexto

            Entity opportunity = (Entity)context.InputParameters["Target"];

            EntityReference accountReference = opportunity.Contains("parentaccountid") ? (EntityReference)opportunity["parentaccountid"]: null;


            tracingService.Trace("Iniciou processo do Plugin");


            if(accountReference != null)
            {
                tracingService.Trace("Referencia encontrada");
                ContaController contaController = new ContaController(service);
                Entity oppAccount = contaController.GetAccountById(accountReference.Id, new string[] { "gbr_num_total_opp" });
                contaController.IncrementNumberOfOpp(oppAccount);
                tracingService.Trace("Conta Atualizada");
            }



        }

 
    }
}
