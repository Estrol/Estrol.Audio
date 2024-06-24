namespace Estrol.Audio.Bindings
{
    using System.Runtime.InteropServices;

    public partial class Bindings_Channel
    {
        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_ChannelLoad(IntPtr devHandle, IntPtr path);

        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_ChannelLoadMemory(IntPtr devHandle, IntPtr data, int size);

        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_SampleGetChannel(IntPtr devHandle, IntPtr handle);

        [LibraryImport("EstAudio")]
        public static partial int EST_SampleGetChannels(IntPtr devHandle, IntPtr handle, int howMany, IntPtr ptrToArray);

        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_EncoderGetChannel(IntPtr devHandle, IntPtr handle);

        [LibraryImport("EstAudio")]
        public static partial int EST_EncoderGetChannels(IntPtr devHandle, IntPtr handle, int howMany, IntPtr ptrToArray);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_ChannelFree(IntPtr handle);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_ChannelPlay(IntPtr handle, int restart);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_ChannelStop(IntPtr handle);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_ChannelPause(IntPtr handle);

        [LibraryImport("EstAudio")]
        public static partial EST_BOOL EST_ChannelIsPlaying(IntPtr handle);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_ChannelSetAttribute(IntPtr handle, in ESTAttributeValue value);

        [LibraryImport("EstAudio")]
        public static partial EST_RESULT EST_ChannelGetAttribute(IntPtr handle, in ESTAttributeValue value);
    }
}