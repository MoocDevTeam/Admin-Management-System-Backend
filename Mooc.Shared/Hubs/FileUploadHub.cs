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
        /// Adds the client to a specific group based on the upload ID.
        /// </summary>
        /// <param name="uploadId">The unique ID for the file upload.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task JoinGroup(string uploadId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, uploadId);
            Console.WriteLine($"Client {Context.ConnectionId} joined group {uploadId}");
        }

        /// <summary>
        /// Removes the client from all groups on disconnection.
        /// </summary>
        /// <param name="exception">The exception that occurred, if any.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Client {Context.ConnectionId} disconnected.");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
