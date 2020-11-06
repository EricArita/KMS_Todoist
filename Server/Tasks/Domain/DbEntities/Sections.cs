﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.DbEntities
{
    public partial class Sections
    {
        public Sections()
        {
            Tasks = new HashSet<Tasks>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        [InverseProperty("Sections")]
        public virtual Project Project { get; set; }
        [InverseProperty("Section")]
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}