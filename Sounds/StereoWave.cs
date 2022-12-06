namespace SoundMaker.Sounds;
/// <summary>
/// stereo wave. ステレオ波形データのクラス
/// </summary>
public class StereoWave : IWave
{
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="rightWave">the wave of right. 右の波形データ</param>
    /// <param name="leftWave">the wave of left. 左の波形データ</param>
    public StereoWave(IReadOnlyCollection<ushort> rightWave, IReadOnlyCollection<ushort> leftWave)
    {
        ushort[] rightArgument = rightWave.ToArray();
        ushort[] leftArgument = leftWave.ToArray();

        this.RightOriginalVolumeWave = new ushort[rightWave.Count];
        this.LeftOriginalVolumeWave = new ushort[leftWave.Count];
        Array.Copy(rightArgument, this.RightOriginalVolumeWave, rightArgument.Length);
        Array.Copy(leftArgument, this.LeftOriginalVolumeWave, leftArgument.Length);

        this.RightWave = new ushort[rightWave.Count];
        this.LeftWave = new ushort[leftWave.Count];
        Array.Copy(rightArgument, this.RightWave, rightArgument.Length);
        Array.Copy(leftArgument, this.LeftWave, leftArgument.Length);
    }

    private ushort[] RightOriginalVolumeWave { get; set; }

    private ushort[] LeftOriginalVolumeWave { get; set; }

    private ushort[] RightWave { get; set; }

    private ushort[] LeftWave { get; set; }

    /// <summary>
    /// volume of the wave. 波形データの音量
    /// </summary>
    public int Volume { get; private set; } = 100;

    [Obsolete("if you want to get length of bytes, call GetLengthOfBytes()")]
    public int Length
    {
        get => this.GetMaxAndMinWaveLength().Max;
    }

    /// <summary>
    /// change the volume this. 音量を変更するメソッド。
    /// </summary>
    /// <param name="volume">volume(0 ~ 100) 音量(0 ~ 100)</param>
    /// <param name="channelType">Channel to change the sound. 左右・両方の中から音量を変更するものを選ぶ</param>
    public void ChangeVolume(int volume, SoundDirectionType channelType)
    {
        volume = volume < 0 ? 0 : volume;
        volume = volume > 100 ? 100 : volume;
        switch (channelType)
        {
            case SoundDirectionType.Left:
                for (int i = 0; i < this.LeftWave.Length; i++)
                {
                    this.LeftWave[i] = (ushort)(this.LeftOriginalVolumeWave[i] * (volume / 100d));
                }
                break;
            case SoundDirectionType.Right:
                for (int i = 0; i < this.RightWave.Length; i++)
                {
                    this.RightWave[i] = (ushort)(this.RightOriginalVolumeWave[i] * (volume / 100d));
                }
                break;
            default:
                var maxAndMinLength = this.GetMaxAndMinWaveLength();
                for (int i = 0; i < maxAndMinLength.Min; i++)
                {
                    this.RightWave[i] = (ushort)(this.RightOriginalVolumeWave[i] * (volume / 100d));
                    this.LeftWave[i] = (ushort)(this.LeftOriginalVolumeWave[i] * (volume / 100d));
                }

                // 残りを処理する。
                ushort[] wave = this.RightWave.Length == maxAndMinLength.Max ? this.RightWave : this.LeftWave;
                ushort[] originalWave = this.RightWave.Length == maxAndMinLength.Max ?
                    this.RightOriginalVolumeWave : this.LeftOriginalVolumeWave;
                for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
                {
                    wave[i] = (ushort)(originalWave[i] * (volume / 100d));
                }
                break;
        }
    }

