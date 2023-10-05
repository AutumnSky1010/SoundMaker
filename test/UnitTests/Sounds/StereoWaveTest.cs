using SoundMaker.Sounds;
using Xunit.Abstractions;

namespace SoundMakerTests.UnitTests.Sounds;
public class StereoWaveTest
{
    public StereoWaveTest(ITestOutputHelper output)
    {
        this.Output = output;
    }

    private ITestOutputHelper Output { get; }

    [Fact(DisplayName = "ボリュームの大きさが正しく反映されているかのテスト。ボリュームは0から100の間の整数値")]
    public void TestReflectedVolume()
    {
        var wave = GetTestStereoWave(defaultWaveValue: ushort.MinValue, leftWaveLength: 1, rightWaveLength: 100);
        wave.ChangeVolume(-1, SoundDirectionType.Both);
        // 両方-1になってたらやばい
        Assert.Equal(0, wave.RightVolume);
        Assert.Equal(0, wave.LeftVolume);

        wave.ChangeVolume(1000, SoundDirectionType.Right);
        // 1000になってたらファッキン
        Assert.Equal(100, wave.RightVolume);
        Assert.Equal(0, wave.LeftVolume);

        wave.ChangeVolume(50, SoundDirectionType.Left);
        Assert.Equal(50, wave.LeftVolume);
        // 右も50になってたらファッキン
        Assert.Equal(100, wave.RightVolume);
    }

    [Fact(DisplayName = "ステレオ波形の左右両方のボリュームが変更されるかのテスト")]
    public void TestChangeVolumeBoth()
    {
        var defaultWaveValue = ushort.MaxValue;
        var wave = GetTestStereoWave(defaultWaveValue, 100, 100);
        // 音量を二分の一にする。波形データの値も半分になるはず。
        int volume = 50;
        double volumeMultiplier = volume / 100d;
        wave.ChangeVolume(volume, SoundDirectionType.Both);
        this.Output.WriteLine("ボリューム: 50, 方向: 両方でテストする。");
        this.Output.WriteLine("右の波形をテストする。");
        WaveAssert.Equal((ushort)(defaultWaveValue * volumeMultiplier), wave.GetRightWave());

        this.Output.WriteLine("ボリューム: 50, 方向: 両方でテストする。");
        this.Output.WriteLine("左の波形をテストする。");
        WaveAssert.Equal((ushort)(defaultWaveValue * volumeMultiplier), wave.GetLeftWave());
    }

    [Fact(DisplayName = "ステレオ波形の右のボリュームが変更されるかのテスト")]
    public void TestChangeVolumeRight()
    {
        var defaultWaveValue = ushort.MaxValue;
        var wave = GetTestStereoWave(defaultWaveValue, 100, 100);
        // 音量を二分の一にする。波形データの値も半分になるはず。
        int volume = 50;
        double volumeMultiplier = volume / 100d;
        wave.ChangeVolume(volume, SoundDirectionType.Right);
        this.Output.WriteLine("ボリューム: 50, 方向: 右でテストする。");
        this.Output.WriteLine("右の波形をテストする。");
        WaveAssert.Equal((ushort)(defaultWaveValue * volumeMultiplier), wave.GetRightWave());

        this.Output.WriteLine("ボリューム: 50, 方向: 右でテストする。");
        this.Output.WriteLine("左の波形をテストする。");
        WaveAssert.Equal(defaultWaveValue, wave.GetLeftWave());
    }
    
    [Fact(DisplayName = "ステレオ波形の右のボリュームが変更されるかのテスト")]
    public void TestChangeVolumeLeft()
    {
        var defaultWaveValue = ushort.MaxValue;
        var wave = GetTestStereoWave(defaultWaveValue, 100, 100);
        // 音量を二分の一にする。波形データの値も半分になるはず。
        int volume = 50;
        double volumeMultiplier = volume / 100d;
        wave.ChangeVolume(volume, SoundDirectionType.Left);
        this.Output.WriteLine("ボリューム: 50, 方向: 左でテストする。");
        this.Output.WriteLine("右の波形をテストする。");
        WaveAssert.Equal(defaultWaveValue, wave.GetRightWave());

        this.Output.WriteLine("ボリューム: 50, 方向: 左でテストする。");
        this.Output.WriteLine("左の波形をテストする。");
        WaveAssert.Equal((ushort)(defaultWaveValue * volumeMultiplier), wave.GetLeftWave());
    }

    [Fact(DisplayName = "生成したバイト列の長さを正しく取得できるかをテストする。")]
    public void TestGetLengthOfBytes()
    {
        var wave = this.GetTestStereoWave(defaultWaveValue: 0, leftWaveLength: 1, rightWaveLength: 100);
        // 16bitの場合
        var bitRate = BitRateType.SixteenBit;
        Assert.Equal(wave.GetLengthOfBytes(bitRate), wave.GetLengthOfBytes(bitRate));
        // 8bitの場合
        bitRate = BitRateType.EightBit;
        Assert.Equal(wave.GetLengthOfBytes(bitRate), wave.GetLengthOfBytes(bitRate));
    }

    [Fact(DisplayName = "二つの同じ長さの波形が正しく追加されているかをテストする。")]
    public void TestAppendWave()
    {
        var stereoWave1 = this.GetTestStereoWave(defaultWaveValue: ushort.MinValue, leftWaveLength: 1, rightWaveLength: 100);
        var stereoWave2 = this.GetTestStereoWave(defaultWaveValue: ushort.MaxValue, leftWaveLength: 1, rightWaveLength: 100);
        // 16bitの場合
        var bitRate = BitRateType.SixteenBit;
        // 期待する長さを計算する。
        int lengthOf16bit = stereoWave1.GetLengthOfBytes(bitRate) + stereoWave2.GetLengthOfBytes(bitRate);
        // 8bitの場合
        bitRate = BitRateType.EightBit;
        int lengthOf8bit = stereoWave1.GetLengthOfBytes(bitRate) + stereoWave2.GetLengthOfBytes(bitRate);

        // 追加する
        stereoWave1.Append(stereoWave2);

        Assert.Equal(stereoWave1.GetLengthOfBytes(BitRateType.SixteenBit), lengthOf16bit);
        Assert.Equal(stereoWave1.GetLengthOfBytes(BitRateType.EightBit), lengthOf8bit);

        // 末尾に追加されていることを確認する。先頭の値はusort.MinValue、末尾の値はushort.MaxValueになっているはず。
        ushort[] wave = stereoWave1.GetLeftWave();
        Assert.True(wave[0] == ushort.MinValue && wave[wave.Length - 1] == ushort.MaxValue);
        wave = stereoWave1.GetRightWave();
        Assert.True(wave[0] == ushort.MinValue && wave[wave.Length - 1] == ushort.MaxValue);
    }

    private StereoWave GetTestStereoWave(ushort defaultWaveValue, int leftWaveLength, int rightWaveLength)
    {
        ushort[] rightWave = Enumerable.Repeat<ushort>(defaultWaveValue, rightWaveLength).ToArray();
        ushort[] leftWave = Enumerable.Repeat<ushort>(defaultWaveValue, leftWaveLength).ToArray();
        return new StereoWave(rightWave, leftWave);
    }
}
