﻿using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.Score;
/// <summary>
/// the note. 音符を表すクラス
/// </summary>
public class Note : BasicSoundComponentBase
{
    /// <summary>
    /// constructor コンストラクタ
    /// </summary>
    /// <param name="scale">scale of the note.音の高さ</param>
    /// <param name="scaleNumber">sound height number. (C"4" is middle C.)音の高さの番号（Cの「4」が真ん中のド）</param>
    /// <param name="length">length (ex. "quarter" note) 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">is note/rest dotted. 付点かを表す論理型</param>
    /// <exception cref="ArgumentException">Scale and scale number must be only the range of sound that the piano can produce.</exception>
    public Note(Scale scale, int scaleNumber, LengthType length, bool isDotted = false)
        : base(length, isDotted)
    {
        CheckArgument(scale, scaleNumber);
        Hertz += ScaleHertzDictionary.GetHertz(scale, scaleNumber);
        Scale = scale;
        ScaleNumber = scaleNumber;
    }

    /// <summary>
    /// easiness constructor(use case: construct Tie).Scale is "A4". 簡易コンストラクタ(使用場面: タイの初期化)。音の高さは"A4"
    /// </summary>
    /// <param name="length">length (ex. "quarter" note) 長さ（音楽的な、「四分」音符、「全」休符のような長さを表す。）</param>
    /// <param name="isDotted">is note/rest dotted. 付点かを表す論理型</param>
    public Note(LengthType length, bool isDotted = false) : base(length, isDotted)
    {
        var scale = Scale.A;
        var scaleNumber = 4;
        Hertz += ScaleHertzDictionary.GetHertz(scale, scaleNumber);
        Scale = scale;
        ScaleNumber = scaleNumber;
    }

    /// <summary>
    /// scale of the note. 音の高さ
    /// </summary>
    public Scale Scale { get; }

    /// <summary>
    /// sound height number. (C"4" is middle C.)音の高さの番号（Cの「4」が真ん中のド）
    /// </summary>
    public int ScaleNumber { get; }

    /// <summary>
    /// hertz of the sound. 音の周波数
    /// </summary>
    public double Hertz { get; } = 0d;

    private int _volume = 100;
    /// <summary>
    /// volume of the sound.(0 ~ 100) 音の大きさ(0 ~ 100の間)
    /// </summary>
    public int Volume
    {
        get => _volume;
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            else if (value > 100)
            {
                value = 100;
            }
            _volume = value;
        }
    }

    private static void CheckArgument(Scale scale, int scaleNumber)
    {
        var message = "'scale' and 'scaleNumber' must be only the range of sound that the piano can produce.";
        if (!ScaleHertzDictionary.IsValidScale(scale, scaleNumber))
        {
            throw new ArgumentException(message);
        }
    }

    public override short[] GenerateWave(SoundFormat format, int tempo, int length, WaveTypeBase waveType)
    {
        return waveType.GenerateWave(format, length, Volume, Hertz);
    }

    public override short[] GenerateWave(SoundFormat format, int tempo, WaveTypeBase waveType)
    {
        var length = GetWaveArrayLength(format, tempo);
        return GenerateWave(format, tempo, length, waveType);
    }

    public override Note Clone()
    {
        return new(Scale, ScaleNumber, Length, IsDotted)
        {
            Volume = Volume,
        };
    }
}
