﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using MyWork.GabrielCoimbra.Controller;
using MyWork.GabrielCoimbra.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWork.GabrielCoimbra
{
     class Program
    {
        static void Main(string[] args)
        {

            CrmServiceClient serviceClient = Singleton.GetService();

            ContaController contaController = new ContaController(serviceClient);

            Console.WriteLine("O que você deseja fazer?");
            Console.WriteLine("1 - Buscar dados de uma conta pelo id");
            Console.WriteLine("2 - Buscar uma conta por nome");
            Console.WriteLine("3 - Buscar uma conta por nome de contato");

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
                } else
                {
                    if (answer == "3")
                    {
                        Console.WriteLine("Qual o nome do contato relacionado a conta que você deseja pesquisar?");
                        var name = Console.ReadLine();
                        Entity account = contaController.GetAccountByContactName(name, new string[] { "name" });
                        ShowAccountName(account);
                    }
                }

               
            }
            Console.ReadKey();



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
