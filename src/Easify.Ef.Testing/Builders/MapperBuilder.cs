using System;
using AutoMapper;

namespace Easify.Ef.Testing.Builders
{
    
    public static class MapperBuilder
    {
        public static IMapper Build(Action<IMapperConfigurationExpression> configurator)
        {
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));

            var config = new MapperConfiguration(configurator);
            var mapper = config.CreateMapper();

            return mapper;
        }
    }
}