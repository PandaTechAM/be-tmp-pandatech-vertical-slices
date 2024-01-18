using PandaTech.IEnumerableFilters.Attributes;
using PandaTech.IEnumerableFilters.Converters;
using PandaWebApi.DTOs.User;
using PandaWebApi.Enums;

namespace PandaWebApi.FilterModels;

public abstract class UserFilter
{
    [MappedToProperty(nameof(Models.User.Id), ConverterType = typeof(FilterPandaBaseConverter))]
    public long Id { get; set; }

    [MappedToProperty(nameof(Models.User.Username))]
    [Order]
    public string Username { get; set; } = null!;

    [MappedToProperty(nameof(Models.User.FullName))]
    public string FullName { get; set; } = null!;

    [MappedToProperty(nameof(Models.User.Role))]
    public RolesSelect Role { get; set; } = null!;

    [MappedToProperty(nameof(Models.User.CreatedAt))]
    public DateTime CreationDate { get; set; }

    [MappedToProperty(nameof(Models.User.Status))]
    public Statuses Status { get; set; }

    [MappedToProperty(nameof(Models.User.Comment))]
    public string? Comment { get; set; }
}