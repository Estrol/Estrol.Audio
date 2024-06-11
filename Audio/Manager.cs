namespace Estrol.Audio
{
    using System;
    using Estrol.Audio.Bindings;

    public class AudioManager
    {
        public static AudioManager Instance { get; private set; } = new();
        internal readonly List<Sample> Samples = [];
        internal readonly List<Encoder> Encoders = [];
        internal IntPtr DeviceHandle = Bindings_Header.INVALID_HANDLE;

        public static void Init()
        {
            EST_RESULT rc = Bindings_Sample.EST_DeviceInit(44100, (int)EST_DEVICE_FLAGS.EST_DEVICE_STEREO, out Instance.DeviceHandle);
            if (rc != EST_RESULT.EST_OK)
            {
                throw new Exception("Failed to initialize device");
            }
        }

        public static void Destroy()
        {
            foreach (var Sample in Instance.Samples.ToList())
            {
                Sample.Destroy();
            }

            Instance.Samples.Clear();
            Bindings_Sample.EST_DeviceFree(Instance.DeviceHandle);
        }

        public static void PlaySample(string path, float volume = 1.0f, float rate = 1.0f, float pan = 0.0f)
        {
            Sample Sample = LoadSample(path);

            Sample.Volume = volume;
            Sample.Rate = rate;
            Sample.Pan = pan;

            Sample.Play();
        }

        public static Sample LoadSample(string path)
        {
            Sample Sample = new();
            Sample.LoadFromFile(path);

            Instance.Samples.Add(Sample);
            return Sample;
        }

        public static Sample LoadSample(byte[] data)
        {
            Sample Sample = new();
            Sample.LoadFromMemory(data);

            Instance.Samples.Add(Sample);
            return Sample;
        }

        public static Sample LoadSample(Encoder encoder)
        {
            Sample Sample = new();
            Sample.LoadFromEncoder(encoder);

            Instance.Samples.Add(Sample);
            return Sample;
        }

        public static Encoder LoadEncoder(string path)
        {
            Encoder encoder = new();
            encoder.LoadFromFile(path);

            Instance.Encoders.Add(encoder);
            return encoder;
        }

        public static Encoder LoadEncoder(byte[] data)
        {
            Encoder encoder = new();
            encoder.LoadFromMemory(data);

            Instance.Encoders.Add(encoder);
            return encoder;
        }
    }
}
