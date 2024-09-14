using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200025B RID: 603
	public sealed class ServerStaticMethod<T> : ServerMethodHandle
	{
		// Token: 0x06001235 RID: 4661 RVA: 0x0003E8E8 File Offset: 0x0003CAE8
		public static ServerStaticMethod<T> Get(ServerStaticMethod<T>.ReceiveDelegate action)
		{
			return ServerStaticMethod<T>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0003E905 File Offset: 0x0003CB05
		public static ServerStaticMethod<T> Get(ServerStaticMethod<T>.ReceiveDelegateWithContext action)
		{
			return ServerStaticMethod<T>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0003E922 File Offset: 0x0003CB22
		public static ServerStaticMethod<T> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerStaticMethod<T>, ServerStaticMethod<T>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerStaticMethod<T>.WriteDelegate generatedWrite) => new ServerStaticMethod<T>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0003E94C File Offset: 0x0003CB4C
		public void Invoke(ENetReliability reliability, T arg)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg);
			base.SendAndLoopbackIfLocal(reliability, writerWithStaticHeader);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0003E975 File Offset: 0x0003CB75
		private ServerStaticMethod(ServerMethodInfo serverMethodInfo, ServerStaticMethod<T>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005B2 RID: 1458
		private ServerStaticMethod<T>.WriteDelegate generatedWrite;

		// Token: 0x020008E4 RID: 2276
		// (Invoke) Token: 0x060049B4 RID: 18868
		public delegate void ReceiveDelegate(T arg);

		// Token: 0x020008E5 RID: 2277
		// (Invoke) Token: 0x060049B8 RID: 18872
		public delegate void ReceiveDelegateWithContext(in ServerInvocationContext context, T arg);

		// Token: 0x020008E6 RID: 2278
		// (Invoke) Token: 0x060049BC RID: 18876
		private delegate void WriteDelegate(NetPakWriter writer, T arg);
	}
}
