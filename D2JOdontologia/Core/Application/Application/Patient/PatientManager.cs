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
                patient.SetPassword(request.PatientData.Password);
                await patient.Save(_patientRepository);

                var responseDto = PatientResponseDto.MapToResponseDto(patient);

                return new PatientResponse
                {
                    PatientData = responseDto,
                    Success = true,
                    Message = "Patient created successfully."
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
                    PatientData = PatientResponseDto.MapToResponseDto(patient)
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
                PatientData = PatientResponseDto.MapToResponseDto(patient)
            };
        }

        public async Task<PatientResponse> UpdatePatient(int patientId, UpdatePatientRequest updateRequest)
        {
            try
            {
                var patient = await _patientRepository.Get(patientId);
                if (patient == null)
                    throw new PatientNotFoundException();

                if (!string.IsNullOrWhiteSpace(updateRequest.PatientData.Address))
                    patient.Address = updateRequest.PatientData.Address;

                if (!string.IsNullOrWhiteSpace(updateRequest.PatientData.Fone))
                    patient.Fone = updateRequest.PatientData.Fone;

                await _patientRepository.Update(patient);

                return new PatientResponse
                {
                    Success = true,
                    PatientData = PatientResponseDto.MapToResponseDto(patient),
                    Message = "Patient updated successfully."
                };
            }
            catch (PatientNotFoundException)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.PATIENT_NOT_FOUND,
                    Message = "Patient not found."
                };
            }
            catch (Exception ex)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = $"Error updating patient: {ex.Message}"
                };
            }
        }

    }
}
