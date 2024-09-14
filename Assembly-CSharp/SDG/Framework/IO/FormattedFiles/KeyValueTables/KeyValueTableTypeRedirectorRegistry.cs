using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	// Token: 0x020000C2 RID: 194
	public static class KeyValueTableTypeRedirectorRegistry
	{
		/// <summary>
		/// If the type name has been redirected this method will be called recursively until the most recent name is found and returned.
		/// </summary>
		// Token: 0x06000542 RID: 1346 RVA: 0x00014920 File Offset: 0x00012B20
		public static string chase(string assemblyQualifiedName)
		{
			string assemblyQualifiedName2;
			if (KeyValueTableTypeRedirectorRegistry.redirects.TryGetValue(assemblyQualifiedName, ref assemblyQualifiedName2))
			{
				return KeyValueTableTypeRedirectorRegistry.chase(assemblyQualifiedName2);
			}
			return assemblyQualifiedName;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00014944 File Offset: 0x00012B44
		public static void add(string oldAssemblyQualifiedName, string newAssemblyQualifiedName)
		{
			KeyValueTableTypeRedirectorRegistry.redirects.Add(oldAssemblyQualifiedName, newAssemblyQualifiedName);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00014952 File Offset: 0x00012B52
		public static void remove(string oldAssemblyQualifiedName)
		{
			KeyValueTableTypeRedirectorRegistry.redirects.Remove(oldAssemblyQualifiedName);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00014960 File Offset: 0x00012B60
		static KeyValueTableTypeRedirectorRegistry()
		{
			KeyValueTableTypeRedirectorRegistry.add("SDG.Framework.Landscapes.PlayerClipVolume, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", typeof(PlayerClipVolume).AssemblyQualifiedName);
			KeyValueTableTypeRedirectorRegistry.add("SDG.Framework.Foliage.KillVolume, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", typeof(KillVolume).AssemblyQualifiedName);
		}

		// Token: 0x040001FC RID: 508
		private static Dictionary<string, string> redirects = new Dictionary<string, string>();
	}
}
