//--------------------------------------------------------------------------------------------
// NHibernate-X-Factories v1.0.1, Copyright 2013 roydukkey, 2013-06-20 (Thu, 20 June 2013).
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//--------------------------------------------------------------------------------------------

namespace NHibernate.Cfg
{
	using System;
	using System.IO;
	using System.Xml;

	public static class ConfigurationExtensions
	{

		/// <summary>
		///		Configure NHibernate from a specified session-factory.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="fileName">System location of '.cfg.xml' configuration file.</param>
		/// <param name="factoryName">Name value of the desired session-factory.</param>
		/// <returns></returns>
		public static Configuration Configure(this Configuration config, string fileName, string factoryName) {

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
			const string CONFIG_XSD_MUTATION = "-x-factories";
			
			// Add Proper Namespace
			XmlNamespaceManager namespaceMgr = new XmlNamespaceManager(doc.NameTable);
			namespaceMgr.AddNamespace("nhibernate", "urn:nhibernate-configuration-2.2" + CONFIG_XSD_MUTATION);

			// Query Elements
			XmlNode nhibernateNode = doc.SelectSingleNode("descendant::nhibernate:hibernate-configuration", namespaceMgr);

			if (nhibernateNode != null) {
				if (nhibernateNode.SelectSingleNode("descendant::nhibernate:session-factory[@name='" + factoryName + "']", namespaceMgr) != default(XmlNode))
					foreach (XmlNode node in nhibernateNode.SelectNodes("descendant::nhibernate:session-factory[@name!='" + factoryName + "']", namespaceMgr)) nhibernateNode.RemoveChild(node);
				else
					throw new Exception("<session-factory name=\"" + factoryName + "\"> element was not found in the configuration file.");
			}
			else
				throw new Exception("<hibernate-configuration xmlns=\"urn:nhibernate-configuration-2.2-x-factories\"> element was not found in the configuration file.");

			return new XmlTextReader(new StringReader(nhibernateNode.OuterXml.Replace(CONFIG_XSD_MUTATION, "")));
		}

	}
}
