using System;
using System.IO;

namespace SilentOrbit.ProtocolBuffers
{
	public interface MemoryStreamStack : IDisposable
	{
		MemoryStream Pop();

		void Push(MemoryStream stream);
	}
}
