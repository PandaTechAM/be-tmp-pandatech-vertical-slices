﻿using System.ComponentModel.DataAnnotations;
using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.UserManagement
{
    public class UpdateUserStatusDto
    {
        [Required] 
        public long Id { get; set; } 
        [Required] 
        public Statuses Status { get; set; } 
    }
}