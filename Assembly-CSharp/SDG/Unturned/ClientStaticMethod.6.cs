using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000234 RID: 564
	public sealed class ClientStaticMethod<T1, T2, T3, T4, T5> : ClientMethodHandle
	{
		// Token: 0x06001170 RID: 4464 RVA: 0x0003C132 File Offset: 0x0003A332
		public static ClientStaticMethod<T1, T2, T3, T4, T5> Get(ClientStaticMethod<T1, T2, T3, T4, T5>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x0003C14F File Offset: 0x0003A34F
		public static ClientStaticMethod<T1, T2, T3, T4, T5> Get(ClientStaticMethod<T1, T2, T3, T4, T5>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x0003C16C File Offset: 0x0003A36C
		public static ClientStaticMethod<T1, T2, T3, T4, T5> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T1, T2, T3, T4, T5>, ClientStaticMethod<T1, T2, T3, T4, T5>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5>.WriteDelegate generatedWrite) => new ClientStaticMethod<T1, T2, T3, T4, T5>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x0003C194 File Offset: 0x0003A394
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x0003C1C8 File Offset: 0x0003A3C8
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x0003C1FC File Offset: 0x0003A3FC
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg1, arg2, arg3, arg4, arg5);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x0003C234 File Offset: 0x0003A434
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x0003C268 File Offset: 0x0003A468
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg1, arg2, arg3, arg4, arg5);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x0003C2A0 File Offset: 0x0003A4A0
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x0400056D RID: 1389
		private ClientStaticMethod<T1, T2, T3, T4, T5>.WriteDelegate generatedWrite;

		// Token: 0x020008AF RID: 2223
		// (Invoke) Token: 0x060048FC RID: 18684
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		// Token: 0x020008B0 RID: 2224
		// (Invoke) Token: 0x06004900 RID: 18688
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		// Token: 0x020008B1 RID: 2225
		// (Invoke) Token: 0x06004904 RID: 18692
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	}
}
