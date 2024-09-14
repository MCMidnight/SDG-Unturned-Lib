using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200025C RID: 604
	public sealed class ServerStaticMethod<T1, T2> : ServerMethodHandle
	{
		// Token: 0x0600123A RID: 4666 RVA: 0x0003E985 File Offset: 0x0003CB85
		public static ServerStaticMethod<T1, T2> Get(ServerStaticMethod<T1, T2>.ReceiveDelegate action)
		{
			return ServerStaticMethod<T1, T2>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0003E9A2 File Offset: 0x0003CBA2
		public static ServerStaticMethod<T1, T2> Get(ServerStaticMethod<T1, T2>.ReceiveDelegateWithContext action)
		{
			return ServerStaticMethod<T1, T2>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0003E9BF File Offset: 0x0003CBBF
		public static ServerStaticMethod<T1, T2> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerStaticMethod<T1, T2>, ServerStaticMethod<T1, T2>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2>.WriteDelegate generatedWrite) => new ServerStaticMethod<T1, T2>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0003E9E8 File Offset: 0x0003CBE8
		public void Invoke(ENetReliability reliability, T1 arg1, T2 arg2)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2);
			base.SendAndLoopbackIfLocal(reliability, writerWithStaticHeader);
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0003EA12 File Offset: 0x0003CC12
		private ServerStaticMethod(ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005B3 RID: 1459
		private ServerStaticMethod<T1, T2>.WriteDelegate generatedWrite;

		// Token: 0x020008E8 RID: 2280
		// (Invoke) Token: 0x060049C3 RID: 18883
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2);

		// Token: 0x020008E9 RID: 2281
		// (Invoke) Token: 0x060049C7 RID: 18887
		public delegate void ReceiveDelegateWithContext(in ServerInvocationContext context, T1 arg1, T2 arg2);

		// Token: 0x020008EA RID: 2282
		// (Invoke) Token: 0x060049CB RID: 18891
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2);
	}
}
