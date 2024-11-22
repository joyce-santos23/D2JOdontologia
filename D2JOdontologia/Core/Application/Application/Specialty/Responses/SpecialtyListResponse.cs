using Application.Dtos;

namespace Application.Specialty.Responses
{
    public class SpecialtyListResponse : Response
    {
        public List<SpecialtyDto> Data { get; set; }
    }
}
