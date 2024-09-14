using System;
using SDG.Framework.Devkit;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000157 RID: 343
	public class AirdropDevkitNode : TempNodeBase
	{
		// Token: 0x06000895 RID: 2197 RVA: 0x0001E2C3 File Offset: 0x0001C4C3
		internal override ISleekElement CreateMenu()
		{
			return new AirdropDevkitNode.Menu(this);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001E2CB File Offset: 0x0001C4CB
		internal void UpdateEditorVisibility()
		{
			this.boxCollider.enabled = SpawnpointSystemV2.Get().IsVisible;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001E2E2 File Offset: 0x0001C4E2
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.id = reader.readValue<ushort>("SpawnTable_ID");
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001E2FC File Offset: 0x0001C4FC
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<ushort>("SpawnTable_ID", this.id);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001E316 File Offset: 0x0001C516
		private void OnEnable()
		{
			LevelHierarchy.addItem(this);
			AirdropDevkitNodeSystem.Get().AddNode(this);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0001E329 File Offset: 0x0001C529
		private void OnDisable()
		{
			AirdropDevkitNodeSystem.Get().RemoveNode(this);
			LevelHierarchy.removeItem(this);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001E33C File Offset: 0x0001C53C
		private void Awake()
		{
			base.name = "Airdrop";
			base.gameObject.layer = 30;
			if (Level.isEditor)
			{
				this.boxCollider = base.gameObject.GetOrAddComponent<BoxCollider>();
				this.boxCollider.center = new Vector3(0f, 16f, 0f);
				this.boxCollider.size = new Vector3(1f, 32f, 1f);
				this.UpdateEditorVisibility();
			}
		}

		// Token: 0x0400034B RID: 843
		public ushort id;

		// Token: 0x0400034C RID: 844
		[SerializeField]
		private BoxCollider boxCollider;

		// Token: 0x02000874 RID: 2164
		private class Menu : SleekWrapper
		{
			// Token: 0x0600483C RID: 18492 RVA: 0x001AF6F4 File Offset: 0x001AD8F4
			public Menu(AirdropDevkitNode node)
			{
				this.node = node;
				base.SizeOffset_X = 400f;
				float num = 0f;
				ISleekUInt16Field sleekUInt16Field = Glazier.Get().CreateUInt16Field();
				sleekUInt16Field.PositionOffset_Y = num;
				sleekUInt16Field.SizeOffset_X = 200f;
				sleekUInt16Field.SizeOffset_Y = 30f;
				sleekUInt16Field.Value = node.id;
				sleekUInt16Field.AddLabel("ID", 1);
				sleekUInt16Field.OnValueChanged += new TypedUInt16(this.OnIdTyped);
				base.AddChild(sleekUInt16Field);
				num += sleekUInt16Field.SizeOffset_Y + 10f;
				base.SizeOffset_Y = num - 10f;
			}

			// Token: 0x0600483D RID: 18493 RVA: 0x001AF794 File Offset: 0x001AD994
			private void OnIdTyped(ISleekUInt16Field field, ushort state)
			{
				this.node.id = state;
			}

			// Token: 0x04003182 RID: 12674
			private AirdropDevkitNode node;
		}
	}
}
