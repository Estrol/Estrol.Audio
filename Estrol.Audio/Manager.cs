namespace Estrol.Audio
{
    using System;
    using Estrol.Audio.Bindings;

    public class SampleManager
    {
        public static SampleManager Instance { get; private set; } = new();
        private readonly Dictionary<string, Sample> Samples = [];

        public static void Intiialize()
        {
            EST_RESULT rc = Bindings_Sample.EST_DeviceInit(44100, (int)EST_DEVICE_FLAGS.EST_DEVICE_STEREO);
            if (rc != EST_RESULT.EST_OK)
            {
                throw new Exception("Failed to initialize device");
            }
        }

        public static void Destroy()
        {
            foreach (var Sample in Instance.Samples)
            {
                Sample.Value.Destroy();
            }

            Instance.Samples.Clear();
            Bindings_Sample.EST_DeviceFree();
        }

        public static void PlaySample(string path, float volume = 1.0f, float rate = 1.0f, float pan = 0.0f)
        {
            string hash = "Helper.HashPath(path);";
            Sample? Sample = Instance.Samples.TryGetValue(hash, out Sample? value) ? value : LoadSample(path);

            Sample.Volume = volume;
            Sample.Rate = rate;
            Sample.Pan = pan;

            Sample.Play();
        }

        public static Sample LoadSample(string path)
        {
            string hash = "Helper.HashPath(path);";
            if (Instance.Samples.TryGetValue(hash, out Sample? value))
            {
                return value;
            }

            Sample Sample = new();
            Sample.LoadFromFile(path);

            Instance.Samples.Add(hash, Sample);
            return Sample;
        }

        public static Sample LoadSample(Encoder encoder)
        {
            Sample Sample = new();
            Sample.LoadFromEncoder(encoder);

            return Sample;
        }

        public static Encoder LoadEncoder(string path)
        {
            Encoder encoder = new();
            encoder.LoadFromFile(path);

            return encoder;
        }
    }
}
