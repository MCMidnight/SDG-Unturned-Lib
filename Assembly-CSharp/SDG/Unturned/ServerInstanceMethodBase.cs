using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200024E RID: 590
	public abstract class ServerInstanceMethodBase : ServerMethodHandle
	{
		// Token: 0x06001201 RID: 4609 RVA: 0x0003E22F File Offset: 0x0003C42F
		protected NetPakWriter GetWriterWithInstanceHeader(NetId netId)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			writerWithStaticHeader.WriteNetId(netId);
			return writerWithStaticHeader;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0003E23F File Offset: 0x0003C43F
		protected ServerInstanceMethodBase(ServerMethodInfo serverMethodInfo) : base(serverMethodInfo)
		{
		}
	}
}
