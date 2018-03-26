using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace SignalRImageModerator.Pages
{
    public class TransmissionMessageModel : PageModel
    {

        private readonly IHubContext<ImageModeratorHub> _hubContext;

        public TransmissionMessageModel(IHubContext<ImageModeratorHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [BindProperty]
        public string Message { get; set; }
        public async void OnGet()
        {
            using (var ImageModeratorHub = new SignalRImageModerator.ImageModeratorHub())
            {
                //ImageModeratorHub.Send(@"moderator", message: Messsage);
                await _hubContext.Clients.All.InvokeAsync("broadcastMessage", "Moderator", Message);
            }
            RedirectToPage("index.cshtml");

        }

        public async void OnPost()
        {
            using (var ImageModeratorHub = new SignalRImageModerator.ImageModeratorHub())
            {
                //ImageModeratorHub.Send(@"moderator", message: Messsage);
                await _hubContext.Clients.All.InvokeAsync("broadcastMessage", "Moderator/post", Message);

            }
            RedirectToPage("index.cshtml");
        }
    }
}