using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ComponentRequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class ComponentRequestItemService : IComponentRequestItemService
    {
        private readonly IComponentRequestItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ComponentRequestItemService(IComponentRequestItemRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ComponentRequestItemResponse> MarkNotFoundAsync(int id)
        {
            var item = await _repository.FindAsync(i => i.Id == id);

            if (item == null)
                throw new NotFoundException("Not found");

            if (item.Status != ComponentRequestItemStatus.Requested)
                throw new ConflictException("Item is not in Requested state");

            item.Status = ComponentRequestItemStatus.NotFound;
            _repository.Update(item);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ComponentRequestItem, ComponentRequestItemResponse>(item);
        }

        public async Task<IEnumerable<ComponentRequestItemResponse>> TransferAsync(ComponentRequestItemTransfer transfer)
        {
            var ids = transfer.Ids ?? new List<int>();
            var items = (await _repository.GetByIdsAsync(ids)).ToList();

            if (items.Count != ids.Distinct().Count())
                throw new NotFoundException("One or more items were not found");

            var notRequested = items.Where(i => i.Status != ComponentRequestItemStatus.Requested).ToList();
            if (notRequested.Any())
                throw new ConflictException($"Item(s) {string.Join(", ", notRequested.Select(i => i.Id))} are not in Requested state");

            foreach (var item in items)
            {
                item.Status = ComponentRequestItemStatus.Transferred;
                item.TransferredDate = DateTime.Now;
                _repository.Update(item);
            }

            await _unitOfWork.SaveAsync();

            return _mapper.Map<IEnumerable<ComponentRequestItem>, IEnumerable<ComponentRequestItemResponse>>(items);
        }
    }
}
