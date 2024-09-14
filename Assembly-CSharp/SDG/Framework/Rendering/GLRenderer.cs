using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Rendering
{
	// Token: 0x02000088 RID: 136
	public class GLRenderer : MonoBehaviour
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600034E RID: 846 RVA: 0x0000CA50 File Offset: 0x0000AC50
		// (remove) Token: 0x0600034F RID: 847 RVA: 0x0000CA84 File Offset: 0x0000AC84
		public static event GLRenderHandler render;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000350 RID: 848 RVA: 0x0000CAB8 File Offset: 0x0000ACB8
		// (remove) Token: 0x06000351 RID: 849 RVA: 0x0000CAEC File Offset: 0x0000ACEC
		public static event GLRenderHandler OnGameRender;

		// Token: 0x06000352 RID: 850 RVA: 0x0000CB20 File Offset: 0x0000AD20
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (Level.isEditor)
			{
				flag2 = (GLRenderer.render != null);
				if (EditorUI.window == null || !EditorUI.window.isEnabled)
				{
					flag2 = false;
				}
				flag = (flag || flag2);
			}
			else
			{
				flag3 = (GLRenderer.OnGameRender != null);
				if (PlayerUI.window == null || !PlayerUI.window.isEnabled)
				{
					flag3 = false;
				}
				flag = (flag || flag3);
			}
			flag4 = RuntimeGizmos.Get().HasQueuedElements;
			flag = (flag || flag4);
			if (flag)
			{
				RenderTexture.active = destination;
				if (flag2)
				{
					GL.PushMatrix();
					try
					{
						GLRenderer.render();
					}
					catch (Exception e)
					{
						UnturnedLog.exception(e);
					}
					GL.PopMatrix();
				}
				if (flag3)
				{
					GL.PushMatrix();
					try
					{
						GLRenderer.OnGameRender();
					}
					catch (Exception e2)
					{
						UnturnedLog.exception(e2);
					}
					GL.PopMatrix();
				}
				if (flag4)
				{
					GL.PushMatrix();
					try
					{
						RuntimeGizmos.Get().Render();
					}
					catch (Exception e3)
					{
						UnturnedLog.exception(e3);
					}
					GL.PopMatrix();
				}
				RenderTexture.active = null;
			}
		}
	}
}
