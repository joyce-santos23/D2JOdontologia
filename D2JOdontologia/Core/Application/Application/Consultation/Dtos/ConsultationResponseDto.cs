using ConsultationEntity = Domain.Entities.Consultation;
namespace Application.Dtos
{
    public class ConsultationResponseDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string SpecialistName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string Procedure { get; set; }
        public DateTime CreatedAt { get; set; }

        public static ConsultationResponseDto MapToResponseDto(ConsultationEntity consultation)
        {
            return new ConsultationResponseDto
            {
                Id = consultation.Id,
                PatientName = consultation.Patient?.Name,
                SpecialistName = consultation.Schedule?.Specialist?.Name,
                ScheduleDate = consultation.Schedule?.Data ?? DateTime.MinValue,
                Procedure = consultation.Procedure,
                CreatedAt = consultation.CreatedAt
            };
        }
    }
}
