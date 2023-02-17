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
        public CrmServiceClient ServiceClient { get; set; }
        public Conta Conta { get; set; }
        public ContaController(CrmServiceClient crmServiceClient)
        {
            ServiceClient = crmServiceClient;
            this.Conta = new Conta(ServiceClient);
        }

        public Guid Create()
        {
            return Conta.Create();
        }

       // public Guid CreateDynamic(string accountName, string telephone, string fax, int numTotalOpp, int tipoRelacao, decimal valorTotalOpp, string primaryContact)
       public Guid CreateDynamic(string accountName, string cnpj, string telephone, string fax, int numTotalOpp, int tipoRelacao, decimal valorTotalOpp, string createdBy)
        {
            return Conta.CreateDynamic( accountName, cnpj, telephone,  fax,  numTotalOpp,  tipoRelacao,  valorTotalOpp, createdBy);
        }


        public Guid CreateContactDynamic(string fullName, string cpf, string cargo)
        {
            return Conta.CreateContactDynamic( fullName, cpf, cargo);
        }

        public bool Update(Guid accountId, string telephone1)
        {
            return Conta.Update(accountId, telephone1);
        }


        public Entity GetAccountById(Guid id)
        {
            return Conta.GetAccountById(id);
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
    }
}
