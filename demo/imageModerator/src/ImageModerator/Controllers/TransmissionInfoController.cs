using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRImageModerator;

namespace WebApiWithSignalR.Controllers
{
    [Route("api/TransmissionInfo")]
    public class TransmissionInfoController : Controller
    {

        private readonly IHubContext<ImageModeratorHub> _hubContext;
        
        public TransmissionInfoController(IHubContext<ImageModeratorHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // GET: api/TransmissionInfo
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/TransmissionInfo/5
        [HttpGet("{id}", Name = "Get")]
        [Produces("application/json")]
        public string Get(int id)
        {
            _hubContext.Clients.All.InvokeAsync("broadcastMessage", "trefee", $"my message {id}");
            return $"value {id}";
        }
        
        // POST: api/TransmissionInfo
        [HttpPost]
        public void Post(string sender, string message, IFormFile image)
        {
            image = Request.Form.Files["attachment"];
            var name = image.FileName;
            var length = image.Length;
            byte[] img = new byte[length];
            _hubContext.Clients.All.InvokeAsync("broadcastMessage", sender, message).ConfigureAwait(false);

            image.OpenReadStream().ReadAsync(img, 0x0, (int)length).ConfigureAwait(false);

            string imreBase64Data = Convert.ToBase64String(img);
            string imgDataURL = $"data:image/png;base64,{imreBase64Data}";

            _hubContext.Clients.All.InvokeAsync("broadcastMessageImage", sender, imgDataURL).ConfigureAwait(false);
            image = null;
        }
        // PUT: api/TransmissionInfo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

    }

}
