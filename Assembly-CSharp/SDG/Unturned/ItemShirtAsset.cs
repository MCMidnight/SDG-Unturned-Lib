using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Mesh Replacement Details
	/// .dat Flags:
	/// 	Has_1P_Character_Mesh_Override True		Bool
	/// 	Character_Mesh_3P_Override_LODs #		Int
	/// 	Has_Character_Material_Override True	Bool
	/// Asset Bundle Objects:
	/// 	Character_Mesh_1P_Override_#			GameObject with MeshFilter (mesh set to a skinned mesh)
	/// 	Character_Mesh_3P_Override_#			GameObject with MeshFilter (mesh set to a skinned mesh)
	/// 	Character_Material_Override				Material
	/// </summary>
	// Token: 0x020002FE RID: 766
	public class ItemShirtAsset : ItemBagAsset
	{
		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x00054A30 File Offset: 0x00052C30
		public Texture2D shirt
		{
			get
			{
				return this._shirt;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x00054A38 File Offset: 0x00052C38
		public Texture2D emission
		{
			get
			{
				return this._emission;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x00054A40 File Offset: 0x00052C40
		public Texture2D metallic
		{
			get
			{
				return this._metallic;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001714 RID: 5908 RVA: 0x00054A48 File Offset: 0x00052C48
		public bool ignoreHand
		{
			get
			{
				return this._ignoreHand;
			}
		}

		/// <summary>
		/// Replacements for the main 1st-person character mesh.
		/// </summary>
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001715 RID: 5909 RVA: 0x00054A50 File Offset: 0x00052C50
		// (set) Token: 0x06001716 RID: 5910 RVA: 0x00054A58 File Offset: 0x00052C58
		public Mesh[] characterMeshOverride1pLODs { get; protected set; }

		/// <summary>
		/// Replacements for the main 3rd-person character mesh.
		/// </summary>
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001717 RID: 5911 RVA: 0x00054A61 File Offset: 0x00052C61
		// (set) Token: 0x06001718 RID: 5912 RVA: 0x00054A69 File Offset: 0x00052C69
		public Mesh[] characterMeshOverride3pLODs { get; protected set; }

		// Token: 0x06001719 RID: 5913 RVA: 0x00054A74 File Offset: 0x00052C74
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.characterMeshOverride1pLODs = null;
			this.characterMeshOverride3pLODs = null;
			this.characterMaterialOverride = null;
			this._ignoreHand = data.ContainsKey("Ignore_Hand");
		}

		// Token: 0x04000A22 RID: 2594
		protected Texture2D _shirt;

		// Token: 0x04000A23 RID: 2595
		protected Texture2D _emission;

		// Token: 0x04000A24 RID: 2596
		protected Texture2D _metallic;

		// Token: 0x04000A25 RID: 2597
		protected bool _ignoreHand;

		/// <summary>
		/// Replacement for the main character material that typically has clothes and skin color.
		/// </summary>
		// Token: 0x04000A28 RID: 2600
		public Material characterMaterialOverride;
	}
}
