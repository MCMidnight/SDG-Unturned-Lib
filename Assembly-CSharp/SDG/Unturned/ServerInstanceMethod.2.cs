using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000250 RID: 592
	public sealed class ServerInstanceMethod<T> : ServerInstanceMethodBase
	{
		// Token: 0x06001207 RID: 4615 RVA: 0x0003E2B5 File Offset: 0x0003C4B5
		public static ServerInstanceMethod<T> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerInstanceMethod<T>, ServerInstanceMethod<T>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T>.WriteDelegate generatedWrite) => new ServerInstanceMethod<T>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x0003E2E0 File Offset: 0x0003C4E0
		public void Invoke(NetId netId, ENetReliability reliability, T arg)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg);
			base.SendAndLoopbackIfLocal(reliability, writerWithInstanceHeader);
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x0003E30A File Offset: 0x0003C50A
		private ServerInstanceMethod(ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005A5 RID: 1445
		private ServerInstanceMethod<T>.WriteDelegate generatedWrite;

		// Token: 0x020008D1 RID: 2257
		// (Invoke) Token: 0x06004974 RID: 18804
		private delegate void WriteDelegate(NetPakWriter writer, T arg);
	}
}
