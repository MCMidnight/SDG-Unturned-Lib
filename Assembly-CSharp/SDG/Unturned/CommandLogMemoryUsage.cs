using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003B1 RID: 945
	public class CommandLogMemoryUsage : Command
	{
		// Token: 0x06001D05 RID: 7429 RVA: 0x000688C4 File Offset: 0x00066AC4
		protected override void execute(CSteamID executorID, string parameter)
		{
			List<string> list = new List<string>();
			Action<List<string>> onExecuted = CommandLogMemoryUsage.OnExecuted;
			if (onExecuted != null)
			{
				onExecuted.Invoke(list);
			}
			foreach (Type type in new Type[]
			{
				typeof(GameObject),
				typeof(AudioSource),
				typeof(ParticleSystem),
				typeof(Collider),
				typeof(Rigidbody),
				typeof(Renderer),
				typeof(MeshRenderer),
				typeof(SkinnedMeshRenderer),
				typeof(Animation),
				typeof(Animator),
				typeof(Camera),
				typeof(Light),
				typeof(LODGroup)
			})
			{
				Object[] array2 = Object.FindObjectsOfType(type, true);
				list.Add(string.Format("{0}(s) in scene: {1}", type.Name, array2.Length));
			}
			foreach (Type type2 in new Type[]
			{
				typeof(Object),
				typeof(GameObject),
				typeof(Texture),
				typeof(AudioClip),
				typeof(AnimationClip),
				typeof(Mesh)
			})
			{
				Object[] array3 = Resources.FindObjectsOfTypeAll(type2);
				list.Add(string.Format("{0}(s) in resources: {1}", type2.Name, array3.Length));
			}
			CommandWindow.Log(string.Format("{0} memory usage result(s):", list.Count));
			for (int j = 0; j < list.Count; j++)
			{
				CommandWindow.Log(string.Format("[{0}] {1}", j, list[j]));
			}
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x00068AB5 File Offset: 0x00066CB5
		public CommandLogMemoryUsage(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = "LogMemoryUsage";
			this._info = string.Empty;
			this._help = string.Empty;
		}

		// Token: 0x04000DC3 RID: 3523
		internal static Action<List<string>> OnExecuted;
	}
}
