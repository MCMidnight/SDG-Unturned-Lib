using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000262 RID: 610
	public sealed class ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8> : ServerMethodHandle
	{
		// Token: 0x06001258 RID: 4696 RVA: 0x0003ED50 File Offset: 0x0003CF50
		public static ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8> Get(ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.ReceiveDelegate action)
		{
			return ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0003ED6D File Offset: 0x0003CF6D
		public static ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8> Get(ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.ReceiveDelegateWithContext action)
		{
			return ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0003ED8A File Offset: 0x0003CF8A
		public static ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8> Get(Type declaringType, string methodName)
		{
			return ServerMethodHandle.GetInternal<ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>, ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate>(declaringType, methodName, (ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate generatedWrite) => new ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>(serverMethodInfo, generatedWrite));
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0003EDB4 File Offset: 0x0003CFB4
		public void Invoke(ENetReliability reliability, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			base.SendAndLoopbackIfLocal(reliability, writerWithStaticHeader);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0003EDEA File Offset: 0x0003CFEA
		private ServerStaticMethod(ServerMethodInfo serverMethodInfo, ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate generatedWrite) : base(serverMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x040005B9 RID: 1465
		private ServerStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate generatedWrite;

		// Token: 0x02000900 RID: 2304
		// (Invoke) Token: 0x06004A1D RID: 18973
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

		// Token: 0x02000901 RID: 2305
		// (Invoke) Token: 0x06004A21 RID: 18977
		public delegate void ReceiveDelegateWithContext(in ServerInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

		// Token: 0x02000902 RID: 2306
		// (Invoke) Token: 0x06004A25 RID: 18981
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
	}
}
