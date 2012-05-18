NHibernate X-Factories
==========================

NHibernate X-Factories allow you to combine multiple .cfg.xml into one. X-Factories does this by allowing each session-factory to be named and refer to them individually by name.


Setup
---------

Setup is really quite simple. Just add the schema, include the extension and you're good to go.

**Adding the Schema**

1. Location the Visual Studios XML Schemas folder on your computer. Should be something similar to `%ProgramFiles%\Microsoft Visual Studio 10.0\Xml\Schemas`.
2. Copy and paste nhibernate-configuration-x-factories.xsd into the Schemas folder.
3. Change the xmlns attribute of the hibernate-configuration element in your .cfg.xml
4. Give the session-factory element a name and create as many session-factory elements as you like.

**Including the Extension**

1. Open your Visual Studio project that already has NHibernate included.
2. Copy the ConfigurationExtensions.cs into the project.

Note: Visual Studio Website projects will require that the extension be located in the App_Code folder.

Usage
---------

~~~ xml
	<hibernate-configuration xmlns="urn:nhibernate-configuration-2.2-x-factories">
		
		<session-factory name="Development">
			<property name="connection.provider">NHibernate.Connection.DriverConnectionProvider</property>
			<property name="dialect">NHibernate.Dialect.MsSql2008Dialect</property>
			<property name="connection.driver_class">NHibernate.Driver.SqlClientDriver</property>
			<property name="connection.connection_string">Server=dsql01;DataBase=dbDev;uid=nhDeveloper;pwd=pass1234</property>

			<property name="show_sql">true</property>

			<mapping assembly="DataLayer" />
		</session-factory>
		
		<session-factory name="Producton">
			<property name="connection.provider">NHibernate.Connection.DriverConnectionProvider</property>
			<property name="dialect">NHibernate.Dialect.MsSql2008Dialect</property>
			<property name="connection.driver_class">NHibernate.Driver.SqlClientDriver</property>
			<property name="connection.connection_string">Server=psql02;DataBase=dbDev;uid=nhDeveloper;pwd=pass5678</property>
			
			<property name="show_sql">false</property>

			<mapping assembly="DataLayer" />
		</session-factory>
		
	</hibernate-configuration>
~~~

~~~ c#
	NHibernate.Cfg.Configuration config = new NHibernate.Cfg.Configuration();
	config.Configure("~/nhibernate.cfg.xml", "Development").BuildSessionFactory()
~~~

Support
-----------
**Required**

* NHibernate (http://nhforge.org/Default.aspx)


License
-----------

Dual licensed under the MIT (http://www.roydukkey.com/mit) and GPL (http://www.roydukkey.com/gpl) licenses.