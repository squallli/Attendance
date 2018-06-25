namespace AttendanceSyncApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tbAttendance")]
    public partial class tbAttendance
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(6)]
        public string EmployeeNo { get; set; }

        [StringLength(10)]
        public string Company { get; set; }

        [StringLength(10)]
        public string EmployeeName { get; set; }

        [StringLength(50)]
        public string EnEmployeeName { get; set; }

        [StringLength(10)]
        public string DepartMentName { get; set; }

        [Required]
        [StringLength(10)]
        public string CardNo { get; set; }

        public int? CardType { get; set; }

        [Required]
        [StringLength(8)]
        public string WriteDate { get; set; }

        [Required]
        [StringLength(6)]
        public string WriteTime { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string CardDate { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4)]
        public string CardTime { get; set; }

        public int? CheckInFlag { get; set; }

        [StringLength(6)]
        public string CheckInEmployeeNo { get; set; }

        [StringLength(8)]
        public string CheckInDate { get; set; }

        [StringLength(6)]
        public string CheckInTime { get; set; }

        [StringLength(10)]
        public string CheckInEmployeeName { get; set; }

        [StringLength(255)]
        public string CheckInDescription { get; set; }
    }
}
