using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ApiService.Models
{
    public partial class accountdbContext : DbContext
    {
        public accountdbContext()
        {
        }

        public accountdbContext(DbContextOptions<accountdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=accountdb;Username=postgres;Password=Test@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthId)
                    .HasMaxLength(40)
                    .HasColumnName("auth_id");

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<PhoneNumber>(entity =>
            {
                entity.ToTable("phone_number");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Number)
                    .HasMaxLength(40)
                    .HasColumnName("number");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.PhoneNumbers)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("phone_number_account_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
