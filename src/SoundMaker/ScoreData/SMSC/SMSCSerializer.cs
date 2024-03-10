using SoundMaker.Sounds.Score;
using System.Text;

namespace SoundMaker.ScoreData.SMSC;
internal static class SMSCSerializer
{
    public static string Serialize(IEnumerable<ISoundComponent> components)
    {
        var smscBuilder = new StringBuilder();
        foreach (var component in components)
        {
            var line = component switch
            {
                Note note => SerializeNote(note),
                Rest rest => SerializeRest(rest),
                Tie tie => SerializeTie(tie),
                Tuplet tuplet => SerializeTuplet(tuplet),
                _ => "",
            };
            _ = smscBuilder.AppendLine(line);
        }
        return smscBuilder.ToString();
    }

    private static string SerializeNote(Note note)
    {
        var scale = SerializeScale(note.Scale, note.ScaleNumber);
        var length = SerializeLength(note.Length, note.IsDotted);
        return new StringBuilder(scale).Append(',').Append(length).ToString();
    }

    private static string SerializeRest(Rest rest)
    {
        var length = SerializeLength(rest.Length, rest.IsDotted);
        return new StringBuilder("rest").Append(',').Append(length).ToString();
    }

    private static string SerializeTie(Tie tie)
    {
        var builder = new StringBuilder("tie(");
        var scale = SerializeScale(tie.BaseNote.Scale, tie.BaseNote.ScaleNumber);
        var length = SerializeLength(tie.BaseNote.Length, tie.BaseNote.IsDotted);
        _ = builder.Append(scale).Append(',').Append(length);
        foreach (var note in tie.AdditionalNotes)
        {
            length = SerializeLength(note.Length, note.IsDotted);
            _ = builder.Append(',').Append(length);
        }
        _ = builder.Append(')');
        return builder.ToString();
    }

    private static string SerializeTuplet(Tuplet tuplet)
    {
        var builder = new StringBuilder("tup(");
        var length = SerializeLength(tuplet.Length, tuplet.IsDotted);
        _ = builder.Append(length);
        foreach (var component in tuplet.TupletComponents)
        {
            if (component is Tuplet tup)
            {
                var tupStr = SerializeTuplet(tup);
                _ = builder.Append(',').Append(tupStr);
            }
            else if (component is Tie tie)
            {
                var tieStr = SerializeTie(tie);
                _ = builder.Append(',').Append(tieStr);
            }
            else if (component is Rest rest)
            {
                _ = builder.Append(',').Append("rest");
                if (rest.IsDotted)
                {
                    _ = builder.Append('.');
                }
            }
            else if (component is Note note)
            {
                var scale = SerializeScale(note.Scale, note.ScaleNumber);
                _ = builder.Append(',').Append(scale);
                if (note.IsDotted)
                {
                    _ = builder.Append('.');
                }
            }
        }
        _ = builder.Append(')');
        return builder.ToString();
    }

    private static string SerializeScale(Scale scaleType, int scaleNumber)
    {
        var scale = scaleType switch
        {
            Scale.A => "A",
            Scale.ASharp => "A#",
            Scale.B => "B",
            Scale.C => "C",
            Scale.CSharp => "C#",
            Scale.D => "D",
            Scale.DSharp => "D#",
            Scale.E => "E",
            Scale.F => "F",
            Scale.FSharp => "F#",
            Scale.G => "G",
            Scale.GSharp => "G#",
            _ => "A"
        };
        return new StringBuilder(scale).Append(scaleNumber.ToString()).ToString();
    }

    private static string SerializeLength(LengthType lengthType, bool isDotted)
    {
        var length = lengthType switch
        {
            LengthType.Whole => "1",
            LengthType.Half => "2",
            LengthType.Quarter => "4",
            LengthType.Eighth => "8",
            LengthType.Sixteenth => "16",
            LengthType.ThirtySecond => "32",
            LengthType.SixtyFourth => "64",
            _ => "1"
        };
        var builder = new StringBuilder(length);
        if (isDotted)
        {
            _ = builder.Append('.');
        }
        return builder.ToString();
    }
}
