using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Mooc.Shared.Hubs
{
    /// <summary>
    /// SignalR hub for handling file upload progress updates.
    /// </summary>
    public class FileUploadHub : Hub
    {
        /// <summary>
        /// Sends a progress update to all connected clients.
        /// </summary>
        /// <param name="percentage">The percentage of the upload completed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendProgressUpdate(int percentage)
        {
            await Clients.All.SendAsync("ReceiveProgressUpdate", percentage);
        }
    }
}
