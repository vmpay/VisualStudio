﻿using AutoMapper;
using Template.Data.Core;
using Template.Data.Logging;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Objects;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Xunit;

namespace Template.Tests.Unit.Data.Core
{
    public class UnitOfWorkTests : IDisposable
    {
        private TestingContext context;
        private UnitOfWork unitOfWork;
        private IAuditLogger logger;

        public UnitOfWorkTests()
        {
            context = new TestingContext();
            logger = Substitute.For<IAuditLogger>();
            unitOfWork = new UnitOfWork(context, logger);

            context.Set<TestModel>().RemoveRange(context.Set<TestModel>());
            context.SaveChanges();
        }
        public void Dispose()
        {
            unitOfWork.Dispose();
            context.Dispose();
        }

        #region Method: GetAs<TModel, TDestination>(String id)

        [Fact]
        public void GetAs_ReturnsModelAsDestinationModelById()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            TestView expected = Mapper.Map<TestView>(context.Set<TestModel>().AsNoTracking().Single());
            TestView actual = unitOfWork.GetAs<TestModel, TestView>(model.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Text, actual.Text);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Get<TModel>(String id)

        [Fact]
        public void Get_ModelById()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            TestModel expected = context.Set<TestModel>().AsNoTracking().Single();
            TestModel actual = unitOfWork.Get<TestModel>(model.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Text, actual.Text);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Get_NotFound_ReturnsNull()
        {
            Assert.Null(unitOfWork.Get<TestModel>(""));
        }

        #endregion

        #region Method: To<TDestination>(Object source)

        [Fact]
        public void To_ConvertsSourceToDestination()
        {
            TestModel model = ObjectFactory.CreateTestModel();

            TestView actual = unitOfWork.To<TestView>(model);
            TestView expected = Mapper.Map<TestView>(model);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Text, actual.Text);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Select<TModel>()

        [Fact]
        public void Select_FromSet()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            IEnumerable<TestModel> actual = unitOfWork.Select<TestModel>();
            IEnumerable<TestModel> expected = context.Set<TestModel>();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: InsertRange<TModel>(IEnumerable<TModel> models)

        [Fact]
        public void InsertRange_AddsModelsToDbSet()
        {
            IEnumerable<TestModel> models = new[] { ObjectFactory.CreateTestModel(1), ObjectFactory.CreateTestModel(2) };
            unitOfWork.InsertRange(models);

            IEnumerator<TestModel> actual = context.ChangeTracker.Entries<TestModel>().Select(entry => entry.Entity).GetEnumerator();
            IEnumerator<TestModel> expected = models.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(EntityState.Added, context.Entry(actual.Current).State);
                Assert.Same(expected.Current, actual.Current);
            }
        }

        #endregion

        #region Method: Insert<TModel>(TModel model)

        [Fact]
        public void Insert_AddsModelToDbSet()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            unitOfWork.Insert(model);

            Object actual = context.ChangeTracker.Entries<TestModel>().Single().Entity;
            Object expected = model;

            Assert.Equal(EntityState.Added, context.Entry(model).State);
            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Update(TModel model)

        [Fact]
        public void Update_UpdatesNotAttachedModel()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            model.Text += "Test";

            unitOfWork.Update(model);

            DbEntityEntry<TestModel> actual = context.Entry(model);
            TestModel expected = model;

            Assert.Equal(expected.CreationDate, actual.Entity.CreationDate);
            Assert.Equal(EntityState.Modified, actual.State);
            Assert.Equal(expected.Text, actual.Entity.Text);
            Assert.Equal(expected.Id, actual.Entity.Id);
        }

        [Fact]
        public void Update_UpdatesAttachedModel()
        {
            TestModel attachedModel = ObjectFactory.CreateTestModel();
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(attachedModel);
            model.Text += "Test";

            unitOfWork.Update(model);

            DbEntityEntry<TestModel> actual = context.Entry(attachedModel);
            TestModel expected = model;

            Assert.Equal(expected.CreationDate, actual.Entity.CreationDate);
            Assert.Equal(EntityState.Modified, actual.State);
            Assert.Equal(expected.Text, actual.Entity.Text);
            Assert.Equal(expected.Id, actual.Entity.Id);
        }

        [Fact]
        public void Update_DoesNotModifyCreationDate()
        {
            TestModel model = ObjectFactory.CreateTestModel();

            unitOfWork.Update(model);

            Assert.False(context.Entry(model).Property(prop => prop.CreationDate).IsModified);
        }

        #endregion

        #region Method: DeleteRange<TModel>(IEnumerable<TModel> models)

        [Fact]
        public void DeleteRange_Models()
        {
            IEnumerable<TestModel> models = new[] { ObjectFactory.CreateTestModel(1), ObjectFactory.CreateTestModel(2) };
            foreach (TestModel model in models)
                context.Set<TestModel>().Add(model);

            context.SaveChanges();

            unitOfWork.DeleteRange(models);
            unitOfWork.Commit();

            Assert.Empty(context.Set<TestModel>());
        }

        #endregion

        #region Method: Delete<TModel>(TModel model)

        [Fact]
        public void Delete_Model()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            unitOfWork.Delete(model);
            unitOfWork.Commit();

            Assert.Empty(context.Set<TestModel>());
        }

        #endregion

        #region Method: Delete<TModel>(String id)

        [Fact]
        public void Delete_ModelById()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            unitOfWork.Delete<TestModel>(model.Id);
            unitOfWork.Commit();

            Assert.Empty(context.Set<TestModel>());
        }

        #endregion

        #region Method: Rollback()

        [Fact]
        public void Rollback_Changes()
        {
            context.Set<TestModel>().Add(ObjectFactory.CreateTestModel());

            unitOfWork.Rollback();
            unitOfWork.Commit();

            Assert.Empty(unitOfWork.Select<TestModel>());
        }

        #endregion

        #region Method: Commit()

        [Fact]
        public void Commit_SavesChanges()
        {
            TestingContext context = Substitute.For<TestingContext>();
            UnitOfWork unitOfWork = new UnitOfWork(context);

            unitOfWork.Commit();

            context.Received().SaveChanges();
        }

        [Fact]
        public void Commit_Logs()
        {
            unitOfWork.Commit();

            logger.Received().Log(Arg.Any<IEnumerable<DbEntityEntry<BaseModel>>>());
            logger.Received().Save();
        }

        [Fact]
        public void Commit_Failed_DoesNotSaveLogs()
        {
            unitOfWork.Insert(new TestModel { Text = new String('X', 513) });
            Exception exception = Record.Exception(() => unitOfWork.Commit());

            logger.Received().Log(Arg.Any<IEnumerable<DbEntityEntry<BaseModel>>>());
            logger.DidNotReceive().Save();
            Assert.NotNull(exception);
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_Logger()
        {
            unitOfWork.Dispose();

            logger.Received().Dispose();
        }

        [Fact]
        public void Dispose_Context()
        {
            TestingContext context = Substitute.For<TestingContext>();
            UnitOfWork unitOfWork = new UnitOfWork(context);

            unitOfWork.Dispose();

            context.Received().Dispose();
        }

        [Fact]
        public void Dispose_MultipleTimes()
        {
            unitOfWork.Dispose();
            unitOfWork.Dispose();
        }

        #endregion
    }
}
