﻿namespace SoundMaker.Sounds;
/// <summary>
/// monaural wave. モノラル波形データを表すクラス。
/// </summary>
public class MonauralWave : IWave
{
    /// <summary>
    /// constructor. コンストラクタ
    /// </summary>
    /// <param name="wave">the collection of wave data. 波形データを表す配列</param>
    public MonauralWave(IReadOnlyCollection<short> wave)
    {
        var argumentIntegers = wave.ToArray();
        OriginalVolumeWave = new short[wave.Count];
        Wave = new short[wave.Count];
        Array.Copy(argumentIntegers, OriginalVolumeWave, argumentIntegers.Length);
        Array.Copy(argumentIntegers, Wave, argumentIntegers.Length);
    }

    private short[] OriginalVolumeWave { get; set; }

    private short[] Wave { get; set; }

    public int Volume { get; private set; } = 100;

    /// <summary>
    /// change volume this. 音量を変更するメソッド
    /// </summary>
    /// <param name="volume">音量(0 ~ 100)</param>
    public void ChangeVolume(int volume)
    {
        volume = volume < 0 ? 0 : volume;
        volume = volume > 100 ? 100 : volume;
        Volume = volume;
        for (var i = 0; i < Wave.Length; i++)
        {
            Wave[i] = (short)(Wave[i] * (volume / 100d));
        }
    }

    /// <summary>
    /// append deferent MonauralWave. 別の波形データを末尾に繋げるメソッド。
    /// </summary>
    /// <param name="wave">monaural wave.モノラルの波形データ</param>
    public void Append(MonauralWave wave)
    {
        Wave = Wave.Concat(wave.GetWave()).ToArray();
        OriginalVolumeWave = new short[Wave.Length];
        Array.Copy(Wave, OriginalVolumeWave, Wave.Length);
    }

    public byte[] GetBytes(BitRateType bitRate)
    {
        if (bitRate is BitRateType.SixteenBit)
        {
            var result = new List<byte>(Wave.Length * 2);
            foreach (var value in Wave)
            {
                var bytes = BitConverter.GetBytes(value);
                result.Add(bytes[0]);
                result.Add(bytes[1]);
            }
            return result.ToArray();
        }
        else
        {
            var result = new List<byte>(Wave.Length);
            foreach (var value in Wave)
            {
                result.Add((byte)(value / 256 + 128));
            }
            return result.ToArray();
        }
    }

    /// <summary>
    /// get the wave. 音の波形データを取得するメソッド。
    /// </summary>
    /// <returns>the wave. 波形データ : short[]</returns>
    public short[] GetWave()
    {
        var result = new short[Wave.Length];
        Array.Copy(Wave, result, Wave.Length);
        return result;
    }

    public int GetLengthOfBytes(BitRateType bitRate)
    {
        return bitRate is BitRateType.SixteenBit ? Wave.Length * 2 : Wave.Length;
    }
}
