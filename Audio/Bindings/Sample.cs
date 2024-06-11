namespace Estrol.Audio.Bindings
{
    using System.Runtime.InteropServices;

    public partial class Bindings_Sample
    {
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_DeviceInit(int sampleRate, int flags, out IntPtr outHandle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_DeviceFree(IntPtr devHandle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_DeviceInfo(IntPtr devHandle, IntPtr info);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleLoad(IntPtr devHandle, IntPtr path, out IntPtr out_handle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleLoadMemory(IntPtr devHandle, IntPtr data, int size, out IntPtr out_handle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleLoadRawPCM(IntPtr devHandle, IntPtr data, int size, int channels, int sampleRate, out IntPtr out_handle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleFree(IntPtr devHandle, IntPtr handle);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SamplePlay(IntPtr devHandle, IntPtr handle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleStop(IntPtr devHandle, IntPtr handle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleGetStatus(IntPtr devHandle, IntPtr handle, IntPtr status);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleSetAttribute(IntPtr devHandle, IntPtr handle, int attribute, float value);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleGetAttribute(IntPtr devHandle, IntPtr handle, int attribute, out IntPtr value);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleSlideAttribute(IntPtr devHandle, IntPtr handle, int attribute, float value, float time);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleSlideAttributeAsync(IntPtr devHandle, IntPtr handle, int attribute, float value, float time);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial IntPtr EST_GetError();
    }
}