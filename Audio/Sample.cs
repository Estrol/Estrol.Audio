using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Estrol.Audio.Bindings;

namespace Estrol.Audio
{
    public class Sample
    {
        public IntPtr Handle { get; private set; }

        internal Sample(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new Exception("Invalid Sample handle");
            }

            Handle = handle;
        }

        internal Sample(string path)
        {
            IntPtr pathPtr = Marshal.StringToHGlobalAnsi(path);
            Handle = Bindings_Sample.EST_SampleLoad(pathPtr);
            Marshal.FreeHGlobal(pathPtr);

            if (Handle == IntPtr.Zero)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to load Sample: {errorStr}");
            }
        }

        internal Sample(byte[] data)
        {
            IntPtr dataPtr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, dataPtr, data.Length);

            Handle = Bindings_Sample.EST_SampleLoadFromMemory(dataPtr, data.Length);
            Marshal.FreeHGlobal(dataPtr);

            if (Handle == IntPtr.Zero)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to load Sample: {errorStr}");
            }
        }

        internal Sample(Encoder encoder)
        {
            Handle = Bindings_Sample.EST_SampleFromEncoder(encoder.Handle);

            if (Handle == IntPtr.Zero)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to load Sample: {errorStr}");
            }
        }

        public void Free()
        {
            if (Handle == IntPtr.Zero)
            {
                return;
            }

            Bindings_Sample.EST_SampleFree(Handle);
            Handle = IntPtr.Zero;
        }

        public Channel GetChannel()
        {
            if (Handle == IntPtr.Zero)
            {
                throw new Exception("Invalid Sample handle");
            }

            IntPtr channel = Bindings_Channel.EST_SampleGetChannel(AudioManager.Instance.Handle, Handle);
            if (channel == IntPtr.Zero)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to get channel: {errorStr}");
            }

            Channel instance = new(channel);
            AudioManager.Instance.Channels.Add(instance);

            return instance;
        }

        public Channel[] GetChannels(int size)
        {
            if (Handle == IntPtr.Zero)
            {
                throw new Exception("Invalid Sample handle");
            }

            IntPtr channelsPtr = Marshal.AllocHGlobal(size * IntPtr.Size);
            int count = Bindings_Channel.EST_SampleGetChannels(AudioManager.Instance.Handle, Handle, size, channelsPtr);

            if (count == 0)
            {
                Marshal.FreeHGlobal(channelsPtr);
                return [];
            }

            IntPtr[] channels = new IntPtr[count];
            Marshal.Copy(channelsPtr, channels, 0, count);
            Marshal.FreeHGlobal(channelsPtr);

            Channel[] instances = new Channel[count];
            for (int i = 0; i < count; i++)
            {
                instances[i] = new(channels[i]);
                AudioManager.Instance.Channels.Add(instances[i]);
            }

            return instances;
        }
    }
}