using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
namespace SignalRImageModerator
{
    public class ImageModeratorHub : Hub
    {

        //Methodes accessibles / appelables depuis les clients 
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            if (message.IndexOf("trump") > 0)
            {
                Clients.Client(Context.ConnectionId).InvokeAsync("broadcastMessage", "Client : {name}", "DANGER!!!");
            }
            else
                Clients.All.InvokeAsync("broadcastMessage", "Client : {name}", "Message from client : {message}");
        }

        //public override Task OnConnectedAsync()
        //{
        //    this.Groups.AddAsync(this.Context.ConnectionId, "groupName");

        //    return base.OnConnectedAsync();
        //}

        public Task SendToGroup(string groupName, string message)
        {
            return Clients.Group(groupName).InvokeAsync("broadcastMessage", $"{Context.ConnectionId}@{groupName}: {message}");
        }
        public async Task JoinGroupAsync(string groupName)
        {
            await Groups.AddAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).InvokeAsync("broadcastMessage", $"{Context.ConnectionId} joined {groupName}");
        }
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).InvokeAsync("broadcastMessage", $"{Context.ConnectionId} left {groupName}");
        }
        public Task Echo(string message)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync("broadcastMessage", $"{Context.ConnectionId}: {message}");
        }

    }
}