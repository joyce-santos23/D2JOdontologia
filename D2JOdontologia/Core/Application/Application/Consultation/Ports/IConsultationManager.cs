using Application.Consultation.Dtos;
using Application.Consultation.Responses;
using Application.Dtos;
using Application.Responses;

namespace Application.Ports
{
    public interface IConsultationManager
    {
        Task<ConsultationResponse> CreateConsultation(ConsultationRequestDto consultationDto);
        Task<ConsultationResponse> GetConsultation(int consultationId);
        Task<ConsultationListResponse> GetConsultationsByDate(DateTime date);
        Task<ConsultationListResponse> GetConsultationsByPatient(int patientId);
        Task<ConsultationListResponse> GetConsultationsBySpecialist(int specialistId);
        Task<ConsultationResponse> UpdateConsultation(int id, ConsultationUpdateRequestDto consultationDto);
    }
}
