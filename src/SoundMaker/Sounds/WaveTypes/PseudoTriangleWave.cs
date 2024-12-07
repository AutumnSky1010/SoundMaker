namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// the pseudo triangle wave. 疑似三角波
/// </summary>
public class PseudoTriangleWave : WaveTypeBase
{
    private static readonly short[] _leftHeights = new short[]
    {
        0, -4096, -8192, -12288, -16384, -20480, -24576, -28672, -28672, -24576, -20480, -16384, -12288, -8192, -4096, 0,
    };
    private static readonly short[] _rightHeights = new short[]
    {
        4095, 8191, 12287, 16383, 20479, 24575, 28671, short.MaxValue, short.MaxValue, 28671, 24575, 20479, 16383, 12287, 8191, 4095
    };

    public override short[] GenerateWave(SoundFormat format, int length, int volume, double hertz)
    {
        CheckGenerateWaveArgs(length, volume, hertz);

        // △の波形を作るための繰り返し回数
        var triangleWidth = (int)((int)format.SamplingFrequency / hertz);
        // 疑似三角波に出来ない場合は普通の三角波を生成する。
        if (triangleWidth <= 64)
        {
            return new TriangleWave().GenerateWave(format, length, volume, hertz);
        }

        var result = new List<short>(length);
        var unitWave = GenerateUnitWaveInternal(format, volume, hertz);
        for (var i = 0; i < length / unitWave.Length; i++)
        {
            result.AddRange(unitWave);
        }
        for (var i = 0; i < length % unitWave.Length; i++)
        {
            result.Add(0);
        }
        return result.ToArray();
    }

    internal override WaveTypeBase Clone()
    {
        return new PseudoTriangleWave();
    }

    /// <summary>
    /// Generates one cycle of a sound waveform at the specified frequency.<br/>
    /// 指定した周波数の音声波形1周期分を生成する。
    /// </summary>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="volume">Volume <br/>音量（0 ~ 100）</param>
    /// <param name="hertz">Hertz of the sound. <br/>音の周波数</param>
    /// <returns>The array of wave data.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Hertz must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Volume must be below 100 and above 0.</exception>
    public short[] GenerateUnitWave(SoundFormat format, int volume, double hertz)
    {
        CheckGenerateUnitWaveArgs(volume, hertz);
        return GenerateUnitWaveInternal(format, volume, hertz);
    }

    private static short[] GenerateUnitWaveInternal(SoundFormat format, int volume, double hertz)
    {
        var repeatNumber = (int)((int)format.SamplingFrequency / hertz);

        var result = new short[repeatNumber];
        var steps = 32;
        // 音量の倍率(1.00 ~ 0.00)
        var volumeMagnification = volume / 100d;
        // 階段の幅
        var stairsWidth = repeatNumber / steps;
        // 幅の余り
        var r = repeatNumber % steps;

        int position = 0;
        for (int i = 0; i < _leftHeights.Length; i++)
        {
            var leftHeight = (short)(_leftHeights[i] * volumeMagnification);
            var rightHeight = (short)(_rightHeights[i] * volumeMagnification);

            for (int j = 0; j < stairsWidth; j++)
            {
                result[position] = leftHeight;
                result[result.Length - 1 - position] = rightHeight;
                position++;
            }
            if (r > 0)
            {
                result[position] = leftHeight;
                result[result.Length - 1 - position] = rightHeight;
                r -= 2;
                position++;
            }
        }

        return result;
    }
}
