using System;
using AutoMapper;
using ICG.Core.Extensions;

namespace Easify.Ef.Testing.Builders
{
    public class TypeMapper : ITypeMapper
    {
        private readonly Action<IMapperConfigurationExpression> _configurator;
        private readonly IMapper _mapper;
            
        public TypeMapper(Action<IMapperConfigurationExpression> configurator)
        {
            _configurator = configurator;
            _mapper = MapperBuilder.Build(configurator);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}