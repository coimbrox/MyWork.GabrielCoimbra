using Microsoft.Rest;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using MyWork.GabrielCoimbra.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWork.GabrielCoimbra.Controller
{
    public class ContaController
    {
        public IOrganizationService ServiceClient { get; set; }
        public Conta Conta { get; set; }
        public ContaController(IOrganizationService crmServiceClient)
        {
            ServiceClient = crmServiceClient;
            this.Conta = new Conta(ServiceClient);
        }
        public ContaController(CrmServiceClient crmServiceClient)
        {
            ServiceClient = crmServiceClient;
            this.Conta = new Conta(ServiceClient);
        }

        public Guid Create()
        {
            return Conta.Create();
        }

        public Guid CreateDynamic(string accountName, string telephone, string cnpj, string fax, int numTotalOpp, int tipoRelacao, decimal valorTotalOpp, string createdBy, Guid primarycontact)
      //  public Guid CreateDynamic(string accountName, string cnpj, string telephone, string fax, int numTotalOpp, int tipoRelacao, decimal valorTotalOpp, string createdBy)
        {
            return Conta.CreateDynamic( accountName, cnpj, telephone,  fax,  numTotalOpp,  tipoRelacao,  valorTotalOpp, createdBy, primarycontact);
        }
              

        public bool Update(Guid accountId, string telephone1)
        {
            return Conta.Update(accountId, telephone1);
        }


        public Entity GetAccountById(Guid id, string[] colums)
        {
            return Conta.GetAccountById(id, colums);
        }


        public Entity GetAccountById(Guid id)
        {
            return Conta.GetAccountById(id);
        }

        public Entity GetAccountByCNPJ(string cnpj)
        {
            return Conta.GetAccountByCNPJ(cnpj);
        }

        public Entity GetAccountByName(string name)
        {
            return Conta.GetAccountByName(name);
        }

        public EntityCollection GetAccountByLike(string like)
        {
            return Conta.GetAccountByLike(like);
        }

        public void UpsertMultipleRequest(EntityCollection entityCollection)
        {
            Conta.UpsertMultipleRequest(entityCollection);
        }
        public Entity GetAccountByContactName(string name, string[] columns)
        {
            return Conta.GetAccountByContactName(name, columns);
        }



        public void IncrementNumberOfOpp(Entity oppAccount)
        {
             Conta.IncrementNumberOfOpp(oppAccount);
        }


    }
}
