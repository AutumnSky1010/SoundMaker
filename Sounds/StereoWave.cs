namespace SoundMaker.Sounds;
/// <summary>
/// ステレオ波形データのクラス
/// </summary>
public class StereoWave : IWave
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="rightWave">右の波形データ</param>
    /// <param name="leftWave">左の波形データ</param>
    public StereoWave(IReadOnlyCollection<ushort> rightWave, IReadOnlyCollection<ushort> leftWave)
    {
        ushort[] rightArgument = rightWave.ToArray();
        ushort[] leftArgument = leftWave.ToArray();

        this._rightOriginalVolumeWave = new ushort[rightWave.Count];
        this._leftOriginalVolumeWave = new ushort[leftWave.Count];
        Array.Copy(rightArgument, this._rightOriginalVolumeWave, rightArgument.Length);
        Array.Copy(leftArgument, this._leftOriginalVolumeWave, leftArgument.Length);

        this._rightWave = new ushort[rightWave.Count];
        this._leftWave = new ushort[leftWave.Count];
        Array.Copy(rightArgument, this._rightWave, rightArgument.Length);
        Array.Copy(leftArgument, this._leftWave, leftArgument.Length);
    }

    private ushort[] _rightOriginalVolumeWave { get; set; }

    private ushort[] _leftOriginalVolumeWave { get; set; }

    private ushort[] _rightWave { get; set; }

    private ushort[] _leftWave { get; set; }

    /// <summary>
    /// 波形データの音量
    /// </summary>
    public int Volume { get; private set; }

    /// <summary>
    /// 波形データを表すの配列の長さ。GetBytes()メソッドで得られるバイト列の長さとは異なる
    /// </summary>
    public int Length
    {
        get => this.GetMaxAndMinWaveLength().Max;
    }

    /// <summary>
    /// 音量を変更するメソッド。
    /// </summary>
    /// <param name="volume">音量(0 ~ 100)</param>
    /// <param name="channelType">左右・両方の中から音量を変更するものを選ぶ</param>
    public void ChangeVolume(int volume, SoundDirectionType channelType)
    {
        volume = volume < 0 ? 0 : volume;
        volume = volume > 100 ? 100 : volume;
        switch (channelType)
        {
            case SoundDirectionType.Left:
                for (int i = 0; i < this._leftWave.Length; i++)
                {
                    this._leftWave[i] = (ushort)(this._leftOriginalVolumeWave[i] * volume / 100);
                }
                break;
            case SoundDirectionType.Right:
                for (int i = 0; i < this._rightWave.Length; i++)
                {
                    this._rightWave[i] = (ushort)(this._rightOriginalVolumeWave[i] * volume / 100);
                }
                break;
            default:
                var maxAndMinLength = this.GetMaxAndMinWaveLength();
                for (int i = 0; i < maxAndMinLength.Min; i++)
                {
                    this._rightWave[i] = (ushort)(this._rightOriginalVolumeWave[i] * volume / 100);
                    this._leftWave[i] = (ushort)(this._leftOriginalVolumeWave[i] * volume / 100);
                }

                // 残りを処理する。
                ushort[] wave = this._rightWave.Length == maxAndMinLength.Max ? this._rightWave : this._leftWave;
                ushort[] originalWave = this._rightWave.Length == maxAndMinLength.Max ?
                    this._rightOriginalVolumeWave : this._leftOriginalVolumeWave;
                for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
                {
                    wave[i] = (ushort)(originalWave[i] * volume / 100);
                }
                break;
        }
    }

    /// <summary>
    /// 波形データのバイト列を取得するメソッド。
    /// </summary>
    /// <param name="bitRate">量子化ビット数</param>
    /// <returns>波形データのバイト列 : byte[]</returns>
    public byte[] GetBytes(BitRateType bitRate)
    {
        if (bitRate == BitRateType.SixteenBit)
        {
            return this.Get16BitBytes().ToArray();
        }
        else
        {
            return this.Get8BitBytes().ToArray();
        }
    }

    /// <summary>
    /// 量子化ビット数が8bitの場合の波形データのバイト列を取得する。
    /// </summary>
    /// <returns>量子化ビット数が8bitの場合の波形データのバイト列 : byte[]</returns>
    private List<byte> Get8BitBytes()
    {
        var maxAndMinLength = this.GetMaxAndMinWaveLength();
        var resultWave = new List<byte>(maxAndMinLength.Max * 2);
        for (int i = 0; i < maxAndMinLength.Min; i++)
        {
            // Point : ステレオの波は左右左右左右左右・・・・・・左右
            resultWave.Add((byte)(this._leftWave[i] / 256));
            resultWave.Add((byte)(this._rightWave[i] / 256));
        }
        // 追加しきれていない波形データを追加する
        if (this._leftWave.Length == maxAndMinLength.Max)
        {
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
                resultWave.Add((byte)(this._leftWave[i] / 256));
                resultWave.Add(0);
            }
        }
        else
        {
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
                resultWave.Add(0);
                resultWave.Add((byte)(this._rightWave[i] / 256));
            }
        }
        return resultWave;
    }

    /// <summary>
    /// 量子化ビット数が16bitの場合の波形データのバイト列を取得する。
    /// </summary>
    /// <returns>量子化ビット数が16bitの場合の波形データのバイト列 : byte[]</returns>
    private List<byte> Get16BitBytes()
    {
        var maxAndMinLength = this.GetMaxAndMinWaveLength();
        var resultWave = new List<byte>(maxAndMinLength.Max * 4);
        for (int i = 0; i < maxAndMinLength.Min; i++)
        {
            byte[] leftBytes = BitConverter.GetBytes((short)((this._leftWave[i] - short.MaxValue) / 2));
            byte[] rightBytes = BitConverter.GetBytes((short)((this._rightWave[i] - short.MaxValue) / 2));
            // Point : ステレオの波は左右左右左右左右・・・・・・左右
            resultWave.Add(leftBytes[0]);
            resultWave.Add(leftBytes[1]);
            resultWave.Add(rightBytes[0]);
            resultWave.Add(rightBytes[1]);
        }
        // 追加しきれていない波形データを追加する
        if (this._leftWave.Length == maxAndMinLength.Max)
        {
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
                byte[] leftBytes = BitConverter.GetBytes((short)((this._leftWave[i] - short.MaxValue) / 2));
                resultWave.Add(leftBytes[0]);
                resultWave.Add(leftBytes[1]);
                resultWave.Add(0);
                resultWave.Add(0);
            }
        }
        else
        {
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i += 2)
            {
                byte[] rightBytes = BitConverter.GetBytes((short)((this._rightWave[i] - short.MaxValue) / 2));
                resultWave.Add(0);
                resultWave.Add(0);
                resultWave.Add(rightBytes[0]);
                resultWave.Add(rightBytes[1]);
            }
        }
        return resultWave;
    }

    /// <summary>
    /// 右側のチャンネルの音の波形データを取得するメソッド。
    /// </summary>
    /// <returns>右側のチャンネルの音の波形データ : unsigned short[]</returns>
    public ushort[] GetRightWave()
    {
        var resultushorts = new ushort[this._rightWave.Length];
        Array.Copy(this._rightWave, resultushorts, this._rightWave.Length);
        return resultushorts;
    }

    /// <summary>
    /// 左側のチャンネルの音の波形データを取得するメソッド。
    /// </summary>
    /// <returns>左側のチャンネルの音の波形データ : unsigned short[]</returns>
    public ushort[] GetLeftWave()
    {
        var resultushorts = new ushort[this._leftWave.Length];
        Array.Copy(this._leftWave, resultushorts, this._leftWave.Length);
        return resultushorts;
    }

    /// <summary>
    /// 別のステレオ波形を末尾に繋げるメソッド。
    /// </summary>
    /// <param name="wave"></param>
    public void Append(StereoWave wave)
    {
        this._rightWave = this._rightWave.Concat(wave.GetRightWave()).ToArray();
        this._rightOriginalVolumeWave = new ushort[this._rightWave.Length];
        Array.Copy(this._rightWave, this._rightOriginalVolumeWave, this._rightWave.Length);

        this._leftWave = this._leftWave.Concat(wave.GetLeftWave()).ToArray();
        this._leftOriginalVolumeWave = new ushort[this._leftWave.Length];
        Array.Copy(this._leftWave, this._leftOriginalVolumeWave, this._leftWave.Length);
    }

    /// <summary>
    /// 左右どちらの配列が長いかを調べ、返却するメソッド。
    /// </summary>
    /// <returns>配列の長さを比較し、最大・最小を格納した構造体 : MaxAndMin</returns>
    private MaxAndMin GetMaxAndMinWaveLength()
    {
        if (this._rightWave.Length > this._leftWave.Length)
        {
            return new MaxAndMin(this._rightWave.Length, this._leftWave.Length);
        }
        return new MaxAndMin(this._leftWave.Length, this._rightWave.Length);
    }

    private struct MaxAndMin
    {
        public MaxAndMin(int max, int min)
        {
            this.Min = min;
            this.Max = max;
        }

        public int Max { get; }

        public int Min { get; }
    }
}
