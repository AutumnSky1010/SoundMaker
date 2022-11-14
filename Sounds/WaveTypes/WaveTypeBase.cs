﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.Sounds.WaveTypes;
public abstract class WaveTypeBase
{
    /// <summary>
    /// 波形データの配列を生成する。
    /// </summary>
    /// <param name="format">format of the sound. 音のフォーマット</param>
    /// <param name="tempo">quarter note/rest per minute. 一分間の四分音符・休符の数</param>
    /// <param name="length">length of the array. 配列の長さ</param>
    /// <param name="volume">volume 音量（0 ~ 100）</param>
    /// <param name="hertz">hertz of the sound. 音の周波数</param>
    /// <returns></returns>
    public abstract ushort[] GenerateWave(SoundFormat format, int tempo, int length, int volume, double hertz);
}