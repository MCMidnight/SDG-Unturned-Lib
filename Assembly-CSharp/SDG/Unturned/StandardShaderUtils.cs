using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Standard shader mode changes are based on built-in StandardShaderGUI.cs 
	/// </summary>
	// Token: 0x0200036B RID: 875
	public static class StandardShaderUtils
	{
		/// <summary>
		/// Does shader name match any of the standard shaders?
		/// Standard, StandardSpecular and the Unturned "Decalable" variants all share nearly identical parameters.
		/// </summary>
		// Token: 0x06001A7E RID: 6782 RVA: 0x0005FB07 File Offset: 0x0005DD07
		public static bool isNameStandard(string name)
		{
			return !string.IsNullOrEmpty(name) && name.StartsWith("Standard") && (name.Length == 8 || name.EndsWith(" (Decalable)") || name.EndsWith(" (Specular setup)"));
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x0005FB45 File Offset: 0x0005DD45
		public static bool isMaterialUsingStandardShader(Material material)
		{
			return material != null && material.shader != null && StandardShaderUtils.isNameStandard(material.shader.name);
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x0005FB70 File Offset: 0x0005DD70
		public static bool isModeFade(Material material)
		{
			return material.IsKeywordEnabled("_ALPHABLEND_ON");
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x0005FB7D File Offset: 0x0005DD7D
		public static bool isModeTransparent(Material material)
		{
			return material.IsKeywordEnabled("_ALPHAPREMULTIPLY_ON");
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x0005FB8C File Offset: 0x0005DD8C
		public static void setModeToOpaque(Material material)
		{
			material.SetFloat("_Mode", 0f);
			material.SetOverrideTag("RenderType", "");
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 0);
			material.SetInt("_ZWrite", 1);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = -1;
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x0005FC08 File Offset: 0x0005DE08
		public static void setModeToCutout(Material material)
		{
			material.SetFloat("_Mode", 1f);
			material.SetOverrideTag("RenderType", "TransparentCutout");
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 0);
			material.SetInt("_ZWrite", 1);
			material.EnableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 2450;
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x0005FC88 File Offset: 0x0005DE88
		public static void setModeToFade(Material material)
		{
			material.SetFloat("_Mode", 2f);
			material.SetOverrideTag("RenderType", "Transparent");
			material.SetInt("_SrcBlend", 5);
			material.SetInt("_DstBlend", 10);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.EnableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x0005FD08 File Offset: 0x0005DF08
		public static void setModeToTransparent(Material material)
		{
			material.SetFloat("_Mode", 3f);
			material.SetOverrideTag("RenderType", "Transparent");
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 10);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;
		}

		/// <summary>
		/// Based on fixup routine in StandardShaderGUI SetMaterialKeywords.
		/// </summary>
		// Token: 0x06001A86 RID: 6790 RVA: 0x0005FD88 File Offset: 0x0005DF88
		public static void fixupEmission(Material material)
		{
			if (material.GetColor("_EmissionColor").maxColorComponent < 0.01f)
			{
				material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
			}
			else
			{
				material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
			}
			bool flag = (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == MaterialGlobalIlluminationFlags.None;
			if (flag)
			{
				flag = (material.GetTexture("_EmissionMap") != null);
			}
			if (flag)
			{
				material.EnableKeyword("_EMISSION");
				return;
			}
			material.DisableKeyword("_EMISSION");
		}

		/// <summary>
		/// Conditionally fixup older standard materials.
		/// </summary>
		/// <returns>True if material was edited.</returns>
		// Token: 0x06001A87 RID: 6791 RVA: 0x0005FDFC File Offset: 0x0005DFFC
		public static bool maybeFixupMaterial(Material material)
		{
			if (!StandardShaderUtils.isMaterialUsingStandardShader(material))
			{
				return false;
			}
			bool flag;
			if (StandardShaderUtils.isModeFade(material))
			{
				StandardShaderUtils.setModeToFade(material);
				flag = true;
			}
			else if (StandardShaderUtils.isModeTransparent(material))
			{
				StandardShaderUtils.setModeToTransparent(material);
				flag = true;
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				StandardShaderUtils.fixupEmission(material);
			}
			return true;
		}
	}
}
