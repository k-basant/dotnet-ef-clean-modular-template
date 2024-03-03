using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace ProjName.Infrastructure.Shared.Services;

public class MemoryQueueService : IQueueService
{
    private static readonly ConcurrentDictionary<string, Queue<string>> Queues = new ConcurrentDictionary<string, Queue<string>>();

    public async Task<T> DequeueMessage<T>(string queueName)
    {
        if (Queues.TryGetValue(queueName, out var queue))
        {
            if (queue.TryDequeue(out var message))
            {
                return (await Task.FromResult(typeof(T) == typeof(string) ? (T)(object)message : JsonConvert.DeserializeObject<T>(message)).ConfigureAwait(false))!;
            }
        }

        return default(T)!;
    }

    public async Task<List<T>> DequeueMessages<T>(string queueName, int count = 500)
    {
        if (Queues.TryGetValue(queueName, out var queue))
        {
            // create list of messages after deserializing
            var messages = new List<T>();
            for (int i = 0; i < count && queue.Count > 0; i++)
            {
                var message = queue.Dequeue();
                messages.Add((typeof(T) == typeof(string) ? (T)(object)message : JsonConvert.DeserializeObject<T>(message))!);
            }

            return await Task.FromResult(messages).ConfigureAwait(false);
        }

        return new List<T>();
    }

    public async Task EnqueueMessage<T>(string queueName, T message)
    {
        var queue = Queues.GetOrAdd(queueName, new Queue<string>());

        var serializedMessage = typeof(T) == typeof(string) ? (string)(object)message! : JsonConvert.SerializeObject(message, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        queue.Enqueue(serializedMessage);
        await Task.CompletedTask.ConfigureAwait(false);
    }

    public async Task EnqueueMessages<T>(string queueName, List<T> messages)
    {
        var queue = Queues.GetOrAdd(queueName, new Queue<string>());

        foreach (var message in messages)
        {
            var serializedMessage = typeof(T) == typeof(string) ? (string)(object)message! : JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            queue.Enqueue(serializedMessage);
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }    
}
