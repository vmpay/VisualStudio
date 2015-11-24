using AutoMapper;
using Template.Tests.Objects;

namespace Template.Tests.Data.Mapping
{
    public static class TestObjectMapper
    {
        public static void MapObjects()
        {
            Mapper.CreateMap<TestModel, TestView>();
            Mapper.CreateMap<TestView, TestModel>();
        }
    }
}
