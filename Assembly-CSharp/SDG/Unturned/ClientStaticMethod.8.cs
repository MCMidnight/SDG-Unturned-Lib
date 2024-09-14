using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000236 RID: 566
	public sealed class ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7> : ClientMethodHandle
	{
		// Token: 0x06001182 RID: 4482 RVA: 0x0003C436 File Offset: 0x0003A636
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x0003C453 File Offset: 0x0003A653
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x0003C470 File Offset: 0x0003A670
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7>, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite) => new ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x0003C498 File Offset: 0x0003A698
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0003C4D0 File Offset: 0x0003A6D0
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0003C508 File Offset: 0x0003A708
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0003C544 File Offset: 0x0003A744
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0003C57C File Offset: 0x0003A77C
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0003C5B8 File Offset: 0x0003A7B8
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x0400056F RID: 1391
		private ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite;

		// Token: 0x020008B7 RID: 2231
		// (Invoke) Token: 0x0600491A RID: 18714
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

		// Token: 0x020008B8 RID: 2232
		// (Invoke) Token: 0x0600491E RID: 18718
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

		// Token: 0x020008B9 RID: 2233
		// (Invoke) Token: 0x06004922 RID: 18722
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
	}
}
