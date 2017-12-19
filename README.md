NHibernate X-Factories v5.0.0
=================================

NHibernate X-Factories allow you to combine multiple .cfg.xml into one. X-Factories does this by allowing each session-factory to be named and referred to individually by name. This gives a cleaner and less verbose configuration for using NHiberate between multiple databases.


Setup
---------

Setup is really quite simple.

1. Install via [NuGet](https://www.nuget.org/packages/NHibernate.XFactories/).
2. The `nhibernate-configuration-x-factories.xsd` scheme may be included to a Project or Solution and is available in the NuGet package root folder.
3. Change the `xmlns` attribute of the `hibernate-configuration` element in your .cfg.xml to `urn:nhibernate-configuration-2.2-x-factories`.
4. Change `hibernate-configuration` element to `hibernate-configuration-x-factories`.
5. Give the `session-factory` element a name and create as many `session-factory` elements as you like.

Usage
---------

~~~ xml
<?xml version="1.0" encoding="utf-8" ?>
<hibernate-configuration-x-factories xmlns="urn:nhibernate-configuration-2.2-x-factories">
	
	<session-factory name="Development">
		<property name="connection.provider">NHibernate.Connection.DriverConnectionProvider</property>
		<property name="dialect">NHibernate.Dialect.MsSql2008Dialect</property>
		<property name="connection.driver_class">NHibernate.Driver.SqlClientDriver</property>
		<property name="connection.connection_string">
			Server=dsql01;DataBase=dbDev;uid=nhDeveloper;pwd=pass1234
		</property>

		<property name="show_sql">true</property>

		<mapping assembly="DataLayer" />
	</session-factory>
	
	<session-factory name="Production">
		<property name="connection.provider">NHibernate.Connection.DriverConnectionProvider</property>
		<property name="dialect">NHibernate.Dialect.MsSql2008Dialect</property>
		<property name="connection.driver_class">NHibernate.Driver.SqlClientDriver</property>
		<property name="connection.connection_string">
			Server=psql02;DataBase=dbDev;uid=nhDeveloper;pwd=pass5678
		</property>
		
		<property name="show_sql">false</property>

		<mapping assembly="DataLayer" />
	</session-factory>
	
</hibernate-configuration-x-factories>
~~~

~~~ c
using NHibernate;
using NHibernate.Cfg;
using NHibernate.XFactories;

NHibernate.Cfg.Configuration config = new NHibernate.Cfg.Configuration();
config
	.Configure(HostingEnvironment.MapPath("~/nhibernate.cfg.xml"), "Development")
	.BuildSessionFactory();
~~~

Support
-----------
**Required**

* .NETFramework ≥ 4.6.1
* Iesi.Collections ≥ 4.0.0 && < 5.0.0
* NHibernate 5.0.0 (http://nhforge.org/Default.aspx)

**Optional**

* [Configuration from Web.Config](https://github.com/roydukkey/NHibernate-X-Factories/wiki/Configuration-from-Web.Config)


License
-----------

Licensed under the [MIT](LICENSE).
