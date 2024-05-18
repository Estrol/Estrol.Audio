namespace Estrol.Audio.Bindings
{
    using System.Runtime.InteropServices;

    partial class Bindings_Encoder
    {
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderLoad(IntPtr path, IntPtr callback, int flags, out IntPtr out_handle);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderFree(IntPtr handle);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderLoadMemory(IntPtr data, int size, IntPtr callback, EST_DECODER_FLAGS flags, IntPtr handle);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderGetInfo(IntPtr handle, IntPtr info);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderRender(IntPtr handle);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderSetAttribute(IntPtr handle, int attribute, float value);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderGetAttribute(IntPtr handle, int attribute, out float value);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderGetData(IntPtr handle, IntPtr data, out int size);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderFlushData(IntPtr handle);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderGetAvailableDataSize(IntPtr handle, out int size);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderGetSample(IntPtr handle, out IntPtr outSample);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_EncoderExportFile(IntPtr handle, EST_FILE_EXPORT type, IntPtr path);
    }
}