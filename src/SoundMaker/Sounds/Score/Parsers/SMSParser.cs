using System.Text.RegularExpressions;

namespace SoundMaker.Sounds.Score.Parsers;
public class SMSParser : IScoreParser
{
    public SMSParser(string data) 
    {
        this.Data = data;
    }

    private string Data { get; }

    public IEnumerable<ISoundComponent> Parse()
    {
        throw new NotImplementedException();
    }
}