    /// <summary>
    /// get byte array of the wave. 波形データのバイト列を取得するメソッド。
    /// </summary>
    /// <param name="bitRate">bitrate of the sound. 量子化ビット数</param>
    /// <returns>byte array of wave data. 波形データのバイト列 : byte[]</returns>
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
            resultWave.Add((byte)(this.LeftWave[i] / 256));
            resultWave.Add((byte)(this.RightWave[i] / 256));
        }
        // 追加しきれていない波形データを追加する
        if (this.LeftWave.Length == maxAndMinLength.Max)
        {
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
                resultWave.Add((byte)(this.LeftWave[i] / 256));
                resultWave.Add(0);
            }
        }
        else
        {
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
                resultWave.Add(0);
                resultWave.Add((byte)(this.RightWave[i] / 256));
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
            short left = this.LeftWave[i] == ushort.MaxValue ? 
                (short)(this.LeftWave[i] - short.MaxValue - 1) : (short)(this.LeftWave[i] - short.MaxValue);
            short right = this.RightWave[i] == ushort.MaxValue ?
                (short)(this.RightWave[i] - short.MaxValue - 1) : (short)(this.RightWave[i] - short.MaxValue);
            byte[] leftBytes = BitConverter.GetBytes(left);
            byte[] rightBytes = BitConverter.GetBytes(right);
            // Point : ステレオの波は左右左右左右左右・・・・・・左右
            resultWave.Add(leftBytes[0]);
            resultWave.Add(leftBytes[1]);
            resultWave.Add(rightBytes[0]);
            resultWave.Add(rightBytes[1]);
        }
        // 追加しきれていない波形データを追加する
        if (this.LeftWave.Length == maxAndMinLength.Max)
        {
            for (int i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
                short left = this.LeftWave[i] == ushort.MaxValue ?
                (short)(this.LeftWave[i] - short.MaxValue - 1) : (short)(this.LeftWave[i] - short.MaxValue);
                byte[] leftBytes = BitConverter.GetBytes(left);
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
                short right = this.RightWave[i] == ushort.MaxValue ?
                    (short)(this.RightWave[i] - short.MaxValue - 1) : (short)(this.RightWave[i] - short.MaxValue);
                byte[] rightBytes = BitConverter.GetBytes(right);
                resultWave.Add(0);
                resultWave.Add(0);
                resultWave.Add(rightBytes[0]);
                resultWave.Add(rightBytes[1]);
            }
        }
        return resultWave;
    }

    /// <summary>
    /// get the wave on the right. 右側のチャンネルの音の波形データを取得するメソッド。
    /// </summary>
    /// <returns>the wave on the right. 右側のチャンネルの音の波形データ : unsigned short[]</returns>
    public ushort[] GetRightWave()
    {
        var resultushorts = new ushort[this.RightWave.Length];
        Array.Copy(this.RightWave, resultushorts, this.RightWave.Length);
        return resultushorts;
    }

    /// <summary>
    /// get the wave on the left. 左側のチャンネルの音の波形データを取得するメソッド。
    /// </summary>
    /// <returns>the wave on the left. 左側のチャンネルの音の波形データ : unsigned short[]</returns>
    public ushort[] GetLeftWave()
    {
        var resultushorts = new ushort[this.LeftWave.Length];
        Array.Copy(this.LeftWave, resultushorts, this.LeftWave.Length);
        return resultushorts;
    }

    /// <summary>
    /// append wave to this. 別のステレオ波形を末尾に繋げるメソッド。
    /// </summary>
    /// <param name="wave">wave</param>
    public void Append(StereoWave wave)
    {
        this.RightWave = this.RightWave.Concat(wave.GetRightWave()).ToArray();
        this.RightOriginalVolumeWave = new ushort[this.RightWave.Length];
        Array.Copy(this.RightWave, this.RightOriginalVolumeWave, this.RightWave.Length);

        this.LeftWave = this.LeftWave.Concat(wave.GetLeftWave()).ToArray();
        this.LeftOriginalVolumeWave = new ushort[this.LeftWave.Length];
        Array.Copy(this.LeftWave, this.LeftOriginalVolumeWave, this.LeftWave.Length);
    }

    /// <summary>
    /// 左右どちらの配列が長いかを調べ、返却するメソッド。
    /// </summary>
    /// <returns>配列の長さを比較し、最大・最小を格納した構造体 : MaxAndMin</returns>
    private MaxAndMin GetMaxAndMinWaveLength()
    {
        if (this.RightWave.Length > this.LeftWave.Length)
        {
            return new MaxAndMin(this.RightWave.Length, this.LeftWave.Length);
        }
        return new MaxAndMin(this.LeftWave.Length, this.RightWave.Length);
    }

    public int GetLengthOfBytes(BitRateType bitRate)
    {
        var maxAndMinLength = this.GetMaxAndMinWaveLength();
        if (bitRate is BitRateType.SixteenBit)
        {
            return maxAndMinLength.Max * 4;
        }
        else
        {
            return maxAndMinLength.Max * 2;
        }
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
