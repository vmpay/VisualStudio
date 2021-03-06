﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using System.Web;
using Gladiator.Models;
using System.Configuration;
using System.Net.Mail;
using Microsoft.WindowsAzure.Storage.Queue;

/*********************************************
    Error codes:
    0? - Ingame message
    00 - Defeat
    01 - Victory
    02 - Cannot connect to the server
    03 - Looking for opponents
    1? - User level message
    10 - Entity is added
    11 - Entity already exists
    12 - You've signed in. Your lvl is LVL
    13 - Password updated.
    14 - Entity can't be retreived
    15 - Incorrect password
    16 - Recovery password mail has been sent to you
    171 - Message is added to the queue
    172 - Message peeking finished successful
    173 - There is no messages in the queue
    174 - Message is updated
    175 - Message is deleted
    2? - Service information
    20 - Your lvl is increased
    21 - Entity is deleted
    22 - Table is deleted
    3? - Technical errors. Programmers should solve them
    30 - Table is created successfully
    31 - Table already exists
    32 - Table not found
    33 - Something goes wrong  - Empty code
    34 - Authentification failed. Check Primary & Secondary keys
    35 - Send mail error
    36 - Queue is created successfully
*********************************************/

namespace Gladiator.Controllers
{
    public class GladController : ApiController
    {
        [Route("api/fight")]
        [HttpGet]
        public HttpResponseMessage Fight([FromUri]string login, [FromUri]double a, [FromUri]double b, [FromUri]double c, [FromUri]int d)
        {
            Gladiator enemy = new Gladiator(d);
            Gladiator player = new Gladiator(a, b, c);
            bool result;
            string resultmsg = "33";
            result = player.battle(enemy);
            azureTable user = new azureTable();
            if (result)
            {
                resultmsg = "01" + player.GetLog(true);// victory
            }
            else
                resultmsg = "00" + player.GetLog(true);// defeat
            user.UpdateLvl(login, result);
            string xml = string.Format("{0}", resultmsg);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/plain");
            return response;
        }

        [Route("api/fightpvp")]
        [HttpGet]
        public HttpResponseMessage FightPVP([FromUri]string login, [FromUri]double a, [FromUri]double b, [FromUri]double c)
        {
            AzureQueue queue = new AzureQueue();
            if (queue.PeekMsg(true))
            {
                bool result;
                string resultmsg = "33", resultopmsg = "33";
                CloudQueueMessage opponent = queue.deQueueMsg(true);
                string[] words = opponent.AsString.Split(new Char[] { ' ' });
                int enemyA = Int32.Parse(words[1]);
                int enemyB = Int32.Parse(words[2]);
                int enemyC = Int32.Parse(words[3]);
                Gladiator player = new Gladiator(a, b, c);
                Gladiator enemy = new Gladiator(enemyA, enemyB, enemyC);
                result = player.battle(enemy);
                azureTable user = new azureTable();
                if (result)
                {
                    resultmsg = "01" + login + " vs " + words[0] + "NL" + player.GetLog(true);// victory
                    resultopmsg = "00" + player.GetLog(false);
                }
                else
                {
                    resultmsg = "00" + login + " vs " + words[0] + "NL" + player.GetLog(true);// defeat
                    resultopmsg = "01" + player.GetLog(false);
                }
                user.UpdateLvl(login, result);
                user.UpdateLvl(words[0], !result);
                queue.AddMsgQ(String.Format("{0}&{1}&{2}", words[0], login, resultopmsg), false);
                string xml = string.Format("{0}", resultmsg);
                HttpResponseMessage response = Request.CreateResponse();
                response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/plain");
                return response;
            }
            else
            {
                queue.AddMsgQ(String.Format("{0} {1} {2} {3}", login, a, b, c), true);
                string xml = string.Format("03");//Looking for opponents
                HttpResponseMessage response = Request.CreateResponse();
                response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/plain");
                return response;
            }
        }

        [Route("api/fightpvp")]
        [HttpGet]
        public HttpResponseMessage FightPVP([FromUri]string login)
        {
            AzureQueue queue = new AzureQueue();
            string resultmsg = "33";
            resultmsg = queue.ViewMsg(login, false);
            if (resultmsg.Substring(0, 3) == "173" || resultmsg.Substring(0, 3) == "172" || resultmsg.Substring(0, 2) == "34")
                resultmsg = "03";
            else
            {
                string[] words = resultmsg.Split(new Char[] { '&' });
                resultmsg = String.Format("{0}{1} vs {2}NL{3}", words[2].Substring(0, 2), words[0], words[1], words[2].Substring(2));
            }
            string xml = string.Format("{0}", resultmsg);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/plain");
            return response;
        }

