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
    public StereoWave(IReadOnlyCollection<short> rightWave, IReadOnlyCollection<short> leftWave)
    {
        var rightArgument = rightWave.ToArray();
        var leftArgument = leftWave.ToArray();

        RightOriginalVolumeWave = new short[rightWave.Count];
        LeftOriginalVolumeWave = new short[leftWave.Count];
        Array.Copy(rightArgument, RightOriginalVolumeWave, rightArgument.Length);
        Array.Copy(leftArgument, LeftOriginalVolumeWave, leftArgument.Length);

        RightWave = new short[rightWave.Count];
        LeftWave = new short[leftWave.Count];
        Array.Copy(rightArgument, RightWave, rightArgument.Length);
        Array.Copy(leftArgument, LeftWave, leftArgument.Length);
    }

    private short[] RightOriginalVolumeWave { get; set; }

    private short[] LeftOriginalVolumeWave { get; set; }

    private short[] RightWave { get; set; }

    private short[] LeftWave { get; set; }

    /// <summary>
    /// volume of the wave. 波形データの音量
    /// </summary>
    [Obsolete]
    public int Volume { get; private set; } = 100;

    public int RightVolume { get; private set; } = 100;

    public int LeftVolume { get; private set; } = 100;

    [Obsolete("if you want to get length of bytes, call GetLengthOfBytes()")]
    public int Length => GetMaxAndMinWaveLength().Max;

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
                for (var i = 0; i < LeftWave.Length; i++)
                {
                    LeftWave[i] = (short)(LeftOriginalVolumeWave[i] * (volume / 100d));
                }
                LeftVolume = volume;
                break;
            case SoundDirectionType.Right:
                for (var i = 0; i < RightWave.Length; i++)
                {
                    RightWave[i] = (short)(RightOriginalVolumeWave[i] * (volume / 100d));
                }
                RightVolume = volume;
                break;
            default:
                var maxAndMinLength = GetMaxAndMinWaveLength();
                for (var i = 0; i < maxAndMinLength.Min; i++)
                {
                    RightWave[i] = (short)(RightOriginalVolumeWave[i] * (volume / 100d));
                    LeftWave[i] = (short)(LeftOriginalVolumeWave[i] * (volume / 100d));
                }

                // 残りを処理する。
                var wave = RightWave.Length == maxAndMinLength.Max ? RightWave : LeftWave;
                var originalWave = RightWave.Length == maxAndMinLength.Max ?
                    RightOriginalVolumeWave : LeftOriginalVolumeWave;
                for (var i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
                {
                    wave[i] = (short)(originalWave[i] * (volume / 100d));
                }
                RightVolume = volume;
                LeftVolume = volume;
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
        return bitRate == BitRateType.SixteenBit ? Get16BitBytes().ToArray() : Get8BitBytes().ToArray();
    }

    /// <summary>
    /// 量子化ビット数が8bitの場合の波形データのバイト列を取得する。
    /// </summary>
    /// <returns>量子化ビット数が8bitの場合の波形データのバイト列 : byte[]</returns>
    private List<byte> Get8BitBytes()
    {
        var maxAndMinLength = GetMaxAndMinWaveLength();
        var resultWave = new List<byte>(maxAndMinLength.Max * 2);
        for (var i = 0; i < maxAndMinLength.Min; i++)
        {
            // Point : ステレオの波は左右左右左右左右・・・・・・左右
            resultWave.Add((byte)(LeftWave[i] / 256 + 127));
            resultWave.Add((byte)(RightWave[i] / 256 + 127));
        }
        // 追加しきれていない波形データを追加する
        if (LeftWave.Length == maxAndMinLength.Max)
        {
            for (var i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
                resultWave.Add((byte)(LeftWave[i] / 256 + 127));
                resultWave.Add(0);
            }
        }
        else
        {
            for (var i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
                resultWave.Add(0);
                resultWave.Add((byte)(RightWave[i] / 256 + 128));
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
        var maxAndMinLength = GetMaxAndMinWaveLength();
        var resultWave = new List<byte>(maxAndMinLength.Max * 4);
        for (var i = 0; i < maxAndMinLength.Min; i++)
        {
            var left = LeftWave[i];
            var right = RightWave[i];
            var leftBytes = BitConverter.GetBytes(left);
            var rightBytes = BitConverter.GetBytes(right);
            // Point : ステレオの波は左右左右左右左右・・・・・・左右
            resultWave.Add(leftBytes[0]);
            resultWave.Add(leftBytes[1]);
            resultWave.Add(rightBytes[0]);
            resultWave.Add(rightBytes[1]);
        }
        // 追加しきれていない波形データを追加する
        if (LeftWave.Length == maxAndMinLength.Max)
        {
            for (var i = maxAndMinLength.Min; i < maxAndMinLength.Max; i++)
            {
                var left = LeftWave[i];
                var leftBytes = BitConverter.GetBytes(left);
                resultWave.Add(leftBytes[0]);
                resultWave.Add(leftBytes[1]);
                resultWave.Add(0);
                resultWave.Add(0);
            }
        }
        else
        {
            for (var i = maxAndMinLength.Min; i < maxAndMinLength.Max; i += 2)
            {
                var right = RightWave[i];
                var rightBytes = BitConverter.GetBytes(right);
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
    /// <returns>the wave on the right. 右側のチャンネルの音の波形データ :  short[]</returns>
    public short[] GetRightWave()
    {
        var resultshorts = new short[RightWave.Length];
        Array.Copy(RightWave, resultshorts, RightWave.Length);
        return resultshorts;
    }

    /// <summary>
    /// get the wave on the left. 左側のチャンネルの音の波形データを取得するメソッド。
    /// </summary>
    /// <returns>the wave on the left. 左側のチャンネルの音の波形データ :  short[]</returns>
    public short[] GetLeftWave()
    {
        var resultshorts = new short[LeftWave.Length];
        Array.Copy(LeftWave, resultshorts, LeftWave.Length);
        return resultshorts;
    }

    /// <summary>
    /// append wave to this. 別のステレオ波形を末尾に繋げるメソッド。
    /// </summary>
    /// <param name="wave">wave</param>
    public void Append(StereoWave wave)
    {
        RightWave = RightWave.Concat(wave.GetRightWave()).ToArray();
        RightOriginalVolumeWave = new short[RightWave.Length];
        Array.Copy(RightWave, RightOriginalVolumeWave, RightWave.Length);

        LeftWave = LeftWave.Concat(wave.GetLeftWave()).ToArray();
        LeftOriginalVolumeWave = new short[LeftWave.Length];
        Array.Copy(LeftWave, LeftOriginalVolumeWave, LeftWave.Length);
    }

    /// <summary>
    /// 左右どちらの配列が長いかを調べ、返却するメソッド。
    /// </summary>
    /// <returns>配列の長さを比較し、最大・最小を格納した構造体 : MaxAndMin</returns>
    private MaxAndMin GetMaxAndMinWaveLength()
    {
        return RightWave.Length > LeftWave.Length
            ? new MaxAndMin(RightWave.Length, LeftWave.Length)
            : new MaxAndMin(LeftWave.Length, RightWave.Length);
    }

    public int GetLengthOfBytes(BitRateType bitRate)
    {
        var maxAndMinLength = GetMaxAndMinWaveLength();
        return bitRate is BitRateType.SixteenBit ? maxAndMinLength.Max * 4 : maxAndMinLength.Max * 2;
    }

    private readonly struct MaxAndMin
    {
        public MaxAndMin(int max, int min)
        {
            Min = min;
            Max = max;
        }

        public int Max { get; }

        public int Min { get; }
    }
}
