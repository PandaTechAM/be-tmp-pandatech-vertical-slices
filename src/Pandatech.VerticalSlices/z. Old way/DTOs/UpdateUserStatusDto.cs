﻿using System.ComponentModel.DataAnnotations;
using BaseConverter.Attributes;
using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.DTOs.User
{
    public class UpdateUserStatusDto
    {
        [Required]
        [PandaPropertyBaseConverter]

        public long Id { get; set; }

        [Required] public UserStatus Status { get; set; }
    }
}
