using Application.Dtos;
using Application.Ports;
using Application.Responses;
using Application.Specialty.Responses;
using Domain.Ports;
using Domain.Specialty.Exceptions;

namespace Application.Specialty
{
    public class SpecialtyManager : ISpecialtyManager
    {
        private readonly ISpecialtyRepository _specialtyRepository;

        public SpecialtyManager(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }

        public async Task<SpecialtyResponse> GetSpecialty(int Id)
        {
            try
            {
                var specialty = await _specialtyRepository.Get(Id);

                if (specialty == null)
                {
                    throw new SpecialtyNotFoundException();
                }

                return new SpecialtyResponse
                {
                    Success = true,
                    SpecialtyData = SpecialtyDto.MapToDto(specialty),
                    Message = "Specialty retrieved successfully."
                };
            }
            catch (SpecialtyNotFoundException)
            {
                return new SpecialtyResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.SPECIALTY_NOT_FOUND,
                    Message = "Specialty not found."
                };
            }
            catch (Exception ex)
            {
                return new SpecialtyResponse
                {
                    Success = false,
                    Message = $"Error retrieving specialty: {ex.Message}"
                };
            }
        }

        public async Task<SpecialtyListResponse> GetAllSpecialties()
        {
            try
            {
                var specialties = await _specialtyRepository.GetAll();

                if (!specialties.Any())
                {
                    throw new SpecialtyNotFoundException();
                }

                return new SpecialtyListResponse
                {
                    Success = true,
                    Data = specialties.Select(SpecialtyDto.MapToDto).ToList(),
                    Message = "Specialties retrieved successfully."
                };
            }
            catch (SpecialtyNotFoundException)
            {
                return new SpecialtyListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.SPECIALTY_NOT_FOUND,
                    Message = "No specialties found."
                };
            }
            catch (Exception ex)
            {
                return new SpecialtyListResponse
                {
                    Success = false,
                    Message = $"Error retrieving specialties: {ex.Message}"
                };
            }
        }
    }
}
