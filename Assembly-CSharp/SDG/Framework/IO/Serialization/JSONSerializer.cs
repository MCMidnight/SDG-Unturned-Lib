using System;
using System.IO;
using Newtonsoft.Json;

namespace SDG.Framework.IO.Serialization
{
	// Token: 0x020000B7 RID: 183
	public class JSONSerializer : ISerializer
	{
		// Token: 0x060004E8 RID: 1256 RVA: 0x00013F8C File Offset: 0x0001218C
		public void serialize<T>(T instance, byte[] data, int index, out int size, bool isFormatted)
		{
			MemoryStream memoryStream = new MemoryStream(data, index, data.Length - index);
			this.serialize<T>(instance, memoryStream, isFormatted);
			size = (int)memoryStream.Position;
			memoryStream.Close();
			memoryStream.Dispose();
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00013FC8 File Offset: 0x000121C8
		public void serialize<T>(T instance, MemoryStream memoryStream, bool isFormatted)
		{
			StreamWriter streamWriter = new StreamWriter(memoryStream);
			JsonWriter jsonWriter = isFormatted ? new JsonTextWriter(streamWriter) : new JsonTextWriterFormatted(streamWriter);
			JsonSerializer jsonSerializer = new JsonSerializer();
			try
			{
				jsonSerializer.Serialize(jsonWriter, instance);
				jsonWriter.Flush();
			}
			finally
			{
				jsonWriter.Close();
				streamWriter.Close();
				streamWriter.Dispose();
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001402C File Offset: 0x0001222C
		public void serialize<T>(T instance, string path, bool isFormatted)
		{
			StreamWriter streamWriter = new StreamWriter(path);
			JsonWriter jsonWriter = new JsonTextWriterFormatted(streamWriter);
			JsonSerializer jsonSerializer = new JsonSerializer();
			jsonSerializer.Formatting = (isFormatted ? Formatting.Indented : Formatting.None);
			try
			{
				jsonSerializer.Serialize(jsonWriter, instance);
				jsonWriter.Flush();
			}
			finally
			{
				jsonWriter.Close();
				streamWriter.Close();
				streamWriter.Dispose();
			}
		}
	}
}
