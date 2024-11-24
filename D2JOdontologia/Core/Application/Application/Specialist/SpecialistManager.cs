using Application.Dtos;
using Application.Ports;
using Application.Responses;
using Application.Specialist.Requests;
using Domain.Ports;
using Domain.Schedule.Exceptions;
using Domain.Specialist.Exceptions;
using Domain.User.Exceptions;

namespace Application.Specialist
{
    public class SpecialistManager : ISpecialistManager
    {
        private readonly ISpecialistRepository _specialistRepository;
        private readonly ISpecialtyRepository _specialtyRepository;

        public SpecialistManager(ISpecialistRepository specialistRepository, ISpecialtyRepository specialtyRepository)
        {
            _specialistRepository = specialistRepository;
            _specialtyRepository = specialtyRepository;
        }

        public async Task<SpecialistResponse> CreateSpecialist(CreateSpecialistRequest request)
        {
            try
            {
                var specialist = SpecialistDto.MapToEntity(request.SpecialistData);

                var existingSpecialties = new List<Domain.Entities.Specialty>();
                foreach (var specialtyId in request.SpecialistData.SpecialtyIds)
                {
                    var specialty = await _specialtyRepository.Get(specialtyId);
                    if (specialty == null)
                    {
                        throw new InvalidSpecialtyException($"Specialty with ID {specialtyId} not found.");
                    }
                    existingSpecialties.Add(specialty);
                }
                specialist.Specialties = existingSpecialties;

                await specialist.Save(_specialistRepository);

                return new SpecialistResponse
                {
                    SpecialistData = SpecialistResponseDto.MapToResponseDto(specialist),
                    Success = true,
                    Message = "Specialist created successfully!"
                };
            }
            catch (InvalidEmailException ex)
            {
                return new SpecialistResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.INVALID_EMAIL,
                    Message = ex.Message
                };
            }
            catch (MissingRequiredInformationException ex)
            {
                return new SpecialistResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.MISSING_REQUIRED_INFORMATION,
                    Message = ex.Message
                };
            }
            catch (InvalidSpecialtyException ex)
            {
                return new SpecialistResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.SPECIALTY_NOT_FOUND,
                    Message = ex.Message
                };
            }
            catch (InvalidCroException ex)
            {
                return new SpecialistResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.INVALID_CRO,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new SpecialistResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }

        }

        public async Task<IEnumerable<SpecialistResponseDto>> GetAllSpecialists()
        {
            var specialists = await _specialistRepository.GetAll();

            var responseList = specialists.Select(s => SpecialistResponseDto.MapToResponseDto(s)).ToList();

            return responseList;
        }

        public async Task<SpecialistResponse> GetSpecialist(int specialistId)
        {
            var specialist = await _specialistRepository.Get(specialistId);

            if (specialist == null)
            {
                return new SpecialistResponse
                {
                    Success = false,
                };
            }

            return new SpecialistResponse
            {
                SpecialistData = SpecialistResponseDto.MapToResponseDto(specialist),
                Success = true,
            };
        }

        public async Task<SpecialistResponse> UpdateSpecialist(int id, UpdateSpecialistRequest updateRequest)
        {
            try
            {
                var specialist = await _specialistRepository.Get(id);
                if (specialist == null)
                    throw new SpecialistNotFoundException($"Specialist with ID {id} not found.");

                var data = updateRequest.SpecialistData;

                if (!string.IsNullOrWhiteSpace(data.Address))
                    specialist.Address = data.Address;

                if (!string.IsNullOrWhiteSpace(data.Fone))
                    specialist.Fone = data.Fone;

                if (data.SpecialtyIds != null && data.SpecialtyIds.Any())
                {
                    var existingSpecialtyIds = specialist.Specialties.Select(s => s.Id).ToList();
                    var newSpecialtyIds = data.SpecialtyIds.Except(existingSpecialtyIds).ToList();

                    if (newSpecialtyIds.Any())
                    {
                        var newSpecialties = await _specialtyRepository.GetByIds(newSpecialtyIds);
                        if (!newSpecialties.Any())
                            throw new InvalidSpecialtyException("Some or all provided specialty IDs are invalid.");

                        foreach (var specialty in newSpecialties)
                        {
                            specialist.Specialties.Add(specialty);
                        }
                    }
                }

                await _specialistRepository.Update(specialist);

                return new SpecialistResponse
                {
                    Success = true,
                    SpecialistData = SpecialistResponseDto.MapToResponseDto(specialist),
                    Message = "Specialist updated successfully."
                };
            }
            catch (SpecialistNotFoundException ex)
            {
                return new SpecialistResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.SPECIALIST_NOT_FOUND,
                    Message = ex.Message
                };
            }
            catch (InvalidSpecialtyException ex)
            {
                return new SpecialistResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.SPECIALTY_NOT_FOUND,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new SpecialistResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = $"Error updating specialist: {ex.Message}"
                };
            }
        }



    }
}
