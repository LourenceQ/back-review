﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankTransfer.BankAccount;
using BankTransfer.Enum;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;

namespace BankTransfer
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        private static async Task ProcessRepositories()
        {
            List<string> responseList = new List<string>();
            List<decimal> decimalResponseList = new List<decimal>();

            // var stringTask = client.GetStringAsync("https://api.currencyfreaks.com/latest?apikey=5470ff823cd34acc9c596841a07bb9a2"); // Todas as moedas
            // var stringTask = client.GetStringAsync("https://api.currencyfreaks.com/currency-symbols"); // Todos as abreveações das moedas

            var stringTask = client.GetStringAsync("https://api.currencyfreaks.com/latest?apikey=3e8a112f03184081b6d183cd452cac0c&symbols=PKR,GBP,EUR,USD,BRL,BCH,ARS,AUD,BOB,BTC,CNH,CAD");

            string msg = await stringTask;

            string[] myString = msg.Split(",");

            // add items from string[] to list
            for (int i = 0; i < myString.Length; i++)
            {
                responseList.Add(myString[i]);
            }

            System.Console.WriteLine();

            // removing undesired characters
            for (int i = 0; i < responseList.Count; i++)
            {
                responseList[i] = responseList[i].Replace(".", ",");
                responseList[i] = responseList[i].Remove(0, 6);
                responseList[i] = responseList[i].Remove(0, 1);
                responseList[i] = responseList[i].Remove(responseList[i].Length - 1);
            }

            responseList.RemoveRange(0, 2);
            responseList[0] = responseList[0].Remove(0, 9);
            responseList[11] = responseList[11].Remove(7, 2);


            for (int x = 0; x < responseList.Count; x++)
            {
                decimalResponseList.Add(decimal.Parse(responseList[x]));
            }

        }

        static List<Account> accountList = new List<Account>();
        static async Task Main(string[] args)
        {

            await ProcessRepositories();


            string userOptions = GetUserOp();

            while (userOptions.ToUpper() != "X")
            {
                switch (userOptions)
                {
                    case "1":
                        AccountLists();
                        break;
                    case "2":
                        AccountCreate();
                        break;
                    case "3":
                        AccountTransfer();
                        break;
                    case "4":
                        AccountWithDraw();
                        break;
                    case "5":
                        AccountDeposit();
                        break;
                    case "C":
                        Console.Clear();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                userOptions = GetUserOp();
            }
            System.Console.WriteLine("Obrigado por utilizar nossos serviços.");
        }

        private static string GetUserOp()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Bem vindo ao Banco da União");
            System.Console.WriteLine("Informe a operaçao desejada:");

            System.Console.WriteLine("1- Listar Contas");
            System.Console.WriteLine("2- Criar conta");
            System.Console.WriteLine("3- Transferência");
            System.Console.WriteLine("4- Sacar");
            System.Console.WriteLine("5- Depositar");
            System.Console.WriteLine("C- Limpar Tela");
            System.Console.WriteLine("X- Sair");
            System.Console.WriteLine();

            string userOp = Console.ReadLine().ToUpper();
            System.Console.WriteLine();

            return userOp;
        }

        private static void AccountDeposit()
        {
            System.Console.WriteLine("Informe o número da conta: ");
            int accountIndex = int.Parse(Console.ReadLine());

            Console.WriteLine("informe o valor para depósito: ");
            decimal depositAmount = decimal.Parse(Console.ReadLine());

            accountList[accountIndex].Deposit(depositAmount);
        }

        private static void AccountWithDraw()
        {
            System.Console.WriteLine("Informe o número da conta: ");
            int accountIndex = int.Parse(Console.ReadLine());

            System.Console.WriteLine("Iniforme o valor para saque: ");
            decimal depositAmount = decimal.Parse(Console.ReadLine());

            accountList[accountIndex].WithDraw(depositAmount);
        }

        private static void AccountTransfer()
        {
            System.Console.WriteLine();
            int accountIndexOrigin = int.Parse(Console.ReadLine());

            System.Console.WriteLine();
            int accountIndexDestination = int.Parse(Console.ReadLine());

            System.Console.Write("Informe o valor a ser transferido: ");
            decimal transferAmount = decimal.Parse(Console.ReadLine());

            accountList[accountIndexOrigin].Transfer(transferAmount, accountList[accountIndexDestination]);
        }

        private static void AccountLists()
        {
            System.Console.WriteLine("Listar contas");

            if (accountList.Count == 0)
            {
                System.Console.WriteLine("Não existem contas cadastrdas.");
                return;
            }

            for (int i = 0; i < accountList.Count; i++)
            {
                Account userAccount = accountList[i];
                System.Console.Write("#{0} - ", i);
                System.Console.WriteLine(userAccount);
            }
        }

        private static void AccountCreate()
        {
            System.Console.WriteLine("Cirar nova conta");

            System.Console.Write("Digite 1 para Conta Física ou 2 para Conta Jurídica: ");
            int accountTypeCreate = int.Parse(Console.ReadLine());

            System.Console.Write("Digite o nome do Cliente: ");
            string accountUserName = Console.ReadLine();

            System.Console.Write("Digite o saldo inicial: ");
            decimal accountBalance = decimal.Parse(Console.ReadLine());

            System.Console.Write("Digite o crédito: ");
            decimal accountDeposit = decimal.Parse(Console.ReadLine());


            Account newAccount = new Account(accountType: (AccountType)accountTypeCreate,
                                             name: accountUserName,
                                             balance: accountBalance,
                                             credit: accountDeposit);
            accountList.Add(newAccount);

        }


    }
}
