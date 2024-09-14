using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000252 RID: 594
	public sealed class ServerInstanceMethod<T1, T2, T3> : ServerInstanceMethodBase
	{
		// Token: 0x0600120D RID: 4621 RVA: 0x0003E380 File Offset: 0x0003C580
		public static ServerInstanceMethod<T1, T2, T3> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerInstanceMethod<T1, T2, T3>, ServerInstanceMethod<T1, T2, T3>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3>.WriteDelegate generatedWrite) => new ServerInstanceMethod<T1, T2, T3>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0003E3A8 File Offset: 0x0003C5A8
		public void Invoke(NetId netId, ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3);
			base.SendAndLoopbackIfLocal(reliability, writerWithInstanceHeader);
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0003E3D6 File Offset: 0x0003C5D6
		private ServerInstanceMethod(ServerMethodInfo serverMethodInfo, ServerInstanceMethod<T1, T2, T3>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005A7 RID: 1447
		private ServerInstanceMethod<T1, T2, T3>.WriteDelegate generatedWrite;

		// Token: 0x020008D5 RID: 2261
		// (Invoke) Token: 0x06004982 RID: 18818
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3);
	}
}
