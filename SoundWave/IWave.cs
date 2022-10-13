using SoundMaker.WaveFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave;
public interface IWave
{
	int Volume { get; }

	int Length { get; }
	
	byte[] GetBytes(BitRateType bitRate);
}