        [Route("api/login")]
        [HttpGet]
        public HttpResponseMessage Login([FromUri]string login, [FromUri]string password)
        {
            string resultmsg = "33";
            azureTable user = new azureTable();
            resultmsg = user.AuthenticateUser(login, password);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(resultmsg, System.Text.Encoding.UTF8, "text/plain");
            return response;
        }

        [Route("api/recallpsw")]
        [HttpGet]
        public HttpResponseMessage Recallpsw([FromUri]string login)
        {
            string resultmsg = "33";
            //TODO: Connecto DB and complete password recall prcedure
            // Проверить наличие такого логина
            // Выслать мейл
            // Записать в базу новый пароль
            // Выслать ответ в приложение
            azureTable user = new azureTable();
            resultmsg = user.RecallPassword(login);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(resultmsg, System.Text.Encoding.UTF8, "text/plain");
            return response;
        }

        [Route("api/signup")]
        [HttpGet]
        public HttpResponseMessage Signup([FromUri]string login, [FromUri]string password)
        {
            string resultmsg = "33";
            //TODO: Connecto DB and complete sign up procedure
            // Проверить наличие такого логина
            // Выслать мейл
            // Записать в базу новый пароль
            // Выслать ответ в приложение
            azureTable user = new azureTable();
            resultmsg = user.InsertEntity(login, password);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(resultmsg, System.Text.Encoding.UTF8, "text/plain");
            return response;
        }

    }

    public class Gladiator
    {
        private double hp;//hit points
        private double ap;//attack points
        private double crit;//critical strike chance bonus
        private double block = 0.3;//block chance
        private double miss = 0.05;//chance to miss
        private double ev = 0.05;//chance evade
        private double xcrit = 3;//Xcrit times critical strike damage is increased
        private double xblock = 0.6;//Amount of blocked damage reduced to Xblock times of normal damage
        private double critcoef = 0.98;//Each N next critical strike point adds (critcoef)^N critical strike chance bonus
        private double maxcritbonus = 0.7;// Max critical strike chance bonus (MCSCB)
        private double basecrit = 0.1;// Base critical strike chance  (BCSC) + MCSCB = Max allowed critical strike chance
        private double basehp = 20;// Base hit points
        private double baseap = 5;// Base attack points
        private string log = "Empty", log2 = "Empty";// Fight log

        public Gladiator()
        {
            hp = basehp;
            ap = baseap;
            crit = basecrit;
        }

        public Gladiator(int lvl)
        {
            switch (lvl)
            {
                case 0:
                    hp = basehp;
                    ap = baseap;
                    crit = basecrit;
                    break;
                case 1:
                    hp = 1.5 * basehp;
                    ap = baseap;
                    crit = basecrit;
                    break;
                case 2:
                    hp = 1.5 * basehp;
                    ap = 1.2 * baseap;
                    crit = 2 * basecrit;
                    break;
                case 3:
                    hp = 3 * basehp;
                    ap = 1.8 * baseap;
                    crit = 2 * basecrit;
                    break;
                default://Like lvl 0
                    hp = basehp;
                    ap = baseap;
                    crit = basecrit;
                    break;
            }
        }

        public Gladiator(double a, double b, double c)
        {
            double critbonus = 0;
            for (int i = 0; i < c; i++)
            {
                critbonus = critbonus + Math.Pow(critcoef, i) / 100;//Formula for critical strike chance bonus
                if (critbonus > maxcritbonus) break;//max allowed crit chance is 0.8
            }
            hp = a + basehp;
            ap = b + baseap;
            crit = basecrit + critbonus;
           // Console.WriteLine("My crit chance={0}", crit);
        }

