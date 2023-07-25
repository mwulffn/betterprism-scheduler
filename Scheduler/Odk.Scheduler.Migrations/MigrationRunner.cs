using Microsoft.Extensions.DependencyInjection;
using System;
using FluentMigrator.Runner;

namespace Odk.Scheduler.Migrations
{
    public class MigrationRunner
    {
        public string ConnectionString { get; }

        public MigrationRunner(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Up()
        {
            var serviceProvider = CreateServices();

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }

        public void Down(int level = 0)
        {
            var serviceProvider = CreateServices();

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateDown(level);
            }
        }

        private IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(ConnectionString)
                    .ScanIn(typeof(Migrations.Tasks).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
    }
}