using AutoMapper;
using SMT.Domain.BoardFlow.V2;
using SMT.ViewModel.Dto.BoardMovementV2Dto;
using SMT.ViewModel.Dto.BoardV2Dto;
using SMT.ViewModel.Dto.QrReaderV2Dto;

namespace SMT.Services.Mapping
{
    // Kept as its own Profile (rather than added to ModelToResourceProfile) so the
    // V2 board-flow redesign doesn't require touching the existing V1 mapping file.
    // AutoMapper is registered via assembly scan (see ServiceExtension.AddAutoMapper),
    // so any Profile in this assembly is picked up automatically.
    public class BoardFlowV2MappingProfile : Profile
    {
        public BoardFlowV2MappingProfile()
        {
            CreateMap<QrReaderV2, QrReaderV2Response>();

            CreateMap<BoardV2, BoardV2Response>();

            CreateMap<BoardMovementV2, BoardMovementV2Response>()
                .ForMember(d => d.QrReaderName, o => o.MapFrom(s => s.QrReader != null ? s.QrReader.Name : null));
        }
    }
}
