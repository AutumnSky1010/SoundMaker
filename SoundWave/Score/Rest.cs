using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.SoundWave.Score;
public class Rest : ISoundComponent
{
	public double Second { get; }
	
	public Rest(double second)
	{
		this.Second = second;
	}
}
