namespace Estrol.Audio.Bindings
{
    using System.Runtime.InteropServices;

    partial class Bindings_Sample
    {
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_DeviceInit(int sampleRate, int flags);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_DeviceFree();
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_DeviceInfo(IntPtr info);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleLoad(IntPtr path, out IntPtr out_handle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleLoadMemory(IntPtr data, int size, out IntPtr out_handle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleLoadRawPCM(IntPtr data, int size, int channels, int sampleRate, out IntPtr out_handle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleFree(IntPtr handle);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SamplePlay(IntPtr handle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleStop(IntPtr handle);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleGetStatus(IntPtr handle, IntPtr status);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleSetAttribute(IntPtr handle, int attribute, float value);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleGetAttribute(IntPtr handle, int attribute, out IntPtr value);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleSlideAttribute(IntPtr handle, int attribute, float value, float time);
        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial EST_RESULT EST_SampleSlideAttributeAsync(IntPtr handle, int attribute, float value, float time);

        [LibraryImport("EstAudio")]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial IntPtr EST_GetError();
    }
}