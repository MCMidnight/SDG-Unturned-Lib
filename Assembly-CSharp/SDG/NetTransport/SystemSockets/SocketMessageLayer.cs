using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace SDG.NetTransport.SystemSockets
{
	/// <summary>
	/// Implements message boundaries on top of a TCP stream socket.
	/// </summary>
	// Token: 0x0200006A RID: 106
	internal class SocketMessageLayer
	{
		// Token: 0x06000254 RID: 596 RVA: 0x000097C0 File Offset: 0x000079C0
		public void SendMessage(Socket socket, byte[] buffer, int size)
		{
			SocketMessageLayer.sizeBuffer[0] = (byte)(size >> 8 & 255);
			SocketMessageLayer.sizeBuffer[1] = (byte)(size & 255);
			socket.Send(SocketMessageLayer.sizeBuffer);
			SocketError socketError;
			socket.Send(buffer, 0, size, 0, ref socketError);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00009808 File Offset: 0x00007A08
		public void ReceiveMessages(Socket socket)
		{
			int num = (this.pendingMessage == null) ? 2 : 1;
			if (socket.Available < num)
			{
				return;
			}
			SocketError socketError;
			int num2 = socket.Receive(SocketMessageLayer.internalBuffer, 0, SocketMessageLayer.internalBuffer.Length, 0, ref socketError);
			if (socketError == 10035)
			{
				return;
			}
			if (socketError != null)
			{
				return;
			}
			if (num2 < 1)
			{
				return;
			}
			int i = 0;
			while (i < num2)
			{
				if (this.pendingMessage == null)
				{
					int num3 = (int)SocketMessageLayer.internalBuffer[i] << 8 | (int)SocketMessageLayer.internalBuffer[i + 1];
					this.pendingMessage = new byte[num3];
					this.pendingMessageOffset = 0;
					i += 2;
				}
				else
				{
					int num4 = num2 - i;
					int num5 = this.pendingMessage.Length - this.pendingMessageOffset;
					if (num4 < num5)
					{
						Array.Copy(SocketMessageLayer.internalBuffer, i, this.pendingMessage, this.pendingMessageOffset, num4);
						this.pendingMessageOffset += num4;
						i += num4;
					}
					else
					{
						Array.Copy(SocketMessageLayer.internalBuffer, i, this.pendingMessage, this.pendingMessageOffset, num5);
						i += num5;
						this.messageQueue.Enqueue(this.pendingMessage);
						this.pendingMessage = null;
					}
				}
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000991E File Offset: 0x00007B1E
		public bool DequeueMessage(out byte[] buffer)
		{
			if (this.messageQueue.Count > 0)
			{
				buffer = this.messageQueue.Dequeue();
				return true;
			}
			buffer = null;
			return false;
		}

		// Token: 0x04000113 RID: 275
		private static byte[] sizeBuffer = new byte[2];

		// Token: 0x04000114 RID: 276
		private static byte[] internalBuffer = new byte[1200];

		// Token: 0x04000115 RID: 277
		private Queue<byte[]> messageQueue = new Queue<byte[]>();

		// Token: 0x04000116 RID: 278
		private byte[] pendingMessage;

		// Token: 0x04000117 RID: 279
		private int pendingMessageOffset;
	}
}
