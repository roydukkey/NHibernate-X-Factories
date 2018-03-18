// ================================================================= //
// Copyright (c) roydukkey. All rights reserved.                     //
// Licensed under the MIT license.                                   //
// See the LICENSE file in the project root for more information.    //
// ================================================================= //

namespace NHibernate.XFactories
{
	using Cfg;
	using Cfg.ConfigurationSchema;
	using System;
	using System.IO;
	using System.Xml;

	/// <summary>
	///		Provides extension methods for configuring NHibernate when the configuration contains multiple session factories.
	/// </summary>
	public static class ConfigurationExtensions
	{
		/// <summary>
		///		Suffix used for identifying XFactory configurations.
		/// </summary>
		public const string ConfigMutationSuffix = "-x-factories";

		/// <summary>
		///		The XML node name for hibernate x-factory configuration section in the App.config/Web.config and for the hibernate.cfg.xml.
		/// </summary>
		public const string CfgSectionName = CfgXmlHelper.CfgSectionName + ConfigMutationSuffix;

		/// <summary>
		///		The XML Namespace for the nhibernate-configuration-x-factories
		/// </summary>
		public const string CfgSchemaXMLNS = CfgXmlHelper.CfgSchemaXMLNS + ConfigMutationSuffix;

		/// <summary>
		///		Configure NHibernate from a specified session-factory using the file specified.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="fileName">The location of the XML file to use to configure NHibernate.</param>
		/// <param name="factoryName">Name value of the desired session-factory.</param>
		/// <returns>A Configuration object initialized with the file.</returns>
		/// <remarks>Calling Configure(string) will override/merge the values set in app.config or web.config</remarks>
		public static Configuration Configure(this Configuration config, string fileName, string factoryName)
		{
			// Load Configuration XML
			XmlDocument doc = new XmlDocument();
			doc.Load(fileName);

			return config.Configure(PrepareConfiguration(doc, factoryName));
		}

		/// <summary>
		///		Configure NHibernate from a specified session-factory using the specified XmlReader.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="textReader">The System.Xml.XmlReader that contains the Xml to configure NHibernate.</param>
		/// <param name="factoryName">Name value of the desired session-factory.</param>
		/// <returns>A Configuration object initialized with the file.</returns>
		/// <remarks>Calling Configure(XmlReader) will overwrite the values set in app.config or web.config</remarks>
		public static Configuration Configure(this Configuration config, XmlReader textReader, string factoryName)
		{
			// Load Configuration XML
			XmlDocument doc = new XmlDocument();
			doc.Load(textReader);

			return config.Configure(PrepareConfiguration(doc, factoryName));
		}

		/// <summary>
		///		Parses the configuration xml and ensures the target session-factory is the only one included.
		/// </summary>
		/// <param name="doc">The XmlDocument containing the hibernate-configuration.</param>
		/// <param name="factoryName">Name value of the desired session-factory.</param>
		/// <returns>A XmlTextReader object pared down to include only the target session-factory.</returns>
		private static XmlTextReader PrepareConfiguration(XmlDocument doc, string factoryName)
		{
			string rootPrefixPath = "//" + CfgXmlHelper.CfgNamespacePrefix + ":";

			// Add Proper Namespace
			XmlNamespaceManager namespaceMgr = new XmlNamespaceManager(doc.NameTable);
			namespaceMgr.AddNamespace(CfgXmlHelper.CfgNamespacePrefix, CfgSchemaXMLNS);

			// Query Elements
			XmlNode nhibernateNode = doc.SelectSingleNode(rootPrefixPath + CfgSectionName, namespaceMgr);

			if (nhibernateNode != null) {
				if (nhibernateNode.SelectSingleNode(rootPrefixPath + $"session-factory[@name='{factoryName}']", namespaceMgr) != default(XmlNode)) {
					foreach (XmlNode node in nhibernateNode.SelectNodes(rootPrefixPath + $"session-factory[@name!='{factoryName}']", namespaceMgr)) {
						nhibernateNode.RemoveChild(node);
					}
				}
				else {
					throw new Exception($"<session-factory name=\"{factoryName}\"> element was not found in the configuration file.");
				}
			}
			else {
				throw new Exception($"<{CfgSectionName} xmlns=\"{CfgSchemaXMLNS}\"> element was not found in the configuration file.");
			}

			return new XmlTextReader(new StringReader(nhibernateNode.OuterXml.Replace(ConfigMutationSuffix, "")));
		}
	}
}
