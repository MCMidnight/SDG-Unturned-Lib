using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using SDG.Unturned;

namespace UnityEngine
{
	// Token: 0x02000010 RID: 16
	public static class CharacterControllerExtension
	{
		/// <summary>
		/// Does initialOverlaps array contain hit collider?
		/// </summary>
		// Token: 0x06000034 RID: 52 RVA: 0x00002DC4 File Offset: 0x00000FC4
		private static bool wasHitInitialOverlap(RaycastHit hit, int initialOverlapCount)
		{
			for (int i = 0; i < initialOverlapCount; i++)
			{
				if (hit.collider == CharacterControllerExtension.initialOverlaps[i])
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Does initialOverlaps array contain every hit collider?
		/// </summary>
		// Token: 0x06000035 RID: 53 RVA: 0x00002DF8 File Offset: 0x00000FF8
		private static bool wereAllHitsInitialOverlaps(int hitCount, int initialOverlapCount)
		{
			for (int i = 0; i < hitCount; i++)
			{
				if (!CharacterControllerExtension.wasHitInitialOverlap(CharacterControllerExtension.results[i], initialOverlapCount))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Perform a move, then do a capsule cast to determine if Unity PhysX went through a wall.
		///
		/// Required when disabling overlap recovery because there are issues when walking toward slopes that bend inward.
		/// To test if Unity works properly in the future; walk toward the inside of a barracks building in the PEI base.
		/// </summary>
		// Token: 0x06000036 RID: 54 RVA: 0x00002E28 File Offset: 0x00001028
		public static void CheckedMove(this CharacterController component, Vector3 motion)
		{
			Vector3 position = component.transform.position;
			component.Move(motion);
			Vector3 a = component.transform.position - position;
			float sqrMagnitude = a.sqrMagnitude;
			if (sqrMagnitude < 1E-05f)
			{
				return;
			}
			float num = Mathf.Sqrt(sqrMagnitude);
			Vector3 direction = a / num;
			float num2 = component.height / 3f;
			float num3 = component.radius / 2f;
			Vector3 b = new Vector3(0f, num2 - num3, 0f);
			Vector3 a2 = position + component.center;
			Vector3 vector = a2 - b;
			Vector3 vector2 = a2 + b;
			int layerMask = 406437888;
			int num4 = Physics.OverlapCapsuleNonAlloc(vector, vector2, num3, CharacterControllerExtension.initialOverlaps, layerMask, QueryTriggerInteraction.Ignore);
			if (num4 >= CharacterControllerExtension.initialOverlaps.Length)
			{
				return;
			}
			int num5 = Physics.CapsuleCastNonAlloc(vector, vector2, num3, direction, CharacterControllerExtension.results, num, layerMask, QueryTriggerInteraction.Ignore);
			if (num5 >= CharacterControllerExtension.results.Length)
			{
				return;
			}
			if (num5 > 0)
			{
				if (num4 > 0 && CharacterControllerExtension.wereAllHitsInitialOverlaps(num5, num4))
				{
					return;
				}
				component.transform.position = position;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002F3C File Offset: 0x0000113C
		private static void removePendingChange(CharacterController component)
		{
			for (int i = CharacterControllerExtension.pendingChanges.Count - 1; i >= 0; i--)
			{
				if (CharacterControllerExtension.pendingChanges[i].component == component)
				{
					CharacterControllerExtension.pendingChanges.RemoveAtFast(i);
					return;
				}
			}
		}

		/// <summary>
		/// Set detectCollisions to false and cancel deferred requests to enable.
		/// </summary>
		// Token: 0x06000038 RID: 56 RVA: 0x00002F84 File Offset: 0x00001184
		public static void DisableDetectCollisions(this CharacterController component)
		{
			component.detectCollisions = false;
			CharacterControllerExtension.removePendingChange(component);
		}

		/// <summary>
		/// Set detectCollisions to true on the next frame.
		/// Useful when CharacterController is teleported to prevent adding huge forces to overlapping rigidbodies.
		/// </summary>
		// Token: 0x06000039 RID: 57 RVA: 0x00002F93 File Offset: 0x00001193
		public static void EnableDetectCollisionsNextFrame(this CharacterController component)
		{
			CharacterControllerExtension.removePendingChange(component);
			CharacterControllerExtension.pendingChanges.Add(new CharacterControllerExtension.PendingEnableRigidbody(component));
		}

		/// <summary>
		/// If true EnableDetectCollisionsNextFrame, if false DisableDetectCollisions.
		/// </summary>
		// Token: 0x0600003A RID: 58 RVA: 0x00002FAB File Offset: 0x000011AB
		public static void SetDetectCollisionsDeferred(this CharacterController component, bool detectCollisions)
		{
			if (detectCollisions)
			{
				component.EnableDetectCollisionsNextFrame();
				return;
			}
			component.DisableDetectCollisions();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002FBD File Offset: 0x000011BD
		public static void DisableDetectCollisionsUntilNextFrame(this CharacterController component)
		{
			component.DisableDetectCollisions();
			component.EnableDetectCollisionsNextFrame();
		}

		/// <summary>
		/// Intentionally Update, not FixedUpdate. Physics transforms are applied between frames, whereas at low frame
		/// rates there may be multiple FixedUpdates per frame.
		/// </summary>
		// Token: 0x0600003C RID: 60 RVA: 0x00002FCC File Offset: 0x000011CC
		private static void OnUpdate()
		{
			int frameCount = Time.frameCount;
			for (int i = CharacterControllerExtension.pendingChanges.Count - 1; i >= 0; i--)
			{
				if (frameCount >= CharacterControllerExtension.pendingChanges[i].frameNumber)
				{
					CharacterController component = CharacterControllerExtension.pendingChanges[i].component;
					if (component != null)
					{
						component.detectCollisions = true;
					}
					CharacterControllerExtension.pendingChanges.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003038 File Offset: 0x00001238
		static CharacterControllerExtension()
		{
			TimeUtility.updated += CharacterControllerExtension.OnUpdate;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(CharacterControllerExtension.OnLogMemoryUsage));
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003096 File Offset: 0x00001296
		private static void OnLogMemoryUsage(List<string> results)
		{
			results.Add(string.Format("Character controller pending changes: {0}", CharacterControllerExtension.pendingChanges.Count));
		}

		// Token: 0x04000024 RID: 36
		private const int CHECKED_MOVE_BUFFER_SIZE = 8;

		// Token: 0x04000025 RID: 37
		private static Collider[] initialOverlaps = new Collider[8];

		// Token: 0x04000026 RID: 38
		private static RaycastHit[] results = new RaycastHit[8];

		// Token: 0x04000027 RID: 39
		private static List<CharacterControllerExtension.PendingEnableRigidbody> pendingChanges = new List<CharacterControllerExtension.PendingEnableRigidbody>();

		// Token: 0x0200082C RID: 2092
		private struct PendingEnableRigidbody
		{
			// Token: 0x06004760 RID: 18272 RVA: 0x001ADA94 File Offset: 0x001ABC94
			public PendingEnableRigidbody(CharacterController component)
			{
				this.component = component;
				this.frameNumber = Time.frameCount + 1;
			}

			// Token: 0x0400311D RID: 12573
			public CharacterController component;

			// Token: 0x0400311E RID: 12574
			public int frameNumber;
		}
	}
}
