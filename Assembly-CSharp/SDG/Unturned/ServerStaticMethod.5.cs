using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200025E RID: 606
	public sealed class ServerStaticMethod<T1, T2, T3, T4> : ServerMethodHandle
	{
		// Token: 0x06001244 RID: 4676 RVA: 0x0003EAC0 File Offset: 0x0003CCC0
		public static ServerStaticMethod<T1, T2, T3, T4> Get(ServerStaticMethod<T1, T2, T3, T4>.ReceiveDelegate action)
		{
			return ServerStaticMethod<T1, T2, T3, T4>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0003EADD File Offset: 0x0003CCDD
		public static ServerStaticMethod<T1, T2, T3, T4> Get(ServerStaticMethod<T1, T2, T3, T4>.ReceiveDelegateWithContext action)
		{
			return ServerStaticMethod<T1, T2, T3, T4>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0003EAFA File Offset: 0x0003CCFA
		public static ServerStaticMethod<T1, T2, T3, T4> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerStaticMethod<T1, T2, T3, T4>, ServerStaticMethod<T1, T2, T3, T4>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite) => new ServerStaticMethod<T1, T2, T3, T4>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0003EB24 File Offset: 0x0003CD24
		public void Invoke(ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4);
			base.SendAndLoopbackIfLocal(reliability, writerWithStaticHeader);
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0003EB52 File Offset: 0x0003CD52
		private ServerStaticMethod(ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005B5 RID: 1461
		private ServerStaticMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite;

		// Token: 0x020008F0 RID: 2288
		// (Invoke) Token: 0x060049E1 RID: 18913
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		// Token: 0x020008F1 RID: 2289
		// (Invoke) Token: 0x060049E5 RID: 18917
		public delegate void ReceiveDelegateWithContext(in ServerInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		// Token: 0x020008F2 RID: 2290
		// (Invoke) Token: 0x060049E9 RID: 18921
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	}
}
