using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Media.Core;
using Windows.Media.MediaProperties;

namespace AzureAudioGenerator.Controllers
{
    public class AudioSampleGenerator
    {
        const int m_sampleRate = 44100;
        const double m_sineFrequency = 440;

        public AudioEncodingProperties EncodingProperties { get; private set; }

        // Start time of the next sample in seconds
        public TimeSpan Time { get; private set; }

        public AudioSampleGenerator()
        {
            EncodingProperties = AudioEncodingProperties.CreatePcm(m_sampleRate, 1, 16);
        }

        public MediaStreamSample GenerateSample()
        {
            // Generate 1s of data
            var buffer = new byte[2 * m_sampleRate];

            var time = Time.TotalSeconds;
            for (int i = 0; i < m_sampleRate; i++)
            {
                Int16 value = (Int16)(Int16.MaxValue * Math.Sin(2 * Math.PI * m_sineFrequency * time * time)); // Chirp sine wave

                buffer[2 * i] = (byte)(value & 0xFF);
                buffer[2 * i + 1] = (byte)((value >> 8) & 0xFF);

                time += (1 / (double)m_sampleRate);
            }

            var sample = MediaStreamSample.CreateFromBuffer(buffer.AsBuffer(), Time);
            sample.Discontinuous = (Time == TimeSpan.Zero);
            sample.Duration = TimeSpan.FromSeconds(1);
            Time += TimeSpan.FromSeconds(1);

            return sample;
        }
    }
}