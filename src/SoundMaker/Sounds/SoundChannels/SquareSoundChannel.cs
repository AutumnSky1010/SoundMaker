﻿using SoundMaker.Sounds.WaveTypes;

namespace SoundMaker.Sounds.SoundChannels;

/// <summary>
/// This generates square wave. <br/>矩形波を生成するサウンドチャンネル
/// </summary>
public class SquareSoundChannel : SoundChannelBase
{
    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="tempo">Quarter note/rest per minute. <br/>一分間の四分音符・休符の数</param>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="ratio">Duty cycle. <br/>デューティ比</param>
    /// <param name="panType">Pan of the sound. <br/>左右どちらから音が出るか</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    public SquareSoundChannel(int tempo, SoundFormat format, SquareWaveRatio ratio, PanType panType) : base(tempo, format, panType)
    {
        Ratio = ratio;
    }

    /// <summary>
    /// Constructor. <br/>コンストラクタ
    /// </summary>
    /// <param name="tempo">Quarter note/rest per minute. <br/>一分間の四分音符・休符の数</param>
    /// <param name="format">Format of the sound. <br/>音のフォーマット</param>
    /// <param name="ratio">Duty cycle. <br/>デューティ比</param>
    /// <param name="panType">Pan of the sound. <br/>左右どちらから音が出るか</param>
    /// <param name="capacity">The total number of sound components the internal data structure can hold without resizing. <br/>内部データ構造がリサイズされずに保持できるサウンドコンポーネントの総数。</param>
    /// <exception cref="ArgumentOutOfRangeException">Tempo must be non-negative and greater than 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Capacity must be non-negative.</exception>
    public SquareSoundChannel(int tempo, SoundFormat format, SquareWaveRatio ratio, PanType panType, int capacity)
        : base(tempo, format, panType, capacity)
    {
        Ratio = ratio;
    }

    /// <summary>
    /// Duty cycle. <br/>デューティ比
    /// </summary>
    private SquareWaveRatio Ratio { get; }

    public override short[] GenerateWave()
    {
        var result = new List<short>();
        foreach (var soundComponent in SoundComponents)
        {
            var wave = soundComponent.GenerateWave(Format, Tempo, new SquareWave(Ratio));
            result.AddRange(wave);
        }
        return result.ToArray();
    }
}
