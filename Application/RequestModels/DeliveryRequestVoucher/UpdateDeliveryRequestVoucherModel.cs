﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.Enums;

namespace Application.RequestModels
{
    public class UpdateDeliveryRequestVoucherModel
    {
        [Required] 
        public Guid Id { get; set; }

        public DateTime? ReportingDate { get; set; }
        
        public string Description { get; set; }

        public Guid? CustomerId { get; set; }

        [EnumDataType(typeof(EnumStatusRequest))]
        public EnumStatusRequest? Status { get; set; }

        public ICollection<DeliveryRequestVoucherDetailModel> Details { get; set; }
    }
}