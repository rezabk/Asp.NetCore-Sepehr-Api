using AutoMapper;

namespace BusinessLogic.Utils
{
    public class Mapper
    {
        public Task<TDestination> MapAsync<TSource, TDestination>(TSource source, TDestination destination)
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<TSource, TDestination>();
            });
            var mapper = mapperConfiguration.CreateMapper();
            return Task.Run(() => mapper.Map(source, destination));
        }
    }
}