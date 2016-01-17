using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Configuration;

namespace StorageAccountQueueTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!VerifyConfiguration())
            {
                Console.ReadLine();
                return;
            }
            int action = 0;
            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.ConnectionStrings["queueConnectionString"].ConnectionString);
            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            for (int count=0; count < 20; count++)
            {            
                Console.WriteLine(
                    "Choose action:\n1 - Create queue\n2 - Add message\n3 - Peek at the next message\n4 - Change message");
                Console.WriteLine(
                    "5 - De-queue message\n6 - Read all(20) messages\n7 - Get the queue length\n8 - Delete queue");
                string actioninput = Console.ReadLine();
                if (!int.TryParse(actioninput, out action))
                {
                    Console.WriteLine("{0} - is not a number", actioninput);
                    break;
                }
                switch (action)
                {
                    case 0:
                        Console.WriteLine("count={0}", count);
                        Console.ReadLine();
                        return;
                    case 1:
                        // Create the queue if it doesn't already exist
                        try
                        {
                            queue.CreateIfNotExists();
                            Console.WriteLine("Queue has been created.");
                        } catch
                        {
                            Console.WriteLine("Queue creation failed.");
                        }                    
                        break;
                    case 2:
                        {
                            // Create a message and add it to the queue.
                            //CloudQueueMessage message = new CloudQueueMessage("Hello, World");
                            CloudQueueMessage message = new CloudQueueMessage(Console.ReadLine());
                            try
                            {
                                queue.AddMessage(message);
                                Console.WriteLine("Message has been added.");
                            }
                            catch
                            {
                                Console.WriteLine("Message creation failed.");
                            }                        
                            break;
                        }
                    case 3:
                        {
                            try
                            {
                                // Peek at the next message
                                CloudQueueMessage peekedMessage = queue.PeekMessage();
                                // Display message.
                                Console.WriteLine(peekedMessage.AsString);
                            } catch
                            {
                                Console.WriteLine("Message peeking failed.");
                            }                        
                            break;
                        }
                    case 4:
                        {
                            try
                            {
                                // Get the message from the queue and update the message contents.
                                CloudQueueMessage message = queue.GetMessage();
                                //message.SetMessageContent("Updated contents.");
                                message.SetMessageContent(Console.ReadLine());
                                queue.UpdateMessage(message,
                                    TimeSpan.FromSeconds(6.0),  // Make it visible for another 60 seconds.
                                    MessageUpdateFields.Content | MessageUpdateFields.Visibility);
                                Console.WriteLine("Message has been changed.");
                            } catch
                            {
                                Console.WriteLine("Message changing failed.");
                            }                        
                            break;
                        }
                    case 5:
                        {
                            try
                            {
                                // Get the next message
                                CloudQueueMessage retrievedMessage = queue.GetMessage();
                                //Process the message in less than 30 seconds, and then delete the message
                                queue.DeleteMessage(retrievedMessage);
                                Console.WriteLine("Message has been de-queued.");
                            } catch
                            {
                                Console.WriteLine("Message de-queueing failed.");
                            }
                            break;
                        }
                    case 6:
                        {
                            try
                            {
                                int i = 1;
                                foreach (CloudQueueMessage message in queue.GetMessages(20, TimeSpan.FromSeconds(30)))
                                {
                                    Console.WriteLine("{0}.{1}", i, message.AsString);
                                    i++;
                                    // Process all messages in less than 5 minutes, deleting each message after processing.
                                    queue.DeleteMessage(message);
                                }
                            } catch
                            {
                                Console.WriteLine("Read all(20) messages fails.");
                            }
                            break;
                        }
                    case 7:
                        {
                            try
                            {
                                // Fetch the queue attributes.
                                queue.FetchAttributes();
                                // Retrieve the cached approximate message count.
                                int? cachedMessageCount = queue.ApproximateMessageCount;
                                // Display number of messages.
                                Console.WriteLine("Number of messages in queue: " + cachedMessageCount);
                            } catch
                            {
                                Console.WriteLine("Counting messages fails.");
                            }
                            break;
                        }
                    case 8:
                        {
                            try
                            {
                                // Delete the queue.
                                queue.Delete();
                                Console.WriteLine("Queue has been deleted.");
                            } catch
                            {
                                Console.WriteLine("Queue deletion fails.");
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            Console.ReadLine();
        }

        private static bool VerifyConfiguration()
        {
            string queueConnectionString = ConfigurationManager.ConnectionStrings["queueConnectionString"].ConnectionString;
            
            bool configOK = true;
            if (string.IsNullOrWhiteSpace(queueConnectionString))
            {
                configOK = false;
                Console.WriteLine("Please add the Azure Storage account credentials in App.config");

            }
            return configOK;
        }
    }
}
