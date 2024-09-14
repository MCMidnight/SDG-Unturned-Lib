using System;
using System.IO;
using System.Xml.Serialization;

namespace SDG.Framework.IO.Deserialization
{
	// Token: 0x020000E6 RID: 230
	public class XMLDeserializer : IDeserializer
	{
		// Token: 0x060005A6 RID: 1446 RVA: 0x000156E0 File Offset: 0x000138E0
		public T deserialize<T>(byte[] data, int offset)
		{
			MemoryStream memoryStream = new MemoryStream(data, offset, data.Length - offset);
			T result = this.deserialize<T>(memoryStream);
			memoryStream.Close();
			memoryStream.Dispose();
			return result;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001570D File Offset: 0x0001390D
		public T deserialize<T>(MemoryStream memoryStream)
		{
			return (T)((object)new XmlSerializer(typeof(T)).Deserialize(memoryStream));
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001572C File Offset: 0x0001392C
		public T deserialize<T>(string path)
		{
			T result = default(T);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			StreamReader streamReader = new StreamReader(path);
			try
			{
				result = (T)((object)xmlSerializer.Deserialize(streamReader));
			}
			finally
			{
				streamReader.Close();
				streamReader.Dispose();
			}
			return result;
		}
	}
}
