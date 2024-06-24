namespace Estrol.Audio.Example
{
    using System.Runtime.CompilerServices;
    using Estrol.Audio;
    using Estrol.Audio.Bindings;

    internal class Program
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void hello_test();

        static void Main(string[] args)
        {
            hello_test();

            AudioManager.Init(44100, DeviceFlags.Stereo);

            Encoder encoder = AudioManager.EncoderLoad("F:\\test.wav");
            encoder.Tempo *= 1.5f;
            encoder.Render();

            Sample sample = encoder.GetSample();
            Channel channel = sample.GetChannel();

            channel.Play();

            while (channel.IsPlaying)
            {
                Thread.Sleep(100);
            }

            channel.Stop();
            channel.Free();
            encoder.Free();

            AudioManager.Free();
        }
    }
}
