using Swashbuckle.AspNetCore.Annotations;

namespace Backend.DTO
{
    public class TimeSheetDTO
    {
        [SwaggerSchema(Format = "time")]
        public int? Id { get; set; }

        [SwaggerSchema(Format = "time")]
        public TimeOnly? StartTime { get; set; }

        [SwaggerSchema(Format = "time")]
        public TimeOnly? EndTime { get; set; }

        [SwaggerSchema(Format = "time")]
        public TimeOnly? LunchBreak { get; set; }

    }
}
