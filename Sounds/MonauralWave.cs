namespace SoundMaker.Sounds;
/// <summary>
/// モノラル波形データを表すクラス。
/// </summary>
public class MonauralWave : IWave
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="wave">波形データを表す配列</param>
    public MonauralWave(IReadOnlyCollection<ushort> wave)
    {
        ushort[] argumentIntegers = wave.ToArray();
        this._originalVolumeWave = new ushort[wave.Count];
        this._wave = new ushort[wave.Count];
        Array.Copy(argumentIntegers, this._originalVolumeWave, argumentIntegers.Length);
        Array.Copy(argumentIntegers, this._wave, argumentIntegers.Length);
    }

    private ushort[] _originalVolumeWave { get; set; }

    private ushort[] _wave { get; set; }

    /// <summary>
    /// 波形データの音量(100が基準)
    /// </summary>
    public int Volume { get; private set; }

    /// <summary>
    /// 波形データを表す配列の長さ
    /// </summary>
    public int Length
    {
        get => this._wave.Length;
    }

    /// <summary>
    /// 音量を変更するメソッド
    /// </summary>
    /// <param name="volume">音量(0 ~ 100)</param>
    public void ChangeVolume(int volume)
    {
        volume = volume < 0 ? 0 : volume;
        volume = volume > 100 ? 100 : volume;
        this.Volume = volume;
        for (int i = 0; i < this._wave.Length; i++)
        {
            this._wave[i] = (ushort)(this._originalVolumeWave[i] * volume / 100);
        }
    }

    /// <summary>
    /// 別の波形データを末尾に繋げるメソッド。
    /// </summary>
    /// <param name="wave">モノラルの波形データ</param>
    public void Append(MonauralWave wave)
    {
        this._wave = this._wave.Concat(wave.GetValues()).ToArray();
        this._originalVolumeWave = new ushort[this._wave.Length];
        Array.Copy(this._wave, this._originalVolumeWave, this._wave.Length);
    }

    /// <summary>
    /// 波形データのバイト列を取得する。
    /// </summary>
    /// <param name="bitRate">量子化ビット数</param>
    /// <returns>波形データのバイト列 : byte[]</returns>
    public byte[] GetBytes(BitRateType bitRate)
    {
        if (bitRate == BitRateType.SixteenBit)
        {
            var result = new List<byte>(this._wave.Length * 2);
            foreach (ushort value in this._wave)
            {
                var bytes = BitConverter.GetBytes((short)((value - short.MaxValue) / 2));
                result.Add(bytes[0]);
                result.Add(bytes[1]);
            }
            return result.ToArray();
        }
        else
        {
            var result = new List<byte>(this._wave.Length);
            foreach (ushort value in this._wave)
            {
                result.Add((byte)(value / 256));
            }
            return result.ToArray();
        }
    }

    public ushort[] GetValues() => this._wave;
}
