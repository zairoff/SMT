﻿using SMT.Common.Dto.DefectDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IDefectService
    {
        Task<IEnumerable<DefectResponse>> GetAllAsync();

        Task<DefectResponse> GetAsync(int id);

        Task<DefectResponse> GetByNameAsync(string name);

        Task<DefectResponse> AddAsync(DefectCreate defectCreate);

        Task<DefectResponse> UpdateAsync(int id, DefectUpdate defectUpdate);

        Task<DefectResponse> DeleteAsync(int id);
    }
}
