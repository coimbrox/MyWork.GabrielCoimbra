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

        public Entity GetAccountByContactName(string name, string[] columns)
        {
            return Conta.GetAccountByContactName(name, columns);
        }
    }
}
