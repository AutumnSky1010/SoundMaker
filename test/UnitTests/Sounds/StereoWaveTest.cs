﻿using SoundMaker.Sounds;
using Xunit.Abstractions;

namespace SoundMakerTests.UnitTests.Sounds;
public class StereoWaveTest
{
    public StereoWaveTest(ITestOutputHelper output)
    {
        Output = output;
    }

    private ITestOutputHelper Output { get; }

    [Fact(DisplayName = "ボリュームの大きさが正しく反映されているかのテスト。ボリュームは0から100の間の整数値")]
    public void TestReflectedVolume()
    {
        var wave = GetTestStereoWave(defaultWaveValue: short.MinValue, leftWaveLength: 1, rightWaveLength: 100);
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
        var defaultWaveValue = short.MaxValue;
        var wave = GetTestStereoWave(defaultWaveValue, 100, 100);
        // 音量を二分の一にする。波形データの値も半分になるはず。
        var volume = 50;
        var volumeMultiplier = volume / 100d;
        wave.ChangeVolume(volume, SoundDirectionType.Both);
        Output.WriteLine("ボリューム: 50, 方向: 両方でテストする。");
        Output.WriteLine("右の波形をテストする。");
        WaveAssert.Equal((short)(defaultWaveValue * volumeMultiplier), wave.GetRightWave());

        Output.WriteLine("ボリューム: 50, 方向: 両方でテストする。");
        Output.WriteLine("左の波形をテストする。");
        WaveAssert.Equal((short)(defaultWaveValue * volumeMultiplier), wave.GetLeftWave());
    }

    [Fact(DisplayName = "ステレオ波形の右のボリュームが変更されるかのテスト")]
    public void TestChangeVolumeRight()
    {
        var defaultWaveValue = short.MaxValue;
        var wave = GetTestStereoWave(defaultWaveValue, 100, 100);
        // 音量を二分の一にする。波形データの値も半分になるはず。
        var volume = 50;
        var volumeMultiplier = volume / 100d;
        wave.ChangeVolume(volume, SoundDirectionType.Right);
        Output.WriteLine("ボリューム: 50, 方向: 右でテストする。");
        Output.WriteLine("右の波形をテストする。");
        WaveAssert.Equal((short)(defaultWaveValue * volumeMultiplier), wave.GetRightWave());

        Output.WriteLine("ボリューム: 50, 方向: 右でテストする。");
        Output.WriteLine("左の波形をテストする。");
        WaveAssert.Equal(defaultWaveValue, wave.GetLeftWave());
    }

    [Fact(DisplayName = "ステレオ波形の右のボリュームが変更されるかのテスト")]
    public void TestChangeVolumeLeft()
    {
        var defaultWaveValue = short.MaxValue;
        var wave = GetTestStereoWave(defaultWaveValue, 100, 100);
        // 音量を二分の一にする。波形データの値も半分になるはず。
        var volume = 50;
        var volumeMultiplier = volume / 100d;
        wave.ChangeVolume(volume, SoundDirectionType.Left);
        Output.WriteLine("ボリューム: 50, 方向: 左でテストする。");
        Output.WriteLine("右の波形をテストする。");
        WaveAssert.Equal(defaultWaveValue, wave.GetRightWave());

        Output.WriteLine("ボリューム: 50, 方向: 左でテストする。");
        Output.WriteLine("左の波形をテストする。");
        WaveAssert.Equal((short)(defaultWaveValue * volumeMultiplier), wave.GetLeftWave());
    }

    [Fact(DisplayName = "生成したバイト列の長さを正しく取得できるかをテストする。")]
    public void TestGetLengthOfBytes()
    {
        var wave = GetTestStereoWave(defaultWaveValue: 0, leftWaveLength: 1, rightWaveLength: 100);
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
        var stereoWave1 = GetTestStereoWave(defaultWaveValue: short.MinValue, leftWaveLength: 1, rightWaveLength: 100);
        var stereoWave2 = GetTestStereoWave(defaultWaveValue: short.MaxValue, leftWaveLength: 1, rightWaveLength: 100);
        // 16bitの場合
        var bitRate = BitRateType.SixteenBit;
        // 期待する長さを計算する。
        var lengthOf16bit = stereoWave1.GetLengthOfBytes(bitRate) + stereoWave2.GetLengthOfBytes(bitRate);
        // 8bitの場合
        bitRate = BitRateType.EightBit;
        var lengthOf8bit = stereoWave1.GetLengthOfBytes(bitRate) + stereoWave2.GetLengthOfBytes(bitRate);

        // 追加する
        stereoWave1.Append(stereoWave2);

        Assert.Equal(stereoWave1.GetLengthOfBytes(BitRateType.SixteenBit), lengthOf16bit);
        Assert.Equal(stereoWave1.GetLengthOfBytes(BitRateType.EightBit), lengthOf8bit);

        // 末尾に追加されていることを確認する。先頭の値はusort.MinValue、末尾の値はshort.MaxValueになっているはず。
        var wave = stereoWave1.GetLeftWave();
        Assert.True(wave[0] == short.MinValue && wave[^1] == short.MaxValue);
        wave = stereoWave1.GetRightWave();
        Assert.True(wave[0] == short.MinValue && wave[^1] == short.MaxValue);
    }

    private StereoWave GetTestStereoWave(short defaultWaveValue, int leftWaveLength, int rightWaveLength)
    {
        var rightWave = Enumerable.Repeat<short>(defaultWaveValue, rightWaveLength).ToArray();
        var leftWave = Enumerable.Repeat<short>(defaultWaveValue, leftWaveLength).ToArray();
        return new StereoWave(rightWave, leftWave);
    }
}
