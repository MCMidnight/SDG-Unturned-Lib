using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000255 RID: 597
	public sealed class ServerInstanceMethod<T1, T2, T3, T4, T5, T6> : ServerInstanceMethodBase
	{
		// Token: 0x06001216 RID: 4630 RVA: 0x0003E4BA File Offset: 0x0003C6BA
		public static ServerInstanceMethod<T1, T2, T3, T4, T5, T6> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerInstanceMethod<T1, T2, T3, T4, T5, T6>, ServerInstanceMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite) => new ServerInstanceMethod<T1, T2, T3, T4, T5, T6>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0003E4E4 File Offset: 0x0003C6E4
		public void Invoke(NetId netId, ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6);
			base.SendAndLoopbackIfLocal(reliability, writerWithInstanceHeader);
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0003E518 File Offset: 0x0003C718
		private ServerInstanceMethod(ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005AA RID: 1450
		private ServerInstanceMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite;

		// Token: 0x020008DB RID: 2267
		// (Invoke) Token: 0x06004997 RID: 18839
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	}
}
