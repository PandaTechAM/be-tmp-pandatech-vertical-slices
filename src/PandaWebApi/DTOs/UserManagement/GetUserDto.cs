using PandaTech.IEnumerableFilters.Attributes;
using PandaVault.DTOs;
using PandaWebApi.Enums;
using PandaWebApi.Models;

namespace PandaWebApi.DTOs.UserManagement
{
    [MappedToClass(typeof(User))]
    public class GetUserDto
    {
        [MappedToProperty(nameof(User.Id))]
        public long Id { get; set; }
        [MappedToProperty(nameof(User.Username))]
        public string Username { get; set; } = null!;
        [MappedToProperty(nameof(User.FullName))]
        public string FullName { get; set; } = null!;
        public RolesSelect Role { get; set; } = null!;
        [MappedToProperty(nameof(User.CreationDate))]
        public DateTime CreationDate { get; set; }
        [MappedToProperty(nameof(User.Status))]
        public Statuses Status { get; set; }
        [MappedToProperty(nameof(User.Comment))]
        public string? Comment { get; set; } 
    }
}