using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Consultation.Responses
{
    public class ConsultationListResponse : Response  
    {
        public List<ConsultationResponseDto> Data { get; set; }
    }
}
