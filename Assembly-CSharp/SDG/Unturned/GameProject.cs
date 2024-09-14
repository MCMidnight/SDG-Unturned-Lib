using System;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	// Token: 0x02000671 RID: 1649
	public class GameProject
	{
		/// <summary>
		/// Absolute path to project directory, e.g. C:/U3
		/// </summary>
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060036C1 RID: 14017 RVA: 0x00100D9D File Offset: 0x000FEF9D
		[Obsolete("Replaced by UnityPaths.ProjectDirectory")]
		public static string PROJECT_PATH
		{
			get
			{
				if (string.IsNullOrEmpty(GameProject._projectPath))
				{
					if (UnityPaths.ProjectDirectory != null)
					{
						GameProject._projectPath = UnityPaths.ProjectDirectory.FullName;
					}
					else
					{
						GameProject._projectPath = UnityPaths.GameDirectory.FullName;
					}
				}
				return GameProject._projectPath;
			}
		}

		// Token: 0x04002070 RID: 8304
		private static string _projectPath;
	}
}
