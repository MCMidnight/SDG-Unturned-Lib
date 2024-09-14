using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000257 RID: 599
	public sealed class ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8> : ServerInstanceMethodBase
	{
		// Token: 0x0600121C RID: 4636 RVA: 0x0003E596 File Offset: 0x0003C796
		public static ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8>, ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate generatedWrite) => new ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0003E5C0 File Offset: 0x0003C7C0
		public void Invoke(NetId netId, ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			base.SendAndLoopbackIfLocal(reliability, writerWithInstanceHeader);
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0003E5F8 File Offset: 0x0003C7F8
		private ServerInstanceMethod(ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005AC RID: 1452
		private ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate generatedWrite;

		// Token: 0x020008DF RID: 2271
		// (Invoke) Token: 0x060049A5 RID: 18853
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
	}
}
