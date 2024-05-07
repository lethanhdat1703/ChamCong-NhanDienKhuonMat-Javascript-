using Backend.DTO;

namespace Backend.Repository
{
    public interface ITimeSheetRepository
    {
        public Task<List<TimeSheetDTO>>getAllTimeSheetsAsync();
        public Task<TimeSheetDTO> getTimeSheetsAsync(int id);
        public  Task<int> AddTiemSheetAsync(TimeSheetDTO timeSheetDTO);
        public Task  UpdateTimeSheetAsync(TimeSheetDTO timeSheetDTO,int id  );
        public Task DeleteTimeSheetAsync(int id);

    }
}
