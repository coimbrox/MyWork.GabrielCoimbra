using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWork.GabrielCoimbra.Model
{
    public class Contato
    {
        public CrmServiceClient ServiceClient { get; set; }

        public string LogicalNameContact { get; set; }

        public Contato(CrmServiceClient crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.LogicalNameContact = "contact";
        }


        public Guid CreateContactDynamic(string firstName, string lastName, string cpf, string cargo)
        {
            Entity contato = new Entity(this.LogicalNameContact);
            contato["firstname"] = firstName;
            contato["lastname"] = lastName;
            contato["gbr_cpf"] = cpf;
            contato["jobtitle"] = cargo;

            Guid contactId = this.ServiceClient.Create(contato);
            return contactId;
        }

        public Entity GetContactByCPF(string contactCPF)
        {
            QueryExpression busca = new QueryExpression(LogicalNameContact);
            busca.ColumnSet.AddColumn("gbr_cpf");
            busca.Criteria.AddCondition("gbr_cpf", ConditionOperator.Equal, LogicalNameContact);
            EntityCollection contact = ServiceClient.RetrieveMultiple(busca);

            return contact.Entities.FirstOrDefault();
        }
    }
}
