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

            //se for create busca no target se ñ pega na pre image
            Entity opportunity = new Entity();
            bool? incrementOrDecrement = null;
            SetVariables(context, out opportunity, out incrementOrDecrement);

            EntityReference accountReference = opportunity.Contains("parentaccountid") ? (EntityReference)opportunity["parentaccountid"] : null;




            if (accountReference != null)
            {
                ContaController contaController = new ContaController(service);
                Entity oppAccount = contaController.GetAccountById(accountReference.Id, new string[] { "gbr_num_total_opp" });
                contaController.IncrementOrDecrementNumberOfOpp(oppAccount, incrementOrDecrement);
                tracingService.Trace("Conta Atualizada");
            }



        }

        private static void SetVariables(IPluginExecutionContext context, out Entity opportunity, out bool? incrementOrDecrement)
        {
            if (context.MessageName == "Create")
            {
                opportunity = (Entity)context.InputParameters["Target"];
                incrementOrDecrement = true;
            }
            else
            {
                opportunity = (Entity)context.PreEntityImages["PreImage"];
                incrementOrDecrement = false;
            }
        }

    }
}
