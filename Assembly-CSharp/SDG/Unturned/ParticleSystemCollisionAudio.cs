using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SDG.Unturned
{
	// Token: 0x020005D3 RID: 1491
	[AddComponentMenu("Unturned/Particle System Collision Audio")]
	public class ParticleSystemCollisionAudio : MonoBehaviour
	{
		// Token: 0x04001A01 RID: 6657
		public ParticleSystem particleSystemComponent;

		// Token: 0x04001A02 RID: 6658
		[FormerlySerializedAs("audioPrefab")]
		public OneShotAudioDefinition audioDef;

		/// <summary>
		/// If set, audio clip associated with physics material will take priority.
		/// </summary>
		// Token: 0x04001A03 RID: 6659
		public string materialPropertyName;

		/// <summary>
		/// Collision with speed lower than this value will not play a sound.
		/// </summary>
		// Token: 0x04001A04 RID: 6660
		public float speedThreshold = 0.01f;

		// Token: 0x04001A05 RID: 6661
		public float minSpeed = 0.2f;

		// Token: 0x04001A06 RID: 6662
		public float maxSpeed = 1f;

		// Token: 0x04001A07 RID: 6663
		public float minVolume = 0.5f;

		// Token: 0x04001A08 RID: 6664
		public float maxVolume = 1f;
	}
}
