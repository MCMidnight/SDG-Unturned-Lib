using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000299 RID: 665
	internal static class ClientAssetIntegrity
	{
		/// <summary>
		/// Reset prior to joining a new server.
		/// </summary>
		// Token: 0x06001414 RID: 5140 RVA: 0x0004A9E4 File Offset: 0x00048BE4
		public static void Clear()
		{
			ClientAssetIntegrity.timer = 0f;
			ClientAssetIntegrity.validatedGuids.Clear();
			ClientAssetIntegrity.serverKnownMissingGuids.Clear();
			ClientAssetIntegrity.pendingValidation.Clear();
		}

		/// <summary>
		/// By default if the client submits an asset guid which the server cannot find an asset for the client will
		/// be kicked. This is necessary to prevent cheaters from spamming huge numbers of random guids. In certain cases
		/// like a terrain material missing the server knows the client will be missing it as well, and can register
		/// it here to prevent the client from being kicked unnecessarily.
		/// </summary>
		// Token: 0x06001415 RID: 5141 RVA: 0x0004AA0E File Offset: 0x00048C0E
		public static void ServerAddKnownMissingAsset(Guid guid, string context)
		{
			if (guid != Guid.Empty && ClientAssetIntegrity.serverKnownMissingGuids.Add(guid))
			{
				UnturnedLog.info(string.Format("Context \"{0}\" known missing asset {1:N}, server will not kick clients for this", context, guid));
			}
		}

		/// <summary>
		/// Send asset hash (or lack thereof) to server.
		///
		/// IMPORTANT: should only be called in cases where the server has verified the asset exists by loading it,
		/// otherwise only if the asset exists on the client. This is because the server kicks if the asset does not
		/// exist in order to prevent hacked clients from spamming requests. Context parameter is intended to help
		/// narrow down cases where this rule is being broken.
		/// </summary>
		// Token: 0x06001416 RID: 5142 RVA: 0x0004AA40 File Offset: 0x00048C40
		public static void QueueRequest(Guid guid, Asset asset, string context)
		{
			if (guid == Guid.Empty)
			{
				return;
			}
			if (!ClientAssetIntegrity.validatedGuids.Add(guid))
			{
				return;
			}
			if (asset == null)
			{
				UnturnedLog.warn(string.Format("Context \"{0}\" missing asset {1:N}", context, guid));
			}
			ClientAssetIntegrity.pendingValidation.Add(new KeyValuePair<Guid, Asset>(guid, asset));
		}

		/// <summary>
		/// Send asset hash to server.
		/// Used in cases where server does not verify asset exists. (see other method's comment)
		/// </summary>
		// Token: 0x06001417 RID: 5143 RVA: 0x0004AA93 File Offset: 0x00048C93
		public static void QueueRequest(Asset asset)
		{
			if (asset != null)
			{
				ClientAssetIntegrity.QueueRequest(asset.GUID, asset, null);
			}
		}

		/// <summary>
		/// Called each Update on the client.
		/// </summary>
		// Token: 0x06001418 RID: 5144 RVA: 0x0004AAA8 File Offset: 0x00048CA8
		public static void SendRequests()
		{
			if (ClientAssetIntegrity.pendingValidation.Count < 1)
			{
				return;
			}
			ClientAssetIntegrity.timer += Time.unscaledDeltaTime;
			if (ClientAssetIntegrity.timer > 0.1f)
			{
				ClientAssetIntegrity.timer = 0f;
				NetMessages.SendMessageToServer(EServerMessage.ValidateAssets, ENetReliability.Reliable, delegate(NetPakWriter writer)
				{
					int count = ClientAssetIntegrity.pendingValidation.Count;
					int b = (int)(ServerMessageHandler_ValidateAssets.MAX_ASSETS.value + 1U);
					int num = Mathf.Min(count, b);
					int num2 = count - num;
					uint num3 = (uint)(num - 1);
					writer.WriteBits(num3, ServerMessageHandler_ValidateAssets.MAX_ASSETS.bitCount);
					uint num4 = 0U;
					int num5 = 0;
					int i = count - 1;
					while (i >= num2)
					{
						KeyValuePair<Guid, Asset> keyValuePair = ClientAssetIntegrity.pendingValidation[i];
						if (keyValuePair.Value != null && keyValuePair.Value.hash != null && keyValuePair.Value.hash.Length == 20)
						{
							num4 |= 1U << num5;
						}
						i--;
						num5++;
					}
					writer.WriteBits(num4, num);
					num5 = 0;
					int j = count - 1;
					while (j >= num2)
					{
						KeyValuePair<Guid, Asset> keyValuePair2 = ClientAssetIntegrity.pendingValidation[j];
						Guid key = keyValuePair2.Key;
						SystemNetPakWriterEx.WriteGuid(writer, key);
						if ((num4 & 1U << num5) > 0U)
						{
							Asset value = keyValuePair2.Value;
							if (value.originMasterBundle != null && value.originMasterBundle.doesHashFileExist && value.originMasterBundle.hash != null && value.originMasterBundle.hash.Length == 20)
							{
								byte[] array = Hash.combine(new byte[][]
								{
									value.hash,
									value.originMasterBundle.hash
								});
								writer.WriteBytes(array);
							}
							else
							{
								writer.WriteBytes(value.hash);
							}
						}
						j--;
						num5++;
					}
					ClientAssetIntegrity.pendingValidation.RemoveRange(num2, num);
				});
				return;
			}
		}

		// Token: 0x040006E9 RID: 1769
		private static float timer;

		// Token: 0x040006EA RID: 1770
		private static HashSet<Guid> validatedGuids = new HashSet<Guid>();

		// Token: 0x040006EB RID: 1771
		internal static HashSet<Guid> serverKnownMissingGuids = new HashSet<Guid>();

		// Token: 0x040006EC RID: 1772
		private static List<KeyValuePair<Guid, Asset>> pendingValidation = new List<KeyValuePair<Guid, Asset>>();
	}
}
