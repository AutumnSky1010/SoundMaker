using SoundMaker.ScoreData.SMSC;
using SoundMaker.Sounds.Score;

namespace SoundMakerTests.UnitTests.ScoreData.SMSC;
public class SMSCSerializerTest
{
    [Fact(DisplayName = "SoundComponentが正しくSMSCデータになるか")]
    public void TestSerialize()
    {
        var note = new Note(Scale.C, 4, LengthType.Whole, true);

        var expected = @"C4,1.
rest,4
tie(C4,1.,1.,1.)
tup(2,C4.,C4.,C4.)
";

        var components = new List<ISoundComponent>()
        {
            note,
            new Rest(LengthType.Quarter, false),
            new Tie(note, new List<Note>()
            {
                note, note
            }),
            new Tuplet(new List<ISoundComponent>() {note, note, note}, LengthType.Half, false)
        };

        var actual = SMSCSerializer.Serialize(components);

        Assert.Equal(expected, actual);
    }
}
