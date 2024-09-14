using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SDG.Framework.IO.Serialization
{
	// Token: 0x020000B9 RID: 185
	public class XMLSerializer : ISerializer
	{
		// Token: 0x060004EE RID: 1262 RVA: 0x000140D0 File Offset: 0x000122D0
		public void serialize<T>(T instance, byte[] data, int index, out int size, bool isFormatted)
		{
			MemoryStream memoryStream = new MemoryStream(data, index, data.Length - index);
			this.serialize<T>(instance, memoryStream, isFormatted);
			size = (int)memoryStream.Position;
			memoryStream.Close();
			memoryStream.Dispose();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001410C File Offset: 0x0001230C
		public void serialize<T>(T instance, MemoryStream memoryStream, bool isFormatted)
		{
			XmlWriter xmlWriter = XmlWriter.Create(memoryStream, isFormatted ? XMLSerializer.XML_WRITER_SETTINGS_FORMATTED : XMLSerializer.XML_WRITER_SETTINGS_UNFORMATTED);
			new XmlSerializer(typeof(T)).Serialize(xmlWriter, instance, XMLSerializer.XML_SERIALIZER_NAMESPACES);
			xmlWriter.Flush();
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00014158 File Offset: 0x00012358
		public void serialize<T>(T instance, string path, bool isFormatted)
		{
			StreamWriter streamWriter = new StreamWriter(path);
			XmlWriter xmlWriter = XmlWriter.Create(streamWriter, isFormatted ? XMLSerializer.XML_WRITER_SETTINGS_FORMATTED : XMLSerializer.XML_WRITER_SETTINGS_UNFORMATTED);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			try
			{
				xmlSerializer.Serialize(xmlWriter, instance, XMLSerializer.XML_SERIALIZER_NAMESPACES);
				xmlWriter.Flush();
			}
			finally
			{
				streamWriter.Close();
				streamWriter.Dispose();
			}
		}

		// Token: 0x040001EF RID: 495
		private static readonly XmlSerializerNamespaces XML_SERIALIZER_NAMESPACES = new XmlSerializerNamespaces(new XmlQualifiedName[]
		{
			XmlQualifiedName.Empty
		});

		// Token: 0x040001F0 RID: 496
		private static readonly XmlWriterSettings XML_WRITER_SETTINGS_FORMATTED = new XmlWriterSettings
		{
			Indent = true,
			OmitXmlDeclaration = true,
			Encoding = new UTF8Encoding()
		};

		// Token: 0x040001F1 RID: 497
		private static readonly XmlWriterSettings XML_WRITER_SETTINGS_UNFORMATTED = new XmlWriterSettings
		{
			Indent = false,
			OmitXmlDeclaration = true,
			Encoding = new UTF8Encoding()
		};
	}
}
