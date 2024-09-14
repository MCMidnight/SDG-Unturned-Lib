using System;
using System.IO;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	// Token: 0x0200042F RID: 1071
	public static class UnturnedPaths
	{
		// Token: 0x060020B0 RID: 8368 RVA: 0x0007E2A1 File Offset: 0x0007C4A1
		static UnturnedPaths()
		{
			if (UnityPaths.ProjectDirectory != null)
			{
				UnturnedPaths.RootDirectory = UnityPaths.ProjectDirectory.CreateSubdirectory("Builds").CreateSubdirectory("Shared");
				return;
			}
			UnturnedPaths.RootDirectory = UnityPaths.GameDirectory;
		}

		/// <summary>
		/// Directory the game files are installed in. For the editor this is the /Builds/Shared directory.
		/// Windows and Linux: contains the executable and the Unturned_Data directory.
		/// MacOS: contains the Unturned.app bundle.
		/// </summary>
		// Token: 0x04000FCD RID: 4045
		public static readonly DirectoryInfo RootDirectory;
	}
}
