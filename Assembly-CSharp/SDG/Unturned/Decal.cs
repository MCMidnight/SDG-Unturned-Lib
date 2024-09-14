using System;
using SDG.Framework.Devkit.Interactable;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003F6 RID: 1014
	public class Decal : MonoBehaviour, IDevkitInteractableBeginSelectionHandler, IDevkitInteractableEndSelectionHandler
	{
		// Token: 0x06001DFA RID: 7674 RVA: 0x0006D3A0 File Offset: 0x0006B5A0
		public virtual void beginSelection(InteractionData data)
		{
			this.isSelected = true;
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x0006D3A9 File Offset: 0x0006B5A9
		public virtual void endSelection(InteractionData data)
		{
			this.isSelected = false;
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0006D3B4 File Offset: 0x0006B5B4
		private MeshRenderer getMesh()
		{
			MeshRenderer component = base.transform.parent.GetComponent<MeshRenderer>();
			if (component == null)
			{
				Transform transform = base.transform.parent.Find("Mesh");
				if (transform != null)
				{
					component = transform.GetComponent<MeshRenderer>();
				}
			}
			return component;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x0006D404 File Offset: 0x0006B604
		private void onGraphicsSettingsApplied()
		{
			MeshRenderer mesh = this.getMesh();
			if (mesh != null)
			{
				mesh.enabled = (GraphicsSettings.renderMode == ERenderMode.FORWARD);
			}
			if (GraphicsSettings.renderMode == ERenderMode.DEFERRED)
			{
				DecalSystem.add(this);
				return;
			}
			DecalSystem.remove(this);
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x0006D443 File Offset: 0x0006B643
		internal void UpdateEditorVisibility()
		{
			if (this.box != null)
			{
				if (Level.isEditor)
				{
					this.box.enabled = DecalSystem.IsVisible;
					return;
				}
				this.box.enabled = false;
			}
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x0006D477 File Offset: 0x0006B677
		private void Awake()
		{
			this.box = base.transform.parent.GetComponent<BoxCollider>();
			this.UpdateEditorVisibility();
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x0006D498 File Offset: 0x0006B698
		private void Start()
		{
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x0006D4A5 File Offset: 0x0006B6A5
		private void OnEnable()
		{
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x0006D4A7 File Offset: 0x0006B6A7
		private void OnDisable()
		{
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x0006D4A9 File Offset: 0x0006B6A9
		private void DrawGizmo(bool selected)
		{
			Gizmos.color = (selected ? Color.yellow : Color.red);
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x0006D4DE File Offset: 0x0006B6DE
		private void OnDrawGizmos()
		{
			this.DrawGizmo(false);
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x0006D4E7 File Offset: 0x0006B6E7
		private void OnDrawGizmosSelected()
		{
			this.DrawGizmo(true);
		}

		// Token: 0x04000E5B RID: 3675
		public EDecalType type;

		// Token: 0x04000E5C RID: 3676
		public Material material;

		// Token: 0x04000E5D RID: 3677
		public bool isSelected;

		// Token: 0x04000E5E RID: 3678
		public float lodBias = 1f;

		// Token: 0x04000E5F RID: 3679
		protected BoxCollider box;
	}
}
