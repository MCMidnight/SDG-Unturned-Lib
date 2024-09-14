using System;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Transactions;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000400 RID: 1024
	internal class EditorInteract : MonoBehaviour
	{
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001E3A RID: 7738 RVA: 0x0006DE6A File Offset: 0x0006C06A
		public static bool isFlying
		{
			get
			{
				return EditorInteract._isFlying;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001E3B RID: 7739 RVA: 0x0006DE71 File Offset: 0x0006C071
		public static Ray ray
		{
			get
			{
				return EditorInteract._ray;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001E3C RID: 7740 RVA: 0x0006DE78 File Offset: 0x0006C078
		public static RaycastHit worldHit
		{
			get
			{
				return EditorInteract._worldHit;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x0006DE7F File Offset: 0x0006C07F
		public static RaycastHit objectHit
		{
			get
			{
				return EditorInteract._objectHit;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001E3E RID: 7742 RVA: 0x0006DE86 File Offset: 0x0006C086
		public static RaycastHit logicHit
		{
			get
			{
				return EditorInteract._logicHit;
			}
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0006DE8D File Offset: 0x0006C08D
		public void SetActiveTool(IDevkitTool tool)
		{
			if (this.activeTool != null)
			{
				this.activeTool.dequip();
			}
			this.activeTool = tool;
			if (this.activeTool != null)
			{
				this.activeTool.equip();
			}
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x0006DEBC File Offset: 0x0006C0BC
		private void Update()
		{
			if (Glazier.Get().ShouldGameProcessInput)
			{
				EditorInteract._isFlying = InputEx.GetKey(ControlsSettings.secondary);
			}
			else
			{
				EditorInteract._isFlying = false;
			}
			EditorInteract._ray = MainCamera.instance.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(EditorInteract.ray, out EditorInteract._worldHit, 2048f, RayMasks.EDITOR_WORLD);
			Physics.Raycast(EditorInteract.ray, out EditorInteract._objectHit, 2048f, RayMasks.EDITOR_INTERACT);
			Physics.Raycast(EditorInteract.ray, out EditorInteract._logicHit, 2048f, RayMasks.EDITOR_LOGIC);
			if (InputEx.GetKeyDown(KeyCode.S) && InputEx.GetKey(KeyCode.LeftControl))
			{
				Level.save();
			}
			if (InputEx.GetKeyDown(KeyCode.F1))
			{
				LevelVisibility.roadsVisible = !LevelVisibility.roadsVisible;
				EditorLevelVisibilityUI.roadsToggle.Value = LevelVisibility.roadsVisible;
			}
			if (InputEx.GetKeyDown(KeyCode.F2))
			{
				LevelVisibility.navigationVisible = !LevelVisibility.navigationVisible;
				EditorLevelVisibilityUI.navigationToggle.Value = LevelVisibility.navigationVisible;
			}
			if (InputEx.GetKeyDown(KeyCode.F3))
			{
				LevelVisibility.nodesVisible = !LevelVisibility.nodesVisible;
				EditorLevelVisibilityUI.nodesToggle.Value = LevelVisibility.nodesVisible;
			}
			if (InputEx.GetKeyDown(KeyCode.F4))
			{
				LevelVisibility.itemsVisible = !LevelVisibility.itemsVisible;
				EditorLevelVisibilityUI.itemsToggle.Value = LevelVisibility.itemsVisible;
			}
			if (InputEx.GetKeyDown(KeyCode.F5))
			{
				LevelVisibility.playersVisible = !LevelVisibility.playersVisible;
				EditorLevelVisibilityUI.playersToggle.Value = LevelVisibility.playersVisible;
			}
			if (InputEx.GetKeyDown(KeyCode.F6))
			{
				LevelVisibility.zombiesVisible = !LevelVisibility.zombiesVisible;
				EditorLevelVisibilityUI.zombiesToggle.Value = LevelVisibility.zombiesVisible;
			}
			if (InputEx.GetKeyDown(KeyCode.F7))
			{
				LevelVisibility.vehiclesVisible = !LevelVisibility.vehiclesVisible;
				EditorLevelVisibilityUI.vehiclesToggle.Value = LevelVisibility.vehiclesVisible;
			}
			if (InputEx.GetKeyDown(KeyCode.F8))
			{
				LevelVisibility.borderVisible = !LevelVisibility.borderVisible;
				EditorLevelVisibilityUI.borderToggle.Value = LevelVisibility.borderVisible;
			}
			if (InputEx.GetKeyDown(KeyCode.F9))
			{
				LevelVisibility.animalsVisible = !LevelVisibility.animalsVisible;
				EditorLevelVisibilityUI.animalsToggle.Value = LevelVisibility.animalsVisible;
			}
			if (this.activeTool != null)
			{
				this.activeTool.update();
				if (InputEx.GetKeyDown(KeyCode.Z) && InputEx.GetKey(KeyCode.LeftControl))
				{
					if (InputEx.GetKey(KeyCode.LeftShift))
					{
						DevkitTransactionManager.redo();
						return;
					}
					DevkitTransactionManager.undo();
				}
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0006E111 File Offset: 0x0006C311
		private void OnDisable()
		{
			this.SetActiveTool(null);
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0006E11A File Offset: 0x0006C31A
		private void Start()
		{
			EditorInteract.load();
			EditorInteract.instance = this;
			this.terrainTool = new TerrainEditor();
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0006E134 File Offset: 0x0006C334
		public static void load()
		{
			if (ReadWrite.fileExists(Level.info.path + "/Editor/Camera.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Camera.dat", false, false, 1);
				MainCamera.instance.transform.parent.position = block.readSingleVector3();
				MainCamera.instance.transform.localRotation = Quaternion.Euler(block.readSingle(), 0f, 0f);
				MainCamera.instance.transform.parent.rotation = Quaternion.Euler(0f, block.readSingle(), 0f);
				return;
			}
			MainCamera.instance.transform.parent.position = new Vector3(0f, Level.TERRAIN, 0f);
			MainCamera.instance.transform.parent.rotation = Quaternion.identity;
			MainCamera.instance.transform.localRotation = Quaternion.identity;
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x0006E23C File Offset: 0x0006C43C
		public static void save()
		{
			Block block = new Block();
			block.writeByte(EditorInteract.SAVEDATA_VERSION);
			block.writeSingleVector3(MainCamera.instance.transform.position);
			block.writeSingle(EditorLook.pitch);
			block.writeSingle(EditorLook.yaw);
			ReadWrite.writeBlock(Level.info.path + "/Editor/Camera.dat", false, false, block);
		}

		// Token: 0x04000E7C RID: 3708
		public static readonly byte SAVEDATA_VERSION = 1;

		// Token: 0x04000E7D RID: 3709
		private static bool _isFlying;

		// Token: 0x04000E7E RID: 3710
		private static Ray _ray;

		// Token: 0x04000E7F RID: 3711
		private static RaycastHit _worldHit;

		// Token: 0x04000E80 RID: 3712
		private static RaycastHit _objectHit;

		// Token: 0x04000E81 RID: 3713
		private static RaycastHit _logicHit;

		// Token: 0x04000E82 RID: 3714
		public static EditorInteract instance;

		// Token: 0x04000E83 RID: 3715
		private IDevkitTool activeTool;

		// Token: 0x04000E84 RID: 3716
		public TerrainEditor terrainTool;
	}
}
