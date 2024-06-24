namespace Estrol.EstAudio;
using Estrol.Audio;
using Xunit.Abstractions;

// public class PlaybackTest(ITestOutputHelper output)
// {
//     private readonly ITestOutputHelper output = output;

//     [Fact]
//     public void Test()
//     {
//         output.WriteLine("[Test] Initializing AudioManager");
//         AudioManager.Init(44100, 0);

//         output.WriteLine("[Test] Loading channel");
//         var channel = AudioManager.ChannelLoad("F:\\test.wav");

//         output.WriteLine("[Test] Playing channel");
//         channel.Play(false);

//         output.WriteLine("[Test] Waiting for 1 second");
//         Thread.Sleep(1000);

//         output.WriteLine("[Test] Stopping channel and freeing resources");
//         channel.Stop();
//         channel.Free();

//         output.WriteLine("[Test] Freeing AudioManager resources");
//         AudioManager.Free();
//     }
// }

public class EncoderTest(ITestOutputHelper output)
{
    private readonly ITestOutputHelper output = output;

    [Fact]
    public void Test()
    {
        output.WriteLine("[Test] Initializing AudioManager");
        AudioManager.Init(44100, 0);

        var sample = AudioManager.SampleLoad("F:\\test.wav");

        List<Channel> channels = [];
        channels.Add(sample.GetChannel());
        channels.Add(sample.GetChannel());

        output.WriteLine("[Test] Playing channels");
        channels[0].Play();

        Thread.Sleep(1000);

        channels[1].Play();

        Thread.Sleep(1000);

        output.WriteLine("[Test] Waiting for 1 second");

        channels.ForEach(channel => channel.Stop());
        channels.ForEach(channel => channel.Free());

        output.WriteLine("[Test] Freeing AudioManager resources");
        AudioManager.Free();
    }
}