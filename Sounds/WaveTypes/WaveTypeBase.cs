﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.Sounds.WaveTypes;
/// <summary>
/// Provides a base class for a basic wave type to inherit from.
/// </summary>
public abstract class WaveTypeBase
{
    /// <summary>
    /// generate array of wave data. 波形データの配列を生成する。
    /// </summary>
    /// <param name="format">format of the sound. 音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="length">length of the array. 配列の長さ</param>
    /// <param name="volume">volume 音量（0 ~ 100）</param>
    /// <param name="hertz">hertz of the sound. 音の周波数</param>
    /// <returns>the array of wave data. : unsigned short[]</returns>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Length must be non-negative.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Hertz must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Volume must be below than 100 and more than 0.</exception>
    public abstract ushort[] GenerateWave(SoundFormat format, int tempo, int length, int volume, double hertz);

    protected void CheckGenerateWaveArgs(int tempo, int length, int volume, double hertz)
    {
        if (tempo <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tempo), "'tempo' must be non-negative and greater than 0.");
        }
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "'length' must be non-negative.");
        }
        if (volume > 100 || volume < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(volume), "'volume must be below than 100 and more than 0.");
        }
        if (hertz <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hertz), "'hertz must be non-negative and greater than 0.");
        }
    }
}
