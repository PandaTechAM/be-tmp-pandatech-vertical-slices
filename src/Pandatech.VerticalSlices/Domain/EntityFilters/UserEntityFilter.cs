using PandaTech.IEnumerableFilters.Attributes;
using PandaTech.IEnumerableFilters.Converters;
using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.z._Old_way.DTOs;

namespace Pandatech.VerticalSlices.Domain.EntityFilters;

public abstract class UserEntityFilter
{
    [MappedToProperty(nameof(UserEntity.Id), ConverterType = typeof(FilterPandaBaseConverter))]
    public long Id { get; set; }

    [MappedToProperty(nameof(UserEntity.Username))]
    [Order]
    public string Username { get; set; } = null!;

    [MappedToProperty(nameof(UserEntity.FullName))]
    public string FullName { get; set; } = null!;

    [MappedToProperty(nameof(UserEntity.Role))]
    public RolesSelect Role { get; set; } = null!;

    [MappedToProperty(nameof(UserEntity.CreatedAt))]
    public DateTime CreationDate { get; set; }

    [MappedToProperty(nameof(UserEntity.Status))]
    public UserStatus Status { get; set; }

    [MappedToProperty(nameof(UserEntity.Comment))]
    public string? Comment { get; set; }
}