using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200024F RID: 591
	public sealed class ServerInstanceMethod : ServerInstanceMethodBase
	{
		// Token: 0x06001203 RID: 4611 RVA: 0x0003E248 File Offset: 0x0003C448
		public static ServerInstanceMethod Get(Type declaringType, string methodName)
		{
			ServerMethodInfo serverMethodInfo = NetReflection.GetServerMethodInfo(declaringType, methodName);
			if (serverMethodInfo != null)
			{
				return new ServerInstanceMethod(serverMethodInfo);
			}
			return null;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0003E268 File Offset: 0x0003C468
		public void Invoke(NetId netId, ENetReliability reliability)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			base.SendAndLoopbackIfLocal(reliability, writerWithInstanceHeader);
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x0003E288 File Offset: 0x0003C488
		public void Invoke(NetId netId, ENetReliability reliability, Action<NetPakWriter> callback)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			callback.Invoke(writerWithInstanceHeader);
			base.SendAndLoopbackIfLocal(reliability, writerWithInstanceHeader);
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0003E2AC File Offset: 0x0003C4AC
		private ServerInstanceMethod(ServerMethodInfo serverMethodInfo) : base(serverMethodInfo)
		{
		}
	}
}
