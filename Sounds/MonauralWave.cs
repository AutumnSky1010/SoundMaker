namespace SoundMaker.Sounds;
/// <summary>
/// monaural wave. モノラル波形データを表すクラス。
/// </summary>
public class MonauralWave : IWave
{
    /// <summary>
    /// constructor. コンストラクタ
    /// </summary>
    /// <param name="wave">the collection of wave data. 波形データを表す配列</param>
    public MonauralWave(IReadOnlyCollection<ushort> wave)
    {
        ushort[] argumentIntegers = wave.ToArray();
        this.OriginalVolumeWave = new ushort[wave.Count];
        this.Wave = new ushort[wave.Count];
        Array.Copy(argumentIntegers, this.OriginalVolumeWave, argumentIntegers.Length);
        Array.Copy(argumentIntegers, this.Wave, argumentIntegers.Length);
    }

    private ushort[] OriginalVolumeWave { get; set; }

    private ushort[] Wave { get; set; }

    public int Volume { get; private set; } = 100;

    [Obsolete("if you want to get length of bytes, call GetLengthOfBytes()")]
    public int Length
    {
        get => this.Wave.Length;
    }

    /// <summary>
    /// change volume this. 音量を変更するメソッド
    /// </summary>
    /// <param name="volume">音量(0 ~ 100)</param>
    public void ChangeVolume(int volume)
    {
        volume = volume < 0 ? 0 : volume;
        volume = volume > 100 ? 100 : volume;
        this.Volume = volume;
        for (int i = 0; i < this.Wave.Length; i++)
        {
            this.Wave[i] = (ushort)(this.Wave[i] * (volume / 100d));
        }
    }

    /// <summary>
    /// append deferent MonauralWave. 別の波形データを末尾に繋げるメソッド。
    /// </summary>
    /// <param name="wave">monaural wave.モノラルの波形データ</param>
    public void Append(MonauralWave wave)
    {
        this.Wave = this.Wave.Concat(wave.GetWave()).ToArray();
        this.OriginalVolumeWave = new ushort[this.Wave.Length];
        Array.Copy(this.Wave, this.OriginalVolumeWave, this.Wave.Length);
    }

    public byte[] GetBytes(BitRateType bitRate)
    {
        if (bitRate is BitRateType.SixteenBit)
        {
            var result = new List<byte>(this.Wave.Length * 2);
            foreach (ushort value in this.Wave)
            {
                var bytes = BitConverter.GetBytes((short)((value - short.MaxValue) / 2));
                result.Add(bytes[0]);
                result.Add(bytes[1]);
            }
            return result.ToArray();
        }
        else
        {
            var result = new List<byte>(this.Wave.Length);
            foreach (ushort value in this.Wave)
            {
                result.Add((byte)(value / 256));
            }
            return result.ToArray();
        }
    }

    /// <summary>
    /// get the wave. 音の波形データを取得するメソッド。
    /// </summary>
    /// <returns>the wave. 波形データ : unsigned short[]</returns>
    public ushort[] GetWave()
    {
        var result = new ushort[this.Wave.Length];
        Array.Copy(this.Wave, result, this.Wave.Length);
        return result;
    }

    public int GetLengthOfBytes(BitRateType bitRate)
    {
        if (bitRate is BitRateType.SixteenBit)
        {
            return this.Wave.Length * 2;
        }
        else
        {
            return this.Wave.Length;
        }
    }
}
