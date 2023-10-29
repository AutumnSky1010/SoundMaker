using SoundMaker.Sounds;
using SoundMaker.WaveFile;

namespace SoundMakerTests.UseCaseTests;
internal class WaveCase
{
    public FormatChunk FormatChunk { get; init; }

    public SoundFormat SoundFormat { get; init; }

    public string Path { get; init; } = "";
}
