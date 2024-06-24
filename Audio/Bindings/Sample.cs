namespace Estrol.Audio.Bindings
{
    using System.Runtime.InteropServices;

    public partial class Bindings_Sample
    {
        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_SampleLoad(IntPtr path);

        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_SampleLoadMemory(IntPtr data, int size);

        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_SampleFree(IntPtr handle);

        [LibraryImport("EstAudio")]
        public static partial IntPtr EST_SampleFromEncoder(IntPtr handle);
    }
}