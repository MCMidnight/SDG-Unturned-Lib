using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Used by the server to validate client Unity player files (assemblies and resources).
	/// </summary>
	// Token: 0x02000428 RID: 1064
	public class PlayerHashValidation
	{
		// Token: 0x06002025 RID: 8229 RVA: 0x0007C370 File Offset: 0x0007A570
		internal static bool IsAssemblyHashValid(byte[] hash, EClientPlatform clientPlatform)
		{
			if (!PlayerHashValidation.hasLoadedHashes)
			{
				PlayerHashValidation.hasLoadedHashes = true;
				PlayerHashValidation.LoadHashes();
			}
			if (!PlayerHashValidation.areHashesAvailable)
			{
				return true;
			}
			switch (clientPlatform)
			{
			case EClientPlatform.Windows:
				return Hash.verifyHash(hash, PlayerHashValidation.winAssemblyHash);
			case EClientPlatform.Mac:
				return Hash.verifyHash(hash, PlayerHashValidation.macAssemblyHash);
			case EClientPlatform.Linux:
				return Hash.verifyHash(hash, PlayerHashValidation.linuxAssemblyHash);
			default:
				return false;
			}
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x0007C3D4 File Offset: 0x0007A5D4
		internal static bool IsResourcesHashValid(byte[] hash, EClientPlatform clientPlatform)
		{
			if (!PlayerHashValidation.hasLoadedHashes)
			{
				PlayerHashValidation.hasLoadedHashes = true;
				PlayerHashValidation.LoadHashes();
			}
			if (!PlayerHashValidation.areHashesAvailable)
			{
				return true;
			}
			switch (clientPlatform)
			{
			case EClientPlatform.Windows:
				return Hash.verifyHash(hash, PlayerHashValidation.winResourcesHash);
			case EClientPlatform.Mac:
				return Hash.verifyHash(hash, PlayerHashValidation.macResourcesHash);
			case EClientPlatform.Linux:
				return Hash.verifyHash(hash, PlayerHashValidation.linuxResourcesHash);
			default:
				return false;
			}
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x0007C438 File Offset: 0x0007A638
		private static void LoadHashes()
		{
			try
			{
				Block block = ReadWrite.readBlock("/Extras/Sources/Animation/appout.log", false, 0);
				PlayerHashValidation.winAssemblyHash = block.readByteArray();
				PlayerHashValidation.macAssemblyHash = block.readByteArray();
				PlayerHashValidation.linuxAssemblyHash = block.readByteArray();
				PlayerHashValidation.winResourcesHash = block.readByteArray();
				PlayerHashValidation.macResourcesHash = block.readByteArray();
				PlayerHashValidation.linuxResourcesHash = block.readByteArray();
				PlayerHashValidation.areHashesAvailable = true;
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception loading Unity player hashes:");
				PlayerHashValidation.areHashesAvailable = false;
			}
		}

		// Token: 0x04000FB8 RID: 4024
		private static bool hasLoadedHashes;

		// Token: 0x04000FB9 RID: 4025
		private static bool areHashesAvailable;

		// Token: 0x04000FBA RID: 4026
		private static byte[] winAssemblyHash;

		// Token: 0x04000FBB RID: 4027
		private static byte[] macAssemblyHash;

		// Token: 0x04000FBC RID: 4028
		private static byte[] linuxAssemblyHash;

		// Token: 0x04000FBD RID: 4029
		private static byte[] winResourcesHash;

		// Token: 0x04000FBE RID: 4030
		private static byte[] macResourcesHash;

		// Token: 0x04000FBF RID: 4031
		private static byte[] linuxResourcesHash;
	}
}
