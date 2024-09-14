using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003FA RID: 1018
	public class Editor : MonoBehaviour
	{
		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x0006D9C4 File Offset: 0x0006BBC4
		public static Editor editor
		{
			get
			{
				return Editor._editor;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001E18 RID: 7704 RVA: 0x0006D9CB File Offset: 0x0006BBCB
		public EditorArea area
		{
			get
			{
				return this._area;
			}
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x0006D9D3 File Offset: 0x0006BBD3
		public virtual void init()
		{
			this._area = base.GetComponent<EditorArea>();
			Editor._editor = this;
			EditorCreated editorCreated = Editor.onEditorCreated;
			if (editorCreated == null)
			{
				return;
			}
			editorCreated();
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x0006D9F6 File Offset: 0x0006BBF6
		private void Start()
		{
			this.init();
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x0006D9FE File Offset: 0x0006BBFE
		public static void save()
		{
			EditorInteract.save();
			EditorObjects.save();
			EditorSpawns.save();
		}

		// Token: 0x04000E69 RID: 3689
		public static EditorCreated onEditorCreated;

		// Token: 0x04000E6A RID: 3690
		private static Editor _editor;

		// Token: 0x04000E6B RID: 3691
		private EditorArea _area;
	}
}
