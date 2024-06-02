namespace Estrol.Audio
{
    using System.Runtime.InteropServices;
    using Estrol.Audio.Bindings;

    public class Sample : IDisposable
    {
        private IntPtr Handle = Bindings_Header.INVALID_HANDLE;

        internal Sample()
        {

        }

        public void Play()
        {
            if (Handle == Bindings_Header.INVALID_HANDLE)
            {
                return;
            }

            Bindings_Sample.EST_SamplePlay(Handle);
        }

        public void Stop()
        {
            if (Handle == Bindings_Header.INVALID_HANDLE)
            {
                return;
            }

            Bindings_Sample.EST_SampleStop(Handle);
        }

        public void Destroy()
        {
            AudioManager.Instance.Samples.Remove(this);

            if (Handle == Bindings_Header.INVALID_HANDLE)
            {
                return;
            }

            Bindings_Sample.EST_SampleFree(Handle);
            Handle = Bindings_Header.INVALID_HANDLE;
        }

        public void LoadFromFile(string file)
        {
            if (Handle != IntPtr.Zero)
            {
                Bindings_Sample.EST_SampleFree(Handle);
            }

            unsafe
            {
                IntPtr ptr = Marshal.StringToHGlobalAnsi(file);

                EST_RESULT rc = Bindings_Sample.EST_SampleLoad(ptr, out Handle);
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
                Bindings_Sample.EST_SampleFree(Handle);
            }

            IntPtr ptr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, ptr, data.Length);

            EST_RESULT rc = Bindings_Sample.EST_SampleLoadMemory(ptr, data.Length, out Handle);
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
                Bindings_Sample.EST_SampleFree(Handle);
            }

            var result = Bindings_Encoder.EST_EncoderGetSample(encoder.Handle, out Handle);
            if (result != EST_RESULT.EST_OK)
            {
                IntPtr error = Bindings_Sample.EST_GetError();
                string? errorStr = Marshal.PtrToStringUTF8(error);

                throw new Exception("Failed to load sample: " + errorStr);
            }
        }

        public bool Pitch
        {
            get
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return false;
                }

                Bindings_Sample.EST_SampleGetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PITCH, out IntPtr value);
                float pitch = Marshal.PtrToStructure<float>(value);

                return pitch != 0.0f;
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Sample.EST_SampleSetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PITCH, value ? 1.0f : 0.0f);
            }
        }

        public float Rate
        {
            get
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return 0.0f;
                }

                Bindings_Sample.EST_SampleGetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_RATE, out IntPtr value);
                return Marshal.PtrToStructure<float>(value);
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Sample.EST_SampleSetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_RATE, value);
            }
        }

        public float Volume
        {
            get
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return 0.0f;
                }

                Bindings_Sample.EST_SampleGetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_VOLUME, out IntPtr value);
                return Marshal.PtrToStructure<float>(value);
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Sample.EST_SampleSetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_VOLUME, value);
            }
        }

        public float Pan
        {
            get
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return 0.0f;
                }

                Bindings_Sample.EST_SampleGetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PAN, out IntPtr value);
                return Marshal.PtrToStructure<float>(value);
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Sample.EST_SampleSetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PAN, value);
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