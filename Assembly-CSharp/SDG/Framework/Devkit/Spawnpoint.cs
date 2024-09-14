using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000129 RID: 297
	public class Spawnpoint : TempNodeBase
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0001BAAD File Offset: 0x00019CAD
		// (set) Token: 0x06000792 RID: 1938 RVA: 0x0001BAB5 File Offset: 0x00019CB5
		public SphereCollider sphere { get; protected set; }

		// Token: 0x06000793 RID: 1939 RVA: 0x0001BABE File Offset: 0x00019CBE
		internal override ISleekElement CreateMenu()
		{
			return new Spawnpoint.Menu(this);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0001BAC6 File Offset: 0x00019CC6
		internal void UpdateEditorVisibility()
		{
			this.sphere.enabled = SpawnpointSystemV2.Get().IsVisible;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001BADD File Offset: 0x00019CDD
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.id = reader.readValue<string>("ID");
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001BAF7 File Offset: 0x00019CF7
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue("ID", this.id);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001BB11 File Offset: 0x00019D11
		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			SpawnpointSystemV2.Get().AddSpawnpoint(this);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001BB24 File Offset: 0x00019D24
		protected void OnDisable()
		{
			SpawnpointSystemV2.Get().RemoveSpawnpoint(this);
			LevelHierarchy.removeItem(this);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001BB38 File Offset: 0x00019D38
		protected void Awake()
		{
			base.name = "Spawnpoint";
			base.gameObject.layer = 30;
			if (Level.isEditor)
			{
				this.sphere = base.gameObject.GetOrAddComponent<SphereCollider>();
				this.sphere.radius = 0.5f;
				this.UpdateEditorVisibility();
			}
		}

		// Token: 0x040002C5 RID: 709
		public string id;

		// Token: 0x0200086F RID: 2159
		private class Menu : SleekWrapper
		{
			// Token: 0x0600482B RID: 18475 RVA: 0x001AF388 File Offset: 0x001AD588
			public Menu(Spawnpoint node)
			{
				this.node = node;
				base.SizeOffset_X = 400f;
				float num = 0f;
				ISleekField sleekField = Glazier.Get().CreateStringField();
				sleekField.PositionOffset_Y = num;
				sleekField.SizeOffset_X = 200f;
				sleekField.SizeOffset_Y = 30f;
				sleekField.Text = node.id;
				sleekField.AddLabel("ID", 1);
				sleekField.OnTextChanged += new Typed(this.OnIdTyped);
				base.AddChild(sleekField);
				num += sleekField.SizeOffset_Y + 10f;
				base.SizeOffset_Y = num - 10f;
			}

			// Token: 0x0600482C RID: 18476 RVA: 0x001AF428 File Offset: 0x001AD628
			private void OnIdTyped(ISleekField field, string state)
			{
				this.node.id = state;
			}

			// Token: 0x04003179 RID: 12665
			private Spawnpoint node;
		}
	}
}
