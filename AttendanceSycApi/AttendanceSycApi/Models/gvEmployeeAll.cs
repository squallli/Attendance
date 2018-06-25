namespace AttendanceSyncApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("gvEmployeeAll")]
    public partial class gvEmployeeAll
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(1)]
        public string FulltimeFlag { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(6)]
        public string EmployeeNo { get; set; }

        [StringLength(50)]
        public string EmployeeName { get; set; }

        [StringLength(50)]
        public string DepartMentNo { get; set; }

        [StringLength(10)]
        public string DepartMentName { get; set; }

        [StringLength(1)]
        public string Status { get; set; }

        [StringLength(10)]
        public string CardNo { get; set; }

        [StringLength(20)]
        public string Company { get; set; }

        [StringLength(20)]
        public string CompanyName { get; set; }

        [StringLength(50)]
        public string EmployeeEName { get; set; }

        [StringLength(1)]
        public string Sex { get; set; }
    }
}
