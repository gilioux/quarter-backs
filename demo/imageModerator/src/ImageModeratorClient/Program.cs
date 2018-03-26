using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTestUpload
{
    class Program
    {
        //PS C:\Sources\TestDemo\Demo1\SignalRImageModerator\ConsoleAppTestUpload> dotnet run .\bin\Debug\netcoreapp2.0\ConsoleAppTestUpload.dll c:\temp\0016.jpg http://localhost:3488/api/TransmissionInfo
        public static int Main(string[] args)
        {
            string nomImage = null;
            string path = null;
            var requestUri = "";
            if (args.Length <= 1)
            {
                nomImage = "0016.jpg";
                path = $"c:\\temp\\{nomImage}";
                requestUri = "http://localhost:3488/api/TransmissionInfo";
                Console.WriteLine($"args.Length : {args.Length}");
                Console.WriteLine($"path : {path}");
                Console.WriteLine($"requestUri : {requestUri}");
                ;
            }
            else
            {
                nomImage = Path.GetFileName(args[1]);
                path = args[1];
                if (args.Length > 2)
                {
                    requestUri = args[2]; 
                }
                else
                {
                    requestUri = "http://localhost:3488/api/TransmissionInfo";
                }
                Console.WriteLine($"args.Length : {args.Length}");
                Console.WriteLine($"path : {path}");
                Console.WriteLine($"requestUri : {requestUri}");
            }

            var file = new FileStream(path, FileMode.Open);

            byte[] image;
            using (var reader = new BinaryReader(file))
            {
                image = reader.ReadBytes((int)file.Length);
            }

            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new ByteArrayContent(image);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        FileName = nomImage,
                        Name = "attachment",
                    };

                    var values = new[]
                            {
                                    new KeyValuePair<string, string>("sender","Moderator" ),
                                    new KeyValuePair<string, string>("message","Ceci est un message de modération"),
                                };

                    foreach (var keyValuePair in values)
                    {
                        content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                    }

                    content.Add(fileContent, "image");


                    var result = client.PostAsync(requestUri, content).Result;
                    Console.WriteLine("ok");
                }
            }
            return 0;
        }
    }

}
