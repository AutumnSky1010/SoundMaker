using SoundMaker.Sounds;
using Xunit.Abstractions;

namespace SoundMakerTests.UnitTests.Sounds;
public class MonauralWaveTest
{
    public MonauralWaveTest(ITestOutputHelper output)
    {
        Output = output;
    }

    private ITestOutputHelper Output { get; }

    [Fact(DisplayName = "ボリュームの大きさが正しく反映されているかのテスト。ボリュームは0から100の間の整数値")]
    public void TestReflectedVolume()
    {
        var wave = GetTestMonauralWave(defaultWaveValue: ushort.MinValue, waveLength: 100);
        wave.ChangeVolume(-1);
        // -1になってたらやばい
        Assert.Equal(0, wave.Volume);

        wave.ChangeVolume(1000);
        // 1000になってたらファッキン
        Assert.Equal(100, wave.Volume);

        wave.ChangeVolume(50);
        Assert.Equal(50, wave.Volume);
    }

    [Fact(DisplayName = "波形のボリュームが変更されるかのテスト")]
    public void TestChangeVolume()
    {
        var defaultWaveValue = ushort.MaxValue;
        var wave = GetTestMonauralWave(defaultWaveValue: defaultWaveValue, waveLength: 100);
        // 音量を二分の一にする。波形データの値も半分になるはず。
        var volume = 50;
        var volumeMultiplier = volume / 100d;
        wave.ChangeVolume(volume);

        Output.WriteLine("ボリューム: 50");
        Output.WriteLine("波形をテストする。");
        WaveAssert.Equal((ushort)(defaultWaveValue * volumeMultiplier), wave.GetWave());
    }

    [Fact(DisplayName = "生成したバイト列の長さを正しく取得できるかをテストする。")]
    public void TestGetLengthOfBytes()
    {
        var wave = GetTestMonauralWave(defaultWaveValue: 0, waveLength: 100);
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
        var monauralWave1 = GetTestMonauralWave(defaultWaveValue: ushort.MinValue, waveLength: 100);
        var monauralWave2 = GetTestMonauralWave(defaultWaveValue: ushort.MaxValue, waveLength: 100);
        // 16bitの場合
        var bitRate = BitRateType.SixteenBit;
        // 期待する長さを計算する。
        var lengthOf16bit = monauralWave1.GetLengthOfBytes(bitRate) + monauralWave2.GetLengthOfBytes(bitRate);
        // 8bitの場合
        bitRate = BitRateType.EightBit;
        var lengthOf8bit = monauralWave1.GetLengthOfBytes(bitRate) + monauralWave2.GetLengthOfBytes(bitRate);

        // 追加する
        monauralWave1.Append(monauralWave2);

        Assert.Equal(monauralWave1.GetLengthOfBytes(BitRateType.SixteenBit), lengthOf16bit);
        Assert.Equal(monauralWave1.GetLengthOfBytes(BitRateType.EightBit), lengthOf8bit);

        // 末尾に追加されていることを確認する。先頭の値はusort.MinValue、末尾の値はushort.MaxValueになっているはず。
        var wave = monauralWave1.GetWave();
        Assert.True(wave[0] == ushort.MinValue && wave[^1] == ushort.MaxValue);
    }

    private MonauralWave GetTestMonauralWave(ushort defaultWaveValue, int waveLength)
    {
        var rightWave = Enumerable.Repeat(defaultWaveValue, waveLength).ToArray();
        return new MonauralWave(rightWave);
    }
}
