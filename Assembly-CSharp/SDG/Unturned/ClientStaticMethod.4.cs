using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000232 RID: 562
	public sealed class ClientStaticMethod<T1, T2, T3> : ClientMethodHandle
	{
		// Token: 0x0600115E RID: 4446 RVA: 0x0003BE56 File Offset: 0x0003A056
		public static ClientStaticMethod<T1, T2, T3> Get(ClientStaticMethod<T1, T2, T3>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T1, T2, T3>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0003BE73 File Offset: 0x0003A073
		public static ClientStaticMethod<T1, T2, T3> Get(ClientStaticMethod<T1, T2, T3>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T1, T2, T3>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x0003BE90 File Offset: 0x0003A090
		public static ClientStaticMethod<T1, T2, T3> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T1, T2, T3>, ClientStaticMethod<T1, T2, T3>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3>.WriteDelegate generatedWrite) => new ClientStaticMethod<T1, T2, T3>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0003BEB8 File Offset: 0x0003A0B8
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0003BEE8 File Offset: 0x0003A0E8
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0003BF18 File Offset: 0x0003A118
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg1, arg2, arg3);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0003BF4C File Offset: 0x0003A14C
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0003BF7C File Offset: 0x0003A17C
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg1, arg2, arg3);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x0003BFB0 File Offset: 0x0003A1B0
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x0400056B RID: 1387
		private ClientStaticMethod<T1, T2, T3>.WriteDelegate generatedWrite;

		// Token: 0x020008A7 RID: 2215
		// (Invoke) Token: 0x060048DE RID: 18654
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3);

		// Token: 0x020008A8 RID: 2216
		// (Invoke) Token: 0x060048E2 RID: 18658
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T1 arg1, T2 arg2, T3 arg3);

		// Token: 0x020008A9 RID: 2217
		// (Invoke) Token: 0x060048E6 RID: 18662
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3);
	}
}
