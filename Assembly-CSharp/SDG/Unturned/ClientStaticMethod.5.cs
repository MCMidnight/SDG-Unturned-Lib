using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000233 RID: 563
	public sealed class ClientStaticMethod<T1, T2, T3, T4> : ClientMethodHandle
	{
		// Token: 0x06001167 RID: 4455 RVA: 0x0003BFC0 File Offset: 0x0003A1C0
		public static ClientStaticMethod<T1, T2, T3, T4> Get(ClientStaticMethod<T1, T2, T3, T4>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T1, T2, T3, T4>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0003BFDD File Offset: 0x0003A1DD
		public static ClientStaticMethod<T1, T2, T3, T4> Get(ClientStaticMethod<T1, T2, T3, T4>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T1, T2, T3, T4>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0003BFFA File Offset: 0x0003A1FA
		public static ClientStaticMethod<T1, T2, T3, T4> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T1, T2, T3, T4>, ClientStaticMethod<T1, T2, T3, T4>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite) => new ClientStaticMethod<T1, T2, T3, T4>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x0003C024 File Offset: 0x0003A224
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0003C054 File Offset: 0x0003A254
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0003C084 File Offset: 0x0003A284
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg1, arg2, arg3, arg4);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0003C0BC File Offset: 0x0003A2BC
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0003C0EC File Offset: 0x0003A2EC
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg1, arg2, arg3, arg4);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x0003C122 File Offset: 0x0003A322
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x0400056C RID: 1388
		private ClientStaticMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite;

		// Token: 0x020008AB RID: 2219
		// (Invoke) Token: 0x060048ED RID: 18669
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		// Token: 0x020008AC RID: 2220
		// (Invoke) Token: 0x060048F1 RID: 18673
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		// Token: 0x020008AD RID: 2221
		// (Invoke) Token: 0x060048F5 RID: 18677
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	}
}
