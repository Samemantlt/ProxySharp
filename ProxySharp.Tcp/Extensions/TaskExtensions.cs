using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySharp.Tcp.Extensions
{
    internal static class TaskExtensions
    {
        public static async Task WithCancellationToken(this Task task, CancellationToken cancellationToken)
        {
            var waitTask = Task.Delay(-1, cancellationToken);

            await Task.WhenAny(task, waitTask);

            if (task.IsCompleted)
                return;

            if (task.IsFaulted)
                await task;

            throw new TimeoutException();
        }
    }
}