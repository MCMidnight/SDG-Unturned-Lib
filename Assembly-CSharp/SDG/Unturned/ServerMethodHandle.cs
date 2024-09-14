using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000259 RID: 601
	public abstract class ServerMethodHandle
	{
		// Token: 0x06001229 RID: 4649 RVA: 0x0003E704 File Offset: 0x0003C904
		public override string ToString()
		{
			if (this.serverMethodInfo != null)
			{
				return this.serverMethodInfo.ToString();
			}
			return "invalid";
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0003E720 File Offset: 0x0003C920
		protected static THandle GetInternal<THandle, TWriteDelegate>(Type declaringType, string methodName, Func<ServerMethodInfo, TWriteDelegate, THandle> makeHandle) where THandle : ServerMethodHandle where TWriteDelegate : Delegate
		{
			ServerMethodInfo serverMethodInfo = NetReflection.GetServerMethodInfo(declaringType, methodName);
			if (serverMethodInfo != null)
			{
				TWriteDelegate twriteDelegate = NetReflection.CreateServerWriteDelegate<TWriteDelegate>(serverMethodInfo);
				if (twriteDelegate != null)
				{
					return makeHandle.Invoke(serverMethodInfo, twriteDelegate);
				}
			}
			return default(THandle);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0003E75F File Offset: 0x0003C95F
		protected NetPakWriter GetWriterWithStaticHeader()
		{
			NetPakWriter invokableWriter = NetMessages.GetInvokableWriter();
			invokableWriter.Reset();
			invokableWriter.WriteEnum(EServerMessage.InvokeMethod);
			invokableWriter.WriteBits(this.serverMethodInfo.methodIndex, NetReflection.serverMethodsBitCount);
			return invokableWriter;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0003E78B File Offset: 0x0003C98B
		protected void SendAndLoopbackIfLocal(ENetReliability reliability, NetPakWriter writer)
		{
			writer.Flush();
			this.InvokeLoopback(writer);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0003E79B File Offset: 0x0003C99B
		protected ServerMethodHandle(ServerMethodInfo serverMethodInfo)
		{
			this.serverMethodInfo = serverMethodInfo;
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0003E7AC File Offset: 0x0003C9AC
		private void InvokeLoopback(NetPakWriter writer)
		{
			NetPakReader invokableReader = NetMessages.GetInvokableReader();
			invokableReader.SetBufferSegmentCopy(writer.buffer, Provider.buffer, writer.writeByteIndex);
			invokableReader.Reset();
			EServerMessage eserverMessage;
			invokableReader.ReadEnum(out eserverMessage);
			uint num;
			invokableReader.ReadBits(NetReflection.serverMethodsBitCount, ref num);
			SteamPlayer callingPlayer = null;
			ServerInvocationContext serverInvocationContext = new ServerInvocationContext(ServerInvocationContext.EOrigin.Loopback, callingPlayer, invokableReader, this.serverMethodInfo);
			try
			{
				this.serverMethodInfo.readMethod(serverInvocationContext);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception invoking {0} by server loopback:", new object[]
				{
					this.serverMethodInfo
				});
			}
		}

		// Token: 0x040005B1 RID: 1457
		protected ServerMethodInfo serverMethodInfo;
	}
}
