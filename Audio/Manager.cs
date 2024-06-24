namespace Estrol.Audio
{
    using System;
    using System.Runtime.InteropServices;
    using Estrol.Audio.Bindings;

    public enum DeviceFlags
    {
        Unknown = EST_DEVICE_FLAGS.EST_DEVICE_UNKNOWN,
        Mono = EST_DEVICE_FLAGS.EST_DEVICE_MONO,
        Stereo = EST_DEVICE_FLAGS.EST_DEVICE_STEREO,
        FormatS16 = EST_DEVICE_FLAGS.EST_DEVICE_FORMAT_S16,
        FormatF32 = EST_DEVICE_FLAGS.EST_DEVICE_FORMAT_F32
    }

    public class AudioManager
    {
        public static AudioManager Instance { get; private set; } = new();
        public IntPtr Handle { get; private set; } = IntPtr.Zero;
        internal List<Channel> Channels { get; private set; } = [];

        internal AudioManager()
        {
        }

        public static void Init(int sampleRate, DeviceFlags flags)
        {
            if (Instance.Handle != IntPtr.Zero)
            {
                return;
            }

            IntPtr handle = Device_Bindings.EST_DeviceInit(sampleRate, (int)flags);
            if (handle == IntPtr.Zero)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to initialize audio device: {errorStr}");
            }

            Instance.Handle = handle;
        }

        public static void Free()
        {
            if (Instance.Handle == IntPtr.Zero)
            {
                return;
            }

            Device_Bindings.EST_DeviceFree(Instance.Handle);
        }

        public static Channel ChannelLoad(string file)
        {
            if (!File.Exists(file))
            {
                throw new Exception("File does not exist");
            }

            Channel channel = new(file);
            Instance.Channels.Add(channel);

            return channel;
        }

        public static Channel ChannelLoad(byte[] data)
        {
            Channel channel = new(data);
            Instance.Channels.Add(channel);

            return channel;
        }

        public static Sample SampleLoad(string file)
        {
            if (!File.Exists(file))
            {
                throw new Exception("File does not exist");
            }

            Sample sample = new(file);
            return sample;
        }

        public static Sample SampleLoad(byte[] data)
        {
            Sample sample = new(data);
            return sample;
        }

        public static Sample SampleLoad(Encoder encoder)
        {
            Sample sample = new(encoder);
            return sample;
        }

        public static Encoder EncoderLoad(string file, DecoderFlags flags = DecoderFlags.Unknown)
        {
            if (!File.Exists(file))
            {
                throw new Exception("File does not exist");
            }

            Encoder encoder = new(file, flags);
            return encoder;
        }

        public static Encoder EncoderLoad(byte[] data, DecoderFlags flags = DecoderFlags.Unknown)
        {
            Encoder encoder = new(data, flags);
            return encoder;
        }
    }
}
