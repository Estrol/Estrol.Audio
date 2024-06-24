namespace Estrol.Audio.Bindings
{
    using System.Runtime.InteropServices;

    public partial class Bindings_Encoder
    {
        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_EncoderLoad(IntPtr path, IntPtr callback, int flags);

        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_EncoderLoadMemory(IntPtr data, int size, IntPtr callback, EST_DECODER_FLAGS flags);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_EncoderFree(IntPtr handle);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_EncoderGetInfo(IntPtr handle, in ESTEncoderInfo info);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_EncoderRender(IntPtr handle);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_EncoderSetAttribute(IntPtr handle, in ESTAttributeValue value);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_EncoderGetAttribute(IntPtr handle, in ESTAttributeValue value);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_EncoderGetData(IntPtr handle, IntPtr data, out int size);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_EncoderFlushData(IntPtr handle);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_EncoderGetAvailableDataSize(IntPtr handle, out int size);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_EncoderGetSample(IntPtr handle, IntPtr devHandle, out IntPtr outSample);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_EncoderExportFile(IntPtr handle, EST_FILE_EXPORT type, IntPtr path);
    }
}