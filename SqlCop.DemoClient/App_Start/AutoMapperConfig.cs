using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;
using SqlCop.Common;

namespace SqlCop.DemoClient
{
    public class AutoMapperConfig
    {
        public static void CreateMaps()
        {
          Mapper.CreateMap<RuleModel, ViewModels.Rule>()
            .ForMember(m => m.Selected, opt => opt.UseValue(true));
        }
    }
}