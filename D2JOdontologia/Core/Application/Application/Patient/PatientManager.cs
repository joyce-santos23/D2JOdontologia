using Application.Dtos;
using Application.Patient.Requests;
using Application.Ports;
using Application.Responses;
using Domain.Patient.Exceptions;
using Domain.Ports;
using Domain.User.Exceptions;

namespace Application.Patient
{
    public class PatientManager : IPatientManager
    {
        private readonly IPatientRepository _patientRepository;

        public PatientManager(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public async Task<PatientResponse> CreatePatient(CreatePatientRequest request)
        {
            try
            {
                var patient = PatientDto.MapToEntity(request.PatientData);
                await patient.Save(_patientRepository);
                request.PatientData.Id = patient.Id;

                return new PatientResponse
                {
                    PatientData = request.PatientData,
                    Success = true, 
                };
            }
            catch (MissingRequiredInformationException ex)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.MISSING_REQUIRED_INFORMATION,
                    Message = ex.Message
                };

            }
            catch (InvalidCpfException ex)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = ex.Message
                };
            }
            catch (InvalidBirthDateException ex)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = ex.Message
                };
            }
            catch (InvalidEmailException ex)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.INVALID_EMAIL,
                    Message = ex.Message
                };
            }
            catch (Exception)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }

        public async Task<IEnumerable<PatientResponse>> GetAllPatient()
        {
            var patients = await _patientRepository.GetAll();
            var responseList = new List<PatientResponse>();

            foreach (var patient in patients)
            {
                responseList.Add(new PatientResponse
                {
                    Success = true,
                    PatientData = PatientDto.MapToDto(patient)
                });
            }
            return responseList;
        }

        public async Task<PatientResponse> GetPatient(int patientId)
        {
            var patient = await _patientRepository.Get(patientId);

            if (patient == null)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.PATIENT_NOT_FOUND,
                    Message = "No patient record was found with the given id"
                };
            }
            return new PatientResponse
            {
                Success = true,
                PatientData = PatientDto.MapToDto(patient)
            };
        }
    }
}
