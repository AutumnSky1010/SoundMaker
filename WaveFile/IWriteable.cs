using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMaker.WaveFile;
public interface IWriteable
{
	void Write(string path);
}
