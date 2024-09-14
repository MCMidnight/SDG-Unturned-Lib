using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000230 RID: 560
	public sealed class ClientStaticMethod<T> : ClientMethodHandle
	{
		// Token: 0x0600114C RID: 4428 RVA: 0x0003BBA1 File Offset: 0x00039DA1
		public static ClientStaticMethod<T> Get(ClientStaticMethod<T>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0003BBBE File Offset: 0x00039DBE
		public static ClientStaticMethod<T> Get(ClientStaticMethod<T>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0003BBDB File Offset: 0x00039DDB
		public static ClientStaticMethod<T> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T>, ClientStaticMethod<T>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T>.WriteDelegate generatedWrite) => new ClientStaticMethod<T>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0003BC04 File Offset: 0x00039E04
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T arg)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0003BC30 File Offset: 0x00039E30
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T arg)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x0003BC5C File Offset: 0x00039E5C
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T arg)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0003BC8C File Offset: 0x00039E8C
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T arg)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x0003BCB8 File Offset: 0x00039EB8
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T arg)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x0003BCE8 File Offset: 0x00039EE8
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x04000569 RID: 1385
		private ClientStaticMethod<T>.WriteDelegate generatedWrite;

		// Token: 0x0200089F RID: 2207
		// (Invoke) Token: 0x060048C0 RID: 18624
		public delegate void ReceiveDelegate(T arg);

		// Token: 0x020008A0 RID: 2208
		// (Invoke) Token: 0x060048C4 RID: 18628
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T arg);

		// Token: 0x020008A1 RID: 2209
		// (Invoke) Token: 0x060048C8 RID: 18632
		private delegate void WriteDelegate(NetPakWriter writer, T arg);
	}
}
