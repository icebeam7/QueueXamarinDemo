using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Threading.Tasks;

namespace QueueXamarinDemo.Services
{
    public static class QueueService
    {
        private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse("aqui-va-tu-cadena-de-conexion");
        private const string queueName = "servicequeue";

        public static async Task EnqueueMessage(string message)
        {
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            var cloudQueueMessage = new CloudQueueMessage(message);
            await queue.AddMessageAsync(cloudQueueMessage);
        }

        public static async Task<string> PeekAtNextMessage()
        {
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            var peekedMessage = await queue.PeekMessageAsync();
            return $"Message: {peekedMessage.AsString} expires on {peekedMessage.ExpirationTime.Value.ToString()}";
        }

        public static async Task ChangeContentNextMessage(string message)
        {
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            var cloudQueueMessage = await queue.GetMessageAsync();
            cloudQueueMessage.SetMessageContent(message);
            await queue.UpdateMessageAsync(cloudQueueMessage, TimeSpan.FromSeconds(60.0),
                MessageUpdateFields.Content | MessageUpdateFields.Visibility);
        }

        public static async Task<string> DequeueMessage()
        {
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            var cloudQueueMessage = await queue.GetMessageAsync();
            var message = cloudQueueMessage.AsString;
            await queue.DeleteMessageAsync(cloudQueueMessage);
            return message;
        }

        public static async Task<int> GetQueueLength()
        {
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            await queue.FetchAttributesAsync();
            var cachedMessageCount = queue.ApproximateMessageCount;
            return cachedMessageCount.HasValue ? cachedMessageCount.Value : 0;
        }
    }
}
