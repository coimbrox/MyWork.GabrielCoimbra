using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWork.GabrielCoimbra
{
    public class Singleton
    {
        public static CrmServiceClient GetService()
        {
            string url = "gabrielcoimbra2023";
            string clientId = "6aee358e-af97-4318-b738-a7154d2ae699";
            string clientSecret = "elq8Q~iSopTsqYaIx89XcPuQw6K2xZyMEyz4dc~v";

            CrmServiceClient serviceClient = new CrmServiceClient($"AuthType=ClientSecret;Url=https://{url}.crm2.dynamics.com/;AppId={clientId};ClientSecret={clientSecret};");
            if (!serviceClient.CurrentAccessToken.Equals(null))
                Console.WriteLine("Conexão Realizada com Sucesso");
            else
                Console.WriteLine("Erro na Conexão");

            return serviceClient;
        }
    }
}
