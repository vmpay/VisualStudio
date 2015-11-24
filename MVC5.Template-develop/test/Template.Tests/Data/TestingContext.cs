using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data.Mapping;
using Template.Tests.Objects;
using System.Data.Entity;

namespace Template.Tests.Data
{
    public class TestingContext : Context
    {
        #region Test

        protected DbSet<TestModel> TestModels { get; set; }

        #endregion

        static TestingContext()
        {
            TestObjectMapper.MapObjects();
        }

        public void DropData()
        {
            Set<RolePrivilege>().RemoveRange(Set<RolePrivilege>());
            Set<Privilege>().RemoveRange(Set<Privilege>());
            Set<Account>().RemoveRange(Set<Account>());
            Set<Role>().RemoveRange(Set<Role>());

            SaveChanges();
        }
    }
}
