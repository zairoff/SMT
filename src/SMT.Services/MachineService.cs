using AutoMapper;
using Microsoft.Extensions.Configuration;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.Services.Interfaces.FileSystem;
using SMT.ViewModel.Dto.MachineDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class MachineService : IMachineService
    {
        private readonly IMachineRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;
        private readonly string _imagesFolder;
        private readonly string _requestPath;

        public MachineService(IMapper mapper, IUnitOfWork unitOfWork, IMachineRepository repository,
                                IConfiguration configuration, IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _imageService = imageService;
            _configuration = configuration;
            _imagesFolder = _configuration["AppSettings:MachineImagesFolder"];
            _requestPath = _configuration["AppSettings:MachineRequestPath"];
        }

        public async Task<MachineResponse> AddAsync(MachineCreate machineCreate)
        {
            var machine = await _repository.FindAsync(p => p.Name == machineCreate.Name);

            if (machine != null)
                throw new ConflictException($"{machineCreate.Name} already exists");

            machine = _mapper.Map<MachineCreate, Machine>(machineCreate);

            var fileNme = await _imageService.SaveAsync(machineCreate.File, _imagesFolder);
            machine.ImageUrl = fileNme;

            await _repository.AddAsync(machine);
            await _unitOfWork.SaveAsync();

            UpdateMachineImageUrl(machine);

            return _mapper.Map<Machine, MachineResponse>(machine);
        }

        public async Task<MachineResponse> DeleteAsync(int id)
        {
            var machine = await _repository.FindAsync(p => p.Id == id);

            if (machine == null)
                throw new NotFoundException("Not found");

            _repository.Delete(machine);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Machine, MachineResponse>(machine);
        }

        public async Task<IEnumerable<MachineResponse>> GetAllAsync()
        {
            var machines = await _repository.GetAllAsync();
            UpdateMachinesImageUrl(machines);

            return _mapper.Map<IEnumerable<Machine>, IEnumerable<MachineResponse>>(machines);
        }

        public async Task<MachineResponse> GetAsync(int id)
        {
            var machine = await _repository.FindAsync(p => p.Id == id);
            UpdateMachineImageUrl(machine);

            return _mapper.Map<Machine, MachineResponse>(machine);
        }

        private Machine UpdateMachineImageUrl(Machine machine)
        {
            var url = _imageService.LoadUrl(_requestPath, machine.ImageUrl);
            machine.ImageUrl = url;
            return machine;
        }

        private IEnumerable<Machine> UpdateMachinesImageUrl(IEnumerable<Machine> machines)
        {
            foreach (var machine in machines)
            {
                var url = _imageService.LoadUrl(_requestPath, machine.ImageUrl);
                machine.ImageUrl = url;
            }

            return machines;
        }
    }
}
