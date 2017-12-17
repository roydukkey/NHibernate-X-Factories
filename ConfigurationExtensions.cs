// ================================================================= //
// Copyright (c) roydukkey. All rights reserved.                     //
// Licensed under the MIT license.                                   //
// See the LICENSE file in the project root for more information.    //
// ================================================================= //

namespace NHibernate.Cfg
{
	using ConfigurationSchema;
	using System;
	using System.IO;
	using System.Xml;

	public static class XFactoriesExtensions
	{
		public const string CONFIG_MUTATION_SUFFIX = "-x-factories";
		private const string CHILD_PREFIX_PATH = CfgXmlHelper.CfgNamespacePrefix + ":";
		private const string ROOT_PREFIX_PATH = "//" + CHILD_PREFIX_PATH;

		/// <summary>
		///		Configure NHibernate from a specified session-factory.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="fileName">System location of '.cfg.xml' configuration file.</param>
		/// <param name="factoryName">Name value of the desired session-factory.</param>
		/// <returns></returns>
		public static Configuration Configure(this Configuration config, string fileName, string factoryName)
		{
			// Load Configuration XML
			XmlDocument doc = new XmlDocument();
			doc.Load(fileName);

			return config.Configure(PrepareConfiguration(doc, factoryName));
		}
		/// <summary>
		///		Configure NHibernate from a specified session-factory.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="textReader">The XmlReader containing the hibernate-configuration.</param>
		/// <param name="factoryName">Name value of the desired session-factory.</param>
		/// <returns></returns>
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
		/// <returns></returns>
		private static XmlTextReader PrepareConfiguration(XmlDocument doc, string factoryName)
		{

			// Add Proper Namespace
			XmlNamespaceManager namespaceMgr = new XmlNamespaceManager(doc.NameTable);
			namespaceMgr.AddNamespace(CfgXmlHelper.CfgNamespacePrefix, CfgXmlHelper.CfgSchemaXMLNS + CONFIG_MUTATION_SUFFIX);

			// Query Elements
			XmlNode nhibernateNode = doc.SelectSingleNode(ROOT_PREFIX_PATH + CfgXmlHelper.CfgSectionName + CONFIG_MUTATION_SUFFIX, namespaceMgr);

			if (nhibernateNode != null) {
				if (nhibernateNode.SelectSingleNode(ROOT_PREFIX_PATH + "session-factory[@name='" + factoryName + "']", namespaceMgr) != default(XmlNode))
					foreach (XmlNode node in nhibernateNode.SelectNodes(ROOT_PREFIX_PATH + "session-factory[@name!='" + factoryName + "']", namespaceMgr))
						nhibernateNode.RemoveChild(node);
				else
					throw new Exception(String.Format("<session-factory name=\"{0}\"> element was not found in the configuration file.", factoryName));
			}
			else
				throw new Exception(String.Format("<{1}{0} xmlns=\"{2}{0}\"> element was not found in the configuration file.", CONFIG_MUTATION_SUFFIX, CfgXmlHelper.CfgSectionName, CfgXmlHelper.CfgSchemaXMLNS));

			return new XmlTextReader(new StringReader(nhibernateNode.OuterXml.Replace(CONFIG_MUTATION_SUFFIX, "")));
		}

	}
}