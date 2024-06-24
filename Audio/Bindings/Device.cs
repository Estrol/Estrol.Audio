namespace Estrol.Audio.Bindings
{
    using System.Runtime.InteropServices;

    public partial class Device_Bindings
    {
        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_DeviceInit(int sampleRate, int flags);

        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_DeviceFree(IntPtr devHandle);

        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_DeviceInfo(IntPtr devHandle, IntPtr info);

        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_ErrorGetMessage();

        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_ErrorTranslateMessage(int code);
    }
}