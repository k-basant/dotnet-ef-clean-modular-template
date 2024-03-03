namespace ProjName.Application.Shared.Interfaces;

public interface IQueueService
{
    /// <summary>
    /// Dequeus single message from queue and deserialize it in requested type <typeparamref name="T"/>. If <typeparamref name="T"/> is string then no deserialization is done.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> DequeueMessage<T>(string queueName);
    /// <summary>
    /// Dequeus multiple messages from queue based on specified <paramref name="count"/> and deserialize them in requested type <typeparamref name="T"/>. If <typeparamref name="T"/> is string then no deserialization is done.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<List<T>> DequeueMessages<T>(string queueName, int count = 500);
    /// <summary>
    /// Enqueue the single provided <paramref name="message"/> after serializing it into string (only if <typeparamref name="T"/> is not string). 
    /// Reference looping will be ignored while serializing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    Task EnqueueMessage<T>(string queueName, T message);
    /// <summary>
    /// Enqueue the multiple provided <paramref name="messages"/> after serializing them into string (only if <typeparamref name="T"/> is not string). 
    /// Reference looping will be ignored while serializing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="messages"></param>
    /// <returns></returns>
    Task EnqueueMessages<T>(string queueName, List<T> messages);
}
