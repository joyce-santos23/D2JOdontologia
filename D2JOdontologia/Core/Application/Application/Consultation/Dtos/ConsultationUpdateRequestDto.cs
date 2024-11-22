using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Consultation.Dtos
{
    public class ConsultationUpdateRequestDto
    {
        public string Procedure { get; set; } 
        public int ScheduleId { get; set; } 
    }

}
