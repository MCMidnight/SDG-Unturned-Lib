using System;
using SDG.Framework.Devkit;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000159 RID: 345
	public class LocationDevkitNode : TempNodeBase
	{
		// Token: 0x060008A5 RID: 2213 RVA: 0x0001E508 File Offset: 0x0001C708
		internal override ISleekElement CreateMenu()
		{
			return new LocationDevkitNode.Menu(this);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001E510 File Offset: 0x0001C710
		internal void UpdateEditorVisibility()
		{
			this.boxCollider.enabled = SpawnpointSystemV2.Get().IsVisible;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0001E527 File Offset: 0x0001C727
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.locationName = reader.readValue<string>("LocationName");
			if (reader.containsKey("IsVisibleOnMap"))
			{
				this.isVisibleOnMap = reader.readValue<bool>("IsVisibleOnMap");
				return;
			}
			this.isVisibleOnMap = true;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0001E567 File Offset: 0x0001C767
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue("LocationName", this.locationName);
			writer.writeValue<bool>("IsVisibleOnMap", this.isVisibleOnMap);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0001E592 File Offset: 0x0001C792
		private void OnEnable()
		{
			LevelHierarchy.addItem(this);
			LocationDevkitNodeSystem.Get().AddNode(this);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0001E5A5 File Offset: 0x0001C7A5
		private void OnDisable()
		{
			LocationDevkitNodeSystem.Get().RemoveNode(this);
			LevelHierarchy.removeItem(this);
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001E5B8 File Offset: 0x0001C7B8
		private void Awake()
		{
			base.name = "Location";
			base.gameObject.layer = 30;
			if (Level.isEditor)
			{
				this.boxCollider = base.gameObject.GetOrAddComponent<BoxCollider>();
				this.boxCollider.size = new Vector3(1.5f, 1.5f, 1.5f);
				this.UpdateEditorVisibility();
			}
		}

		// Token: 0x04000350 RID: 848
		public string locationName;

		/// <summary>
		/// If true, visible in chart and satellite UIs.
		/// </summary>
		// Token: 0x04000351 RID: 849
		public bool isVisibleOnMap = true;

		// Token: 0x04000352 RID: 850
		[SerializeField]
		private BoxCollider boxCollider;

		// Token: 0x02000876 RID: 2166
		private class Menu : SleekWrapper
		{
			// Token: 0x06004847 RID: 18503 RVA: 0x001AF924 File Offset: 0x001ADB24
			public Menu(LocationDevkitNode node)
			{
				this.node = node;
				base.SizeOffset_X = 400f;
				float num = 0f;
				ISleekField sleekField = Glazier.Get().CreateStringField();
				sleekField.PositionOffset_Y = num;
				sleekField.SizeOffset_X = 200f;
				sleekField.SizeOffset_Y = 30f;
				sleekField.Text = node.locationName;
				sleekField.AddLabel("Name", 1);
				sleekField.OnTextChanged += new Typed(this.OnIdTyped);
				base.AddChild(sleekField);
				num += sleekField.SizeOffset_Y + 10f;
				ISleekToggle sleekToggle = Glazier.Get().CreateToggle();
				sleekToggle.PositionOffset_Y = num;
				sleekToggle.SizeOffset_X = 40f;
				sleekToggle.SizeOffset_Y = 40f;
				sleekToggle.Value = node.isVisibleOnMap;
				sleekToggle.AddLabel("Visible on map", 1);
				sleekToggle.OnValueChanged += new Toggled(this.OnVisibleOnMapToggled);
				base.AddChild(sleekToggle);
				num += sleekToggle.SizeOffset_Y + 10f;
				base.SizeOffset_Y = num - 10f;
			}

			// Token: 0x06004848 RID: 18504 RVA: 0x001AFA2C File Offset: 0x001ADC2C
			private void OnIdTyped(ISleekField field, string state)
			{
				this.node.locationName = state;
			}

			// Token: 0x06004849 RID: 18505 RVA: 0x001AFA3A File Offset: 0x001ADC3A
			private void OnVisibleOnMapToggled(ISleekToggle toggle, bool state)
			{
				this.node.isVisibleOnMap = state;
			}

			// Token: 0x04003188 RID: 12680
			private LocationDevkitNode node;
		}
	}
}
