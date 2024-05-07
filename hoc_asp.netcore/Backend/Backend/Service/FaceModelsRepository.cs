using AutoMapper;
using Backend.Data;
using Backend.DTO;
using Backend.Models;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service
{
    public class FaceModelsRepository : IFaceModelsRepository
    {
        private readonly NhandienDbContext _dbContext;
        private readonly IMapper _mapper;

        public FaceModelsRepository(NhandienDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> SaveImageUrlsAsync(List<string> imageUrls, int employeeId)
        {
            foreach (var imageUrl in imageUrls)
            {
                var faceModelDto = new FaceModelsDTO { Img = imageUrl, EmployeeId =employeeId };
                var faceModel = _mapper.Map<FaceModels>(faceModelDto);
                _dbContext.Add(faceModel);
            }

            return await _dbContext.SaveChangesAsync();
        }
    }
}
