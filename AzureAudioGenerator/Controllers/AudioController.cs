using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Windows.Media.Core;
using Windows.Media.MediaProperties;
using Windows.Media.Transcoding;

namespace AzureAudioGenerator.Controllers
{
    public class AudioController : ApiController
    {
        // GET: api/Audio
        public HttpResponseMessage Get()
        {
            try
            {
                var audio = new AudioStream();

                var response = Request.CreateResponse();
                response.Content = new PushStreamContent(
                    (Func<Stream, HttpContent, TransportContext, Task>)audio.WriteToStream,
                    new MediaTypeHeaderValue("audio/aac")
                    );

                return response;
            }
            catch(Exception e)
            {
                Debug.WriteLine("Get exception");
            }

            return null;
        }
    }
}
