using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SMT.Access.Data;
using SMT.Access.Repository;
using SMT.Access.Unit;
using SMT.Domain;
using System.Threading.Tasks;
using Xunit;

namespace SMT.Access.Test
{
    public class BrandRepositoryTests : BaseRepositoryHelper
    {
        private readonly BrandRepository _brandRepository;
        private readonly Brand brand1 = new() { Id = 1, Name = "Samsung" };
        private readonly Brand brand2 = new() { Id = 2, Name = "LG" };

        public BrandRepositoryTests()
        {
            _brandRepository = new BrandRepository(_context);
        }

        [Fact]
        public async Task Add_Should_Add_Brand_ToDB()
        {
            await _brandRepository.AddAsync(brand1);
            await _unitOfWork.SaveAsync();

            var result = await _brandRepository.FindAsync(b => b.Name == "Samsung");

            Assert.NotNull(result);
            Assert.Equal(brand1.Id, result.Id);
            Assert.Equal(brand1.Name, result.Name);
        }

        [Fact]
        public async Task Update_Should_Update_Brand()
        {
            await _brandRepository.AddAsync(brand1);
            await _unitOfWork.SaveAsync();

            brand1.Name = "Sony";

            _brandRepository.Update(brand1);

            var result = await _brandRepository.FindAsync(b => b.Id == 1);

            Assert.NotNull(result);
            Assert.Equal(brand1.Id, result.Id);
            Assert.Equal(brand1.Name, result.Name);
        }

        [Fact]
        public async Task Delete_Should_RemoveBrand_FromDB()
        {
            await _brandRepository.AddAsync(brand1);
            await _unitOfWork.SaveAsync();

            _brandRepository.Delete(brand1);
            await _unitOfWork.SaveAsync();

            var result = await _brandRepository.FindAsync(b => b.Id == 1);

            Assert.Null(result);
        }
    }
}
