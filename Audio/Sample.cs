namespace Estrol.Audio
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Estrol.Audio.Bindings;

    public class Sample : IDisposable
    {
        public IntPtr Handle = Bindings_Header.INVALID_HANDLE;

        internal Sample()
        {

        }

        public void Play()
        {
            if (Handle == Bindings_Header.INVALID_HANDLE)
            {
                return;
            }

            Bindings_Sample.EST_SamplePlay(AudioManager.Instance.DeviceHandle, Handle);
        }

        public void Stop()
        {
            if (Handle == Bindings_Header.INVALID_HANDLE)
            {
                return;
            }

            Bindings_Sample.EST_SampleStop(AudioManager.Instance.DeviceHandle, Handle);
        }

        public void Destroy()
        {
            AudioManager.Instance.Samples.Remove(this);

            if (Handle == Bindings_Header.INVALID_HANDLE)
            {
                return;
            }

            Bindings_Sample.EST_SampleFree(AudioManager.Instance.DeviceHandle, Handle);
            Handle = Bindings_Header.INVALID_HANDLE;
        }

        public void LoadFromFile(string file)
        {
            if (Handle != IntPtr.Zero)
            {
                Bindings_Sample.EST_SampleFree(AudioManager.Instance.DeviceHandle, Handle);
            }

            unsafe
            {
                IntPtr ptr = Marshal.StringToHGlobalAnsi(file);

                EST_RESULT rc = Bindings_Sample.EST_SampleLoad(AudioManager.Instance.DeviceHandle, ptr, out Handle);
                if (rc != EST_RESULT.EST_OK)
                {
                    IntPtr error = Bindings_Sample.EST_GetError();
                    string? errorStr = Marshal.PtrToStringUTF8(error);

                    throw new Exception("Failed to load sample: " + errorStr);
                }

                Marshal.FreeHGlobal(ptr);
            }
        }

        public void LoadFromMemory(byte[] data)
        {
            if (Handle != IntPtr.Zero)
            {
                Bindings_Sample.EST_SampleFree(AudioManager.Instance.DeviceHandle, Handle);
            }

            IntPtr ptr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, ptr, data.Length);

            EST_RESULT rc = Bindings_Sample.EST_SampleLoadMemory(AudioManager.Instance.DeviceHandle, ptr, data.Length, out Handle);
            if (rc != EST_RESULT.EST_OK)
            {
                IntPtr error = Bindings_Sample.EST_GetError();
                string? errorStr = Marshal.PtrToStringUTF8(error);

                Marshal.FreeHGlobal(ptr);
                throw new Exception("Failed to load sample: " + errorStr);
            }

            Marshal.FreeHGlobal(ptr);
        }

        public void LoadFromEncoder(Encoder encoder)
        {
            if (Handle != IntPtr.Zero)
            {
                Bindings_Sample.EST_SampleFree(AudioManager.Instance.DeviceHandle, Handle);
            }

            var result = Bindings_Encoder.EST_EncoderGetSample(encoder.Handle, AudioManager.Instance.DeviceHandle, out Handle);
            if (result != EST_RESULT.EST_OK)
            {
                IntPtr error = Bindings_Sample.EST_GetError();
                string? errorStr = Marshal.PtrToStringUTF8(error);

                throw new Exception("Failed to load sample: " + errorStr);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool Pitch
        {
            get
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return false;
                }

                Bindings_Sample.EST_SampleGetAttribute(AudioManager.Instance.DeviceHandle, Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PITCH, out IntPtr value);
                float pitch = Marshal.PtrToStructure<float>(value);

                return pitch != 0.0f;
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Sample.EST_SampleSetAttribute(AudioManager.Instance.DeviceHandle, Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PITCH, value ? 1.0f : 0.0f);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public float Rate
        {
            get
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return 0.0f;
                }

                Bindings_Sample.EST_SampleGetAttribute(AudioManager.Instance.DeviceHandle, Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_RATE, out IntPtr value);
                return Marshal.PtrToStructure<float>(value);
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Sample.EST_SampleSetAttribute(AudioManager.Instance.DeviceHandle, Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_RATE, value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public float Volume
        {
            get
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return 0.0f;
                }

                Bindings_Sample.EST_SampleGetAttribute(AudioManager.Instance.DeviceHandle, Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_VOLUME, out IntPtr value);
                return Marshal.PtrToStructure<float>(value);
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Sample.EST_SampleSetAttribute(AudioManager.Instance.DeviceHandle, Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_VOLUME, value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public float Pan
        {
            get
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return 0.0f;
                }

                Bindings_Sample.EST_SampleGetAttribute(AudioManager.Instance.DeviceHandle, Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PAN, out IntPtr value);
                return Marshal.PtrToStructure<float>(value);
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Sample.EST_SampleSetAttribute(AudioManager.Instance.DeviceHandle, Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PAN, value);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Destroy();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}