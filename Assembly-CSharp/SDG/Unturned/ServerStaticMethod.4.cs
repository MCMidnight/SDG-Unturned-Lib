using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200025D RID: 605
	public sealed class ServerStaticMethod<T1, T2, T3> : ServerMethodHandle
	{
		// Token: 0x0600123F RID: 4671 RVA: 0x0003EA22 File Offset: 0x0003CC22
		public static ServerStaticMethod<T1, T2, T3> Get(ServerStaticMethod<T1, T2, T3>.ReceiveDelegate action)
		{
			return ServerStaticMethod<T1, T2, T3>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0003EA3F File Offset: 0x0003CC3F
		public static ServerStaticMethod<T1, T2, T3> Get(ServerStaticMethod<T1, T2, T3>.ReceiveDelegateWithContext action)
		{
			return ServerStaticMethod<T1, T2, T3>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x0003EA5C File Offset: 0x0003CC5C
		public static ServerStaticMethod<T1, T2, T3> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerStaticMethod<T1, T2, T3>, ServerStaticMethod<T1, T2, T3>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3>.WriteDelegate generatedWrite) => new ServerStaticMethod<T1, T2, T3>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x0003EA84 File Offset: 0x0003CC84
		public void Invoke(ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3);
			base.SendAndLoopbackIfLocal(reliability, writerWithStaticHeader);
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0003EAB0 File Offset: 0x0003CCB0
		private ServerStaticMethod(ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005B4 RID: 1460
		private ServerStaticMethod<T1, T2, T3>.WriteDelegate generatedWrite;

		// Token: 0x020008EC RID: 2284
		// (Invoke) Token: 0x060049D2 RID: 18898
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3);

		// Token: 0x020008ED RID: 2285
		// (Invoke) Token: 0x060049D6 RID: 18902
		public delegate void ReceiveDelegateWithContext(in ServerInvocationContext context, T1 arg1, T2 arg2, T3 arg3);

		// Token: 0x020008EE RID: 2286
		// (Invoke) Token: 0x060049DA RID: 18906
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3);
	}
}
