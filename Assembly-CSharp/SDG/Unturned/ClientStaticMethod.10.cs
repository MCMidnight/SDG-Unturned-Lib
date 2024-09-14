using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000238 RID: 568
	public sealed class ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9> : ClientMethodHandle
	{
		// Token: 0x06001194 RID: 4500 RVA: 0x0003C762 File Offset: 0x0003A962
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0003C77F File Offset: 0x0003A97F
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x0003C79C File Offset: 0x0003A99C
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.WriteDelegate generatedWrite) => new ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0003C7C4 File Offset: 0x0003A9C4
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0003C800 File Offset: 0x0003AA00
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0003C83C File Offset: 0x0003AA3C
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0003C87C File Offset: 0x0003AA7C
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0003C8B8 File Offset: 0x0003AAB8
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0003C8F8 File Offset: 0x0003AAF8
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x04000571 RID: 1393
		private ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.WriteDelegate generatedWrite;

		// Token: 0x020008BF RID: 2239
		// (Invoke) Token: 0x06004938 RID: 18744
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

		// Token: 0x020008C0 RID: 2240
		// (Invoke) Token: 0x0600493C RID: 18748
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

		// Token: 0x020008C1 RID: 2241
		// (Invoke) Token: 0x06004940 RID: 18752
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
	}
}
