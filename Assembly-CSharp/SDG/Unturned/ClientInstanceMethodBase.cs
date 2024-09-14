using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200021F RID: 543
	public abstract class ClientInstanceMethodBase : ClientMethodHandle
	{
		// Token: 0x060010D0 RID: 4304 RVA: 0x0003A4C3 File Offset: 0x000386C3
		protected NetPakWriter GetWriterWithInstanceHeader(NetId netId)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			writerWithStaticHeader.WriteNetId(netId);
			return writerWithStaticHeader;
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0003A4D3 File Offset: 0x000386D3
		protected ClientInstanceMethodBase(ClientMethodInfo clientMethodInfo) : base(clientMethodInfo)
		{
		}
	}
}
