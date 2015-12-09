using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;

namespace StorageAccountTableTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int op=10;
            for (int i=0; i<20; i++)
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
                            customer1.Email = "Jeff@contoso.com";
                            customer1.PhoneNumber = "425-555-0104";

                            // Create another customer entity and add it to the table.
                            CustomerEntity customer2 = new CustomerEntity("Smith", "Ben");
                            customer2.Email = "Ben@contoso.com";
                            customer2.PhoneNumber = "425-555-0102";

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
                                        entity.Email, entity.PhoneNumber);
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
                                        entity.Email, entity.PhoneNumber);
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
                                updateEntity.PhoneNumber = "425-555-0105";

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
                                updateEntity.PhoneNumber = "425-555-1234";

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
            }
            op = Console.Read();




        }
    }

    public class CustomerEntity : TableEntity
    {
        public CustomerEntity(string lastName, string firstName)
        {
            this.PartitionKey = lastName;
            this.RowKey = firstName;
        }

        public CustomerEntity() { }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
