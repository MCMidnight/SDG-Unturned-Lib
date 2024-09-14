using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000251 RID: 593
	public sealed class ServerInstanceMethod<T1, T2> : ServerInstanceMethodBase
	{
		// Token: 0x0600120A RID: 4618 RVA: 0x0003E31A File Offset: 0x0003C51A
		public static ServerInstanceMethod<T1, T2> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerInstanceMethod<T1, T2>, ServerInstanceMethod<T1, T2>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2>.WriteDelegate generatedWrite) => new ServerInstanceMethod<T1, T2>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x0003E344 File Offset: 0x0003C544
		public void Invoke(NetId netId, ENetReliability reliability, T1 arg1, T2 arg2)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2);
			base.SendAndLoopbackIfLocal(reliability, writerWithInstanceHeader);
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0003E370 File Offset: 0x0003C570
		private ServerInstanceMethod(ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005A6 RID: 1446
		private ServerInstanceMethod<T1, T2>.WriteDelegate generatedWrite;

		// Token: 0x020008D3 RID: 2259
		// (Invoke) Token: 0x0600497B RID: 18811
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2);
	}
}
