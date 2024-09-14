using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Hashes for Windows, Linux, and Mac asset bundles.
	/// Only loaded on the dedicated server. Null otherwise.
	/// </summary>
	// Token: 0x0200067A RID: 1658
	internal class MasterBundleHash
	{
		// Token: 0x060036F8 RID: 14072 RVA: 0x001017AA File Offset: 0x000FF9AA
		public byte[] GetPlatformHash(EClientPlatform clientPlatform)
		{
			switch (clientPlatform)
			{
			case EClientPlatform.Windows:
				return this.windowsHash;
			case EClientPlatform.Mac:
				return this.macHash;
			case EClientPlatform.Linux:
				return this.linuxHash;
			default:
				return null;
			}
		}

		/// <summary>
		/// Does given hash match any of the platform hashes?
		/// </summary>
		// Token: 0x060036F9 RID: 14073 RVA: 0x001017D4 File Offset: 0x000FF9D4
		public bool DoesAnyHashMatch(byte[] hash)
		{
			return this.windowsHash == null || this.macHash == null || this.linuxHash == null || Hash.verifyHash(hash, this.windowsHash) || Hash.verifyHash(hash, this.macHash) || Hash.verifyHash(hash, this.linuxHash);
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x00101828 File Offset: 0x000FFA28
		public bool DoesPlatformHashMatch(byte[] hash, EClientPlatform clientPlatform)
		{
			byte[] platformHash = this.GetPlatformHash(clientPlatform);
			return platformHash == null || Hash.verifyHash(hash, platformHash);
		}

		// Token: 0x04002080 RID: 8320
		public byte[] windowsHash;

		// Token: 0x04002081 RID: 8321
		public byte[] macHash;

		// Token: 0x04002082 RID: 8322
		public byte[] linuxHash;
	}
}
