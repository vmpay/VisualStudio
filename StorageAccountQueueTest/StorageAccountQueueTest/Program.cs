using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace StorageAccountQueueTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AzureQueue pvpQueue = new AzureQueue();
            int action = 0;
            bool io = false;
            for (int count = 0; count < 20; count++)
            {
                Console.WriteLine(
                    "Choose action:\n1 - Create queue\n2 - Add message\n3 - Peek at the next message\n4 - Change message");
                Console.WriteLine(
                    "5 - De-queue message\n6 - Read and delete all(20) messages\n7 - Get the queue length\n8 - View all(20) messages");
                Console.WriteLine("9 - Delete queue\n10 - Count Queue Length manually");
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
                        Console.WriteLine("Queues have been already created");
                        break;
                    case 2:
                        Console.WriteLine("Please select queue: 1 - input queue, 0 - output queue");
                        actioninput = Console.ReadLine();
                        if (!int.TryParse(actioninput, out action))
                        {
                            Console.WriteLine("{0} - is not a number", actioninput);
                            break;
                        }
                        if (action < 0 || action > 1)
                        {
                            Console.WriteLine("{0} - is not a valid number", actioninput);
                            break;
                        }
                        if (action == 1)
                            io = true;
                        else
                            io = false;
                        Console.WriteLine("Write down the message:");
                        actioninput = Console.ReadLine();
                        pvpQueue.AddMsgQ(actioninput, io);
                        break;
                    case 3:
                        Console.WriteLine("Please select queue: 1 - input queue, 0 - output queue");
                        actioninput = Console.ReadLine();
                        if (!int.TryParse(actioninput, out action))
                        {
                            Console.WriteLine("{0} - is not a number", actioninput);
                            break;
                        }
                        if (action < 0 || action > 1)
                        {
                            Console.WriteLine("{0} - is not a valid number", actioninput);
                            break;
                        }

                        if (action == 1)
                            io = true;
                        else
                            io = false;
                        pvpQueue.PeekMsg(io);
                        break;
                    case 4:
                        Console.WriteLine("Please select queue: 1 - input queue, 0 - output queue");
                        actioninput = Console.ReadLine();
                        if (!int.TryParse(actioninput, out action))
                        {
                            Console.WriteLine("{0} - is not a number", actioninput);
                            break;
                        }
                        if (action < 0 || action > 1)
                        {
                            Console.WriteLine("{0} - is not a valid number", actioninput);
                            break;
                        }

                        if (action == 1)
                            io = true;
                        else
                            io = false;
                        Console.WriteLine("Write down new message:");
                        actioninput = Console.ReadLine();
                        pvpQueue.UpdMsg(actioninput, io);
                        break;
                    case 5:
                        Console.WriteLine("Please select queue: 1 - input queue, 0 - output queue");
                        actioninput = Console.ReadLine();
                        if (!int.TryParse(actioninput, out action))
                        {
                            Console.WriteLine("{0} - is not a number", actioninput);
                            break;
                        }
                        if (action < 0 || action > 1)
                        {
                            Console.WriteLine("{0} - is not a valid number", actioninput);
                            break;
                        }

                        if (action == 1)
                            io = true;
                        else
                            io = false;
                        pvpQueue.deQueueMsg(io);
                        break;
                    case 6:
                        Console.WriteLine("Please select queue: 1 - input queue, 0 - output queue");
                        actioninput = Console.ReadLine();
                        if (!int.TryParse(actioninput, out action))
                        {
                            Console.WriteLine("{0} - is not a number", actioninput);
                            break;
                        }
                        if (action < 0 || action > 1)
                        {
                            Console.WriteLine("{0} - is not a valid number", actioninput);
                            break;
                        }

                        if (action == 1)
                            io = true;
                        else
                            io = false;
                        Console.WriteLine("Write down quantity of messages:");
                        actioninput = Console.ReadLine();
                        if (!int.TryParse(actioninput, out action))
                        {
                            Console.WriteLine("{0} - is not a number", actioninput);
                            break;
                        }
                        pvpQueue.ReadMsg(action, io);
                        break;
                    case 7:
                        Console.WriteLine("Please select queue: 1 - input queue, 0 - output queue");
                        actioninput = Console.ReadLine();
                        if (!int.TryParse(actioninput, out action))
                        {
                            Console.WriteLine("{0} - is not a number", actioninput);
                            break;
                        }
                        if (action < 0 || action > 1)
                        {
                            Console.WriteLine("{0} - is not a valid number", actioninput);
                            break;
                        }

                        if (action == 1)
                            io = true;
                        else
                            io = false;
                        action = pvpQueue.getQueueLength(io);
                        Console.WriteLine(action);
                        break;
                    case 8:
                        Console.WriteLine("Please select queue: 1 - input queue, 0 - output queue");
                        actioninput = Console.ReadLine();
                        if (!int.TryParse(actioninput, out action))
                        {
                            Console.WriteLine("{0} - is not a number", actioninput);
                            break;
                        }
                        if (action < 0 || action > 1)
                        {
                            Console.WriteLine("{0} - is not a valid number", actioninput);
                            break;
                        }

                        if (action == 1)
                            io = true;
                        else
                            io = false;
                        Console.WriteLine("Write down quantity of messages:");
                        actioninput = Console.ReadLine();
                        if (!int.TryParse(actioninput, out action))
                        {
                            Console.WriteLine("{0} - is not a number", actioninput);
                            break;
                        }
                        pvpQueue.ViewMsg(action, io);
                        break;
                    case 9:
                        Console.WriteLine("Please select queue: 1 - input queue, 0 - output queue");
                        actioninput = Console.ReadLine();
                        if (!int.TryParse(actioninput, out action))
                        {
                            Console.WriteLine("{0} - is not a number", actioninput);
                            break;
                        }
                        if (action < 0 || action > 1)
                        {
                            Console.WriteLine("{0} - is not a valid number", actioninput);
                            break;
                        }

                        if (action == 1)
                            io = true;
                        else
                            io = false;
                        pvpQueue.deleteQueue(io);
                        break;
                    case 10:
                        Console.WriteLine("Please select queue: 1 - input queue, 0 - output queue");
                        actioninput = Console.ReadLine();
                        if (!int.TryParse(actioninput, out action))
                        {
                            Console.WriteLine("{0} - is not a number", actioninput);
                            break;
                        }
                        if (action < 0 || action > 1)
                        {
                            Console.WriteLine("{0} - is not a valid number", actioninput);
                            break;
                        }

                        if (action == 1)
                            io = true;
                        else
                            io = false;
                        Console.WriteLine("Queue count (manually) = {0}", pvpQueue.countQueueLength(io));
                        break;
                    /*case 10:
                        {
                            string blobName = "vmpay";
                            try
                            {
                                Blobs.PointImagesBlob.DownloadMiniature(blobName);
                            }
                            catch
                            {
                                Console.WriteLine("Plakat jeszcze nie został wygenerowany.");
                                //throw new UnauthorizedAccessException("Plakat jeszcze nie został wygenerowany.");
                            }
                            break;
                        }
                    case 11:
                        {
                            try
                            {
                                string blobName = "vmpay";
                                Console.WriteLine("Opening file.");
                                var fileStream = System.IO.File.OpenRead(@"d:\vmpay.pdf");
                                Console.WriteLine("Creating blob.");
                                CloudBlockBlob blob = Blobs.TripPostersBlob.Insert(fileStream, blobName);
                                Console.WriteLine("Inserting successfull.");
                            }
                            catch
                            {
                                Console.WriteLine("Insertion failed.");
                            }
                            break;
                            

                        }*/
                    default:
                        Console.WriteLine("count={0}", count);
                        Console.ReadLine();
                        return;
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

        class AzureQueue
        {
            private CloudStorageAccount storageAccount;
            private CloudQueueClient queueClient;
            private CloudQueue queuei;//TRUE - input queue
            private CloudQueue queueo;//FLASE - output queue

            public AzureQueue()
            {
                if (!VerifyConfiguration())
                {
                    Console.ReadLine();
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
                    Console.WriteLine("Queues have been created.");
                }
                catch
                {
                    Console.WriteLine("Queues haven't been created.");
                }


            }

            public bool createQueue(bool io)
            {
                // Create the queue if it doesn't already exist
                try
                {
                    CloudQueue queue;
                    if (io)
                        queue = queuei;
                    else
                        queue = queueo;
                    queue.CreateIfNotExists();
                    Console.WriteLine("Queue has been created.");
                    return true;
                }
                catch
                {
                    Console.WriteLine("Queue creation failed.");
                    return false;
                }
            }

            public bool AddMsgQ(string inputstring, bool io)
            {
                // Create a message and add it to the queue.
                CloudQueueMessage message = new CloudQueueMessage(inputstring);
                //message.ExpirationTime = 
                try
                {
                    CloudQueue queue;
                    if (io)
                        queue = queuei;
                    else
                        queue = queueo;
                    queue.AddMessage(message, TimeSpan.FromSeconds(30));
                    Console.WriteLine("Message has been added to {0}.", io);
                    return true;
                }
                catch
                {
                    Console.WriteLine("Message creation failed {0}.", io);
                    return false;
                }
            }

            public bool PeekMsg(bool io)
            {
                try
                {
                    // Peek at the next message
                    CloudQueueMessage peekedMessage;
                    if (io)
                        peekedMessage = queuei.PeekMessage();
                    else
                        peekedMessage = queueo.PeekMessage();
                    // Display message.
                    Console.WriteLine(peekedMessage.AsString);
                    return true;
                }
                catch
                {
                    Console.WriteLine("Message peeking failed.");
                    return false;
                }
            }

            public bool UpdMsg(string inputstring, bool io)
            {
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
                    Console.WriteLine("Message has been changed.");
                    return true;
                }
                catch
                {
                    Console.WriteLine("Message changing failed.");
                    return false;
                }
            }

            public bool deQueueMsg(bool io)
            {
                try
                {
                    CloudQueue queue;
                    if (io)
                        queue = queuei;
                    else
                        queue = queueo;
                    // Get the next message
                    CloudQueueMessage retrievedMessage = queue.GetMessage();
                    //Process the message in less than 30 seconds, and then delete the message
                    queue.DeleteMessage(retrievedMessage);
                    Console.WriteLine("Message has been de-queued.");
                    return true;
                }
                catch
                {
                    Console.WriteLine("Message de-queueing failed.");
                    return false;
                }
            }

            public bool ReadMsg(int count, bool io)
            {
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
                        Console.WriteLine("{0}. {1}", i, message.AsString);
                        i++;
                        // Process all messages in less than 30 seconds, deleting each message after processing.
                        queue.DeleteMessage(message);
                    }
                    if (i == 1)
                    {
                        Console.WriteLine("No messages found in the queue.");
                        return false;
                    }
                    return true;
                }
                catch
                {
                    Console.WriteLine("Read and delete all(20) messages fails.");
                    return false;
                }
            }

            public bool ViewMsg(int count, bool io)
            {
                try
                {
                    CloudQueue queue;
                    if (io)
                        queue = queuei;
                    else
                        queue = queueo;
                    int i = 1;
                    foreach (CloudQueueMessage message in queue.GetMessages(count, TimeSpan.FromSeconds(5)))
                    {
                        Console.WriteLine("{0}. {1} - {2}", i, message.AsString, message.ExpirationTime);
                        i++;
                        /*queue.DeleteMessage(message);
                        if (message.AsString != "vip msg")
                            queue.AddMessage(message, TimeSpan.FromSeconds(30));*/
                    }
                    if (i == 0)
                    {
                        Console.WriteLine("No messages found in the queue.");
                        return false;
                    }
                    return true;
                }
                catch
                {
                    Console.WriteLine("View all(20) messages fails.");
                    return false;
                }
            }

            public int getQueueLength(bool io)
            {
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
                    Console.WriteLine("Number of messages in queue: " + cachedMessageCount);
                    return result;
                }
                catch
                {
                    Console.WriteLine("Counting messages fails.");
                    return -1;
                }
            }

            public bool deleteQueue(bool io)
            {
                try
                {
                    CloudQueue queue;
                    if (io)
                        queue = queuei;
                    else
                        queue = queueo;
                    // Delete the queue.
                    queue.Delete();
                    Console.WriteLine("Queue has been deleted.");
                    return true;
                }
                catch
                {
                    Console.WriteLine("Queue deletion fails.");
                    return false;
                }
            }

            public int countQueueLength(bool io)
            {
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
                    Console.WriteLine("View all(20) messages fails.");
                    return -1;
                }
            }

        }

    }

}
