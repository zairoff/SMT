using AutoMapper;
using SMT.Domain.ReturnedProducts;

namespace SMT.Services.Mapping
{
    public class ModelToModelProfile : Profile
    {
        public ModelToModelProfile()
        {
            CreateMap<ReturnedProductTransaction, ReturnedProductRepair>()
               .ForMember(m => m.ReturnedProductTransactionId, s => s.MapFrom(s => s.Id))
               .ForMember(m => m.Id, s => s.Ignore());

            CreateMap<ReturnedProductTransaction, ReturnedProductStore>()
               .ForMember(m => m.ReturnedProductTransactionId, s => s.MapFrom(s => s.Id))
               .ForMember(m => m.Id, s => s.Ignore());

            CreateMap<ReturnedProductTransaction, ReturnedProductUtilize>()
               .ForMember(m => m.ReturnedProductTransactionId, s => s.MapFrom(s => s.Id))
               .ForMember(m => m.Id, s => s.Ignore());

            CreateMap<ReturnedProductTransaction, ReturnedProductBufferZone>()
               .ForMember(m => m.ReturnedProductTransactionId, s => s.MapFrom(s => s.Id))
               .ForMember(m => m.Id, s => s.Ignore());
        }
    }
}
