using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200038D RID: 909
	public class CharacterAnimator : MonoBehaviour
	{
		// Token: 0x06001C35 RID: 7221 RVA: 0x00064B7E File Offset: 0x00062D7E
		public void sample()
		{
			this.anim.Sample();
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x00064B8C File Offset: 0x00062D8C
		public void mixAnimation(string name)
		{
			AnimationState animationState = this.anim[name];
			if (animationState != null)
			{
				animationState.layer = 1;
			}
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x00064BB6 File Offset: 0x00062DB6
		public void mixAnimation(string name, bool mixLeftShoulder, bool mixRightShoulder)
		{
			this.mixAnimation(name, mixLeftShoulder, mixRightShoulder, false);
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x00064BC4 File Offset: 0x00062DC4
		public void mixAnimation(string name, bool mixLeftShoulder, bool mixRightShoulder, bool mixSkull)
		{
			AnimationState animationState = this.anim[name];
			if (animationState == null)
			{
				return;
			}
			if (mixLeftShoulder)
			{
				animationState.AddMixingTransform(this.leftShoulder, true);
			}
			if (mixRightShoulder)
			{
				animationState.AddMixingTransform(this.rightShoulder, true);
			}
			if (mixSkull)
			{
				animationState.AddMixingTransform(this.skull, true);
			}
			animationState.layer = 1;
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x00064C20 File Offset: 0x00062E20
		public void AddEquippedItemAnimation(AnimationClip clip, Transform itemModelTransform)
		{
			if (clip == null)
			{
				return;
			}
			this.anim.AddClip(clip, clip.name);
			this.mixAnimation(clip.name, true, true);
			if (itemModelTransform != null)
			{
				AnimationState animationState = this.anim[clip.name];
				if (animationState != null)
				{
					animationState.AddMixingTransform(this.spineHook, true);
					animationState.AddMixingTransform(itemModelTransform, true);
				}
			}
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x00064C90 File Offset: 0x00062E90
		public void removeAnimation(AnimationClip clip)
		{
			if (clip == null)
			{
				return;
			}
			if (this.anim[clip.name] != null)
			{
				this.anim.RemoveClip(clip);
			}
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x00064CC4 File Offset: 0x00062EC4
		public void setAnimationSpeed(string name, float speed)
		{
			AnimationState animationState = this.anim[name];
			if (animationState != null)
			{
				animationState.speed = speed;
			}
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x00064CEE File Offset: 0x00062EEE
		public float getAnimationLength(string name)
		{
			return this.GetAnimationLength(name, true);
		}

		/// <param name="scaled">If true, include current animation speed modifier.</param>
		// Token: 0x06001C3D RID: 7229 RVA: 0x00064CF8 File Offset: 0x00062EF8
		public float GetAnimationLength(string name, bool scaled = true)
		{
			AnimationState animationState = this.anim[name];
			if (!(animationState != null))
			{
				return 0f;
			}
			if (!scaled)
			{
				return animationState.clip.length;
			}
			if (animationState.speed != 0f)
			{
				return animationState.clip.length / animationState.speed;
			}
			return 0f;
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x00064D55 File Offset: 0x00062F55
		public bool getAnimationPlaying()
		{
			return !string.IsNullOrEmpty(this.clip) && this.anim.IsPlaying(this.clip);
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x00064D77 File Offset: 0x00062F77
		public void state(string name)
		{
			if (this.anim[name] == null)
			{
				return;
			}
			this.anim.CrossFade(name, CharacterAnimator.BLEND);
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x00064D9F File Offset: 0x00062F9F
		public bool checkExists(string name)
		{
			return this.anim[name] != null;
		}

		/// <returns>True if an animation was found and started playing.</returns>
		// Token: 0x06001C41 RID: 7233 RVA: 0x00064DB4 File Offset: 0x00062FB4
		public bool play(string name, bool smooth)
		{
			if (this.anim[name] == null)
			{
				return false;
			}
			if (this.clip != "")
			{
				this.anim.Stop(this.clip);
			}
			this.clip = name;
			if (smooth)
			{
				this.anim.CrossFade(name, CharacterAnimator.BLEND);
			}
			else
			{
				this.anim.Play(name);
			}
			return true;
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x00064E25 File Offset: 0x00063025
		public void stop(string name)
		{
			if (this.anim[name] == null)
			{
				return;
			}
			if (name == this.clip)
			{
				this.anim.Stop(name);
				this.clip = "";
			}
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x00064E64 File Offset: 0x00063064
		protected void init()
		{
			this.clip = "";
			this.anim = base.GetComponent<Animation>();
			this.spine = base.transform.Find("Skeleton").Find("Spine");
			this.skull = this.spine.Find("Skull");
			this.leftShoulder = this.spine.Find("Left_Shoulder");
			this.rightShoulder = this.spine.Find("Right_Shoulder");
			this.spineHook = this.spine.Find("Spine_Hook");
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x00064F00 File Offset: 0x00063100
		private void Awake()
		{
			this.init();
		}

		// Token: 0x04000D5B RID: 3419
		public static readonly float BLEND = 0.25f;

		// Token: 0x04000D5C RID: 3420
		protected Animation anim;

		// Token: 0x04000D5D RID: 3421
		protected Transform spine;

		// Token: 0x04000D5E RID: 3422
		protected Transform skull;

		// Token: 0x04000D5F RID: 3423
		protected Transform leftShoulder;

		// Token: 0x04000D60 RID: 3424
		protected Transform rightShoulder;

		// Token: 0x04000D61 RID: 3425
		protected Transform spineHook;

		// Token: 0x04000D62 RID: 3426
		protected string clip;
	}
}
