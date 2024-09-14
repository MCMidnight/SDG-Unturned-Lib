using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000261 RID: 609
	public sealed class ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7> : ServerMethodHandle
	{
		// Token: 0x06001253 RID: 4691 RVA: 0x0003ECAA File Offset: 0x0003CEAA
		public static ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7> Get(ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7>.ReceiveDelegate action)
		{
			return ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0003ECC7 File Offset: 0x0003CEC7
		public static ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7> Get(ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7>.ReceiveDelegateWithContext action)
		{
			return ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0003ECE4 File Offset: 0x0003CEE4
		public static ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7>, ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite) => new ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0003ED0C File Offset: 0x0003CF0C
		public void Invoke(ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			base.SendAndLoopbackIfLocal(reliability, writerWithStaticHeader);
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0003ED40 File Offset: 0x0003CF40
		private ServerStaticMethod(ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005B8 RID: 1464
		private ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite;

		// Token: 0x020008FC RID: 2300
		// (Invoke) Token: 0x06004A0E RID: 18958
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

		// Token: 0x020008FD RID: 2301
		// (Invoke) Token: 0x06004A12 RID: 18962
		public delegate void ReceiveDelegateWithContext(in ServerInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

		// Token: 0x020008FE RID: 2302
		// (Invoke) Token: 0x06004A16 RID: 18966
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
	}
}
