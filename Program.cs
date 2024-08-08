using System.Runtime.Intrinsics.Arm;
using Azure.Messaging.ServiceBus;


string connectionString = "$StringConnection";
string queueName = "az204-queue";

// the client that owns the connection and can be used to create senders and receivers
ServiceBusClient client;

// the sender used to publish messages to the queue
ServiceBusSender sender;

// Create the clients that we'll use for sending and processing messages.
client = new ServiceBusClient(connectionString);
sender = client.CreateSender(queueName);

// create a batch
using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

for (int i = 0; i < 3; i++)
{
    // try adding message to the batch
    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
    {
        // if an exception occurs
        throw new Exception($"Exception {i} has occured.");
    }
}

try{
    // Use the producer client to send the batch of messages to the Services Bus queue
    await sender.SendMessagesAsync(messageBatch);
    System.Console.WriteLine($"A batch of three messages has been published to the queue.");
}
finally{
    // Calling DisposeAsync on cliente types is required to ensure that nerwork
    // resource and the unmanaged objects are properly cleaned up.
    await sender.DisposeAsync();
    await client.DisposeAsync();
}

Console.WriteLine("Follow the directions in the exercise to review the results in the Azure portal.");
Console.WriteLine("Press any key to continue");
Console.ReadKey();