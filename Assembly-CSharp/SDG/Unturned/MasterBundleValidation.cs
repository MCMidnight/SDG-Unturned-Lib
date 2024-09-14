﻿using System;
using System.Collections.Generic;
using System.IO;

namespace SDG.Unturned
{
	/// <summary>
	/// Compares client asset bundle hash with server known hashes.
	/// </summary>
	// Token: 0x0200067B RID: 1659
	internal static class MasterBundleValidation
	{
		/// <summary>
		/// Called by asset startup to cache which bundles are eligible for hashing.
		/// </summary>
		// Token: 0x060036FC RID: 14076 RVA: 0x00101854 File Offset: 0x000FFA54
		public static void initialize(List<MasterBundleConfig> allMasterBundles)
		{
			foreach (MasterBundleConfig masterBundleConfig in allMasterBundles)
			{
				if (!masterBundleConfig.doesHashFileExist)
				{
					UnturnedLog.info("Asset bundle \"" + masterBundleConfig.assetBundleNameWithoutExtension + "\" does not have server hashes file");
				}
				else
				{
					masterBundleConfig.serverHashes = MasterBundleValidation.loadHashForBundle(masterBundleConfig);
					if (masterBundleConfig.serverHashes != null && !masterBundleConfig.serverHashes.DoesAnyHashMatch(masterBundleConfig.hash))
					{
						masterBundleConfig.serverHashes = null;
						UnturnedLog.warn("Master bundle hash file does not match loaded: {0}", new object[]
						{
							masterBundleConfig.assetBundleName
						});
					}
				}
			}
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x00101908 File Offset: 0x000FFB08
		private static MasterBundleHash loadHashForBundle(MasterBundleConfig bundle)
		{
			if (bundle.sourceConfig != null)
			{
				bundle = bundle.sourceConfig;
			}
			string hashFilePath = bundle.getHashFilePath();
			if (!File.Exists(hashFilePath))
			{
				UnturnedLog.warn("Master bundle hashes file was deleted: {0}", new object[]
				{
					hashFilePath
				});
				return null;
			}
			byte[] array = File.ReadAllBytes(hashFilePath);
			if (array.Length < 1)
			{
				UnturnedLog.warn("Master bundle hashes file is empty: {0}", new object[]
				{
					hashFilePath
				});
				return null;
			}
			int num;
			if (array.Length == 60)
			{
				num = 1;
			}
			else
			{
				num = (int)array[0];
				if (num != 2)
				{
					UnturnedLog.warn("Master bundle hash file is an unknown version ({0}): {1}", new object[]
					{
						num,
						hashFilePath
					});
					return null;
				}
				if (array.Length != 61)
				{
					UnturnedLog.warn("Master bundle hash file is the wrong size ({0}): {1}", new object[]
					{
						array.Length,
						hashFilePath
					});
					return null;
				}
			}
			MasterBundleHash masterBundleHash = new MasterBundleHash();
			masterBundleHash.windowsHash = new byte[20];
			masterBundleHash.macHash = new byte[20];
			masterBundleHash.linuxHash = new byte[20];
			int num2 = 0;
			if (num > 1)
			{
				num2 = 1;
			}
			Array.Copy(array, num2, masterBundleHash.windowsHash, 0, 20);
			num2 += 20;
			Array.Copy(array, num2, masterBundleHash.linuxHash, 0, 20);
			num2 += 20;
			Array.Copy(array, num2, masterBundleHash.macHash, 0, 20);
			return masterBundleHash;
		}
	}
}
