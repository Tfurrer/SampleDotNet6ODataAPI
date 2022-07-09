using AutoMapper;
using Domain.API.Models;
using Domain.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.API.Profiles
{
    public class ExampleDataProfile : Profile
    {
        public ExampleDataProfile()
        {
            CreateMap<ExampleData, ExampleDataContract>().ReverseMap();
            CreateMap<ExampleData, ExampleDataContractNew>().ReverseMap();

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
