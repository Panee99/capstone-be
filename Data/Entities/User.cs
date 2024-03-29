﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities
{
    public class User : ISafeEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public Gender? Gender { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        #region User in group

        public ICollection<UserInGroup> UserInGroups { get; set; }

        #endregion

        #region Token

        public ICollection<AuthToken> AuthTokens { get; set; }

        #endregion

        #region Avatar

        // public Guid? AvatarId { get; set; }

        // public Attachment Avatar { get; set; }

        #endregion

        #region User in warehouse

        public Guid? InWarehouseId { get; set; }

        public Warehouse InWarehouse { get; set; }

        #endregion

        #region Voucher created

        public ICollection<BeginningVoucher> BeginningVouchers { get; set; }

        public ICollection<CheckingVoucher> CheckingVouchers { get; set; }

        #endregion

        #region Voucher deleted

        public ICollection<BeginningVoucher> BeginningVouchersDeleted { get; set; }

        public ICollection<CheckingVoucher> CheckingVouchersDeleted { get; set; }

        #endregion

        #region Created

        public ICollection<User> UsersCreated { get; set; }

        public ICollection<Warehouse> WarehousesCreated { get; set; }

        #endregion

        #region Deleted

        public ICollection<User> UsersDeleted { get; set; }

        public ICollection<Warehouse> WarehousesDeleted { get; set; }

        #endregion

        #region Safe entity

        [Required]
        public DateTime CreatedAt { get; set; }

        public Guid? CreatedById { get; set; }

        public User CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedById { get; set; }

        public User UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Guid? DeletedById { get; set; }

        public User DeletedBy { get; set; }

        [Required]
        public bool? IsDeleted { get; set; }

        #endregion

        #region Test

        public Test Test { get; set; }

        #endregion

        #region Extensions

        public string FullName => LastName + " " + FirstName;

        public string Avatar => "";

        #endregion
    }
}