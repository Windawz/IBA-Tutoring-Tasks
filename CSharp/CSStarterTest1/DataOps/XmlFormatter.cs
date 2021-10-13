using System;
using System.Reflection;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace CSStarterTest1.DataOps
{
    /// <summary>
    /// Formats any data into XML.<br/>
    /// The resulting XML is not guarranteed to be a valid standalone XML document.
    /// </summary>
    internal static class XmlFormatter
    {
        /// <summary>
        /// The default name of the element representing the formatted data, if none is provided.
        /// </summary>
        public static readonly string DefaultElementName = "Object";

        /// <summary>
        /// Formats <paramref name="value"/>'s public properties into XML.
        /// <para/>
        /// Properties, whose type is not primitive (checked by <see cref="Type.IsPrimitive"/>), and is not one of the following:
        /// <see cref="Enum"/>,
        /// <see cref="Guid"/>,
        /// <see cref="string"/>,
        /// <see cref="DateTime"/>; will be formatted recursively.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="elementName">
        /// The name of the element encapsulating the data's XML-converted properties.<br/>
        /// Will be replaced by <see cref="DefaultElementName"/> if null or whitespace.
        /// <see cref="DefaultElementName"/> by default.
        /// Overriden by recursive calls on encountering a composite type.
        /// </param>
        /// <returns>
        /// An <see cref="XElement"/> representing the data, or null if <paramref name="value"/> is null.
        /// </returns>
        public static XElement? Format(object? value, string? elementName = null)
        {
            if (value is null)
            {
                return null;
            }
            if (String.IsNullOrWhiteSpace(elementName))
            {
                elementName = DefaultElementName;
            }
            
            // NOTE: if the data field formatter cannot format the value, we try to break it up further
            // for now, god forbid if it fails
            var elements = value
                .GetType()
                .GetProperties()
                .Select(p => (Name: XmlConvert.EncodeName(p.Name), Value: p.GetValue(value)))
                .Select(nv => // ugly!!!!!!
                    DataFieldFormatter.CanFormat(nv.Value) ? new XElement(nv.Name, DataFieldFormatter.Format(nv.Value)) : Format(nv.Value, nv.Name))
                .Where(xe => xe is not null);

            XElement result = new(elementName);
            result.Add(elements);

            return result;
        }
    }
}
