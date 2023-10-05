using SoundMaker.Sounds;
using SoundMaker.WaveFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMakerTests.UseCaseTests;
internal class WaveCase
{
    public FormatChunk FormatChunk { get; init; }

    public SoundFormat SoundFormat { get; init; }

    public string Path { get; init; } = "";
}
