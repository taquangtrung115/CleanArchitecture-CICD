using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.RentalRoom.Rooms;

/// <summary>
/// Entity quản lý thông tin phòng trọ
/// </summary>
public class Room : AuditableEntity<Guid>
{
    /// <summary>
    /// Số phòng (Ví dụ: P101, A201, ...)
    /// </summary>
    public string RoomNumber { get; private set; }
    
    /// <summary>
    /// Sức chứa tối đa của phòng (số người)
    /// </summary>
    public int Capacity { get; private set; }
    
    /// <summary>
    /// Giá thuê mỗi đêm/tháng (VNĐ)
    /// </summary>
    public decimal PricePerNight { get; private set; }
    
    /// <summary>
    /// Mô tả chi tiết về phòng (tiện nghi, diện tích, ...)
    /// </summary>
    public string Description { get; private set; }
    
    /// <summary>
    /// Trạng thái phòng có sẵn để cho thuê hay không
    /// </summary>
    public bool IsAvailable { get; private set; }
    
    /// <summary>
    /// ID địa chỉ của phòng
    /// </summary>
    public Guid? LocationId { get; private set; }

    // Navigation properties - must be virtual for lazy loading
    /// <summary>
    /// Địa chỉ của phòng (lazy loading)
    /// </summary>
    public virtual Locations.Location? Location { get; private set; }

    // Protected parameterless constructor for EF Core proxies
    protected Room()
    {
        RoomNumber = string.Empty;
        Description = string.Empty;
    }

    public Room(Guid id, string roomNumber, int capacity, decimal pricePerNight, string description, Guid? locationId = null)
    {
        Id = id;
        RoomNumber = roomNumber;
        Capacity = capacity;
        PricePerNight = pricePerNight;
        Description = description;
        IsAvailable = true;
        LocationId = locationId;
    }
    
    public void UpdateDetails(string roomNumber, int capacity, decimal pricePerNight, string description, Guid? locationId = null)
    {
        RoomNumber = roomNumber;
        Capacity = capacity;
        PricePerNight = pricePerNight;
        Description = description;
        LocationId = locationId;
    }
    
    public void SetAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }
}
