using Application.Abstractions.Tenant;
using Application.Contracts.Maintenance;
using Application.Contracts.Room;
using Application.Contracts.Tenant;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Persistence.Abstractions;

namespace Infrastructure.Services.Maintenance;

public class MaintenanceService : IMaintenanceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<MaintenanceCreateDto> _validator;

    public MaintenanceService(IUnitOfWork unitOfWork, IValidator<MaintenanceCreateDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    
    public async Task<MaintenanceResponseDto[]> GetPagedList()
    {
        var repository = _unitOfWork.GetMaintenances();
        var entities = await repository.GetPagedList().ConfigureAwait(false);
        
        var result = entities.Select(ToContract).ToArray();

        return result;
    }
    
    public async Task<MaintenanceResponseDto[]> GetMaintenances(Guid colivingId, Guid roomId, Guid tenantId)
    {
        var repository = _unitOfWork.GetMaintenances();
        var entities = await repository.GetMaintenancesWithRoomAndTenantAsync(colivingId, roomId, tenantId).ConfigureAwait(false);
        
        var result = entities.Select(ToContract).ToArray();

        return result;
    }
    
    private static MaintenanceResponseDto ToContract(Domain.Maintenance.Maintenance maintenance)
    {
        var result = new MaintenanceResponseDto
        {
            Id = maintenance.Id,
            Title = maintenance.Title,
            Description = maintenance.Description,
            Tenant = new TenantResponseDto
            {
                Id = maintenance.Tenant.Id,
                Email = maintenance.Tenant.Email,
                PhoneNumber = maintenance.Tenant.PhoneNumber,
            },
            Room = new RoomResponseDto
            {
                Id = maintenance.AssignedRoom.Id,
                FloorNumber = maintenance.AssignedRoom.FloorNumber,
                Number = maintenance.AssignedRoom.Number,
            }
        };

        return result;
    }
    
    public async Task<MaintenanceResponseDto> Create(MaintenanceCreateDto input)
    {
        var repository = _unitOfWork.GetMaintenances();
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
    
    private static Domain.Maintenance.Maintenance ToEntity(MaintenanceCreateDto maintenance)
    {
        var result = new Domain.Maintenance.Maintenance
        {
            Id = maintenance.Id ?? Guid.NewGuid(),
            Title = maintenance.Title,
            Description = maintenance.Description,
            TenantId = maintenance.TenantId,
            RoomId = maintenance.RoomId
        };

        return result;
    }
    
    public async Task<bool> Remove(Guid id)
    {
        var repository = _unitOfWork.GetMaintenances();
        await repository.RemoveAsync(id).ConfigureAwait(false);

        await _unitOfWork.Commit().ConfigureAwait(false);

        return true;
    }
    
    public async Task<MaintenanceResponseDto?> FindById(Guid id)
    {
        var repository = _unitOfWork.GetMaintenances();
        var entity = await repository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity == null)
        {
            return null;
        }

        var result = ToContract(entity);

        return result;
    }
    
    public async Task<MaintenanceResponseDto?> Edit(Guid id, MaintenanceCreateDto maintenance)
    {
        var repository = _unitOfWork.GetMaintenances();
        var entity = await repository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity == null)
        {
            return null;
        }

        entity.Title = maintenance.Title;
        entity.Description = maintenance.Description;
        entity.TenantId = maintenance.TenantId;
        entity.RoomId = maintenance.RoomId;

        await _unitOfWork.Commit().ConfigureAwait(false);
        
        var result = ToContract(entity);

        return result;
    }
}