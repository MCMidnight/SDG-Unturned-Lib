using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200025F RID: 607
	public sealed class ServerStaticMethod<T1, T2, T3, T4, T5> : ServerMethodHandle
	{
		// Token: 0x06001249 RID: 4681 RVA: 0x0003EB62 File Offset: 0x0003CD62
		public static ServerStaticMethod<T1, T2, T3, T4, T5> Get(ServerStaticMethod<T1, T2, T3, T4, T5>.ReceiveDelegate action)
		{
			return ServerStaticMethod<T1, T2, T3, T4, T5>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0003EB7F File Offset: 0x0003CD7F
		public static ServerStaticMethod<T1, T2, T3, T4, T5> Get(ServerStaticMethod<T1, T2, T3, T4, T5>.ReceiveDelegateWithContext action)
		{
			return ServerStaticMethod<T1, T2, T3, T4, T5>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0003EB9C File Offset: 0x0003CD9C
		public static ServerStaticMethod<T1, T2, T3, T4, T5> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerStaticMethod<T1, T2, T3, T4, T5>, ServerStaticMethod<T1, T2, T3, T4, T5>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3, T4, T5>.WriteDelegate generatedWrite) => new ServerStaticMethod<T1, T2, T3, T4, T5>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0003EBC4 File Offset: 0x0003CDC4
		public void Invoke(ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5);
			base.SendAndLoopbackIfLocal(reliability, writerWithStaticHeader);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0003EBF4 File Offset: 0x0003CDF4
		private ServerStaticMethod(ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3, T4, T5>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005B6 RID: 1462
		private ServerStaticMethod<T1, T2, T3, T4, T5>.WriteDelegate generatedWrite;

		// Token: 0x020008F4 RID: 2292
		// (Invoke) Token: 0x060049F0 RID: 18928
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		// Token: 0x020008F5 RID: 2293
		// (Invoke) Token: 0x060049F4 RID: 18932
		public delegate void ReceiveDelegateWithContext(in ServerInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		// Token: 0x020008F6 RID: 2294
		// (Invoke) Token: 0x060049F8 RID: 18936
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	}
}
