using System;
using System.IO;
using Newtonsoft.Json;

namespace SDG.Framework.IO.Deserialization
{
	// Token: 0x020000E5 RID: 229
	public class JSONDeserializer : IDeserializer
	{
		// Token: 0x060005A2 RID: 1442 RVA: 0x000155F8 File Offset: 0x000137F8
		public T deserialize<T>(byte[] data, int offset)
		{
			MemoryStream memoryStream = new MemoryStream(data, offset, data.Length - offset);
			T result = this.deserialize<T>(memoryStream);
			memoryStream.Close();
			memoryStream.Dispose();
			return result;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00015628 File Offset: 0x00013828
		public T deserialize<T>(MemoryStream memoryStream)
		{
			T result = default(T);
			StreamReader streamReader = new StreamReader(memoryStream);
			JsonReader jsonReader = new JsonTextReader(streamReader);
			JsonSerializer jsonSerializer = new JsonSerializer();
			try
			{
				result = jsonSerializer.Deserialize<T>(jsonReader);
			}
			finally
			{
				jsonReader.Close();
				streamReader.Close();
				streamReader.Dispose();
			}
			return result;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00015680 File Offset: 0x00013880
		public T deserialize<T>(string path)
		{
			T result = default(T);
			StreamReader streamReader = new StreamReader(path);
			JsonReader jsonReader = new JsonTextReader(streamReader);
			JsonSerializer jsonSerializer = new JsonSerializer();
			try
			{
				result = jsonSerializer.Deserialize<T>(jsonReader);
			}
			finally
			{
				jsonReader.Close();
				streamReader.Close();
				streamReader.Dispose();
			}
			return result;
		}
	}
}
