using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieQueue.Services
{
    public interface IQueueService
    {
        Task SendMessageAsync(string message);
        Task<string> ReceiveMessageAsync();
    }
}
