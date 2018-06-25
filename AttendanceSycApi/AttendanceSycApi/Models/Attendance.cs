namespace AttendanceSyncApi
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Models;

    public partial class Attendance : DbContext
    {
        public Attendance()
            : base("name=Attendance")
        {
        }
        
        public virtual DbSet<gvEmployeeAll> gvEmployeeAll { get; set; }
        public virtual DbSet<tbAttendance> tbAttendance { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<gvEmployeeAll>()
                .Property(e => e.FulltimeFlag)
                .IsUnicode(false);

            modelBuilder.Entity<gvEmployeeAll>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<tbAttendance>()
                .Property(e => e.WriteDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbAttendance>()
                .Property(e => e.WriteTime)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbAttendance>()
                .Property(e => e.CardDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbAttendance>()
                .Property(e => e.CardTime)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbAttendance>()
                .Property(e => e.CheckInDate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbAttendance>()
                .Property(e => e.CheckInTime)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
