﻿using Template.Data.Mapping;
using Template.Objects;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Template.Data.Core
{
    public class Context : DbContext
    {
        #region Administration

        protected DbSet<Role> Roles { get; set; }
        protected DbSet<Account> Accounts { get; set; }
        protected DbSet<Privilege> Privileges { get; set; }
        protected DbSet<RolePrivilege> RolePrivileges { get; set; }

        #endregion

        #region System

        protected DbSet<AuditLog> AuditLogs { get; set; }

        #endregion

        static Context()
        {
            ObjectMapper.MapObjects();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Properties<DateTime>().Configure(config => config.HasColumnType("datetime2"));
        }
    }
}
