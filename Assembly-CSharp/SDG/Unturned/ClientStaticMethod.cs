using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200022F RID: 559
	public sealed class ClientStaticMethod : ClientMethodHandle
	{
		// Token: 0x0600113E RID: 4414 RVA: 0x0003B994 File Offset: 0x00039B94
		public static ClientStaticMethod Get(ClientStaticMethod.ReceiveDelegate action)
		{
			Type declaringType = action.Method.DeclaringType;
			string name = action.Method.Name;
			return ClientStaticMethod.Get(declaringType, name);
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0003B9C0 File Offset: 0x00039BC0
		public static ClientStaticMethod Get(ClientStaticMethod.ReceiveDelegateWithContext action)
		{
			Type declaringType = action.Method.DeclaringType;
			string name = action.Method.Name;
			return ClientStaticMethod.Get(declaringType, name);
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x0003B9EC File Offset: 0x00039BEC
		public static ClientStaticMethod Get(Type declaringType, string methodName)
		{
			ClientMethodInfo clientMethodInfo = NetReflection.GetClientMethodInfo(declaringType, methodName);
			if (clientMethodInfo != null)
			{
				return new ClientStaticMethod(clientMethodInfo);
			}
			return null;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0003BA0C File Offset: 0x00039C0C
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0003BA2C File Offset: 0x00039C2C
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, Action<NetPakWriter> callback)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			callback.Invoke(writerWithStaticHeader);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x0003BA50 File Offset: 0x00039C50
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0003BA70 File Offset: 0x00039C70
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0003BAA0 File Offset: 0x00039CA0
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, Action<NetPakWriter> callback)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			callback.Invoke(writerWithStaticHeader);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0003BAC4 File Offset: 0x00039CC4
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, Action<NetPakWriter> callback)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, callback);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0003BAF4 File Offset: 0x00039CF4
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x0003BB14 File Offset: 0x00039D14
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0003BB44 File Offset: 0x00039D44
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, Action<NetPakWriter> callback)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			callback.Invoke(writerWithStaticHeader);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0003BB68 File Offset: 0x00039D68
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, Action<NetPakWriter> callback)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, callback);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0003BB98 File Offset: 0x00039D98
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo) : base(clientMethodInfo)
		{
		}

		// Token: 0x0200089D RID: 2205
		// (Invoke) Token: 0x060048B8 RID: 18616
		public delegate void ReceiveDelegate();

		// Token: 0x0200089E RID: 2206
		// (Invoke) Token: 0x060048BC RID: 18620
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context);
	}
}
