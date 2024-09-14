using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000253 RID: 595
	public sealed class ServerInstanceMethod<T1, T2, T3, T4> : ServerInstanceMethodBase
	{
		// Token: 0x06001210 RID: 4624 RVA: 0x0003E3E6 File Offset: 0x0003C5E6
		public static ServerInstanceMethod<T1, T2, T3, T4> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerInstanceMethod<T1, T2, T3, T4>, ServerInstanceMethod<T1, T2, T3, T4>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite) => new ServerInstanceMethod<T1, T2, T3, T4>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0003E410 File Offset: 0x0003C610
		public void Invoke(NetId netId, ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4);
			base.SendAndLoopbackIfLocal(reliability, writerWithInstanceHeader);
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0003E440 File Offset: 0x0003C640
		private ServerInstanceMethod(ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005A8 RID: 1448
		private ServerInstanceMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite;

		// Token: 0x020008D7 RID: 2263
		// (Invoke) Token: 0x06004989 RID: 18825
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	}
}
