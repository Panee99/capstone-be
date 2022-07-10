﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class BeginningVoucher : ISafeEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime ReportingDate { get; set; }

        public string Description { get; set; }

        #region Warehouse

        [Required]
        public Guid WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

        #endregion

        #region Detail

        public ICollection<BeginningVoucherDetail> Details { get; set; }

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
    }
}