using AutoMapper;
using SMT.Domain.ReturnedProducts;

namespace SMT.Services.Mapping
{
    public class ModelToModelProfile : Profile
    {
        public ModelToModelProfile()
        {
            CreateMap<ReturnedProductTransaction, ReturnedProductRepair>()
               .ForMember(m => m.ReturnedProductionTransactionId, s => s.MapFrom(s => s.Id));

            CreateMap<ReturnedProductTransaction, ReturnedProductStore>()
               .ForMember(m => m.ReturnedProductionTransactionId, s => s.MapFrom(s => s.Id));

            CreateMap<ReturnedProductTransaction, ReturnedProductUtilize>()
               .ForMember(m => m.ReturnedProductionTransactionId, s => s.MapFrom(s => s.Id));
        }
    }
}
