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
                Console.WriteLine("Choose the option:\n1.Create table & Insert data\n2. Read data (Partition key)\n3. Read data (partition & row keys)\n4. Update Ben's number");
                op = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("op = {0}", op);
                switch (op)
            {
                case 1:
                    {
                            // Retrieve the storage account from the connection string.
                            Console.WriteLine("Retrieve the storage account from the connection string.");
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                ConfigurationManager.AppSettings["StorageConnectionString"]);

                            // Create the table client.
                            Console.WriteLine("Create the table client.");
                            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                            // Create the table if it doesn't exist.
                            Console.WriteLine("Create the table if it doesn't exist.");
                            CloudTable table = tableClient.GetTableReference("people");
                            table.CreateIfNotExists();

                            // Create the batch operation.
                            Console.WriteLine("Create the batch operation.");
                            TableBatchOperation batchOperation = new TableBatchOperation();

                            // Create a customer entity and add it to the table.
                            Console.WriteLine("Create a customer entity and add it to the table.");
                            CustomerEntity customer1 = new CustomerEntity("Smith", "Jeff");
                            customer1.Email = "Jeff@contoso.com";
                            customer1.PhoneNumber = "425-555-0104";

                            // Create another customer entity and add it to the table.
                            Console.WriteLine("Create a customer entity and add it to the table 2.");
                            CustomerEntity customer2 = new CustomerEntity("Smith", "Ben");
                            customer2.Email = "Ben@contoso.com";
                            customer2.PhoneNumber = "425-555-0102";

                            // Add both customer entities to the batch insert operation.
                            Console.WriteLine("Add both customer entities to the batch insert operation.");
                            batchOperation.Insert(customer1);
                            batchOperation.Insert(customer2);

                            // Execute the batch operation.
                            Console.WriteLine("Execute the batch operation.");
                            table.ExecuteBatch(batchOperation);
                        break;
                    }
                case 2:
                    {
                            // Retrieve the storage account from the connection string.
                            Console.WriteLine("Retrieve the storage account from the connection string.");
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                ConfigurationManager.AppSettings["StorageConnectionString"]);

                            // Create the table client.
                            Console.WriteLine("Create the table client.");
                            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                            // Create the CloudTable object that represents the "people" table.
                            Console.WriteLine("Create the CloudTable object that represents the people table.");
                            CloudTable table = tableClient.GetTableReference("people");

                            // Construct the query operation for all customer entities where PartitionKey="Smith".
                            Console.WriteLine("Construct the query operation for all customer entities where PartitionKey=Smith.");
                            TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

                            // Print the fields for each customer.
                            Console.WriteLine("Print the fields for each customer.");
                            foreach (CustomerEntity entity in table.ExecuteQuery(query))
                        {
                            Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                                entity.Email, entity.PhoneNumber);
                        }
                        break;
                    }
                    case 3:
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

                            // Loop through the results, displaying information about the entity.
                            foreach (CustomerEntity entity in table.ExecuteQuery(rangeQuery))
                            {
                                Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                                    entity.Email, entity.PhoneNumber);
                            }
                            break;
                        }
                    case 4:
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
                            break;
                        }
                    case 5:
                        {

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
