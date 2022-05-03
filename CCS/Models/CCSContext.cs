using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CCS.Models {
    public partial class CCSContext : DbContext {
        public CCSContext() {
        }

        public CCSContext(DbContextOptions<CCSContext> options)
            : base(options) {
        }
        public virtual DbSet<Challenge> Challenge { get; set; }
        public virtual DbSet<ChallengeLanguage> ChallengeLanguage { get; set; }
        public virtual DbSet<CodeSubmission> CodeSubmission { get; set; }
        public virtual DbSet<DataType> DataType { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Parameter> Parameter { get; set; }
        public virtual DbSet<Result> Result { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
        public virtual DbSet<TestCase> TestCase { get; set; }
        public virtual DbSet<TestCaseParameter> TestCaseParameter { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=csdev.cegep-heritage.qc.ca;Database=CCS;User id=TEAMCCS; Password=TEAMCCS;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Challenge>(entity => {
                entity.Property(e => e.ChallengeId)
                    .HasColumnName("challenge_id")
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creation_date")
                    .HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(600)
                    .IsUnicode(false);

                entity.Property(e => e.DifficultyLevel)
                    .IsRequired()
                    .HasColumnName("difficulty_level")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Example)
                    .HasColumnName("example")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.FunctionName)
                    .IsRequired()
                    .HasColumnName("function_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ReturnTypeId)
                    .HasColumnName("return_type_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.TeacherId)
                    .HasColumnName("teacher_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DataType)
                    .WithMany(p => p.Challenge)
                    .HasForeignKey(d => d.ReturnTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Challenge_DataType_FK");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Challenge)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Challenge_Teacher_FK");
            });

            modelBuilder.Entity<ChallengeLanguage>(entity => {
                entity.Property(e => e.ChallengeLanguageId)
                    .HasColumnName("challenge_language_id")
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ChallengeId)
                    .HasColumnName("challenge_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.LanguageId)
                    .HasColumnName("language_id")
                    .HasColumnType("numeric(7, 0)");

                entity.HasOne(d => d.Challenge)
                    .WithMany(p => p.ChallengeLanguage)
                    .HasForeignKey(d => d.ChallengeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ChallengeLanguage_Challenge_FK");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.ChallengeLanguage)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ChallengeLanguage_Language_FK");
            });

            modelBuilder.Entity<CodeSubmission>(entity => {
                entity.Property(e => e.CodeSubmissionId)
                    .HasColumnName("code_submission_id")
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ChallengeLanguageId)
                    .HasColumnName("challenge_language_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.LastAttempted)
                    .HasColumnName("last_attempted")
                    .HasColumnType("date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.StudentId)
                    .HasColumnName("student_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("bit");

                entity.Property(e => e.LastAttempted)
                    .IsRequired()
                    .HasColumnName("last_attempted")
                    .HasColumnType("date");

                entity.HasOne(d => d.ChallengeLanguage)
                    .WithMany(p => p.CodeSubmission)
                    .HasForeignKey(d => d.ChallengeLanguageId)
                    .HasConstraintName("CodeSubmission_ChallengeLanguage_FK");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.CodeSubmission)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CodeSubmission_Student_FK");
            });

            modelBuilder.Entity<DataType>(entity => {
                entity.Property(e => e.DataTypeId)
                    .HasColumnName("data_type_id")
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DataTypeName)
                    .IsRequired()
                    .HasColumnName("data_type_name")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.LanguageId)
                    .HasColumnName("language_id")
                    .HasColumnType("numeric(7, 0)");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.DataType)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DataType_Language_FK");
            });

            modelBuilder.Entity<Language>(entity => {
                entity.Property(e => e.LanguageId)
                    .HasColumnName("language_id")
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LanguageName)
                    .IsRequired()
                    .HasColumnName("language_name")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Parameter>(entity => {
                entity.Property(e => e.ParameterId)
                    .HasColumnName("parameter_id")
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ChallengeLanguageId)
                    .HasColumnName("challenge_language_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.DataTypeId)
                    .HasColumnName("data_type_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.DefaultValue)
                    .HasColumnName("default_value")
                    .HasMaxLength(7999)
                    .IsUnicode(false);

                entity.Property(e => e.ParameterName)
                    .IsRequired()
                    .HasColumnName("parameter_name")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasColumnName("position")
                    .HasColumnType("numeric(2, 0)");

                entity.HasOne(d => d.ChallengeLanguage)
                    .WithMany(p => p.Parameters)
                    .HasForeignKey(d => d.ChallengeLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Parameter_ChallengeLanguage");

                entity.HasOne(d => d.DataType)
                    .WithMany(p => p.Parameter)
                    .HasForeignKey(d => d.DataTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Parameter_DataType_FK");
            });

            modelBuilder.Entity<Result>(entity => {
                entity.Property(e => e.ResultId)
                    .HasColumnName("result_id")
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CodeOutput)
                    .IsRequired()
                    .HasColumnName("code_output")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.CodeSubmissionId)
                    .HasColumnName("code_submission_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.Passed).HasColumnName("passed");

                entity.Property(e => e.TestCaseId)
                    .HasColumnName("test_case_id")
                    .HasColumnType("numeric(7, 0)");

                entity.HasOne(d => d.CodeSubmission)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.CodeSubmissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Result_CodeSubmission_FK");

                entity.HasOne(d => d.TestCase)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.TestCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Result_TestCase_FK");
            });

            modelBuilder.Entity<Student>(entity => {
                entity.Property(e => e.StudentId)
                    .HasColumnName("student_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Teacher>(entity => {
                entity.Property(e => e.TeacherId)
                    .HasColumnName("teacher_id")
                    .HasColumnType("numeric(7, 0)");
            });

            modelBuilder.Entity<TestCase>(entity => {
                entity.Property(e => e.TestCaseId)
                    .HasColumnName("test_case_id")
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ChallengeLanguageId)
                    .HasColumnName("challenge_language_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.ExpectedResult)
                    .IsRequired()
                    .HasColumnName("expected_result")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.ChallengeLanguage)
                    .WithMany(p => p.TestCase)
                    .HasForeignKey(d => d.ChallengeLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TestCase_ChallengeLanguage_FK");
            });

            modelBuilder.Entity<TestCaseParameter>(entity => {
                entity.Property(e => e.TestCaseParameterId)
                    .HasColumnName("test_case_parameter_id")
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ParameterId)
                    .HasColumnName("parameter_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.TestCaseId)
                    .HasColumnName("test_case_id")
                    .HasColumnType("numeric(7, 0)");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(7999)
                    .IsUnicode(false);

                entity.HasOne(d => d.Parameter)
                    .WithMany(p => p.TestCaseParameter)
                    .HasForeignKey(d => d.ParameterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TestCaseParameter_Parameter");

                entity.HasOne(d => d.TestCase)
                    .WithMany(p => p.TestCaseParameter)
                    .HasForeignKey(d => d.TestCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TestCaseParameter_TestCase");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
