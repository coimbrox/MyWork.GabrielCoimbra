using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWork.GabrielCoimbra.Model
{
    public class Conta
    {
        public IOrganizationService ServiceClient { get; set; }
        public string LogicalName { get; set; }

        public Conta(IOrganizationService crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.LogicalName = "account";
        }

        public Conta(CrmServiceClient crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.LogicalName = "account";
        }
        public  Guid Create()
        {
            Entity conta = new Entity(this.LogicalName);
            conta["name"] = "MyWorkI";
            conta["telephone1"] = "(71) 99304-9142";
            conta["fax"] = "(11) 1515-1512";
            conta["gbr_num_total_opp"] = 0;
            conta["gbr_tipo_relacao"] = new OptionSetValue(803590000);
            conta["gbr_valor_total_opp"] = new Money(0);
            conta["primarycontactid"] = new EntityReference("contact", new Guid("79ae8582-84bb-ea11-a812-000d3a8b3ec6"));

            Guid accountId = this.ServiceClient.Create(conta);
            return accountId;
        }   
      //  public  Guid CreateDynamic(string accountName, string telephone, string fax, int numTotalOpp, int tipoRelacao, decimal valorTotalOpp, string primaryContact)
        public  Guid CreateDynamic(string accountName,string cnpj, string telephone, string fax, int numTotalOpp, int tipoRelacao, decimal valorTotalOpp, string createdBy,Guid primarycontact)
        {
            Entity conta = new Entity(this.LogicalName);
            // primaryContact = "4f31ec64-4bae-ed11-83fe-00224837d145";
            conta["name"] = accountName;
            conta["gbr_cnpj"] = cnpj;
            conta["telephone1"] = telephone;
            conta["fax"] = fax;
            conta["gbr_num_total_opp"] = numTotalOpp;
            conta["gbr_num_total_opp"] = numTotalOpp;
            conta["gbr_tipo_relacao"] = new OptionSetValue(tipoRelacao);
            conta["gbr_valor_total_opp"] = new Money(valorTotalOpp);
            conta["createdby"] = createdBy;
            conta["primarycontactid"] = new EntityReference("contact", primarycontact);

            
            Guid accountId = this.ServiceClient.Create(conta);
            return accountId;
        }

        public Entity GetAccountByCNPJ(string accountCNPJ)
        {
            QueryExpression busca = new QueryExpression(LogicalName);
            busca.ColumnSet.AddColumn("gbr_cnpj");
            busca.Criteria.AddCondition("gbr_cnpj", ConditionOperator.Equal, accountCNPJ);
            EntityCollection accounts = ServiceClient.RetrieveMultiple(busca);

            return accounts.Entities.FirstOrDefault();
        }

        public bool Update(Guid accountId, string thelephone1)
        {
            try
            {
                Entity conta = new Entity(this.LogicalName, accountId);
                conta["telephone1"] = thelephone1;
                this.ServiceClient.Update(conta);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public Entity GetAccountByName(string name)
        {
            QueryExpression queryAccount = new QueryExpression(this.LogicalName);
            queryAccount.ColumnSet.AddColumns("telephone1", "primarycontactid");
            queryAccount.Criteria.AddCondition("name", ConditionOperator.Equal, name);

            return RetriveOneAccount(queryAccount);
        }

        public Entity GetAccountByContactName(string contactName, string[] columns)
        {
            QueryExpression queryAccount = new QueryExpression(this.LogicalName);
            queryAccount.ColumnSet.AddColumns(columns);
            //saber o nome do contato ( nome da tabela , nome do parametro de qual atributo quero fazer o join, atributo da tabel)
            queryAccount.AddLink("contact", "primarycontactid", "contactid");

            // SELECT --- FROM account INNER JOIN contact ON primarycontactid = contactid WHERE contact.fullname = ''
           
            queryAccount.LinkEntities.FirstOrDefault().LinkCriteria.AddCondition("fullname", ConditionOperator.Equal, contactName);
            return RetriveOneAccount(queryAccount);

        }


        public Entity GetAccountById(Guid id , string[] columns)
        {

            return ServiceClient.Retrieve(this.LogicalName,id, new ColumnSet(columns));
        }
                public Entity GetAccountById(Guid id)
        {
            var context = new OrganizationServiceContext(this.ServiceClient);

            return (from a in context.CreateQuery("account")
                    join b in context.CreateQuery("contact")
                    on ((EntityReference)a["primarycontactid"]).Id equals b["contactid"]
                    where (Guid)a["accountid"] == id
                    select a).ToList().FirstOrDefault();
        }

        public EntityCollection GetAccountByLike(string like)
        {
            QueryExpression queryExpression = new QueryExpression(this.LogicalName);
            queryExpression.ColumnSet.AddColumns("name");
            queryExpression.Criteria.AddCondition("name", ConditionOperator.BeginsWith, like);
            return this.ServiceClient.RetrieveMultiple(queryExpression);
        }

        //lotes de creates e updates e enviar para o sistema
        public void UpsertMultipleRequest(EntityCollection entityCollection)
        {
            OrganizationRequestCollection requestColletction = new OrganizationRequestCollection();

            foreach (Entity entity in entityCollection.Entities)
            {
                AddUpsertRequest(requestColletction, entity);
            }

            ExecuteMultipleResponse executeMultipleResponse = AddExecuteMultiple(requestColletction);

            foreach (var executeResponse in executeMultipleResponse.Responses)
            {
                if (executeResponse.Fault != null)
                {
                    Console.WriteLine(executeResponse.Fault.ToString());
                }
            }
        }

        private ExecuteMultipleResponse AddExecuteMultiple(OrganizationRequestCollection requestColletction)
        {
            ExecuteMultipleRequest executeMultipleRequest = new ExecuteMultipleRequest()
            {
                Requests = requestColletction,
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true,
                }
            };

            ExecuteMultipleResponse executeMultipleResponse = (ExecuteMultipleResponse)this.ServiceClient.Execute(executeMultipleRequest);
            return executeMultipleResponse;
        }

        private static void AddUpsertRequest(OrganizationRequestCollection requestColletction, Entity entity)
        {
            UpsertRequest upsertRequest = new UpsertRequest()
            {
                Target = entity
            };

            requestColletction.Add(upsertRequest);
        }

        private Entity RetriveOneAccount(QueryExpression queryAccount)
        {

            // SELECT telephone1, primarycontactid FROM account WHERE name = ''
            EntityCollection accounts = this.ServiceClient.RetrieveMultiple(queryAccount);
            //cada entities é um array
            if (accounts.Entities.Count() > 0)
                return accounts.Entities.FirstOrDefault();
            else
                Console.WriteLine("Nenhuma conta encontrada com esse nome");

            return null;
        }



      public void IncrementOrDecrementNumberOfOpp( Entity oppAccount, bool? decrementOrIncrement)
        {
            int numberOfOpp = oppAccount.Contains("gbr_num_total_opp") ? (int)oppAccount["gbr_num_total_opp"] : 0;
            if(Convert.ToBoolean(decrementOrIncrement))
            {
                numberOfOpp += 1;
            } else
            {
                numberOfOpp -= 1;
            }


            oppAccount["gbr_num_total_opp"] = numberOfOpp;
            ServiceClient.Update(oppAccount);
        }


    }
}
