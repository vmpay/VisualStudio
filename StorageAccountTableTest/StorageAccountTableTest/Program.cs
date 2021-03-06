﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
using System.Net.Mail;

/*********************************************
    Error codes:
    0? - Ingame message
    00 - Defeat
    01 - Victory
    02 - Cannot connect to the server
    1? - User level message
    10 - Entity is added
    11 - Entity already exists
    12 - You've signed in. Your lvl is LVL
    13 - Password updated.
    14 - Entity can't be retreived
    15 - Incorrect password
    16 - Recovery password mail has been sent to you
    2? - Service information
    20 - Your lvl is increased
    21 - Entity is deleted
    22 - Table is deleted
    3? - Technical errors. Programmers should solve them
    30 - Table is created succesfully
    31 - Table already exists
    32 - Table not found
    33 - Something goes wrong  - Empty code
    34 - Authentification failed. Check Primary & Secondary keys
    35 - Send mail error
*********************************************/

namespace StorageAccountTableTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int op=100;
            string mail = "admin@admin.com", email;
            string psw = "123", password;
            azureTable Table1 = new azureTable();
            for (int i = 0; i < 200; i++)
            {
                if (op == 0) break;
                Console.WriteLine("Choose the option:\n1.Create table users\n2. Insert user\n3. Sign in procedure\n4. Password recall\n5. Update password\n6. Updatelvl +1\n7. RetrieveEntity\n8. Delete user\n9. Delete table\n10. Retrieve all entities\n11. Send mail");
                op = Convert.ToInt32(Console.ReadLine());
                //Console.WriteLine("op = {0}", op);
                switch (op)
                {
                    case 1:
                        {
                            Console.WriteLine("CreateTable(users)={0}",Table1.CreateTable("people"));
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Write down Email and password: ");
                            email = Console.ReadLine();
                            password = Console.ReadLine();
                            Console.WriteLine("Table1.InsertEntity={0}", Table1.InsertEntity(email,password));
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Write down Email and password: ");
                            email = Console.ReadLine();
                            password = Console.ReadLine();
                            Console.WriteLine("Table1.AuthenticateUser={0}", Table1.AuthenticateUser(email, password));
                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("Table1.RecallPassword={0}", Table1.RecallPassword(mail));
                            break;
                        }
                    case 5:
                        {
                            Console.WriteLine("Write down password: ");
                            password = Console.ReadLine();
                            Console.WriteLine("Table1.UpdatePsw={0}", Table1.UpdatePsw(mail,password));
                            break;
                        }
                    case 6:
                        {
                            Console.WriteLine("Table1.UpdateLvl={0}", Table1.UpdateLvl(mail));
                            break;
                        }
                    case 7:
                        {
                            Console.WriteLine("Write down Email: ");
                            email = Console.ReadLine();
                            Console.WriteLine("Table1.RetrieveEntity={0}", Table1.RetrieveEntity(email));
                            break;
                        }
                    case 8:
                        {
                            Console.WriteLine("Write down Email: ");
                            email = Console.ReadLine();
                            Console.WriteLine("Table1.DeleteUser={0}", Table1.DeleteUser(email));
                            break;
                        }
                    case 9:
                        {
                            Console.WriteLine("Table1.DeleteTable={0}", Table1.DeleteTable("users"));
                            break;
                        }
                    case 10:
                        {
                            Console.WriteLine("Table1.RetrieveAllEntities=\n{0}", Table1.RetrieveAllEntities());
                            break;
                        }
                    case 11:
                        {
                            email = "vereszp@gmail.com";
                            Console.WriteLine("Table1.SendMail(email)={0}\n", Table1.SendMail(email));
                            break;
                        }
                    default:
                        Console.WriteLine("Default i={0}", i);
                        break;
                }
                i++;
            }
            
            /*for (int i=0; i<20; i++)
            {
                if (op == 0) break;
                Console.WriteLine("Choose the option:\n1.Create table & Insert data\n2. Read data (Partition key)\n3. Read data (partition & row keys)\n4. Update Ben's number\n5. Insert-or-Update (Ben)\n6. Delete entity (Ben)\n7. Delete table");
                op = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("op = {0}", op);
                switch (op)
            {
                case 1: // Create table & insert 2 entities
                    {
                            try
                            {
                                // Retrieve the storage account from the connection string
                                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                    ConfigurationManager.AppSettings["StorageConnectionString"]);
                            
                                // Create the table client.
                                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();


                                // Create the table if it doesn't exist.
                                CloudTable table = tableClient.GetTableReference("people");
                                table.CreateIfNotExists();
 
                            // Create the batch operation.
                            TableBatchOperation batchOperation = new TableBatchOperation();

                            // Create a customer entity and add it to the table.
                            CustomerEntity customer1 = new CustomerEntity("Smith", "Jeff");
                            customer1.password = "Jeff@contoso.com";
                            customer1.lvl = 104;

                            // Create another customer entity and add it to the table.
                            CustomerEntity customer2 = new CustomerEntity("Smith", "Ben");
                            customer2.password = "Ben@contoso.com";
                            customer2.lvl = 102;

                            // Add check if some entity already exists

                            // Add both customer entities to the batch insert operation.
                            batchOperation.Insert(customer1);
                            batchOperation.Insert(customer2);
                            try {
                                // Execute the batch operation.
                                //table.ExecuteBatch(batchOperation);
                                Console.WriteLine("Creating succesfull.");
                            } catch
                            {
                                Console.WriteLine("Creating failed. Entity already exists");
                            }
                            }
                            catch
                            {
                                Console.WriteLine("Authentification failed.");
                            }
                            break;
                    }
                case 2: // Retrieve Smith
                    {
                            try
                            { 
                            // Retrieve the storage account from the connection string.
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                ConfigurationManager.AppSettings["StorageConnectionString"]);

                            // Create the table client.
                            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                            // Create the CloudTable object that represents the "people" table.
                            CloudTable table = tableClient.GetTableReference("people");

                            // Construct the query operation for all customer entities where PartitionKey="Smith".
                            TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

                            // Print the fields for each customer.
                            try
                            {
                                foreach (CustomerEntity entity in table.ExecuteQuery(query))
                                {
                                    Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                                        entity.password, entity.lvl);
                                }
                            } catch 
                            {
                                Console.WriteLine("Table not found");
                            }
                            }
                            catch
                            {
                                Console.WriteLine("Authentification failed.");
                            }
                            break;
                    }
                    case 3: // Retrieve Ben Smith
                        {
                            try
                            { 
                            // Retrieve the storage account from the connection string.
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                ConfigurationManager.AppSettings["StorageConnectionString"]);

                            // Create the table client.
                            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                            // Create the CloudTable object that represents the "people" table.
                            CloudTable table = tableClient.GetTableReference("people");

                            // Create the table query.
                            TableQuery<CustomerEntity> rangeQuery = new TableQuery<CustomerEntity>().Where(
                                TableQuery.CombineFilters(
                                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"),
                                    TableOperators.And,
                                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, "E")));

                            try
                            {
                                // Loop through the results, displaying information about the entity.
                                foreach (CustomerEntity entity in table.ExecuteQuery(rangeQuery))
                                {
                                    Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                                        entity.password, entity.lvl);
                                }
                            } catch
                            {
                                Console.WriteLine("Table not found");
                            }
                            }
                            catch
                            {
                                Console.WriteLine("Authentification failed.");
                            }
                            break;
                        }
                    case 4: // Update entity
                        {
                            try
                            { 
                            // Retrieve the storage account from the connection string.
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                ConfigurationManager.AppSettings["StorageConnectionString"]);

                            // Create the table client.
                            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                            // Create the CloudTable object that represents the "people" table.
                            CloudTable table = tableClient.GetTableReference("people");

                            // Create a retrieve operation that takes a customer entity.
                            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Smith", "Ben");

                            // Execute the operation.
                            TableResult retrievedResult = table.Execute(retrieveOperation);

                            // Assign the result to a CustomerEntity object.
                            CustomerEntity updateEntity = (CustomerEntity)retrievedResult.Result;

                            if (updateEntity != null)
                            {
                                // Change the phone number.
                                updateEntity.lvl = 105;

                                // Create the InsertOrReplace TableOperation.
                                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                                // Execute the operation.
                                table.Execute(updateOperation);

                                Console.WriteLine("Entity updated.");
                            }
                            else
                                Console.WriteLine("Entity could not be retrieved.");
                            }
                            catch
                            {
                                Console.WriteLine("Authentification failed.");
                            }
                            break;
                        }
                    case 5: // Insert-or-Replace
                        {
                            try
                            { 
                            // Retrieve the storage account from the connection string.
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                ConfigurationManager.AppSettings["StorageConnectionString"]);

                            // Create the table client.
                            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                            // Create the CloudTable object that represents the "people" table.
                            CloudTable table = tableClient.GetTableReference("people");

                            // Create a retrieve operation that takes a customer entity.
                            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Smith", "Ben");

                            // Execute the operation.
                            TableResult retrievedResult = table.Execute(retrieveOperation);

                            // Assign the result to a CustomerEntity object.
                            CustomerEntity updateEntity = (CustomerEntity)retrievedResult.Result;

                            if (updateEntity != null)
                            {
                                // Change the phone number.
                                updateEntity.lvl = 234;

                                // Create the InsertOrReplace TableOperation.
                                TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(updateEntity);

                                // Execute the operation.
                                table.Execute(insertOrReplaceOperation);

                                Console.WriteLine("Entity was updated.");
                            }
                            else
                                Console.WriteLine("Entity could not be retrieved.");
                            }
                            catch
                            {
                                Console.WriteLine("Authentification failed.");
                            }
                            break;
                        }
                    case 6: // Delete entity
                        {
                            try { 
                            // Retrieve the storage account from the connection string.
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                ConfigurationManager.AppSettings["StorageConnectionString"]);

                            // Create the table client.
                            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                            // Create the CloudTable that represents the "people" table.
                            CloudTable table = tableClient.GetTableReference("people");

                            // Create a retrieve operation that expects a customer entity.
                            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Smith", "Ben");

                            // Execute the operation.
                            TableResult retrievedResult = table.Execute(retrieveOperation);

                            // Assign the result to a CustomerEntity.
                            CustomerEntity deleteEntity = (CustomerEntity)retrievedResult.Result;

                            // Create the Delete TableOperation.
                            if (deleteEntity != null)
                            {
                                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                                // Execute the operation.
                                table.Execute(deleteOperation);

                                    Console.WriteLine("Entity deleted.");
                                }

                            else
                                Console.WriteLine("Could not retrieve the entity.");
                            }
                            catch
                            {
                                Console.WriteLine("Authentification failed.");
                            }
                            break;
                        }
                    case 7: // Delete table
                        {
                            try { 
                            // Retrieve the storage account from the connection string.
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                ConfigurationManager.AppSettings["StorageConnectionString"]);

                            // Create the table client.
                            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                            // Create the CloudTable that represents the "people" table.
                            CloudTable table = tableClient.GetTableReference("people");

                            // Delete the table it if exists.
                            table.DeleteIfExists();
                            Console.WriteLine("Table deleted.");
                            }
                            catch
                            {
                                Console.WriteLine("Authentification failed.");
                            }
                            break;
                        }
                default:
                    {
                            Console.WriteLine("Default.");
                            break;
                    }
            }
                Console.WriteLine("i={0}", i);
            }*/
            op = Console.Read();




        }
    }

    public class CustomerEntity : TableEntity
    {
        public CustomerEntity(string login, string domain)
        {
            this.PartitionKey = login;
            this.RowKey = domain;
        }

        public CustomerEntity() { }

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
        private int played;
        private DateTime regdate;
        private DateTime lastdate;

        public azureTable()
        {
            tableName = "users";
        }

        public string CreateTable(string name)
        {
            string result = "CODEempty";
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
                    result = "CODETable already exists.";
                    return result;
                }
                table.CreateIfNotExists();
                result = "CODETable has been created.";
            } catch
            {
                result = "CODEAuthentification failed.";
            }
            return result;
        }

        public string InsertEntity (string email, string psw)
        {
            string result = "CODEEmpty";
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
                    result = "CODETable not found.";
                    return result;
                }

                // Create a new customer entity.
                CustomerEntity customer = new CustomerEntity(login, domain);
                customer.password = psw;
                customer.lvl = 0;
                customer.regdate = DateTime.Now;
                customer.lastdate = DateTime.Now;
                customer.played = 0;
                //Console.WriteLine(customer.regdate);

                // Create the TableOperation object that inserts the customer entity.
                TableOperation insertOperation = TableOperation.Insert(customer);
            result = this.RetrieveEntity(email);
            if (result.Equals("CODENo such entity exists."))
            {
                try
                {
                    // Execute the insert operation.
                    table.Execute(insertOperation);
                    result = "CODECreating succesfull.";
                }
                catch
                {
                    result = "CODECreating failed. Entity already exists. Try-Catch";
                    //result = "CODEAuthentification failed.";
                }
            }
            else
            {
                if (result.StartsWith("CODR"))
                {
                    result = "CODECreating failed. Entity already exists. If-else";
                }
            }
            } catch
            {
                result = "CODEAuthentification failed.";
            }
            return result;
        }

        public string RetrieveEntity (string email)
        {
            string result = "CODEEmpty";
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
                    result = "CODETable not found.";
                    return result;
                }

                // Create the table query.
                TableQuery<CustomerEntity> rangeQuery = new TableQuery<CustomerEntity>().Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, login),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, domain)));

                int i = 0;
                result = "";
                
                // Loop through the results, displaying information about the entity.
                foreach (CustomerEntity entity in table.ExecuteQuery(rangeQuery))
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
                        result = "CODETable not found.";
                        //Console.WriteLine("Table not found.");
                    }
                }
                if (i==0)
                {
                    result = "CODENo such entity exists.";
                }
            }
            catch
            {
                result = "CODEAuthentification failed.";
                //Console.WriteLine("Authentification failed.");
            }
            return result;
        }
        
        public string AuthenticateUser(string email, string psw)
        {
            string result = "CODEEmpty";
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
                    result = "CODETable not found.";
                    return result;
                }

                // Create the table query.
                TableQuery<CustomerEntity> rangeQuery = new TableQuery<CustomerEntity>().Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, login),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, domain)));

                
                    int i = 0;
                    // Loop through the results, displaying information about the entity.
                    foreach (CustomerEntity entity in table.ExecuteQuery(rangeQuery))
                    {
                        try
                        {
                        if (entity.password == psw)
                        {
                            result = string.Format("CODE{0}", entity.lvl); // login succesfull
                            entity.lastdate = DateTime.Now;
                            // Create the InsertOrReplace TableOperation.
                            TableOperation updateOperation = TableOperation.Replace(entity);
                            // Execute the operation.
                            table.Execute(updateOperation);
                        }
                        else
                            result = "CODELogin failed. Wrong password.";// login failed
                            i++;
                        }
                        catch
                        {
                            result = "CODETable not found.";
                            //Console.WriteLine("Table not found.");
                        }
                    }
                    if (i == 0)
                    {
                        result = "CODELogin failed. No such entity exists.";
                    }
                
            }
            catch
            {
                result = "CODEAuthentification failed.";
                //Console.WriteLine("Authentification failed.");
            }
            return result;
        }

        public string RecallPassword (string email) // Test hard or die trying
        {
            string result = "CODEEmpty";
            try {
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
                    result = "CODETable not found.";
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
                        result = projectedPassword;
                        //Console.WriteLine(projectedPassword);
                        i++;
                    }
                    catch
                    {
                        result = "CODETable not found.";
                        //Console.WriteLine("Table not found.");
                    }
            }
                    if (i == 0)
                        result = "CODEEntity could not be retrieved.";
            
        }
            catch
            {
                result = "CODEAuthentification failed.";
                //Console.WriteLine("Authentification failed.");
            }


            return result;
        }

        public string UpdateLvl (string email)
        {
            string result = "CODEEmpty";
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
                    result = "CODETable not found.";
                    return result;
                }

                // Create a retrieve operation that takes a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(login, domain);

                // Execute the operation.
                TableResult retrievedResult = table.Execute(retrieveOperation);

                // Assign the result to a CustomerEntity object.
                CustomerEntity updateEntity = (CustomerEntity)retrievedResult.Result;

                if (updateEntity != null)
                {
                    // Change the phone number.
                    updateEntity.lvl++;

                    // Create the InsertOrReplace TableOperation.
                    TableOperation updateOperation = TableOperation.Replace(updateEntity);

                    // Execute the operation.
                    table.Execute(updateOperation);

                    //Console.WriteLine("Entity updated.");
                    result = "CODELevel updated.";
                }
                else
                    result = "CODEEntity could not be retrieved.";
                    //Console.WriteLine("CODEEntity could not be retrieved.");
            }
            catch
            {
                result = "CODEAuthentification failed.";
                //Console.WriteLine("CODEAuthentification failed.");
            }
            return result;
        }

        public string UpdatePsw (string email, string psw)
        {
            string result = "CODEEmpty";
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
                    result = "CODETable not found.";
                    return result;
                }

                // Create a retrieve operation that takes a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(login, domain);

                // Execute the operation.
                TableResult retrievedResult = table.Execute(retrieveOperation);

                // Assign the result to a CustomerEntity object.
                CustomerEntity updateEntity = (CustomerEntity)retrievedResult.Result;

                if (updateEntity != null)
                {
                    // Change the phone number.
                    updateEntity.password = psw;

                    // Create the InsertOrReplace TableOperation.
                    TableOperation updateOperation = TableOperation.Replace(updateEntity);

                    // Execute the operation.
                    table.Execute(updateOperation);

                    //Console.WriteLine("Entity updated.");
                    result = "CODEPassword updated.";
                }
                else
                    result = "CODEEntity could not be retrieved.";
                    //Console.WriteLine("CODEEntity could not be retrieved.");
            }
            catch
            {
                result = "CODEAuthentification failed.";
                //Console.WriteLine("CODEAuthentification failed.");
            }
            return result;
        }

        public string DeleteUser (string email)
        {
            string result = "CODEEmpty";
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
                    result = "CODETable not found.";
                    return result;
                }

                // Create a retrieve operation that expects a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(login, domain);

                // Execute the operation.
                TableResult retrievedResult = table.Execute(retrieveOperation);

                // Assign the result to a CustomerEntity.
                CustomerEntity deleteEntity = (CustomerEntity)retrievedResult.Result;

                // Create the Delete TableOperation.
                if (deleteEntity != null)
                {
                    TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                    // Execute the operation.
                    table.Execute(deleteOperation);

                    result = "CODEUser deleted.";
                    //Console.WriteLine("Entity deleted.");
                }

                else
                    result = "CODECould not retrieve the entity.";
                    //Console.WriteLine("Could not retrieve the entity.");
            }
            catch
            {
                result = "CODEAuthentification failed.";
                //Console.WriteLine("Authentification failed.");
            }
            return result;
        }

        public string DeleteTable (string name)
        {
            string result = "CODEEmpty";
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
                    result = "CODETable not found.";
                    return result;
                }

                // Delete the table it if exists.
                table.DeleteIfExists();
                result = "CODETable deleted.";
                //Console.WriteLine("Table deleted.");
            }
            catch
            {
                result = "CODEAuthentification failed.";
                //Console.WriteLine("Authentification failed.");
            }
            return result;
        }

        public string RetrieveAllEntities()
        {
            string result = "CODEEmpty";
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
                    result = "CODETable not found.";
                    return result;
                }

                // Create the table query.
                TableQuery<CustomerEntity> rangeQuery = new TableQuery<CustomerEntity>();

                int i = 0;
                result = "CODR";

                // Loop through the results, displaying information about the entity.
                foreach (CustomerEntity entity in table.ExecuteQuery(rangeQuery))
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
                        result = "CODETable not found.";
                        Console.WriteLine("Table not found.");
                    }
                }
                if (i == 0)
                {
                    result = "CODENo entities found.";
                }
            }
            catch
            {
                result = "CODEAuthentification failed.";
                Console.WriteLine("Authentification failed.");
            }
            return result;
        }

        public string SendMail(string login)
        {
            string result = "CODEEmpty";
            SmtpClient client = new SmtpClient();
            //client.Port = 25;
            client.Port = 587;
            client.Host = "smtp.rambler.ru";
            //client.Host = "smtp.google.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("support.sendmail@rambler.ru", "password");
            MailMessage mm = new MailMessage("support.sendmail@rambler.ru", "vereszp@gmail.com", "test subject", "test body 3");
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            Console.WriteLine("Sending mail...\n");
            try
            {
                client.Send(mm);
                result = "16Mail has been sent.";
            } catch
            {
                result = "35Mail hasnt been sent.";
            }

            return result;
        }

    }
}
