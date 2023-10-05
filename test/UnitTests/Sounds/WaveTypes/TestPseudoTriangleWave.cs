﻿using SoundMaker.Sounds.WaveTypes;
using SoundMaker.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMakerTests.UnitTests.Sounds.WaveTypes;
public class TestPseudoTriangleWave
{
    [Fact(DisplayName = "指定した長さの波形データが生成されるかのテスト")]
    public void GenerateWaveTest()
    {
        var waveType = new PseudoTriangleWave();
        int length = 100;
        int tempo = 100;
        int volume = 100;
        double hertz = 100;
        SoundFormat format = new SoundFormat(SamplingFrequencyType.FourtyEightKHz, BitRateType.SixteenBit, ChannelType.Stereo);
        var wave = waveType.GenerateWave(format, tempo, length, volume, hertz);
        Assert.Equal(length, wave.Length);
        wave = waveType.GenerateWave(format, length, volume, hertz);
        Assert.Equal(length, wave.Length);
    }
}
