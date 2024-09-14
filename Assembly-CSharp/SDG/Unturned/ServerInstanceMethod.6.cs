using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000254 RID: 596
	public sealed class ServerInstanceMethod<T1, T2, T3, T4, T5> : ServerInstanceMethodBase
	{
		// Token: 0x06001213 RID: 4627 RVA: 0x0003E450 File Offset: 0x0003C650
		public static ServerInstanceMethod<T1, T2, T3, T4, T5> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerInstanceMethod<T1, T2, T3, T4, T5>, ServerInstanceMethod<T1, T2, T3, T4, T5>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3, T4, T5>.WriteDelegate generatedWrite) => new ServerInstanceMethod<T1, T2, T3, T4, T5>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0003E478 File Offset: 0x0003C678
		public void Invoke(NetId netId, ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5);
			base.SendAndLoopbackIfLocal(reliability, writerWithInstanceHeader);
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0003E4AA File Offset: 0x0003C6AA
		private ServerInstanceMethod(ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3, T4, T5>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005A9 RID: 1449
		private ServerInstanceMethod<T1, T2, T3, T4, T5>.WriteDelegate generatedWrite;

		// Token: 0x020008D9 RID: 2265
		// (Invoke) Token: 0x06004990 RID: 18832
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	}
}
