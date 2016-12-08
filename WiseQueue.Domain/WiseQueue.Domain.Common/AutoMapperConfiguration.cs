using System;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using AutoMapper.Configuration;

namespace WiseQueue.Domain.Common
{
    static class AutoMapperConfiguration
    {
        public static IMapper CreateMapper()
        {
            //TODO: Create mapper configuration

            MapperConfigurationExpression config = new MapperConfigurationExpression();
            Mapper.Initialize(config);

            return Mapper.Instance;
        }
    }
}
