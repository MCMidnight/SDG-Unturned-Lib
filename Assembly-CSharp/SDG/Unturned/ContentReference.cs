using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002A0 RID: 672
	public struct ContentReference<T> : IContentReference, IFormattedFileReadable, IFormattedFileWritable, IDatParseable, IEquatable<ContentReference<T>> where T : Object
	{
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x0004AD0E File Offset: 0x00048F0E
		// (set) Token: 0x06001430 RID: 5168 RVA: 0x0004AD16 File Offset: 0x00048F16
		public string name { readonly get; set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x0004AD1F File Offset: 0x00048F1F
		// (set) Token: 0x06001432 RID: 5170 RVA: 0x0004AD27 File Offset: 0x00048F27
		public string path { readonly get; set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x0004AD30 File Offset: 0x00048F30
		public bool isValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.name) && !string.IsNullOrEmpty(this.path);
			}
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0004AD50 File Offset: 0x00048F50
		public bool TryParse(IDatNode node)
		{
			DatValue datValue = node as DatValue;
			if (datValue != null)
			{
				if (string.IsNullOrEmpty(datValue.value))
				{
					return false;
				}
				if (datValue.value.Length < 2)
				{
					return false;
				}
				int num = datValue.value.IndexOf(':');
				if (num < 0)
				{
					if (Assets.currentMasterBundle != null)
					{
						this.name = Assets.currentMasterBundle.assetBundleName;
					}
					this.path = datValue.value;
				}
				else
				{
					this.name = datValue.value.Substring(0, num);
					this.path = datValue.value.Substring(num + 1);
				}
				return true;
			}
			else
			{
				DatDictionary datDictionary = node as DatDictionary;
				if (datDictionary != null)
				{
					this.name = datDictionary.GetString("Name", null);
					this.path = datDictionary.GetString("Path", null);
					return true;
				}
				return false;
			}
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0004AE18 File Offset: 0x00049018
		public void read(IFormattedFileReader reader)
		{
			IFormattedFileReader formattedFileReader = reader.readObject();
			if (formattedFileReader == null)
			{
				if (Assets.currentMasterBundle != null)
				{
					this.name = Assets.currentMasterBundle.assetBundleName;
				}
				this.path = reader.readValue();
				return;
			}
			this.name = formattedFileReader.readValue("Name");
			this.path = formattedFileReader.readValue("Path");
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0004AE75 File Offset: 0x00049075
		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue("Name", this.name);
			writer.writeValue("Path", this.path);
			writer.endObject();
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0004AEA5 File Offset: 0x000490A5
		public static bool operator ==(ContentReference<T> a, ContentReference<T> b)
		{
			return a.name == b.name && a.path == b.path;
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0004AED1 File Offset: 0x000490D1
		public static bool operator !=(ContentReference<T> a, ContentReference<T> b)
		{
			return !(a == b);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0004AEDD File Offset: 0x000490DD
		public override int GetHashCode()
		{
			return this.name.GetHashCode() ^ this.path.GetHashCode();
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0004AEF8 File Offset: 0x000490F8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ContentReference<T> contentReference = (ContentReference<T>)obj;
			return this.name == contentReference.name && this.path == contentReference.path;
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0004AF39 File Offset: 0x00049139
		public override string ToString()
		{
			return "#" + this.name + "::" + this.path;
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0004AF56 File Offset: 0x00049156
		public bool Equals(ContentReference<T> other)
		{
			return this.name == other.name && this.path == other.path;
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0004AF80 File Offset: 0x00049180
		public ContentReference(string newName, string newPath)
		{
			this.name = newName;
			this.path = newPath;
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0004AF90 File Offset: 0x00049190
		public ContentReference(IContentReference contentReference)
		{
			this.name = contentReference.name;
			this.path = contentReference.path;
		}

		// Token: 0x040006F0 RID: 1776
		public static ContentReference<T> invalid = new ContentReference<T>(null, null);
	}
}
