using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Windows.Media.Core;
using Windows.Media.MediaProperties;
using Windows.Media.Transcoding;

namespace AzureAudioGenerator.Controllers
{
    public class AudioStream
    {
        public async Task WriteToStream(Stream output, HttpContent content, TransportContext context)
        {
            try
            {
                //
                // Create a PCM audio source
                //

                var generator = new AudioSampleGenerator();

                var source = new MediaStreamSource(
                    new AudioStreamDescriptor(
                        generator.EncodingProperties
                        )
                    );
                source.CanSeek = false;
                source.MusicProperties.Title = "CS_D_MediaStreamSource_EncodeAudio";

                source.SampleRequested += (MediaStreamSource sender, MediaStreamSourceSampleRequestedEventArgs args) =>
                {
                    try
                    {
                        Console.WriteLine("SampleRequested Time: {0}", generator.Time);

                        // Generate 5s of data
                        if (generator.Time.TotalSeconds < 5)
                        {
                            args.Request.Sample = generator.GenerateSample();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Sample exception");
                    }
                };

                //
                // Encode PCM to ADTS
                //

                var profile = MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Medium);
                profile.Container.Subtype = "ADTS";

                var output2 = new PseudoSeekableStream(output); // Pretend the stream is seekable so .AsRandomAccessStream() works

                var transcoder = new MediaTranscoder();
                var result = await transcoder.PrepareMediaStreamSourceTranscodeAsync(
                    source,
                    output2.AsRandomAccessStream(),
                    profile
                    );
                await result.TranscodeAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Transcode exception");
            }

            output.Close();
        }
    }
}