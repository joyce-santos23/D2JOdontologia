using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Ports;
using Application.Responses;
using Domain.Entities;
using Domain.Ports;
using Domain.Consultation.Exceptions;
using Application.Consultation.Responses;
using Domain.Patient.Exceptions;
using Domain.Schedule.Exceptions;
using Application.Consultation.Dtos;

namespace Application.Consultation
{
    public class ConsultationManager : IConsultationManager
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public ConsultationManager(
            IConsultationRepository consultationRepository,
            IPatientRepository patientRepository,
            IScheduleRepository scheduleRepository)
        {
            _consultationRepository = consultationRepository;
            _patientRepository = patientRepository;
            _scheduleRepository = scheduleRepository;
        }

        public async Task<ConsultationResponse> CreateConsultation(ConsultationRequestDto consultationDto)
        {
            try
            {
                var consultation = ConsultationRequestDto.MapToEntity(consultationDto);

                var patient = await _patientRepository.Get(consultation.PatientId);
                if (patient == null)
                    throw new PatientNotFoundException();

                var schedule = await _scheduleRepository.Get(consultation.ScheduleId);
                if (schedule == null)
                    throw new ScheduleNotFoundException();

                if (!schedule.IsAvailable)
                    throw new InvalidDateException("The selected schedule is not available.");

                consultation.Patient = patient;
                consultation.Schedule = schedule;

                await consultation.Save(_consultationRepository);

                schedule.IsAvailable = false;
                await _scheduleRepository.UpdateAsync(schedule);

                var savedConsultation = await _consultationRepository.Get(consultation.Id);
                var responseDto = ConsultationResponseDto.MapToResponseDto(savedConsultation);

                return new ConsultationResponse
                {
                    Success = true,
                    ConsultationData = responseDto,
                    Message = "Consultation created successfully."
                };
            }
            catch (PatientNotFoundException)
            {
                return new ConsultationResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.PATIENT_NOT_FOUND,
                    Message = "Patient not found."
                };
            }
            catch (ScheduleNotFoundException)
            {
                return new ConsultationResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.SCHEDULE_NOT_FOUND,
                    Message = "Schedule not found."
                };
            }
            catch (InvalidDateException ex)
            {
                return new ConsultationResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.INVALID_DATE,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = $"Error creating consultation: {ex.Message}"
                };
            }
        }

        public async Task<ConsultationResponse> GetConsultation(int consultationId)
        {
            try
            {
                var consultation = await _consultationRepository.Get(consultationId);
                if (consultation == null)
                {
                    throw new ConsultationNotFoundException();
                }

                return new ConsultationResponse
                {
                    Success = true,
                    ConsultationData = ConsultationResponseDto.MapToResponseDto(consultation),
                    Message = "Consultation retrieved successfully."
                };
            }
            catch (ConsultationNotFoundException)
            {
                return new ConsultationResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.CONSULTATION_NOT_FOUND,
                    Message = "Consultation not found."
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse
                {
                    Success = false,
                    Message = $"Error retrieving consultation: {ex.Message}"
                };
            }
        }

        public async Task<ConsultationListResponse> GetConsultationsByDate(DateTime date)
        {
            try
            {
                var consultations = await _consultationRepository.GetByDate(date);

                if (!consultations.Any())
                {
                    throw new ConsultationNotFoundException();
                }

                var responseDtos = consultations.Select(ConsultationResponseDto.MapToResponseDto).ToList();

                return new ConsultationListResponse
                {
                    Success = true,
                    Data = responseDtos,
                    Message = "Consultations retrieved successfully."
                };
            }
            catch (ConsultationNotFoundException)
            {
                return new ConsultationListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.CONSULTATION_NOT_FOUND,
                    Message = "No consultations found for the given date."
                };
            }
            catch (Exception ex)
            {
                return new ConsultationListResponse
                {
                    Success = false,
                    Message = $"Error retrieving consultation: {ex.Message}"
                };
            }
        }


        public async Task<ConsultationListResponse> GetConsultationsByPatient(int patientId)
        {
            try
            {
                var consultations = await _consultationRepository.GetByPatient(patientId);

                if (!consultations.Any())
                {
                    throw new ConsultationNotFoundException();
                }

                var responseDtos = consultations.Select(ConsultationResponseDto.MapToResponseDto).ToList();

                return new ConsultationListResponse
                {
                    Success = true,
                    Data = responseDtos,
                    Message = "Consultations retrieved successfully."
                };
            }
            catch (ConsultationNotFoundException)
            {
                return new ConsultationListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.CONSULTATION_NOT_FOUND,
                    Message = "No consultations found for the given patient."
                };
            }
            catch (Exception ex)
            {
                return new ConsultationListResponse
                {
                    Success = false,
                    Message = $"Error retrieving consultation: {ex.Message}"
                };
            }
        }


        public async Task<ConsultationListResponse> GetConsultationsBySpecialist(int specialistId)
        {
            try
            {
                var consultations = await _consultationRepository.GetBySpecialist(specialistId);

                if (!consultations.Any())
                {
                    throw new ConsultationNotFoundException();
                }

                var responseDtos = consultations.Select(ConsultationResponseDto.MapToResponseDto).ToList();

                return new ConsultationListResponse
                {
                    Success = true,
                    Data = responseDtos,
                    Message = "Consultations retrieved successfully."
                };
            }
            catch (ConsultationNotFoundException)
            {
                return new ConsultationListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.CONSULTATION_NOT_FOUND,
                    Message = "No consultations found for the given specialist."
                };
            }
            catch (Exception ex)
            {
                return new ConsultationListResponse
                {
                    Success = false,
                    Message = $"Error retrieving consultation: {ex.Message}"
                };
            }
        }

        public async Task<ConsultationResponse> UpdateConsultation(int id, ConsultationUpdateRequestDto consultationDto)
        {
            try
            {
                var existingConsultation = await _consultationRepository.Get(id);
                if (existingConsultation == null)
                    throw new ConsultationNotFoundException();

                existingConsultation.Procedure = consultationDto.Procedure;
                existingConsultation.ScheduleId = consultationDto.ScheduleId;

                await _consultationRepository.Update(existingConsultation);

                var responseDto = ConsultationResponseDto.MapToResponseDto(existingConsultation);

                return new ConsultationResponse
                {
                    Success = true,
                    ConsultationData = responseDto,
                    Message = "Consultation updated successfully."
                };
            }
            catch (ConsultationNotFoundException)
            {
                return new ConsultationResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.CONSULTATION_NOT_FOUND,
                    Message = "Consultation not found."
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = $"Error updating consultation: {ex.Message}"
                };
            }
        }
    }
}
