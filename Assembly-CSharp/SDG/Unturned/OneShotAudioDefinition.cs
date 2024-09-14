using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005D2 RID: 1490
	[CreateAssetMenu(fileName = "OneShotAudioDef", menuName = "Unturned/One Shot Audio Def")]
	public class OneShotAudioDefinition : ScriptableObject
	{
		// Token: 0x06003006 RID: 12294 RVA: 0x000D415C File Offset: 0x000D235C
		public AudioClip GetRandomClip()
		{
			if (this.clips == null)
			{
				return null;
			}
			switch (this.clips.Count)
			{
			case 0:
				return null;
			case 1:
				return this.clips[0];
			case 2:
				return this.clips[Random.Range(0, 2)];
			default:
				return this.GetRandomShuffledClip();
			}
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x000D41BC File Offset: 0x000D23BC
		private AudioClip GetRandomShuffledClip()
		{
			List<AudioClip> list = this.clips;
			if (this.shuffledClipIndex < 0)
			{
				this.ShuffleClips(list);
				this.shuffledClipIndex = 0;
			}
			else if (this.shuffledClipIndex >= list.Count)
			{
				this.ReshuffleClips(list);
				this.shuffledClipIndex = 0;
			}
			List<AudioClip> list2 = list;
			int num = this.shuffledClipIndex;
			this.shuffledClipIndex = num + 1;
			return list2[num];
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x000D421C File Offset: 0x000D241C
		private void Swap(List<AudioClip> list, int lhsIndex, int rhsIndex)
		{
			AudioClip audioClip = list[rhsIndex];
			list[rhsIndex] = list[lhsIndex];
			list[lhsIndex] = audioClip;
		}

		/// <summary>
		/// Durstenfeld version of Fisher-Yates shuffle:
		/// https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm
		/// </summary>
		// Token: 0x06003009 RID: 12297 RVA: 0x000D4248 File Offset: 0x000D2448
		private void ShuffleClips(List<AudioClip> list)
		{
			for (int i = list.Count - 1; i > 0; i--)
			{
				int lhsIndex = Random.Range(0, i + 1);
				this.Swap(list, lhsIndex, i);
			}
		}

		/// <summary>
		/// Same as above, but prevent the last clip from being shuffled to the front in order to prevent repeats.
		/// </summary>
		// Token: 0x0600300A RID: 12298 RVA: 0x000D427C File Offset: 0x000D247C
		private void ReshuffleClips(List<AudioClip> list)
		{
			this.Swap(list, 0, Random.Range(0, list.Count - 1));
			for (int i = list.Count - 1; i > 1; i--)
			{
				int lhsIndex = Random.Range(1, i + 1);
				this.Swap(list, lhsIndex, i);
			}
		}

		// Token: 0x040019FC RID: 6652
		public List<AudioClip> clips;

		// Token: 0x040019FD RID: 6653
		public float volumeMultiplier = 1f;

		// Token: 0x040019FE RID: 6654
		public float minPitch = 0.95f;

		// Token: 0x040019FF RID: 6655
		public float maxPitch = 1.05f;

		// Token: 0x04001A00 RID: 6656
		[NonSerialized]
		private int shuffledClipIndex = -1;
	}
}
