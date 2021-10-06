﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CSStarterTest1.DataOps
{
    public sealed class XmlExporter
    {
        public XmlExporter(Stream stream)
        {
            _writer = XmlWriter.Create(stream, _defaultSettings);
        }

        private static readonly XmlWriterSettings _defaultSettings = new()
        {
            CloseOutput = false,
            Encoding = Encoding.UTF8,
            Indent = true,
        };
        private readonly XmlWriter _writer;

        public void Export<T>(T data)
        {
            WriteRootStart();

            WriteData(data);

            WriteRootEnd();
            _writer.Flush();
        }
        public void ExportRange<T>(IEnumerable<T> datas)
        {
            WriteRootStart();

            foreach (var data in datas)
            {
                WriteData(data);
            }

            WriteRootEnd();
            _writer.Flush();
        }
        private void WriteRootStart()
        {
            _writer.WriteStartElement("Root");
        }
        private void WriteRootEnd()
        {
            _writer.WriteEndElement();
        }
        private void WriteData<T>(T data)
        {
            XElement? xe = new XmlFormatter().ToXml(data);
            if (xe is null)
            {
                return;
            }
            xe.WriteTo(_writer);
        }
    }
}