using Microsoft.Xrm.Tooling.Connector;
using MyWork.GabrielCoimbra.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWork.GabrielCoimbra.Controller
{
    public class ContatoController
    {
        public CrmServiceClient ServiceClient { get; set; }
        public Contato Contato { get; set; }
        public ContatoController(CrmServiceClient crmServiceClient)
        {
            ServiceClient = crmServiceClient;
            this.Contato = new Contato(ServiceClient);
        }


        public Guid CreateContactDynamic(string firstName, string lastName, string cpf, string cargo)
        {
            return Contato.CreateContactDynamic(firstName, lastName, cpf, cargo);
        }

    }
}
