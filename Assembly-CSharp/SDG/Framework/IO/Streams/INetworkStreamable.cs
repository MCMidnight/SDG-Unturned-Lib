using System;

namespace SDG.Framework.IO.Streams
{
	// Token: 0x020000B0 RID: 176
	public interface INetworkStreamable
	{
		// Token: 0x0600049F RID: 1183
		void readFromStream(NetworkStream networkStream);

		// Token: 0x060004A0 RID: 1184
		void writeToStream(NetworkStream networkStream);
	}
}
