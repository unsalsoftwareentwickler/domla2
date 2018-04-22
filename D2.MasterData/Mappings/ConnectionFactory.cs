﻿using System.Collections.Generic;
using System.Reflection;
using D2.Common;
using FluentNHibernate.Cfg;
using NHibernate;
using Npgsql;

namespace D2.MasterData.Mappings
{
    public static class ConnectionFactory
    {
        private static ISessionFactory _sessionFactory;

        private static readonly object SyncRoot;

        private static void Initialize()
        {
            lock (SyncRoot)
            {
                if (_sessionFactory == null)
                {
                    var builder = new NpgsqlConnectionStringBuilder();
                    var options = ServiceConfiguration.connectionInfo;

                    builder.ApplicationName = options.Identifier;
                    builder.Database = options.Name;
                    builder.Host = options.Host;
                    builder.Password = options.Password;
                    builder.Username = options.User;
                    builder.Port = options.Port;
            
                    var connectionProperties = new Dictionary<string, string>
                    {
                        {"connection.connection_string", builder.ConnectionString },
                        { "connection.driver_class", "Beginor.NHibernate.NpgSql.NpgSqlDriver,NHibernate.NpgSql" },
                        { "dialect", "NHibernate.Dialect.PostgreSQL83Dialect" },
                        { "use_proxy_validator", "false" }
                    };

                    var configuration = new NHibernate.Cfg.Configuration()
                        .SetProperties(connectionProperties);
                    
                    Initialize(Fluently.Configure(configuration));
                }
            }
        }
        
        static ConnectionFactory()
        {
            SyncRoot = new object();
        }

        public static void Initialize(FluentConfiguration configuration)
        {
            _sessionFactory?.Dispose();

            _sessionFactory = configuration
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildConfiguration()
                .BuildSessionFactory();
        }

        public static void Shutdown()
        {
            _sessionFactory?.Dispose();
        }
        
        public static ISession Open()
        {
            Initialize();
            return _sessionFactory
                .WithOptions()
                .ConnectionReleaseMode(ConnectionReleaseMode.OnClose)
                .NoInterceptor()
                .FlushMode(FlushMode.Auto)
                .OpenSession();
        }
        
        public static ISession OpenReadOnly()
        {
            Initialize();
            return _sessionFactory
                .WithOptions()
                .ConnectionReleaseMode(ConnectionReleaseMode.OnClose)
                .NoInterceptor()
                .FlushMode(FlushMode.Manual)
                .OpenSession();
        }
    }
}