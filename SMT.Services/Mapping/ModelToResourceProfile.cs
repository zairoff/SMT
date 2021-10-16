using AutoMapper;
using SMT.Common.Dto;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<PcbReportResponse, PcbReportResponse>();
        }
    }
}
