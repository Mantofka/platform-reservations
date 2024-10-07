using Application.Abstractions.Colivings;
using Application.Contracts;
using Infrastructure.Persistence.Abstractions;
using Infrastructure.Persistence.Abstractions.Models.Coliving;

namespace Infrastructure.Services.Coliving;

public class ColivingService : IColivingService
{
    private readonly IUnitOfWork _unitOfWork;

    public ColivingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ColivingResponseDto[]> GetPagedList()
    {
        var repository = _unitOfWork.GetColivings();
        var entities = await repository.GetPagedList().ConfigureAwait(false);
        
        var result = entities.Select(ToContract).ToArray();

        return result;
    }
    
    private static ColivingResponseDto ToContract(Coliving coliving)
    {
        var result = new ColivingResponseDto
        {
            Id = coliving.Id,
            Name = coliving.Name,
            Address = coliving.
        };

        return result;
    }
}