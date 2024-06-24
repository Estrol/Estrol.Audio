using System.Runtime.InteropServices;

namespace Estrol.Audio.Bindings
{
    public enum EST_RESULT
    {
        EST_OK = 0,
        EST_ERROR = 1,

        EST_ERROR_OUT_OF_MEMORY = 2,
        EST_ERROR_INVALID_ARGUMENT = 3,
        EST_ERROR_INVALID_STATE = 4,
        EST_ERROR_INVALID_OPERATION = 5,
        EST_ERROR_INVALID_FORMAT = 6,
        EST_ERROR_INVALID_DATA = 7,
        EST_ERROR_TIMEDOUT = 8,
        EST_ERROR_ENCODER_EMPTY = 9,
        EST_ERROR_ENCODER_UNSUPPORTED = 10,
        EST_ERROR_ENCODER_INVALID_WRITE = 11
    };

    public enum EST_BOOL
    {
        EST_FALSE = 0,
        EST_TRUE = 1
    }

    public enum EST_DEVICE_FLAGS
    {
        EST_DEVICE_UNKNOWN,

        EST_DEVICE_MONO,   // Single channel audio
        EST_DEVICE_STEREO, // Two channel audio

        EST_DEVICE_FORMAT_S16, // Device signed 16 bit format (NOT IMPLEMENTED)
        EST_DEVICE_FORMAT_F32, // Device 32 bit floating point format (NOT IMPLEMENTED)

        EST_DEVICE_NOSTOP // Prevent the device from stopping when all samples are finished (NOT IMPLEMENTED)
    };

    public enum EST_DECODER_FLAGS
    {
        EST_DECODER_UNKNOWN,

        EST_DECODER_MONO,
        EST_DECODER_STEREO,

        EST_DECODER_FORMAT_S16, // (NOT IMPLEMENTED)
        EST_DECODER_FORMAT_F32, // (NOT IMPLEMENTED)
    };

    public enum EST_STATUS
    {
        EST_STATUS_UNKNOWN,

        EST_STATUS_IDLE,
        EST_STATUS_PLAYING,
        EST_STATUS_AT_END
    };

    public enum EST_FILE_EXPORT
    {
        EST_EXPORT_UNKNOWN,

        EST_EXPORT_WAV // Export sample as wav 16bit format
    };

    public struct EST_DEVICE_INFO
    {
        public int sampleRate;
        public int channels;
        public int deviceIndex;
        public EST_DEVICE_FLAGS flags;
    }

    public struct EST_DECODER_INFO
    {
        public int sampleRate;
        public int channels;
        public int pcmSize;
    }

    public enum EST_ATTRIBUTE_FLAGS
    {
        EST_ATTRIB_UNKNOWN,

        EST_ATTRIB_VOLUME = 0,  // Volume of the sample
        EST_ATTRIB_RATE = 1,    // Playback rate of the sample
        EST_ATTRIB_PITCH = 2,   // Pitch toggle
        EST_ATTRIB_PAN = 3,     // Pan of the sample
        EST_ATTRIB_LOOPING = 4, // Sample loop

        EST_ATTRIB_ENCODER_TEMPO = 5,      // Encoder tempo control which change audio rate without pitch change (different from sampleRate)
        EST_ATTRIB_ENCODER_PITCH = 6,      // Encoder pitch control without change the audio rate
        EST_ATTRIB_ENCODER_SAMPLERATE = 7, // Encoder both tempo and pitch control
    };

    partial class Bindings_Header
    {
        public const IntPtr INVALID_HANDLE = -1;
    }

    public delegate void EST_AUDIO_CALLBACK(IntPtr Handle, IntPtr pUserData, IntPtr pData, int frameCount);
    public delegate void EST_DECODER_CALLBACK(IntPtr Handle, IntPtr pUserData, IntPtr pData, int frameCount);

    public enum EST_ATTRIB_VAL_TYPE
    {
        EST_ATTRIB_VAL_FLOAT,
        EST_ATTRIB_VAL_INT,
        EST_ATTRIB_VAL_BOOL
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct ESTAttributeValue
    {
        [FieldOffset(0)]
        public EST_ATTRIBUTE_FLAGS attribute;

        [FieldOffset(4)]
        public EST_ATTRIB_VAL_TYPE type;

        [FieldOffset(8)]
        public float floatValue;
        [FieldOffset(8)]
        public int intValue;
        [FieldOffset(8)]
        public EST_BOOL boolValue;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ESTEncoderInfo
    {
        [FieldOffset(0)]
        public int sampleRate;
        [FieldOffset(4)]
        public int channels;
        [FieldOffset(8)]
        public int pcmSize;
    }
}