using SoundMaker.Sounds.Score;

namespace SoundMaker.Sounds.Score.Parsers;
public interface IScoreParser
{
    IEnumerable<ISoundComponent> Parse();
}
