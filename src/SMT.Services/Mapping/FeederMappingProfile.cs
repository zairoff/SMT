using AutoMapper;
using SMT.Domain.Feeder;
using SMT.ViewModel.Dto.FeederSetupDto;
using SMT.ViewModel.Dto.LineMachineDto;

namespace SMT.Services.Mapping
{
    // Feeder verification mappings, kept in their own profile like BoardFlowV2MappingProfile.
    public class FeederMappingProfile : Profile
    {
        public FeederMappingProfile()
        {
            CreateMap<LineMachine, LineMachineResponse>();

            CreateMap<MachineModule, MachineModuleResponse>();

            CreateMap<FeederSlot, FeederSlotResponse>()
                .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.MachineModule.Name))
                .ForMember(d => d.CurrentRCode, o => o.MapFrom(s => s.CurrentComponent.RCode))
                .ForMember(d => d.CurrentSpecification, o => o.MapFrom(s => s.CurrentComponent.Specification));

            // Items are mapped separately (detail requests only) - list queries don't load their slots.
            CreateMap<FeederSetup, FeederSetupResponse>()
                .ForMember(d => d.ItemCount, o => o.MapFrom(s => s.Items.Count))
                .ForMember(d => d.Items, o => o.Ignore());

            CreateMap<FeederSetupItem, FeederSetupItemResponse>()
                .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.FeederSlot.MachineModule.Name))
                .ForMember(d => d.ModuleOrder, o => o.MapFrom(s => s.FeederSlot.MachineModule.Order))
                .ForMember(d => d.SlotNumber, o => o.MapFrom(s => s.FeederSlot.Number))
                .ForMember(d => d.SlotCode, o => o.MapFrom(s => s.FeederSlot.SlotCode))
                .ForMember(d => d.RCode, o => o.MapFrom(s => s.Component.RCode))
                .ForMember(d => d.PartNumbers, o => o.MapFrom(s => s.Component.PartNumber))
                .ForMember(d => d.Specification, o => o.MapFrom(s => s.Component.Specification));
        }
    }
}
