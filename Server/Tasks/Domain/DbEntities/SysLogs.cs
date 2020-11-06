﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.DbEntities
{
    public partial class SysLogs
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime When { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        [StringLength(10)]
        public string Level { get; set; }
        [Required]
        public string Exception { get; set; }
        [Required]
        public string Trace { get; set; }
        [Required]
        public string Logger { get; set; }
    }
}