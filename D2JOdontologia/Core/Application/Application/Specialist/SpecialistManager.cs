using Application.Dtos;
using Application.Ports;
using Application.Responses;
using Application.Specialist.Requests;
using Domain.Ports;
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

                request.SpecialistData.Id = specialist.Id;

                return new SpecialistResponse
                {
                    SpecialistData = SpecialistDto.MapToDto(specialist),
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

        public async Task<IEnumerable<SpecialistDto>> GetAllSpecialists()
        {
            var specialists = await _specialistRepository.GetAll();

            var responseList = specialists.Select(s => SpecialistDto.MapToDto(s)).ToList();

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
                SpecialistData = SpecialistDto.MapToDto(specialist),
                Success = true,
            };
        }
    }
}
