using AutoMapper;
using Domain.API.Models;
using Domain.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System.Collections.Generic;

namespace Domain.API.Profiles
{
    public class ExampleDataProfile : Profile
    {
        public ExampleDataProfile()
        {
            CreateMap<ExampleData, ExampleDataContract>()
                .ForMember(dest=> dest.Id, opt => opt.MapFrom(src=> src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ExampleDataChild, opt => opt.MapFrom(src => src.Children))
                .ForMember(dest => dest.Single, opt => opt.MapFrom(src => src.Single))
                .ReverseMap();
            CreateMap<ExampleData, ExampleDataContractNew>().ReverseMap();

            CreateMap<ExampleDataChild, ExampleDataChildContract>().ReverseMap();
            CreateMap<ExampleDataSingle, ExampleDataSingleContract>().ReverseMap();

            //To See the Error Uncomment Below
            //CreateMap<List<ExampleData>, List<ExampleDataContract>>().ReverseMap();

            CreateMap<JsonPatchDocument<ExampleDataContract>, JsonPatchDocument<ExampleData>>();
            CreateMap<Operation<ExampleDataContract>, Operation<ExampleData>>();
        }
    }
    public static class ExampleDataProfileHelper
    {
        private static Dictionary<string, string> keys = new Dictionary<string, string>()
        {
            //{ "/name","/name" }
        };
        public static List<Operation<ExampleDataContract>> MapPatchPaths(this List<Operation<ExampleDataContract>> ops)
        {
            foreach (var o in ops)
            {
                o.path = PropertyMap(o.path, o.path);
            }
            return ops;
        }
        public static string PropertyMap(this string prop, string _default)
        {
            return keys.GetValueOrDefault(prop, _default);
        }

    }
}
