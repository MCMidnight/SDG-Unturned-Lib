using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000365 RID: 869
	public static class ShaderConsolidator
	{
		/// <summary>
		/// Apply shader name redirects until a final name is found,
		/// and then load shader for compatible version of Unity.
		/// </summary>
		// Token: 0x06001A3E RID: 6718 RVA: 0x0005E968 File Offset: 0x0005CB68
		public static Shader findConsolidatedShader(Shader originalShader)
		{
			if (originalShader == null)
			{
				return null;
			}
			string name = originalShader.name;
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			throw new Exception(string.Format("Dedicated server trying to consolidate '{0}' shader", name));
		}

		/// <summary>
		/// Apply shader name redirects until a final name is found.
		/// Used to fix renamed shaders loaded from old asset bundles.
		/// </summary>
		// Token: 0x06001A3F RID: 6719 RVA: 0x0005E9A4 File Offset: 0x0005CBA4
		public static string redirectShaderName(string shaderName)
		{
			string shaderName2;
			if (ShaderConsolidator.SHADER_REDIRECTS.TryGetValue(shaderName, ref shaderName2))
			{
				return ShaderConsolidator.redirectShaderName(shaderName2);
			}
			return shaderName;
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x0005E9C8 File Offset: 0x0005CBC8
		// Note: this type is marked as 'beforefieldinit'.
		static ShaderConsolidator()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Particles/Additive", "Legacy Shaders/Particles/Additive");
			dictionary.Add("Particles/Additive (Soft)", "Legacy Shaders/Particles/Additive (Soft)");
			dictionary.Add("Particles/Alpha Blended", "Legacy Shaders/Particles/Alpha Blended");
			dictionary.Add("Particles/Anim Alpha Blended", "Legacy Shaders/Particles/Anim Alpha Blended");
			dictionary.Add("Particles/Alpha Blended Premultiply", "Legacy Shaders/Particles/Alpha Blended Premultiply");
			ShaderConsolidator.SHADER_REDIRECTS = dictionary;
		}

		/// <summary>
		/// Names of older shaders mapped to their renamed counterparts.
		/// Used to fix shaders loaded from old asset bundles.
		/// </summary>
		// Token: 0x04000C14 RID: 3092
		private static readonly Dictionary<string, string> SHADER_REDIRECTS;
	}
}
