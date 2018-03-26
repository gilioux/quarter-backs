﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SignalRImageModerator.Controllers
{
    /*
    [Produces("application/json")]
    [Route("api/Image")]
    public class ImageController : Controller
    {
        // GET: api/Image
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Image/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Image
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Image/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    */

using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

using System;

using System.IO;

using System.Threading.Tasks;



namespace IC6.Xamarin.WebApi.Controllers

    {

        [Produces("application/json")]

        [Route("api/Image")]

        public class ImageController : Controller

        {

            private readonly IHostingEnvironment _environment;



            public ImageController(IHostingEnvironment environment)

            {

                _environment = environment ?? throw new ArgumentNullException(nameof(environment));

            }



            // POST: api/Image

            [HttpPost]

            public async Task Post(IFormFile file)

            {

                var uploads = Path.Combine(_environment.WebRootPath, "uploads");

                if (file.Length > 0)

                {

                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))

                    {

                        await file.CopyToAsync(fileStream);

                    }

                }

            }

        }

    }

}
