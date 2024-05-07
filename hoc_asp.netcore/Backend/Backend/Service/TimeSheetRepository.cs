using AutoMapper;
using Backend.Data;
using Backend.DTO;
using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service
{
    public class TimeSheetRepository : ITimeSheetRepository
    {
        private readonly NhandienDbContext _context;
        private readonly IMapper _mapper;

        public TimeSheetRepository(NhandienDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddTiemSheetAsync(TimeSheetDTO timeSheetDTO)
        {
            var addTime = _mapper.Map<TimeSheet>(timeSheetDTO);
            _context.timeSheets.Add(addTime); 
            await _context.SaveChangesAsync();
            return addTime.Id;
        }


        public async Task DeleteTimeSheetAsync(int id)
        {
           var deleteTime= _context.timeSheets.SingleOrDefault(d=> d.Id == id);
            if (deleteTime != null)
            {
                _context.timeSheets.Remove(deleteTime);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TimeSheetDTO>> getAllTimeSheetsAsync()
        {
           var timeSheet= await _context.timeSheets.ToListAsync();
            return _mapper.Map<List<TimeSheetDTO>>(timeSheet);
        }

        public async Task<TimeSheetDTO> getTimeSheetsAsync(int id)
        {
            var timeSheet = await _context.timeSheets.FindAsync(id);
            return _mapper.Map<TimeSheetDTO>(timeSheet);

        }

        public async Task UpdateTimeSheetAsync(TimeSheetDTO timeSheetDTO, int id)
        {
            if(timeSheetDTO.Id == id)
            {
                var updateTime=_mapper.Map<TimeSheet>(timeSheetDTO);
                _context.timeSheets.Update(updateTime);
                await _context.SaveChangesAsync();

            }
        }
        
    }
}
