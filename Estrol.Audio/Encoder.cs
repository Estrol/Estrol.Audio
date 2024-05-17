namespace Estrol.Audio
{
    using System.Runtime.InteropServices;
    using Estrol.Audio.Bindings;

    public class Encoder
    {
        public IntPtr Handle { get; private set; } = Bindings_Header.INVALID_HANDLE;

        public void LoadFromFile(string file)
        {
            if (Handle != Bindings_Header.INVALID_HANDLE)
            {
                Bindings_Encoder.EST_EncoderFree(Handle);
            }

            IntPtr ptr = Marshal.StringToHGlobalAnsi(file);

            EST_RESULT rc = Bindings_Encoder.EST_EncoderLoad(ptr, IntPtr.Zero, (int)EST_DECODER_FLAGS.EST_DECODER_STEREO, out nint _out);
            if (rc != EST_RESULT.EST_OK)
            {
                IntPtr error = Bindings_Sample.EST_GetError();
                string? errorStr = Marshal.PtrToStringUTF8(error);

                throw new Exception("Failed to load sample: " + errorStr);
            }

            Handle = _out;
            Marshal.FreeHGlobal(ptr);
        }

        public void LoadFromMemory(byte[] data)
        {
            if (Handle != Bindings_Header.INVALID_HANDLE)
            {
                Bindings_Encoder.EST_EncoderFree(Handle);
            }

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();

            EST_RESULT rc = Bindings_Encoder.EST_EncoderLoadMemory(ptr, data.Length, IntPtr.Zero, EST_DECODER_FLAGS.EST_DECODER_STEREO, Handle);
            if (rc != EST_RESULT.EST_OK)
            {
                IntPtr error = Bindings_Sample.EST_GetError();
                string? errorStr = Marshal.PtrToStringUTF8(error);

                throw new Exception("Failed to load sample: " + errorStr);
            }

            handle.Free();
        }

        public bool Pitch
        {
            get
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return false;
                }

                Bindings_Encoder.EST_EncoderGetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_ENCODER_PITCH, out float pitch);
                return pitch != 0.0f;
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Encoder.EST_EncoderSetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_ENCODER_PITCH, value ? 1.0f : 0.0f);
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

                EST_RESULT rc = Bindings_Encoder.EST_EncoderGetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_ENCODER_TEMPO, out float attrib_value);
                if (rc != EST_RESULT.EST_OK)
                {
                    return 0.0f;
                }

                return attrib_value;
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Encoder.EST_EncoderSetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_ENCODER_TEMPO, value);
            }
        }

        public float SampleRate
        {
            get
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return 0.0f;
                }

                var rc = Bindings_Encoder.EST_EncoderGetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_ENCODER_SAMPLERATE, out float value);
                if (rc != EST_RESULT.EST_OK)
                {
                    return 0.0f;
                }

                return value;
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Encoder.EST_EncoderSetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_ENCODER_SAMPLERATE, value);
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

                Bindings_Encoder.EST_EncoderGetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_VOLUME, out float value);
                return value;
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Encoder.EST_EncoderSetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_VOLUME, value);
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

                Bindings_Encoder.EST_EncoderGetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PAN, out float value);
                return value;
            }
            set
            {
                if (Handle == Bindings_Header.INVALID_HANDLE)
                {
                    return;
                }

                Bindings_Encoder.EST_EncoderSetAttribute(Handle, (int)EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PAN, value);
            }
        }
    }
}