using System;
using SDG.Framework.IO.Deserialization;
using SDG.Framework.IO.Serialization;
using SDG.Unturned;

namespace SDG.Framework.IO
{
	// Token: 0x020000AF RID: 175
	public class IOUtility
	{
		/// <summary>
		/// Path to the folder which contains the Unity player executable.
		/// </summary>
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x0001347D File Offset: 0x0001167D
		[Obsolete("Replaced by UnturnedPaths.RootDirectory")]
		public static string rootPath
		{
			get
			{
				return UnturnedPaths.RootDirectory.FullName;
			}
		}

		// Token: 0x040001E2 RID: 482
		public static IDeserializer jsonDeserializer = new JSONDeserializer();

		// Token: 0x040001E3 RID: 483
		public static ISerializer jsonSerializer = new JSONSerializer();

		// Token: 0x040001E4 RID: 484
		public static IDeserializer xmlDeserializer = new XMLDeserializer();

		// Token: 0x040001E5 RID: 485
		public static ISerializer xmlSerializer = new XMLSerializer();
	}
}
