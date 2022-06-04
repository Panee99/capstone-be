﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Application.RequestModels
{
    public class UpdateUserGroupModel
    {
        [Required]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}