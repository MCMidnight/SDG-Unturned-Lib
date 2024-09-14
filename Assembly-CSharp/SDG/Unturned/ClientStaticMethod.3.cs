using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000231 RID: 561
	public sealed class ClientStaticMethod<T1, T2> : ClientMethodHandle
	{
		// Token: 0x06001155 RID: 4437 RVA: 0x0003BCF8 File Offset: 0x00039EF8
		public static ClientStaticMethod<T1, T2> Get(ClientStaticMethod<T1, T2>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T1, T2>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x0003BD15 File Offset: 0x00039F15
		public static ClientStaticMethod<T1, T2> Get(ClientStaticMethod<T1, T2>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T1, T2>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x0003BD32 File Offset: 0x00039F32
		public static ClientStaticMethod<T1, T2> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T1, T2>, ClientStaticMethod<T1, T2>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2>.WriteDelegate generatedWrite) => new ClientStaticMethod<T1, T2>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0003BD5C File Offset: 0x00039F5C
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0003BD88 File Offset: 0x00039F88
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x0003BDB4 File Offset: 0x00039FB4
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg1, arg2);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x0003BDE8 File Offset: 0x00039FE8
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x0003BE14 File Offset: 0x0003A014
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg1, arg2);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x0003BE46 File Offset: 0x0003A046
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x0400056A RID: 1386
		private ClientStaticMethod<T1, T2>.WriteDelegate generatedWrite;

		// Token: 0x020008A3 RID: 2211
		// (Invoke) Token: 0x060048CF RID: 18639
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2);

		// Token: 0x020008A4 RID: 2212
		// (Invoke) Token: 0x060048D3 RID: 18643
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T1 arg1, T2 arg2);

		// Token: 0x020008A5 RID: 2213
		// (Invoke) Token: 0x060048D7 RID: 18647
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2);
	}
}
