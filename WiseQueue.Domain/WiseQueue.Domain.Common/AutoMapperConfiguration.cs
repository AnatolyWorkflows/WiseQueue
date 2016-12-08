using System;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using AutoMapper.Configuration;
using WiseQueue.Core.Common.Entities.Tasks;
using WiseQueue.Core.Common.Models.Tasks;

namespace WiseQueue.Domain.Common
{
    static class AutoMapperConfiguration
    {
        public static IMapper CreateMapper()
        {
            //TODO: Create mapper configuration

            MapperConfigurationExpression config = new MapperConfigurationExpression();

            config.CreateMap<TaskModel, TaskEntity>().ForMember(dest => dest.Arguments, opts => opts.MapFrom(src => jsonConverter.ConvertToJson(args)));

            Mapper.Initialize(config);

            return Mapper.Instance;
        }
    }
}
