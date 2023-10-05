using SoundMaker.Sounds.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMakerTests.UnitTests.Sounds.Score;
public class TestRest
{
    [Fact(DisplayName = "")]
    public void InitializeTest()
    {
        bool isDotted = false;
        var length = LengthType.Eighth;
        var rest = new Rest(length, isDotted);
        Assert.Equal(length, rest.Length);
        Assert.Equal(isDotted, rest.IsDotted);
    }
}
