namespace Estrol.EstAudio;
using Estrol.Audio;

public class LocalTest
{
    [Fact(DisplayName = "Sample Test")]
    public void PlaybackTest()
    {
        AudioManager.Init();

        var sample = AudioManager.LoadSample("F:\\test.wav");
        Assert.True(sample != null);

        sample.Play();

        Thread.Sleep(1000);

        sample.Stop();

        sample.Destroy();

        AudioManager.Destroy();
    }

    [Fact(DisplayName = "Encoder Test")]
    public void EncoderTest()
    {
        AudioManager.Init();

        var encoder = AudioManager.LoadEncoder("F:\\test.wav");
        Assert.True(encoder != null);

        encoder.Rate = 1.5f;
        encoder.Volume = 0.5f;

        var sample = AudioManager.LoadSample(encoder);
        Assert.True(sample != null);

        sample.Play();

        Thread.Sleep(1000);

        sample.Stop();

        sample.Destroy();
        encoder.Destroy();

        AudioManager.Destroy();
    }
}