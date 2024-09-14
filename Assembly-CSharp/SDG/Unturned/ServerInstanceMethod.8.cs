using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000256 RID: 598
	public sealed class ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7> : ServerInstanceMethodBase
	{
		// Token: 0x06001219 RID: 4633 RVA: 0x0003E528 File Offset: 0x0003C728
		public static ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7>, ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite) => new ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0003E550 File Offset: 0x0003C750
		public void Invoke(NetId netId, ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			base.SendAndLoopbackIfLocal(reliability, writerWithInstanceHeader);
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0003E586 File Offset: 0x0003C786
		private ServerInstanceMethod(ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005AB RID: 1451
		private ServerInstanceMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite;

		// Token: 0x020008DD RID: 2269
		// (Invoke) Token: 0x0600499E RID: 18846
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
	}
}
