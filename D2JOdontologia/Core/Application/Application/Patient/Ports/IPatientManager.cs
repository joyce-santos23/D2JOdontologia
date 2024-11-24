using Application.Dtos;
using Application.Patient.Requests;
using Application.Responses;

namespace Application.Ports
{
    public interface IPatientManager
    {
        Task<PatientResponse> CreatePatient(CreatePatientRequest request);
        Task<PatientResponse> GetPatient(int patientId);
        Task<IEnumerable<PatientResponse>> GetAllPatient();
        Task<PatientResponse> UpdatePatient(int patientId, UpdatePatientRequest updateRequest);
    }
}
