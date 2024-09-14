using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200022E RID: 558
	public abstract class ClientMethodHandle
	{
		// Token: 0x06001136 RID: 4406 RVA: 0x0003B77A File Offset: 0x0003997A
		public override string ToString()
		{
			if (this.clientMethodInfo != null)
			{
				return this.clientMethodInfo.ToString();
			}
			return "invalid";
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0003B798 File Offset: 0x00039998
		protected static THandle GetInternal<THandle, TWriteDelegate>(Type declaringType, string methodName, Func<ClientMethodInfo, TWriteDelegate, THandle> makeHandle) where THandle : ClientMethodHandle where TWriteDelegate : Delegate
		{
			ClientMethodInfo clientMethodInfo = NetReflection.GetClientMethodInfo(declaringType, methodName);
			if (clientMethodInfo != null)
			{
				TWriteDelegate twriteDelegate = NetReflection.CreateClientWriteDelegate<TWriteDelegate>(clientMethodInfo);
				if (twriteDelegate != null)
				{
					return makeHandle.Invoke(clientMethodInfo, twriteDelegate);
				}
			}
			return default(THandle);
		}

		/// <summary>
		/// Write header common to both static and instance methods, and return writer.
		/// </summary>
		// Token: 0x06001138 RID: 4408 RVA: 0x0003B7D7 File Offset: 0x000399D7
		protected NetPakWriter GetWriterWithStaticHeader()
		{
			NetPakWriter invokableWriter = NetMessages.GetInvokableWriter();
			invokableWriter.Reset();
			invokableWriter.WriteEnum(EClientMessage.InvokeMethod);
			invokableWriter.WriteBits(this.clientMethodInfo.methodIndex, NetReflection.clientMethodsBitCount);
			return invokableWriter;
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0003B804 File Offset: 0x00039A04
		protected void SendAndLoopbackIfLocal(ENetReliability reliability, ITransportConnection transportConnection, NetPakWriter writer)
		{
			writer.Flush();
			transportConnection.Send(writer.buffer, (long)writer.writeByteIndex, reliability);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0003B824 File Offset: 0x00039A24
		protected void SendAndLoopbackIfAnyAreLocal(ENetReliability reliability, List<ITransportConnection> transportConnections, NetPakWriter writer)
		{
			writer.Flush();
			foreach (ITransportConnection transportConnection in transportConnections)
			{
				transportConnection.Send(writer.buffer, (long)writer.writeByteIndex, reliability);
			}
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0003B884 File Offset: 0x00039A84
		protected void SendAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, NetPakWriter writer)
		{
			writer.Flush();
			foreach (ITransportConnection transportConnection in transportConnections)
			{
				transportConnection.Send(writer.buffer, (long)writer.writeByteIndex, reliability);
			}
			this.InvokeLoopback(writer);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0003B8EC File Offset: 0x00039AEC
		protected ClientMethodHandle(ClientMethodInfo clientMethodInfo)
		{
			this.clientMethodInfo = clientMethodInfo;
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x0003B8FC File Offset: 0x00039AFC
		private void InvokeLoopback(NetPakWriter writer)
		{
			NetPakReader invokableReader = NetMessages.GetInvokableReader();
			invokableReader.SetBufferSegmentCopy(writer.buffer, Provider.buffer, writer.writeByteIndex);
			invokableReader.Reset();
			EClientMessage eclientMessage;
			invokableReader.ReadEnum(out eclientMessage);
			uint num;
			invokableReader.ReadBits(NetReflection.clientMethodsBitCount, ref num);
			ClientInvocationContext clientInvocationContext = new ClientInvocationContext(ClientInvocationContext.EOrigin.Loopback, invokableReader, this.clientMethodInfo);
			try
			{
				this.clientMethodInfo.readMethod(clientInvocationContext);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception invoking {0} by client loopback:", new object[]
				{
					this.clientMethodInfo
				});
			}
		}

		// Token: 0x04000568 RID: 1384
		protected ClientMethodInfo clientMethodInfo;
	}
}
