using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using MyWork.GabrielCoimbra.Controller;
using MyWork.GabrielCoimbra.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyWork.GabrielCoimbra
{
     class Program
    {
        static void Main(string[] args)
        {

            CrmServiceClient serviceClient = Singleton.GetService();

            ContaController contaController = new ContaController(serviceClient);
            ContatoController contatoController = new ContatoController(serviceClient);


            // Console.WriteLine("O que você deseja fazer?");
            //     RetrieveMethods(contaController);
            // Console.ReadKey();


             CreateAccountDynamic(contaController);

         


            //CreateContact(contatoController);

        }

        private static void CreateContact(ContatoController contatoController)
        {
            try
            {
                Console.Write("Digite o primeiro nome que deseja para seu Contato: ");
                string firstName = Console.ReadLine();
                Console.Write("Digite o sobrenome que deseja para seu Contato: ");
                string lastName = Console.ReadLine();
                Console.Write("Digite o número do CPF do Contato: ");
                string cpf = Console.ReadLine();
                Console.Write("Digite o Cargo Contato: ");
                string cargo = Console.ReadLine();

                Guid contactId = contatoController.CreateContactDynamic(firstName, lastName, cpf, cargo);

                Console.WriteLine($"https://gabrielcoimbra2023.crm2.dynamics.com/main.aspx?appid=4d306bb3-f4a9-ed11-9885-000d3a888f48&pagetype=entityrecord&etn=contact&id={contactId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Console.ReadKey();
        }

        private static void CreateAccountDynamic(ContaController contaController)
        {
            try
            {
                Console.Write("Digite o nome que deseja para Sua Conta: ");
                string accountName = Console.ReadLine();
                Console.Write("Digite o CNPJ que deseja para Sua Conta: ");
                string cnpj = Console.ReadLine();
                Console.Write("Digite o telefone que deseja para Sua Conta: ");
                string telephone = Console.ReadLine();
                Console.Write("Digite o fax que deseja para Sua Conta: ");
                string fax = Console.ReadLine();
                Console.Write("Digite o número total de oportunidades: ");
                int numTotalOpp = int.Parse(Console.ReadLine());
                Console.Write("Escolha o tipo de Relação (1 - Cliente 2 - Fornecedor 3 - Revenda): ");
                int tipoRelacao = int.Parse(Console.ReadLine());
                Console.Write("Digite o valor total de oportunidades: ");
                decimal valorTotalOpp = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
               //Console.Write("Escolha a Moeda(Real - US Dollar - Euro):  ");
                Console.Write("Criado por:  ");
                // string primaryContact = Console.ReadLine();
                 string createdBy = Console.ReadLine();

               Entity contaExiste = contaController.GetAccountByCNPJ(cnpj);
                if(contaExiste != null)
                {
                    Console.WriteLine("Conta já existente no sistema");
                   
                }
                else
                {
                    Guid accountId = contaController.CreateDynamic(accountName, cnpj, telephone, fax, numTotalOpp, tipoRelacao, valorTotalOpp, createdBy);

                    Console.WriteLine($"https://gabrielcoimbra2023.crm2.dynamics.com/main.aspx?appid=4d306bb3-f4a9-ed11-9885-000d3a888f48&pagetype=entityrecord&etn=account&id={accountId}");

                    Console.Write("Você deseja criar um contato para essa conta? ( S / N)");
                    var resposta = Console.ReadLine();
                   
                    if(resposta == "S")
                    {
                        Console.WriteLine("ta no sim");
                    //    CreateContact(contatoController);
                    } else
                    {
                        if (resposta == "N")
                        {
                            Environment.Exit(0);
                        }
                    }


                }


                        


                //  Guid accountId = contaController.CreateDynamic(accountName, telephone, fax, numTotalOpp, tipoRelacao, valorTotalOpp, primaryContact);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Console.ReadKey();
        }

        private static void RetrieveMethods(ContaController contaController)
        {
            Console.WriteLine("1 - Buscar dados de uma conta pelo id");
            Console.WriteLine("2 - Buscar uma conta por nome");
            Console.WriteLine("3 - Buscar uma conta por nome de contato");
            Console.WriteLine("4 - Buscar diversas contas");

            var answer = Console.ReadLine();

            if (answer == "1")
            {
                Console.WriteLine("Qual o id da conta que você deseja pesquisar?");
                var accountId = Console.ReadLine();
                Entity account = contaController.GetAccountById(new Guid(accountId));
                Console.WriteLine($"A conta recuperada se chama {account["name"].ToString()} ");

            }
            else
            {
                if (answer == "2")
                {
                    Console.WriteLine("Qual o nome da conta que deseja pesquisar?");
                    var name = Console.ReadLine();
                    Entity account = contaController.GetAccountByName(name);
                    Console.WriteLine($"O telefone conta recuperada é {account["telephone1"].ToString()} ");
                }
                else
                {
                    if (answer == "3")
                    {
                        Console.WriteLine("Qual o nome do contato relacionado a conta que você deseja pesquisar?");
                        var name = Console.ReadLine();
                        Entity account = contaController.GetAccountByContactName(name, new string[] { "name" });
                        ShowAccountName(account);
                    }
                    else

                    {
                        if (answer == "4")
                        {
                            Console.WriteLine("A conta que você pesquisa, começa com?");
                            var telephone = Console.ReadLine();
                            EntityCollection accounts = contaController.GetAccountByLike(telephone);

                            foreach (Entity account in accounts.Entities)
                            {
                                Console.WriteLine(account["name"].ToString());
                            }
                        }
                    }
                }


            }
        }

        private static void CreateUpdate(ContaController contaController)
        {
            Console.WriteLine("Digite 1 para Create/Update");

            var answerWhatToDo = Console.ReadLine();


            if (answerWhatToDo.ToString() == "1")
            {
                MakeCreateUpdate(contaController);
            }


        }


 

        private static void MakeCreateUpdate(ContaController contaController)
        {
            Console.WriteLine("Aguarde enquanto a nova Conta é criada");
            Guid accountId = contaController.Create();
            Console.WriteLine("Conta Criada com Sucesso!");

            Console.WriteLine($"https://gabrielcoimbra2023.crm2.dynamics.com/main.aspx?appid=4d306bb3-f4a9-ed11-9885-000d3a888f48&pagetype=entityrecord&etn=account&id={accountId}");

            Console.WriteLine("Deseja fazer a atualização da conta recem criada?");

            var answerToUpdate = Console.ReadLine();

            if (answerToUpdate.ToString().ToUpper() == "S")
            {
                Console.WriteLine("Por favor informe o novo telefone");
                var newTelephone = Console.ReadLine();

                contaController.Update(accountId, newTelephone);
                bool contaAtualizada = contaController.Update(accountId, newTelephone);


                if (contaAtualizada)
                    Console.WriteLine("Telefone atualizado com sucesso");
                else
                    Console.WriteLine("Erro na atualização do Telefone");

            }
        }

        private static void ShowAccountName(Entity account)
        {
            Console.WriteLine($"A conta recuperada se chama {account["name"].ToString()}");
        }

    }
}
