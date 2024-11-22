using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Schedule.Responses
{
    public class ScheduleListResponse : Response
    {
        public List<ScheduleResponseDto> Data { get; set; } = new List<ScheduleResponseDto>();
    }
}
