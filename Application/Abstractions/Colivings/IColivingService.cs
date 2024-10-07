using Application.Contracts;

namespace Application.Abstractions.Colivings;

public interface IColivingService
{
    Task<ColivingResponseDto[]> GetPagedList();

    Task<ColivingResponseDto?> FindById(Guid id);
    
    Task<ColivingResponseDto> Create(ColivingCreateDto input);
    
    Task<ColivingResponseDto?> Edit(Guid id, ColivingCreateDto input);
    
    Task<bool> Remove(Guid id);
}
