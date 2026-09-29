using System;
using System.IO;

namespace SilentOrbit.ProtocolBuffers
{
	public class AllocationStack : MemoryStreamStack, IDisposable
	{
		public MemoryStream Pop()
		{
			return new MemoryStream();
		}

		public void Push(MemoryStream stream)
		{
		}

		public void Dispose()
		{
		}
	}
}