        public bool battle(Gladiator enemy)
        {
            Random rnd = new Random();
            double tmpap = ap, tmphp = hp;
            double tmpape = enemy.ap, tmphpe = enemy.hp;
            log = string.Format("1. My hp={0:0.00} enemy hp={1:0.00}", tmphp, tmphpe) + "NL";
            log2 = string.Format("1. My hp={0:0.00} enemy hp={1:0.00}", tmphpe, tmphp) + "NL";
            //Console.WriteLine("Before the fight: My hp={0:0.00}, enemy hp={1:0.00}", tmphp, tmphpe);
            if ((rnd.Next(0, 100) % 2) == 0)//Player.this strikes first
            {
                int i = 0;
                log += "Player strikes first" + "NL";
                log2 += "Enemy strikes first" + "NL";
                //Console.WriteLine("Player strikes first");
                while ((tmphp > 0) && (tmphpe > 0))
                {

                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= crit * 100)
                        {
                            tmpap = tmpap * xcrit;
                            log += "Player critically hits!" + "NL";
                            log2 += "Enemy critically hits!" + "NL";
                            //Console.WriteLine("Player critically hits!");
                        }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpap = tmpap * xblock;
                                log += "Enemy blocks the attack!" + "NL";
                                log2 += "Player blocks the attack!" + "NL";
                                //Console.WriteLine("Enemy blocks the attack!");
                            }
                            tmphpe = tmphpe - tmpap;
                            log += string.Format("Player hits for {0:0.00}!", tmpap) + "NL";
                            log2 += string.Format("Enemy hits for {0:0.00}!", tmpap) + "NL";
                            //Console.WriteLine("Player hits for {0:0.00}!", tmpap);
                        }
                        else
                        {
                            log += "Enemy evaded the attack!" + "NL";
                            log2 += "Player evaded the attack!" + "NL";
                        }
                        //Console.WriteLine("Enemy evaded the attack!");
                    }
                    else
                    {
                        log += "Player misses the target!" + "NL";
                        log2 += "Enemy misses the target!" + "NL";
                    }
                    //Console.WriteLine("Player misses the target!");
                    if (tmphpe <= 0)
                    {
                        log += string.Format("After the fight: My hp={0:0.00} enemy hp={1:0.00}", tmphp, tmphpe) + "NL";
                        log2 += string.Format("After the fight: My hp={0:0.00} enemy hp={1:0.00}", tmphpe, tmphp) + "NL";
                        //Console.WriteLine("After the fight: My hp={0:0.00}, enemy hp={1:0.00}", tmphp, tmphpe);
                        return true;
                    }
                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= enemy.crit * 100)
                        {
                            tmpape = tmpape * xcrit;
                            log += "Enemy critically hits!" + "NL";
                            log2 += "Player critically hits!" + "NL";
                            //Console.WriteLine("Enemy critically hits!");
                        }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpape = tmpape * xblock;
                                log += "Player blocks the attack!" + "NL";
                                log2 += "Enemy blocks the attack!" + "NL";
                                //Console.WriteLine("Player blocks the attack!");
                            }
                            tmphp = tmphp - tmpape;
                            log += string.Format("Enemy hits for {0:0.00}!", tmpape) + "NL";
                            log2 += string.Format("Player hits for {0:0.00}!", tmpape) + "NL";
                            //Console.WriteLine("Enemy hits for {0:0.00}!", tmpape);
                        }
                        else
                        {
                            log += "Player evaded the attack!" + "NL";
                            log2 += "Enemy evaded the attack!" + "NL";
                        }
                        //Console.WriteLine("Player evaded the attack!");
                    }
                    else
                    {
                        log += "Enemy misses the target!" + "NL";
                        log2 += "Player misses the target!" + "NL";
                    }
                    //Console.WriteLine("Enemy misses the target!");
                    if (tmphp <= 0)
                    {
                        log += string.Format("After the fight: My hp={0:0.00} enemy hp={1:0.00}", tmphp, tmphpe) + "NL";
                        log2 += string.Format("After the fight: My hp={0:0.00} enemy hp={1:0.00}", tmphpe, tmphp) + "NL";
                        //Console.WriteLine("After the fight: My hp={0:0.00}, enemy hp={1:0.00}", tmphp, tmphpe);
                        return false;
                    }
                    tmpap = ap;
                    tmpape = enemy.ap;
                    i++;
                    log += string.Format("{2}. My hp={0:0.00} enemy hp={1:0.00}", tmphp, tmphpe, i + 1) + "NL";
                    log2 += string.Format("{2}. My hp={0:0.00} enemy hp={1:0.00}", tmphpe, tmphp, i + 1) + "NL";
                    //Console.WriteLine("My hp={0:0.00}, enemy hp={1:0.00} Skirmish={2}", tmphp, tmphpe, i);
                }

            }
            else//Enemy strikes first
            {
                log += "Enemy strikes first" + "NL";
                log2 += "Player strikes first" + "NL";
                //Console.WriteLine("Enemy strikes first");
                int i = 0;
                while ((tmphp > 0) && (tmphpe > 0))
                {
                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= enemy.crit * 100)
                        {
                            tmpape = tmpape * xcrit;
                            log += "Enemy critically hits!" + "NL";
                            log2 += "Player critically hits!" + "NL";
                            //Console.WriteLine("Enemy critically hits!");
                        }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpape = tmpape * xblock;
                                log += "Player blocks the attack!" + "NL";
                                log2 += "Enemy blocks the attack!" + "NL";
                                //Console.WriteLine("Player blocks the attack!");
                            }
                            tmphp = tmphp - tmpape;
                            log += string.Format("Enemy hits for {0:0.00}!", tmpape) + "NL";
                            log2 += string.Format("Player hits for {0:0.00}!", tmpape) + "NL";
                            //Console.WriteLine("Enemy hits for {0:0.00}!", tmpape);
                        }
                        else
                        {
                            log += "Player evaded the attack!" + "NL";
                            log2 += "Enemy evaded the attack!" + "NL";
                        }
                        //Console.WriteLine("Player evaded the attack!");
                    }
                    else
                    {
                        log += "Enemy misses the target!" + "NL";
                        log2 += "Player misses the target!" + "NL";
                    }
                    //Console.WriteLine("Enemy misses the target!");
                    if (tmphp <= 0)
                    {
                        log += string.Format("After the fight: My hp={0:0.00} enemy hp={1:0.00}", tmphp, tmphpe) + "NL";
                        log2 += string.Format("After the fight: My hp={0:0.00} enemy hp={1:0.00}", tmphpe, tmphp) + "NL";
                        //Console.WriteLine("After the fight: My hp={0:0.00}, enemy hp={1:0.00}", tmphp, tmphpe);
                        return false;
                    }
                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= crit * 100)
                        {
                            tmpap = tmpap * xcrit;
                            log += "Player critically hits!" + "NL";
                            log2 += "Enemy critically hits!" + "NL";
                            //Console.WriteLine("Player critically hits!");
                        }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpap = tmpap * xblock;
                                log += "Enemy blocks the attack!" + "NL";
                                log2 += "Player blocks the attack!" + "NL";
                                //Console.WriteLine("Enemy blocks the attack!");
                            }
                            tmphpe = tmphpe - tmpap;
                            log += string.Format("Player hits for {0:0.00}!", tmpap) + "NL";
                            log2 += string.Format("Enemy hits for {0:0.00}!", tmpap) + "NL";
                            //Console.WriteLine("Player hits for {0:0.00}!", tmpap);
                        }
                        else
                        {
                            log += "Enemy evaded the attack!" + "NL";
                            log2 += "Player evaded the attack!" + "NL";
                        }
                        //Console.WriteLine("Enemy evaded the attack!");
                    }
                    else
                    {
                        log += "Player misses the target!" + "NL";
                        log2 += "Enemy misses the target!" + "NL";
                    }
                    //Console.WriteLine("Player misses the target!");
                    if (tmphpe <= 0)
                    {
                        log += string.Format("After the fight: My hp={0:0.00} enemy hp={1:0.00}", tmphp, tmphpe) + "NL";
                        log2 += string.Format("After the fight: My hp={0:0.00} enemy hp={1:0.00}", tmphpe, tmphp) + "NL";
                        //Console.WriteLine("After the fight: My hp={0:0.00}, enemy hp={1:0.00}", tmphp, tmphpe);
                        return true;
                    }
                    tmpap = ap;
                    tmpape = enemy.ap;
                    i++;
                    log += string.Format("{2}. My hp={0:0.00} enemy hp={1:0.00}", tmphp, tmphpe, i+1) + "NL";
                    log2 += string.Format("{2}. My hp={0:0.00} enemy hp={1:0.00}", tmphpe, tmphp, i + 1) + "NL";
                    //Console.WriteLine("My hp={0:0.00}, enemy hp={1:0.00} Skirmish={2}", tmphp, tmphpe, i);
                }
            }
            if (tmphpe <= 0)
            {
                //Console.WriteLine("Abnormal return true");
                return true;
            }
            else
            {
                //Console.WriteLine("Abnormal return false");
                return false;
            }

        }

        public string GetLog(bool player)
        {
            if (player)
                return log;
            else
                return log2;
        }
    }

    public class UserEntity : TableEntity
    {
        public UserEntity(string login, string domain)
        {
            this.PartitionKey = login;
            this.RowKey = domain;
        }

        public UserEntity() { }

        public string password { get; set; }

        public int lvl { get; set; }

        public int played { get; set; }

        public DateTime regdate { get; set; }

        public DateTime lastdate { get; set; }
    }

    public class azureTable
    {
        private string tableName;
        private string login;
        private string domain;
        private string password;
        private int lvl;

        public azureTable()
        {
            tableName = "users";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            try
            {
                table.CreateIfNotExists();
            }
            catch
            {
                tableName = "users";
            }
        }

        public string CreateTable(string name)
        {
            string result = "33";
            try
            {
                tableName = name;
                // Retrieve the storage account from the connection string
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the table if it doesn't exist.
                CloudTable table = tableClient.GetTableReference(tableName);
                if (table.Exists())
                {
                    result = "31";
                    return result;
                }
                table.CreateIfNotExists();
                result = "30";
            }
            catch
            {
                result = "34";
            }
            return result;
        }

        public string InsertEntity(string email, string psw)
        {
            string result = "33";
            try
            {
                password = psw;
                string[] tmp = email.Split('@');
                login = tmp[0];
                domain = tmp[1];
                // Retrieve the storage account from the connection string
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);
                if (!table.Exists())
                {
                    result = "32";
                    return result;
                }

                // Create a new customer entity.
                UserEntity customer = new UserEntity(login, domain);
                customer.password = psw;
                customer.lvl = 0;
                customer.regdate = DateTime.Now;
                customer.lastdate = DateTime.Now;
                customer.played = 0;

                // Create the TableOperation object that inserts the customer entity.
                TableOperation insertOperation = TableOperation.Insert(customer);
                result = this.RetrieveEntity(email);
                if (result.Equals("14"))
                {
                    try
                    {
                        // Execute the insert operation.
                        table.Execute(insertOperation);
                        string subject = "GladiatorAPP - Registration";
                        string body = string.Format("Hello! You've signed up for GladiatorApp.\n--\nBest regards,\nGladiatorApp support");
                        result = string.Format("{0}", SendMail(email, subject, body));
                        result = "10";
                    }
                    catch
                    {
                        result = "11";
                        //result = "34";
                    }
                }
                else
                {
                    if (result.StartsWith("CODR"))
                    {
                        result = "11";
                    }
                }
            }
            catch
            {
                result = "34";
            }
            return result;
        }

        public string RetrieveEntity(string email)
        {
            string result = "33";
            try
            {
                string[] tmp = email.Split('@');
                login = tmp[0];
                domain = tmp[1];
                // Retrieve the storage account from the connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);
                if (!table.Exists())
                {
                    result = "32";
                    return result;
                }

                // Create the table query.
                TableQuery<UserEntity> rangeQuery = new TableQuery<UserEntity>().Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, login),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, domain)));

                int i = 0;
                result = "";

                // Loop through the results, displaying information about the entity.
                foreach (UserEntity entity in table.ExecuteQuery(rangeQuery))
                {
                    try
                    {
                        // Console.WriteLine("{0}@{1}\t{2}\t{3}\n", entity.PartitionKey, entity.RowKey,
                        //entity.password, entity.lvl);
                        result = string.Format("CODR{0}@{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n", entity.PartitionKey, entity.RowKey,
                            entity.password, entity.lvl, entity.played, entity.regdate, entity.lastdate);
                        i++;
                    }
                    catch
                    {
                        result = "32";
                        //Console.WriteLine("Table not found.");
                    }
                }
                if (i == 0)
                {
                    result = "14";
                }
            }
            catch
            {
                result = "34";
                //Console.WriteLine("Authentification failed.");
            }
            return result;
        }

        public string AuthenticateUser(string email, string psw)
        {
            string result = "33";
            try
            {
                string[] tmp = email.Split('@');
                login = tmp[0];
                domain = tmp[1];
                // Retrieve the storage account from the connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);
                if (!table.Exists())
                {
                    result = "32";
                    return result;
                }

                // Create the table query.
                TableQuery<UserEntity> rangeQuery = new TableQuery<UserEntity>().Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, login),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, domain)));


                int i = 0;
                // Loop through the results, displaying information about the entity.
                foreach (UserEntity entity in table.ExecuteQuery(rangeQuery))
                {
                    try
                    {
                        if (entity.password == psw)
                        {
                            result = string.Format("12{0}", entity.lvl); // login succesfull
                            entity.lastdate = DateTime.Now;
                            // Create the InsertOrReplace TableOperation.
                            TableOperation updateOperation = TableOperation.Replace(entity);
                            // Execute the operation.
                            table.Execute(updateOperation);
                        }
                        else
                            result = "15";// login failed
                        i++;
                    }
                    catch
                    {
                        result = "32";
                        //Console.WriteLine("Table not found.");
                    }
                }
                if (i == 0)
                {
                    result = "14Login failed. No such entity exists.";
                }

            }
            catch
            {
                result = "34";
                //Console.WriteLine("Authentification failed.");
            }
            return result;
        }

        public string RecallPassword(string email) 
        {
            string result = "33";
            try
            {
                string[] tmp = email.Split('@');
                login = tmp[0];
                domain = tmp[1];
                // Retrieve the storage account from the connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                 ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);
                if (!table.Exists())
                {
                    result = "32";
                    return result;
                }

                // Define the query, and select only the Email property.
                TableQuery<DynamicTableEntity> projectionQuery = new TableQuery<DynamicTableEntity>().Select(
                new string[] { "password" }).Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, login),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, domain)));

                // Define an entity resolver to work with the entity after retrieval.
                EntityResolver<string> resolver = (pk, rk, ts, props, etag) => props.ContainsKey("password") ? props["password"].StringValue : null;

                int i = 0;
                foreach (string projectedPassword in table.ExecuteQuery(projectionQuery, resolver, null, null))
                {
                    try
                    {
                        string subject = "GladiatorAPP - Password recovery";
                        string body = string.Format("Hello! Your password has been recalled. Your password is {0}\n--\nBest regards,\nGladiatorApp support", projectedPassword);
                        result = string.Format("{0}", SendMail(email, subject, body)); // Change when snedmail works well
                        //Console.WriteLine(projectedPassword);
                        i++;
                    }
                    catch
                    {
                        result = "32";
                        //Console.WriteLine("Table not found.");
                    }
                }
                if (i == 0)
                    result = "14";

            }
            catch
            {
                result = "34";
                //Console.WriteLine("Authentification failed.");
            }


            return result;
        }

        public string UpdateLvl(string email, bool fightresult)
        {
            string result = "33";
            try
            {
                string[] tmp = email.Split('@');
                login = tmp[0];
                domain = tmp[1];
                // Retrieve the storage account from the connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);
                if (!table.Exists())
                {
                    result = "32";
                    return result;
                }

                // Create a retrieve operation that takes a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>(login, domain);

                // Execute the operation.
                TableResult retrievedResult = table.Execute(retrieveOperation);

                // Assign the result to a UserEntity object.
                UserEntity updateEntity = (UserEntity)retrievedResult.Result;

                if (updateEntity != null)
                {
                    // Change the phone number.
                    updateEntity.played++;
                    if (fightresult)
                        updateEntity.lvl++;

                    // Create the InsertOrReplace TableOperation.
                    TableOperation updateOperation = TableOperation.Replace(updateEntity);

                    // Execute the operation.
                    table.Execute(updateOperation);

                    //Console.WriteLine("Entity updated.");
                    result = "CODELevel updated.";
                }
                else
                    result = "14";
                //Console.WriteLine("14");
            }
            catch
            {
                result = "34";
                //Console.WriteLine("34");
            }
            return result;
        }

        public string UpdatePsw(string email, string psw)
        {
            string result = "33";
            try
            {
                string[] tmp = email.Split('@');
                login = tmp[0];
                domain = tmp[1];
                // Retrieve the storage account from the connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);
                if (!table.Exists())
                {
                    result = "32";
                    return result;
                }

                // Create a retrieve operation that takes a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>(login, domain);

                // Execute the operation.
                TableResult retrievedResult = table.Execute(retrieveOperation);

                // Assign the result to a UserEntity object.
                UserEntity updateEntity = (UserEntity)retrievedResult.Result;

                if (updateEntity != null)
                {
                    // Change the phone number.
                    updateEntity.password = psw;

                    // Create the InsertOrReplace TableOperation.
                    TableOperation updateOperation = TableOperation.Replace(updateEntity);

                    // Execute the operation.
                    table.Execute(updateOperation);

                    //Console.WriteLine("Entity updated.");
                    result = "13";
                }
                else
                    result = "14";
                //Console.WriteLine("14");
            }
            catch
            {
                result = "34";
                //Console.WriteLine("34");
            }
            return result;
        }

        public string DeleteUser(string email)
        {
            string result = "33";
            try
            {
                string[] tmp = email.Split('@');
                login = tmp[0];
                domain = tmp[1];
                // Retrieve the storage account from the connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);
                if (!table.Exists())
                {
                    result = "32";
                    return result;
                }

                // Create a retrieve operation that expects a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>(login, domain);

                // Execute the operation.
                TableResult retrievedResult = table.Execute(retrieveOperation);

                // Assign the result to a UserEntity.
                UserEntity deleteEntity = (UserEntity)retrievedResult.Result;

                // Create the Delete TableOperation.
                if (deleteEntity != null)
                {
                    TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                    // Execute the operation.
                    table.Execute(deleteOperation);

                    result = "21";
                    //Console.WriteLine("Entity deleted.");
                }

                else
                    result = "14";
                //Console.WriteLine("Could not retrieve the entity.");
            }
            catch
            {
                result = "34";
                //Console.WriteLine("Authentification failed.");
            }
            return result;
        }

        public string DeleteTable(string name)
        {
            string result = "33";
            try
            {
                // Retrieve the storage account from the connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(name);
                if (!table.Exists())
                {
                    result = "32";
                    return result;
                }

                // Delete the table it if exists.
                table.DeleteIfExists();
                result = "22";
                //Console.WriteLine("Table deleted.");
            }
            catch
            {
                result = "34";
                //Console.WriteLine("Authentification failed.");
            }
            return result;
        }

        public string RetrieveAllEntities()
        {
            string result = "33";
            try
            {
                // Retrieve the storage account from the connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings["StorageConnectionString"]);

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);
                if (!table.Exists())
                {
                    result = "32";
                    return result;
                }

                // Create the table query.
                TableQuery<UserEntity> rangeQuery = new TableQuery<UserEntity>();

                int i = 0;
                result = "CODR";

                // Loop through the results, displaying information about the entity.
                foreach (UserEntity entity in table.ExecuteQuery(rangeQuery))
                {
                    try
                    {
                        // Console.WriteLine("{0}@{1}\t{2}\t{3}\n", entity.PartitionKey, entity.RowKey,
                        //entity.password, entity.lvl);
                        result = string.Format("{0}{1}@{2}\t{3}\t{4}\t{5}\t{6}\t{7}\n", result, entity.PartitionKey, entity.RowKey,
                            entity.password, entity.lvl, entity.played, entity.regdate, entity.lastdate);
                        i++;
                    }
                    catch
                    {
                        result = "32";
                        //Console.WriteLine("Table not found.");
                    }
                }
                if (i == 0)
                {
                    result = "14";
                }
            }
            catch
            {
                result = "34";
                //Console.WriteLine("Authentification failed.");
            }
            return result;
        }

        public string SendMail(string login, string subject, string body)
        {
            string result = "33";
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.rambler.ru";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("support.sendmail@rambler.ru", "password");
            MailMessage mm = new MailMessage("support.sendmail@rambler.ru", login, subject, body);
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            try
            {
                client.Send(mm);
                result = "16";
            }
            catch
            {
                result = "35";
            }

            return result;
        }
    }

    class AzureQueue
    {
        private CloudStorageAccount storageAccount;
        private CloudQueueClient queueClient;
        private CloudQueue queuei;//TRUE - input queue
        private CloudQueue queueo;//FLASE - output queue
        private string result;

        public AzureQueue()
        {
            result = "33";
            if (!VerifyConfiguration())
            {
                return;
            }
            // Retrieve storage account from connection string
            storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.ConnectionStrings["queueConnectionString"].ConnectionString);
            // Create the queue client
            queueClient = storageAccount.CreateCloudQueueClient();
            // Retrieve a reference to a queue
            queuei = queueClient.GetQueueReference("inputqueue");
            queueo = queueClient.GetQueueReference("outputqueue");
            try
            {
                queuei.CreateIfNotExists();
                queueo.CreateIfNotExists();
            }
            catch
            {
                result = "33";
            }
        }

        public string createQueue(bool io)
        {
            if (!VerifyConfiguration())
            {
                return "34";
            }
            // Create the queue if it doesn't already exist
            try
            {
                CloudQueue queue;
                if (io)
                    queue = queuei;
                else
                    queue = queueo;
                queue.CreateIfNotExists();
                //Console.WriteLine("Queue has been created.");
                return "36";
            }
            catch
            {
                //Console.WriteLine("Queue creation failed.");
                return "34";
            }
        }

        public string AddMsgQ(string inputstring, bool io)
        {
            if (!VerifyConfiguration())
            {
                return "34";
            }
            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(inputstring);
            try
            {
                CloudQueue queue;
                if (io)
                    queue = queuei;
                else
                    queue = queueo;
                queue.AddMessage(message, TimeSpan.FromSeconds(30));
                //Console.WriteLine("Message has been added to {0}.", io);
                return "171";
            }
            catch
            {
                //Console.WriteLine("Message creation failed {0}.", io);
                return "34";
            }
        }

        public bool PeekMsg(bool io)
        {
            if (!VerifyConfiguration())
            {
                return false;
            }
            try
            {
                // Peek at the next message
                CloudQueueMessage peekedMessage;
                if (io)
                    peekedMessage = queuei.PeekMessage();
                else
                    peekedMessage = queueo.PeekMessage();
                if (peekedMessage.AsString == null)
                    return false;
                // Display message.
                //Console.WriteLine(peekedMessage.AsString);
                return true;
            }
            catch
            {
                //Console.WriteLine("Message peeking failed.");
                return false;
            }
        }

        public string UpdMsg(string inputstring, bool io)
        {
            if (!VerifyConfiguration())
            {
                return "34";
            }
            try
            {
                // Get the message from the queue and update the message contents.
                CloudQueue queue;
                if (io)
                    queue = queuei;
                else
                    queue = queueo;
                CloudQueueMessage message = queue.GetMessage();
                message.SetMessageContent(inputstring);
                queue.UpdateMessage(message,
                    TimeSpan.FromSeconds(6.0),  // Make it visible for another 6 seconds.
                    MessageUpdateFields.Content | MessageUpdateFields.Visibility);
                //Console.WriteLine("Message has been changed.");
                return "174";
            }
            catch
            {
                //Console.WriteLine("Message changing failed.");
                return "173";
            }
        }

        public CloudQueueMessage deQueueMsg(bool io)
        {
            CloudQueueMessage retrievedMessage = new CloudQueueMessage("173");
            try
            {
                CloudQueue queue;
                if (io)
                    queue = queuei;
                else
                    queue = queueo;
                // Get the next message
                retrievedMessage = queue.GetMessage();
                //Process the message in less than 30 seconds, and then delete the message
                queue.DeleteMessage(retrievedMessage);
                //Console.WriteLine("Message has been de-queued.");
                return retrievedMessage;
            }
            catch
            {
                //Console.WriteLine("Message de-queueing failed.");
                return retrievedMessage;
            }
        }

        public string ReadMsg(int count, bool io)
        {
            if (!VerifyConfiguration())
            {
                return "34";
            }
            try
            {
                CloudQueue queue;
                if (io)
                    queue = queuei;
                else
                    queue = queueo;
                int i = 1;
                foreach (CloudQueueMessage message in queue.GetMessages(count, TimeSpan.FromSeconds(30)))
                {
                    //Console.WriteLine("{0}. {1}", i, message.AsString);
                    i++;
                    // Process all messages in less than 30 seconds, deleting each message after processing.
                    queue.DeleteMessage(message);
                }
                if (i == 1)
                {
                    //Console.WriteLine("No messages found in the queue.");
                    return "173";
                }
                return "175";
            }
            catch
            {
                //Console.WriteLine("Read and delete all(20) messages fails.");
                return "34";
            }
        }

        public string ViewMsg(string login, bool io)
        {
            if (!VerifyConfiguration())
            {
                return "34";
            }
            try
            {
                CloudQueue queue;
                if (io)
                    queue = queuei;
                else
                    queue = queueo;
                int i = 1;
                foreach (CloudQueueMessage message in queue.GetMessages(32, TimeSpan.FromSeconds(5)))
                {
                    //Console.WriteLine("{0}. {1} - {2}", i, message.AsString, message.ExpirationTime);
                    //TODO: something
                    string[] words = message.AsString.Split(new Char[] { '&' });
                    if (words[0] == login)
                    {
                        queue.DeleteMessage(message);
                        return message.AsString;
                    }
                    i++;
                    //queue.DeleteMessage(message);
                    //if (message.AsString != "vip msg")
                    queue.DeleteMessage(message);
                    queue.AddMessage(message, TimeSpan.FromSeconds(30));
                }
                if (i == 0)
                {
                    //Console.WriteLine("No messages found in the queue.");
                    return "173";
                }
                return "172";
            }
            catch
            {
                //Console.WriteLine("View all(20) messages fails.");
                return "34";
            }
        }

        public int getQueueLength(bool io)
        {
            if (!VerifyConfiguration())
            {
                return -1;
            }
            try
            {
                CloudQueue queue;
                if (io)
                    queue = queuei;
                else
                    queue = queueo;
                // Fetch the queue attributes.
                queue.FetchAttributes();
                // Retrieve the cached approximate message count.
                int? cachedMessageCount = queue.ApproximateMessageCount;
                int result = cachedMessageCount ?? default(int);
                // Display number of messages.
                //Console.WriteLine("Number of messages in queue: " + cachedMessageCount);
                return result;
            }
            catch
            {
                //Console.WriteLine("Counting messages fails.");
                return -1;
            }
        }

        public bool deleteQueue(bool io)
        {
            if (!VerifyConfiguration())
            {
                return false;
            }
            try
            {
                CloudQueue queue;
                if (io)
                    queue = queuei;
                else
                    queue = queueo;
                // Delete the queue.
                queue.Delete();
                //Console.WriteLine("Queue has been deleted.");
                return true;
            }
            catch
            {
                //Console.WriteLine("Queue deletion fails.");
                return false;
            }
        }

        public int countQueueLength(bool io)
        {
            if (!VerifyConfiguration())
            {
                return -1;
            }
            try
            {
                CloudQueue queue;
                if (io)
                    queue = queuei;
                else
                    queue = queueo;
                int i = 0;
                foreach (CloudQueueMessage message in queue.GetMessages(32, TimeSpan.FromSeconds(1)))
                {
                    i++;
                }
                return i;
            }
            catch
            {
                //Console.WriteLine("View all(20) messages fails.");
                return -1;
            }
        }

        private static bool VerifyConfiguration()
        {
            string queueConnectionString = ConfigurationManager.ConnectionStrings["queueConnectionString"].ConnectionString;
            bool configOK = true;
            if (string.IsNullOrWhiteSpace(queueConnectionString))
            {
                configOK = false;
                //Console.WriteLine("Please add the Azure Storage account credentials in App.config");
            }
            return configOK;
        }

    }
    
}
