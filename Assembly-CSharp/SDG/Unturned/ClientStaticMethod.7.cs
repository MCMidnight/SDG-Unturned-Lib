using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000235 RID: 565
	public sealed class ClientStaticMethod<T1, T2, T3, T4, T5, T6> : ClientMethodHandle
	{
		// Token: 0x06001179 RID: 4473 RVA: 0x0003C2B0 File Offset: 0x0003A4B0
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0003C2CD File Offset: 0x0003A4CD
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0003C2EA File Offset: 0x0003A4EA
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T1, T2, T3, T4, T5, T6>, ClientStaticMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite) => new ClientStaticMethod<T1, T2, T3, T4, T5, T6>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x0003C314 File Offset: 0x0003A514
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0003C348 File Offset: 0x0003A548
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x0003C37C File Offset: 0x0003A57C
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0003C3B8 File Offset: 0x0003A5B8
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0003C3EC File Offset: 0x0003A5EC
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x0003C426 File Offset: 0x0003A626
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x0400056E RID: 1390
		private ClientStaticMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite;

		// Token: 0x020008B3 RID: 2227
		// (Invoke) Token: 0x0600490B RID: 18699
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

		// Token: 0x020008B4 RID: 2228
		// (Invoke) Token: 0x0600490F RID: 18703
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

		// Token: 0x020008B5 RID: 2229
		// (Invoke) Token: 0x06004913 RID: 18707
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	}
}
