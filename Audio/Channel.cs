using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Estrol.Audio.Bindings;

namespace Estrol.Audio
{
    public class Channel
    {
        public IntPtr Handle { get; private set; }

        internal Channel(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new Exception("Invalid channel handle");
            }

            Handle = handle;

            LoadAttributes();
        }

        internal Channel(string path)
        {
            if (AudioManager.Instance.Handle == IntPtr.Zero)
            {
                throw new Exception("AudioManager is not initialized");
            }

            IntPtr pathPtr = Marshal.StringToHGlobalAnsi(path);
            Handle = Bindings_Channel.EST_ChannelLoad(AudioManager.Instance.Handle, pathPtr);
            Marshal.FreeHGlobal(pathPtr);

            if (Handle == IntPtr.Zero)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to load channel: {errorStr}");
            }

            LoadAttributes();
        }

        internal Channel(byte[] data)
        {
            if (AudioManager.Instance.Handle == IntPtr.Zero)
            {
                throw new Exception("AudioManager is not initialized");
            }

            IntPtr dataPtr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, dataPtr, data.Length);

            Handle = Bindings_Channel.EST_ChannelLoadMemory(AudioManager.Instance.Handle, dataPtr, data.Length);
            Marshal.FreeHGlobal(dataPtr);

            if (Handle == IntPtr.Zero)
            {
                IntPtr errorMsg = Device_Bindings.EST_ErrorGetMessage();
                string? errorStr = Marshal.PtrToStringUTF8(errorMsg);

                throw new Exception($"Failed to load channel: {errorStr}");
            }

            LoadAttributes();
        }

        public bool IsPlaying => Bindings_Channel.EST_ChannelIsPlaying(Handle) == EST_BOOL.EST_TRUE;

        internal void LoadAttributes()
        {
            ESTAttributeValue value = new()
            {
                attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_VOLUME
            };

            EST_RESULT result = Bindings_Channel.EST_ChannelGetAttribute(Handle, value);
            if (result != EST_RESULT.EST_OK)
            {
                throw new Exception("Failed to get channel volume");
            }

            _volume = value.floatValue;

            value.attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PAN;
            result = Bindings_Channel.EST_ChannelGetAttribute(Handle, value);
            if (result != EST_RESULT.EST_OK)
            {
                throw new Exception("Failed to get channel pan");
            }

            _pan = value.floatValue;

            value.attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_RATE;
            result = Bindings_Channel.EST_ChannelGetAttribute(Handle, value);
            if (result != EST_RESULT.EST_OK)
            {
                throw new Exception("Failed to get channel rate");
            }

            _rate = value.floatValue;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float _volume = 1.0f;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float _pan = 0.0f;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float _rate = 1.0f;

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;

                ESTAttributeValue _val = new()
                {
                    attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_VOLUME,
                    type = EST_ATTRIB_VAL_TYPE.EST_ATTRIB_VAL_FLOAT,
                    floatValue = value
                };

                EST_RESULT result = Bindings_Channel.EST_ChannelSetAttribute(Handle, _val);
                if (result != EST_RESULT.EST_OK)
                {
                    throw new Exception("Failed to set channel volume");
                }
            }
        }

        public float Pan
        {
            get => _pan;
            set
            {
                _pan = value;

                ESTAttributeValue _val = new()
                {
                    attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_PAN,
                    type = EST_ATTRIB_VAL_TYPE.EST_ATTRIB_VAL_FLOAT,
                    floatValue = value
                };

                EST_RESULT result = Bindings_Channel.EST_ChannelSetAttribute(Handle, _val);
                if (result != EST_RESULT.EST_OK)
                {
                    throw new Exception("Failed to set channel pan");
                }
            }
        }

        public float Rate
        {
            get => _rate;
            set
            {
                _rate = value;

                ESTAttributeValue _val = new()
                {
                    attribute = EST_ATTRIBUTE_FLAGS.EST_ATTRIB_RATE,
                    type = EST_ATTRIB_VAL_TYPE.EST_ATTRIB_VAL_FLOAT,
                    floatValue = value
                };

                EST_RESULT result = Bindings_Channel.EST_ChannelSetAttribute(Handle, _val);
                if (result != EST_RESULT.EST_OK)
                {
                    throw new Exception("Failed to set channel rate");
                }
            }
        }

        public void Free()
        {
            ArgumentNullException.ThrowIfNull(Handle, nameof(Handle));

            Bindings_Channel.EST_ChannelFree(Handle);
            Handle = IntPtr.Zero;

            AudioManager.Instance.Channels.Remove(this);
        }

        public void Play(bool restart = false)
        {
            if (Handle == IntPtr.Zero)
            {
                return;
            }

            int value = restart ? 1 : 0;
            Bindings_Channel.EST_ChannelPlay(Handle, value);
        }

        public void Stop()
        {
            if (Handle == IntPtr.Zero)
            {
                return;
            }

            Bindings_Channel.EST_ChannelStop(Handle);
        }

        public void Pause()
        {
            if (Handle == IntPtr.Zero)
            {
                return;
            }

            Bindings_Channel.EST_ChannelPause(Handle);
        }
    }
}