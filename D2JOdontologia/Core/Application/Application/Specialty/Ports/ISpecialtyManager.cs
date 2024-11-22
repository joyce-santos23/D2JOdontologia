using Application.Responses;
using Application.Specialty.Responses;

namespace Application.Ports
{
    public interface ISpecialtyManager
    {
        Task<SpecialtyResponse> GetSpecialty(int Id);
        public Task<SpecialtyListResponse> GetAllSpecialties();

    }
}
