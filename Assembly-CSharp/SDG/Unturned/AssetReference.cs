using System;
using SDG.Framework.IO.FormattedFiles;

namespace SDG.Unturned
{
	// Token: 0x0200028C RID: 652
	public struct AssetReference<T> : IAssetReference, IFormattedFileReadable, IFormattedFileWritable, IDatParseable, IEquatable<AssetReference<T>> where T : Asset
	{
		// Token: 0x170002AA RID: 682
		// (get) Token: 0x0600135A RID: 4954 RVA: 0x00046AE9 File Offset: 0x00044CE9
		// (set) Token: 0x0600135B RID: 4955 RVA: 0x00046AF1 File Offset: 0x00044CF1
		public Guid GUID
		{
			get
			{
				return this._guid;
			}
			set
			{
				this._guid = value;
			}
		}

		/// <summary>
		/// Whether the asset has been assigned. Note that this doesn't mean an asset with <see cref="P:SDG.Unturned.AssetReference`1.GUID" /> exists.
		/// </summary>
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600135C RID: 4956 RVA: 0x00046AFA File Offset: 0x00044CFA
		public bool isValid
		{
			get
			{
				return this.GUID != Guid.Empty;
			}
		}

		/// <summary>
		/// Is this asset not assigned?
		/// </summary>
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x00046B0C File Offset: 0x00044D0C
		public bool isNull
		{
			get
			{
				return this.GUID == Guid.Empty;
			}
		}

		/// <summary>
		/// True if resolving this asset reference would get that asset.
		/// </summary>
		// Token: 0x0600135E RID: 4958 RVA: 0x00046B1E File Offset: 0x00044D1E
		public bool isReferenceTo(Asset asset)
		{
			return asset != null && this.GUID == asset.GUID;
		}

		/// <summary>
		/// Resolve reference with asset manager.
		/// </summary>
		// Token: 0x0600135F RID: 4959 RVA: 0x00046B36 File Offset: 0x00044D36
		public T Find()
		{
			return Assets.find<T>(this);
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00046B43 File Offset: 0x00044D43
		[Obsolete("Renamed to Find because Get might imply that asset is cached")]
		public T get()
		{
			return Assets.find<T>(this);
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00046B50 File Offset: 0x00044D50
		public bool TryParse(IDatNode node)
		{
			DatValue datValue = node as DatValue;
			if (datValue != null)
			{
				Guid guid;
				bool result = datValue.TryParseGuid(out guid);
				this.GUID = guid;
				return result;
			}
			DatDictionary datDictionary = node as DatDictionary;
			if (datDictionary != null)
			{
				Guid guid2;
				bool result2 = datDictionary.TryParseGuid("GUID", out guid2);
				this.GUID = guid2;
				return result2;
			}
			return false;
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00046B98 File Offset: 0x00044D98
		public void read(IFormattedFileReader reader)
		{
			IFormattedFileReader formattedFileReader = reader.readObject();
			if (formattedFileReader == null)
			{
				this.GUID = reader.readValue<Guid>();
				return;
			}
			this.GUID = formattedFileReader.readValue<Guid>("GUID");
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00046BCD File Offset: 0x00044DCD
		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<Guid>("GUID", this.GUID);
			writer.endObject();
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00046BEC File Offset: 0x00044DEC
		public static bool TryParse(string input, out AssetReference<T> result)
		{
			Guid guid;
			if (Guid.TryParse(input, ref guid))
			{
				result = new AssetReference<T>(guid);
				return true;
			}
			result = AssetReference<T>.invalid;
			return false;
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00046C1D File Offset: 0x00044E1D
		public static bool operator ==(AssetReference<T> a, AssetReference<T> b)
		{
			return a.GUID == b.GUID;
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00046C32 File Offset: 0x00044E32
		public static bool operator !=(AssetReference<T> a, AssetReference<T> b)
		{
			return !(a == b);
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00046C40 File Offset: 0x00044E40
		public override int GetHashCode()
		{
			return this.GUID.GetHashCode();
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00046C64 File Offset: 0x00044E64
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			AssetReference<T> assetReference = (AssetReference<T>)obj;
			return this.GUID.Equals(assetReference.GUID);
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00046C94 File Offset: 0x00044E94
		public override string ToString()
		{
			return this.GUID.ToString("N");
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x00046CB4 File Offset: 0x00044EB4
		public bool Equals(AssetReference<T> other)
		{
			return this.GUID.Equals(other.GUID);
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00046CD6 File Offset: 0x00044ED6
		public AssetReference(Guid GUID)
		{
			this._guid = GUID;
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00046CDF File Offset: 0x00044EDF
		public AssetReference(string GUID)
		{
			Guid.TryParse(GUID, ref this._guid);
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00046CEE File Offset: 0x00044EEE
		public AssetReference(IAssetReference assetReference)
		{
			this._guid = assetReference.GUID;
		}

		// Token: 0x0400068E RID: 1678
		public static AssetReference<T> invalid = new AssetReference<T>(Guid.Empty);

		// Token: 0x0400068F RID: 1679
		private Guid _guid;
	}
}
