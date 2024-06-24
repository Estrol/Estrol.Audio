using System.Runtime.InteropServices;
using Estrol.Audio.Bindings;

namespace Estrol.Audio
{
    public enum DecoderFlags
    {
        Unknown = EST_DECODER_FLAGS.EST_DECODER_UNKNOWN,

        Mono = EST_DECODER_FLAGS.EST_DECODER_MONO,
        Stereo = EST_DECODER_FLAGS.EST_DECODER_STEREO,

        FloatSingle = EST_DECODER_FLAGS.EST_DECODER_FORMAT_S16, // (NOT IMPLEMENTED)
        FloatDouble = EST_DECODER_FLAGS.EST_DECODER_FORMAT_F32, // (NOT IMPLEMENTED)
    }

    public class Encoder
    {
        public IntPtr Handle { get; private set; }

        internal Encoder(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new Exception("Invalid channel handle");
            }

            Handle = handle;

            LoadAttributes();
        }

        internal Encoder(string path, DecoderFlags flags = DecoderFlags.Unknown)
        {
            IntPtr pathPtr = Marshal.StringToHGlobalAnsi(path);
            Handle = Bindings_Encoder.EST_EncoderLoad(pathPtr, IntPtr.Zero, (int)flags);
            Marshal.FreeHGlobal(pathPtr);

            if (Handle == IntPtr.Zero)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to load encoder: {errorStr}");
            }

            LoadAttributes();
        }

        internal Encoder(byte[] data, DecoderFlags flags = DecoderFlags.Unknown)
        {
            IntPtr dataPtr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, dataPtr, data.Length);

            Handle = Bindings_Encoder.EST_EncoderLoadMemory(dataPtr, data.Length, IntPtr.Zero, (EST_DECODER_FLAGS)flags);
            Marshal.FreeHGlobal(dataPtr);

            if (Handle == IntPtr.Zero)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to load encoder: {errorStr}");
            }

            LoadAttributes();
        }

        public bool Render()
        {
            EST_RESULT result = Bindings_Encoder.EST_EncoderRender(Handle);
            return result == EST_RESULT.EST_OK;
        }

        public void Save(string file)
        {
            IntPtr pathPtr = Marshal.StringToHGlobalAnsi(file);
            EST_RESULT result = Bindings_Encoder.EST_EncoderExportFile(Handle, EST_FILE_EXPORT.EST_EXPORT_WAV, pathPtr);
            Marshal.FreeHGlobal(pathPtr);

            if (result != EST_RESULT.EST_OK)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to save encoder: {errorStr}");
            }
        }

        public Channel GetChannel()
        {
            if (Handle == IntPtr.Zero)
            {
                throw new Exception("Invalid encoder handle");
            }

            IntPtr channel = Bindings_Channel.EST_EncoderGetChannel(AudioManager.Instance.Handle, Handle);
            if (channel == IntPtr.Zero)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to get channel: {errorStr}");
            }

            Channel instance = new(channel);
            AudioManager.Instance.Channels.Add(instance);

            return new Channel(channel);
        }

        public Channel[] GetChannels(int size)
        {
            if (Handle == IntPtr.Zero)
            {
                throw new Exception("Invalid Encoder handle");
            }

            IntPtr channelsPtr = Marshal.AllocHGlobal(size * IntPtr.Size);
            int count = Bindings_Channel.EST_EncoderGetChannels(AudioManager.Instance.Handle, Handle, size, channelsPtr);

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

        public Sample GetSample()
        {
            if (Handle == IntPtr.Zero)
            {
                throw new Exception("Invalid Encoder handle");
            }

            IntPtr sample = Bindings_Sample.EST_SampleFromEncoder(Handle);
            if (sample == IntPtr.Zero)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to get sample: {errorStr}");
            }

            return new Sample(sample);
        }

        public void Free()
        {
            if (Handle == IntPtr.Zero)
            {
                return;
            }

            Bindings_Encoder.EST_EncoderFree(Handle);
            Handle = IntPtr.Zero;
        }

        private float _tempo = 1.0f;
        private float _sampleRate = 44100.0f;
        private float _volume = 1.0f;
        private float _pan = 0.0f;

        internal void LoadAttributes()
        {
            ESTAttributeValue value = new()
            {
                attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_ENCODER_TEMPO,
            };

            EST_RESULT result = Bindings_Encoder.EST_EncoderGetAttribute(Handle, value);
            if (result != EST_RESULT.EST_OK)
            {
                throw new Exception("Failed to get encoder tempo");
            }

            _tempo = value.floatValue;

            value.attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_ENCODER_SAMPLERATE;
            result = Bindings_Encoder.EST_EncoderGetAttribute(Handle, value);
            if (result != EST_RESULT.EST_OK)
            {
                throw new Exception("Failed to get encoder sample rate");
            }

            _sampleRate = value.floatValue;

            value.attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_VOLUME;
            result = Bindings_Encoder.EST_EncoderGetAttribute(Handle, value);
            if (result != EST_RESULT.EST_OK)
            {
                throw new Exception("Failed to get encoder volume");
            }

            _volume = value.floatValue;

            value.attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PAN;
            result = Bindings_Encoder.EST_EncoderGetAttribute(Handle, value);
            if (result != EST_RESULT.EST_OK)
            {
                throw new Exception("Failed to get encoder pan");
            }

            _pan = value.floatValue;
        }

        public float Tempo
        {
            get => _tempo;
            set
            {
                ESTAttributeValue val = new()
                {
                    attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_ENCODER_TEMPO,
                    type = EST_ATTRIB_VAL_TYPE.EST_ATTRIB_VAL_FLOAT,
                    floatValue = value,
                };

                EST_RESULT result = Bindings_Encoder.EST_EncoderSetAttribute(Handle, val);
                if (result != EST_RESULT.EST_OK)
                {
                    throw new Exception("Failed to set encoder tempo");
                }

                _tempo = val.floatValue;
            }
        }

        public float SampleRate
        {
            get => _sampleRate;
            set
            {
                ESTAttributeValue val = new()
                {
                    attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_ENCODER_SAMPLERATE,
                    type = EST_ATTRIB_VAL_TYPE.EST_ATTRIB_VAL_FLOAT,
                    floatValue = value,
                };

                EST_RESULT result = Bindings_Encoder.EST_EncoderSetAttribute(Handle, val);
                if (result != EST_RESULT.EST_OK)
                {
                    throw new Exception("Failed to set encoder sample rate");
                }

                _sampleRate = val.floatValue;
            }
        }

        public float Volume
        {
            get => _volume;
            set
            {
                ESTAttributeValue val = new()
                {
                    attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_VOLUME,
                    type = EST_ATTRIB_VAL_TYPE.EST_ATTRIB_VAL_FLOAT,
                    floatValue = value,
                };

                EST_RESULT result = Bindings_Encoder.EST_EncoderSetAttribute(Handle, val);
                if (result != EST_RESULT.EST_OK)
                {
                    throw new Exception("Failed to set encoder volume");
                }

                _volume = val.floatValue;
            }
        }

        public float Pan
        {
            get => _pan;
            set
            {
                ESTAttributeValue val = new()
                {
                    attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PAN,
                    type = EST_ATTRIB_VAL_TYPE.EST_ATTRIB_VAL_FLOAT,
                    floatValue = value,
                };

                EST_RESULT result = Bindings_Encoder.EST_EncoderSetAttribute(Handle, val);
                if (result != EST_RESULT.EST_OK)
                {
                    throw new Exception("Failed to set encoder pan");
                }

                _pan = val.floatValue;
            }
        }
    }
}