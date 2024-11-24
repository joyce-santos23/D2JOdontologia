using Application.Dtos;
using Application.Responses;
using Application.Specialist.Requests;

namespace Application.Ports
{
    public interface ISpecialistManager
    {
        Task<SpecialistResponse> CreateSpecialist(CreateSpecialistRequest request);
        Task<SpecialistResponse> GetSpecialist(int Id);
        Task<IEnumerable<SpecialistResponseDto>> GetAllSpecialists();
        Task<SpecialistResponse> UpdateSpecialist(int id, UpdateSpecialistRequest updateRequest);

    }
}
