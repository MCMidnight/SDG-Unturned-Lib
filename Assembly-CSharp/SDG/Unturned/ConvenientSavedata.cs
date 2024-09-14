using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Unturned equivalent of unity's PlayerPrefs.
	/// Convenient for saving one-off key-value pairs.
	/// </summary>
	// Token: 0x02000422 RID: 1058
	public static class ConvenientSavedata
	{
		// Token: 0x06001FB8 RID: 8120 RVA: 0x0007AA4C File Offset: 0x00078C4C
		public static IConvenientSavedata get()
		{
			if (ConvenientSavedata.instance == null)
			{
				ConvenientSavedata.load();
			}
			return ConvenientSavedata.instance;
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x0007AA60 File Offset: 0x00078C60
		private static void load()
		{
			if (ReadWrite.fileExists(ConvenientSavedata.RELATIVE_PATH, false, true))
			{
				try
				{
					ConvenientSavedata.instance = ReadWrite.deserializeJSON<ConvenientSavedataImplementation>(ConvenientSavedata.RELATIVE_PATH, false, true);
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "Unable to parse {0}! consider validating with a JSON linter", new object[]
					{
						ConvenientSavedata.RELATIVE_PATH
					});
					ConvenientSavedata.instance = null;
				}
				if (ConvenientSavedata.instance == null)
				{
					ConvenientSavedata.instance = new ConvenientSavedataImplementation();
					return;
				}
			}
			else
			{
				ConvenientSavedata.instance = new ConvenientSavedataImplementation();
			}
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0007AADC File Offset: 0x00078CDC
		public static void save()
		{
			if (ConvenientSavedata.instance == null)
			{
				UnturnedLog.info("Skipped saving convenient data");
				return;
			}
			ConvenientSavedata.instance.isDirty = false;
			try
			{
				ReadWrite.serializeJSON<ConvenientSavedataImplementation>(ConvenientSavedata.RELATIVE_PATH, false, true, ConvenientSavedata.instance);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception serializing convenient data:");
			}
			UnturnedLog.info("Saved convenient data");
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0007AB40 File Offset: 0x00078D40
		public static void SaveIfDirty()
		{
			if (ConvenientSavedata.instance != null && ConvenientSavedata.instance.isDirty)
			{
				ConvenientSavedata.instance.isDirty = false;
				try
				{
					ReadWrite.serializeJSON<ConvenientSavedataImplementation>(ConvenientSavedata.RELATIVE_PATH, false, true, ConvenientSavedata.instance);
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "Caught exception serializing convenient data:");
				}
				UnturnedLog.info("Saved convenient data (dirty)");
			}
		}

		// Token: 0x04000FA7 RID: 4007
		private static ConvenientSavedataImplementation instance = null;

		// Token: 0x04000FA8 RID: 4008
		private static readonly string RELATIVE_PATH = "/Cloud/ConvenientSavedata.json";
	}
}
