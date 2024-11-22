using System;

namespace Application.Dtos
{
    public class ConsultationRequestDto
    {
       
        public int PatientId { get; set; }
        public string Procedure { get; set; }
        public int ScheduleId { get; set; }



        public static ConsultationRequestDto MapToDto(Domain.Entities.Consultation consultation)
        {
            return new ConsultationRequestDto
            {
                PatientId = consultation.PatientId,
                Procedure = consultation.Procedure,
                ScheduleId = consultation.ScheduleId,

            };
        }

        public static Domain.Entities.Consultation MapToEntity(ConsultationRequestDto dto)
        {
            return new Domain.Entities.Consultation
            {
                PatientId = dto.PatientId,
                Procedure = dto.Procedure,
                ScheduleId = dto.ScheduleId
            };
        }
    }
}
