using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000FF RID: 255
	public class FoliageVolume : LevelVolume<FoliageVolume, FoliageVolumeManager>
	{
		// Token: 0x06000692 RID: 1682 RVA: 0x00019694 File Offset: 0x00017894
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new FoliageVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x000196B0 File Offset: 0x000178B0
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x000196B8 File Offset: 0x000178B8
		public FoliageVolume.EFoliageVolumeMode mode
		{
			get
			{
				return this._mode;
			}
			set
			{
				if (!base.enabled)
				{
					this._mode = value;
					return;
				}
				base.GetVolumeManager().RemoveVolume(this);
				this._mode = value;
				base.GetVolumeManager().AddVolume(this);
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x000196EC File Offset: 0x000178EC
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.mode = reader.readValue<FoliageVolume.EFoliageVolumeMode>("Mode");
			if (reader.containsKey("Instanced_Meshes"))
			{
				this.instancedMeshes = reader.readValue<bool>("Instanced_Meshes");
			}
			if (reader.containsKey("Resources"))
			{
				this.resources = reader.readValue<bool>("Resources");
			}
			if (reader.containsKey("Objects"))
			{
				this.objects = reader.readValue<bool>("Objects");
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001976C File Offset: 0x0001796C
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<FoliageVolume.EFoliageVolumeMode>("Mode", this.mode);
			writer.writeValue<bool>("Instanced_Meshes", this.instancedMeshes);
			writer.writeValue<bool>("Resources", this.resources);
			writer.writeValue<bool>("Objects", this.objects);
		}

		// Token: 0x04000280 RID: 640
		[SerializeField]
		protected FoliageVolume.EFoliageVolumeMode _mode = FoliageVolume.EFoliageVolumeMode.SUBTRACTIVE;

		// Token: 0x04000281 RID: 641
		public bool instancedMeshes = true;

		// Token: 0x04000282 RID: 642
		public bool resources = true;

		// Token: 0x04000283 RID: 643
		public bool objects = true;

		// Token: 0x02000867 RID: 2151
		public enum EFoliageVolumeMode
		{
			// Token: 0x0400316F RID: 12655
			ADDITIVE,
			// Token: 0x04003170 RID: 12656
			SUBTRACTIVE
		}

		// Token: 0x02000868 RID: 2152
		private class Menu : SleekWrapper
		{
			// Token: 0x06004808 RID: 18440 RVA: 0x001AE410 File Offset: 0x001AC610
			public Menu(FoliageVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				base.SizeOffset_Y = 160f;
				SleekButtonState sleekButtonState = new SleekButtonState(new GUIContent[]
				{
					new GUIContent("Additive"),
					new GUIContent("Subtractive")
				});
				sleekButtonState.SizeOffset_X = 200f;
				sleekButtonState.SizeOffset_Y = 30f;
				sleekButtonState.AddLabel("Mode", 1);
				sleekButtonState.state = ((volume.mode == FoliageVolume.EFoliageVolumeMode.ADDITIVE) ? 0 : 1);
				SleekButtonState sleekButtonState2 = sleekButtonState;
				sleekButtonState2.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState2.onSwappedState, new SwappedState(this.OnSwappedMode));
				base.AddChild(sleekButtonState);
				ISleekToggle sleekToggle = Glazier.Get().CreateToggle();
				sleekToggle.PositionOffset_Y = 40f;
				sleekToggle.SizeOffset_X = 40f;
				sleekToggle.SizeOffset_Y = 40f;
				sleekToggle.Value = volume.instancedMeshes;
				sleekToggle.AddLabel("Instanced Meshes", 1);
				sleekToggle.OnValueChanged += new Toggled(this.OnInstancedMeshesToggled);
				base.AddChild(sleekToggle);
				ISleekToggle sleekToggle2 = Glazier.Get().CreateToggle();
				sleekToggle2.PositionOffset_Y = 80f;
				sleekToggle2.SizeOffset_X = 40f;
				sleekToggle2.SizeOffset_Y = 40f;
				sleekToggle2.Value = volume.resources;
				sleekToggle2.AddLabel("Resources", 1);
				sleekToggle2.OnValueChanged += new Toggled(this.OnResourcesToggled);
				base.AddChild(sleekToggle2);
				ISleekToggle sleekToggle3 = Glazier.Get().CreateToggle();
				sleekToggle3.PositionOffset_Y = 120f;
				sleekToggle3.SizeOffset_X = 40f;
				sleekToggle3.SizeOffset_Y = 40f;
				sleekToggle3.Value = volume.objects;
				sleekToggle3.AddLabel("Objects", 1);
				sleekToggle3.OnValueChanged += new Toggled(this.OnObjectsToggled);
				base.AddChild(sleekToggle3);
			}

			// Token: 0x06004809 RID: 18441 RVA: 0x001AE5DA File Offset: 0x001AC7DA
			private void OnSwappedMode(SleekButtonState button, int state)
			{
				this.volume.mode = ((state == 0) ? FoliageVolume.EFoliageVolumeMode.ADDITIVE : FoliageVolume.EFoliageVolumeMode.SUBTRACTIVE);
			}

			// Token: 0x0600480A RID: 18442 RVA: 0x001AE5EE File Offset: 0x001AC7EE
			private void OnInstancedMeshesToggled(ISleekToggle toggle, bool state)
			{
				this.volume.instancedMeshes = state;
			}

			// Token: 0x0600480B RID: 18443 RVA: 0x001AE5FC File Offset: 0x001AC7FC
			private void OnResourcesToggled(ISleekToggle toggle, bool state)
			{
				this.volume.resources = state;
			}

			// Token: 0x0600480C RID: 18444 RVA: 0x001AE60A File Offset: 0x001AC80A
			private void OnObjectsToggled(ISleekToggle toggle, bool state)
			{
				this.volume.objects = state;
			}

			// Token: 0x04003171 RID: 12657
			private FoliageVolume volume;
		}
	}
}
