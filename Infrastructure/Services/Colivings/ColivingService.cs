using Application.Abstractions.Colivings;
using Application.Contracts;
using Application.Contracts.Room;
using Application.Contracts.Tenant;
using Application.Validators;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Domain.Tenants;
using Infrastructure.Persistence.Abstractions;
using Infrastructure.Persistence.Abstractions.Models.Coliving;

namespace Infrastructure.Services.Colivings;

public class ColivingService : IColivingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ColivingCreateDto> _validator;

    public ColivingService(IUnitOfWork unitOfWork, IValidator<ColivingCreateDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<ColivingResponseDto[]> GetPagedList()
    {
        var repository = _unitOfWork.GetColivings();
        var entities = await repository.GetPagedList().ConfigureAwait(false);
        
        var result = entities.Select(ToContract).ToArray();

        return result;
    }
    
    public async Task<TenantResponseDto[]> GetTenants(Guid id, Guid roomId)
    {
        var repository = _unitOfWork.GetColivings();
        var entities = await repository.GetTenants(id, roomId).ConfigureAwait(false);
        
        var result = entities.Select(ToTenantContract).ToArray();

        return result;
    }
    
    private static TenantResponseDto ToTenantContract(Tenant tenant)
    {
        var result = new TenantResponseDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Email = tenant.Email,
            Surname = tenant.Surname,
            PhoneNumber = tenant.PhoneNumber,
            BirthDate = tenant.BirthDate
        };

        return result;
    }
    
    private static ColivingResponseDto ToContract(Coliving coliving)
    {
        var rooms = coliving.Rooms != null
            ? coliving.Rooms.Select(room => new RoomResponseDto()
            {
                Id = room.Id,
                Number = room.Number,
                Description = room.Description,
                Size = room.Size,
                FloorNumber = room.FloorNumber,
                Price = room.Price,
            }).ToList()
            : null;
        var result = new ColivingResponseDto
        {
            Id = coliving.Id,
            Name = coliving.Name,
            Address = coliving.Address,
            RepresenterName = coliving.RepresenterName,
            PhoneNumber = coliving.PhoneNumber,
            Email = coliving.Email,
        };

        return result;
    }
    
    public async Task<ColivingResponseDto> Create(ColivingCreateDto input)
    {
        var repository = _unitOfWork.GetColivings();
        var entity = ToEntity(input);
        ValidationResult validationResult = await _validator.ValidateAsync(input);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var result = await repository.CreateAsync(entity).ConfigureAwait(false);

        await _unitOfWork.Commit().ConfigureAwait(false);

        var resultContract = ToContract(result);

        return resultContract;
    }
    
    public async Task<bool> Remove(Guid id)
    {
        var repository = _unitOfWork.GetColivings();
        await repository.RemoveAsync(id).ConfigureAwait(false);

        await _unitOfWork.Commit().ConfigureAwait(false);

        return true;
    }
    
    public async Task<ColivingResponseDto?> FindById(Guid id)
    {
        var repository = _unitOfWork.GetColivings();
        var entity = await repository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity == null)
        {
            return null;
        }

        var result = ToContract(entity);

        return result;
    }
    
    private static Coliving ToEntity(ColivingCreateDto coliving)
    {
        var result = new Coliving
        {
            Id = coliving.Id ?? Guid.NewGuid(),
            Name = coliving.Name,
            Address = coliving.Address,
            RepresenterName = coliving.RepresenterName,
            Email = coliving.Email,
            PhoneNumber = coliving.PhoneNumber,
        };

        return result;
    }
    
    public async Task<ColivingResponseDto?> Edit(Guid id, ColivingCreateDto coliving)
    {
        var repository = _unitOfWork.GetColivings();
        var entity = await repository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity == null)
        {
            return null;
        }

        entity.Address = coliving.Address;
        entity.RepresenterName = coliving.RepresenterName;
        entity.PhoneNumber = coliving.PhoneNumber;
        entity.Name = coliving.Name;
        entity.Email = coliving.Email;

        await _unitOfWork.Commit().ConfigureAwait(false);
        
        var result = ToContract(entity);

        return result;
    }
}