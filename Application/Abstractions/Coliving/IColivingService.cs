using Application.Contracts;

namespace Application.Abstractions.Coliving;

public interface IColivingService
{
    Task<ColivingResponseDto[]> GetPagedList();
}
