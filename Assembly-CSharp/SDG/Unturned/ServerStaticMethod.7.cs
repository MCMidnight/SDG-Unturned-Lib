using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000260 RID: 608
	public sealed class ServerStaticMethod<T1, T2, T3, T4, T5, T6> : ServerMethodHandle
	{
		// Token: 0x0600124E RID: 4686 RVA: 0x0003EC04 File Offset: 0x0003CE04
		public static ServerStaticMethod<T1, T2, T3, T4, T5, T6> Get(ServerStaticMethod<T1, T2, T3, T4, T5, T6>.ReceiveDelegate action)
		{
			return ServerStaticMethod<T1, T2, T3, T4, T5, T6>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0003EC21 File Offset: 0x0003CE21
		public static ServerStaticMethod<T1, T2, T3, T4, T5, T6> Get(ServerStaticMethod<T1, T2, T3, T4, T5, T6>.ReceiveDelegateWithContext action)
		{
			return ServerStaticMethod<T1, T2, T3, T4, T5, T6>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0003EC3E File Offset: 0x0003CE3E
		public static ServerStaticMethod<T1, T2, T3, T4, T5, T6> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerStaticMethod<T1, T2, T3, T4, T5, T6>, ServerStaticMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite) => new ServerStaticMethod<T1, T2, T3, T4, T5, T6>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0003EC68 File Offset: 0x0003CE68
		public void Invoke(ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6);
			base.SendAndLoopbackIfLocal(reliability, writerWithStaticHeader);
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0003EC9A File Offset: 0x0003CE9A
		private ServerStaticMethod(ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005B7 RID: 1463
		private ServerStaticMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite;

		// Token: 0x020008F8 RID: 2296
		// (Invoke) Token: 0x060049FF RID: 18943
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

		// Token: 0x020008F9 RID: 2297
		// (Invoke) Token: 0x06004A03 RID: 18947
		public delegate void ReceiveDelegateWithContext(in ServerInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

		// Token: 0x020008FA RID: 2298
		// (Invoke) Token: 0x06004A07 RID: 18951
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	}
}
