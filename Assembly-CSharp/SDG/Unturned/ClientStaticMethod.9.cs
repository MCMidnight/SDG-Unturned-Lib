using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000237 RID: 567
	public sealed class ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8> : ClientMethodHandle
	{
		// Token: 0x0600118B RID: 4491 RVA: 0x0003C5C8 File Offset: 0x0003A7C8
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x0003C5E5 File Offset: 0x0003A7E5
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x0003C602 File Offset: 0x0003A802
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate generatedWrite) => new ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x0003C62C File Offset: 0x0003A82C
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0003C664 File Offset: 0x0003A864
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0003C69C File Offset: 0x0003A89C
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0003C6DC File Offset: 0x0003A8DC
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0003C714 File Offset: 0x0003A914
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0003C752 File Offset: 0x0003A952
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x04000570 RID: 1392
		private ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8>.WriteDelegate generatedWrite;

		// Token: 0x020008BB RID: 2235
		// (Invoke) Token: 0x06004929 RID: 18729
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

		// Token: 0x020008BC RID: 2236
		// (Invoke) Token: 0x0600492D RID: 18733
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

		// Token: 0x020008BD RID: 2237
		// (Invoke) Token: 0x06004931 RID: 18737
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
	}
}
