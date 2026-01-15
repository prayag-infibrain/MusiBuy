using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MusiBuy.Common.DB;

public partial class MusiBuyDB_Connection : DbContext
{
    public MusiBuyDB_Connection()
    {
    }

    public MusiBuyDB_Connection(DbContextOptions<MusiBuyDB_Connection> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminUser> AdminUsers { get; set; }

    public virtual DbSet<BankMaster> BankMasters { get; set; }

    public virtual DbSet<CommentsManagement> CommentsManagements { get; set; }

    public virtual DbSet<CommonSetting> CommonSettings { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<Content> Contents { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Creatore> Creatores { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

    public virtual DbSet<Enum> Enums { get; set; }

    public virtual DbSet<EnumType> EnumTypes { get; set; }

    public virtual DbSet<EventManagement> EventManagements { get; set; }

    public virtual DbSet<FollowersManagement> FollowersManagements { get; set; }

    public virtual DbSet<FrontUser> FrontUsers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<InfluencerCategory> InfluencerCategories { get; set; }

    public virtual DbSet<LikeManagement> LikeManagements { get; set; }

    public virtual DbSet<MarketingUser> MarketingUsers { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<MusicProducer> MusicProducers { get; set; }

    public virtual DbSet<PaymentHistory> PaymentHistories { get; set; }

    public virtual DbSet<PodcastGenre> PodcastGenres { get; set; }

    public virtual DbSet<PostManagement> PostManagements { get; set; }

    public virtual DbSet<ProducerExpertise> ProducerExpertises { get; set; }

    public virtual DbSet<Recipient> Recipients { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePrivilege> RolePrivileges { get; set; }

    public virtual DbSet<StripeWebhookLog> StripeWebhookLogs { get; set; }

    public virtual DbSet<TokenPlan> TokenPlans { get; set; }

    public virtual DbSet<UserPrefrence> UserPrefrences { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    public virtual DbSet<ViewManagement> ViewManagements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=MusiBuyDB_Connection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Designation).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Mobile).HasMaxLength(15);
            entity.Property(e => e.OldPassword).HasMaxLength(250);
            entity.Property(e => e.Otp).HasColumnName("OTP");
            entity.Property(e => e.OtpCreatedOn).HasColumnName("OTP_CreatedOn");
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasOne(d => d.Country).WithMany(p => p.AdminUsers)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdminUsers_Countries");

            entity.HasOne(d => d.Role).WithMany(p => p.AdminUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdminUsers_Roles");
        });

        modelBuilder.Entity<BankMaster>(entity =>
        {
            entity.ToTable("BankMaster");

            entity.Property(e => e.AccountHolder).HasMaxLength(150);
            entity.Property(e => e.AccountNumber).HasMaxLength(100);
            entity.Property(e => e.BankName).HasMaxLength(250);
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(50)
                .HasColumnName("IFSCCode");

            entity.HasOne(d => d.User).WithMany(p => p.BankMasters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankMaster_FrontUser");
        });

        modelBuilder.Entity<CommentsManagement>(entity =>
        {
            entity.ToTable("CommentsManagement");

            entity.HasOne(d => d.Event).WithMany(p => p.CommentsManagements)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_CommentsManagement_EventManagement");

            entity.HasOne(d => d.Post).WithMany(p => p.CommentsManagements)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_CommentsManagement_PostManagement");

            entity.HasOne(d => d.User).WithMany(p => p.CommentsManagements)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommentsManagement_FrontUser");
        });

        modelBuilder.Entity<CommonSetting>(entity =>
        {
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsSsl).HasColumnName("IsSSL");
            entity.Property(e => e.MarktingSiteUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("MarktingSiteURL");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SiteUrl)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SiteURL");
            entity.Property(e => e.Smtpserver)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SMTPServer");
        });

        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.Subject).HasMaxLength(250);

            entity.HasOne(d => d.User).WithMany(p => p.ContactUs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContactUs_FrontUser");
        });

        modelBuilder.Entity<Content>(entity =>
        {
            entity.Property(e => e.Content1).HasColumnName("Content");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Page).WithMany(p => p.Contents)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contents_Enums");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CountryShortCode).HasMaxLength(50);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Region).WithMany(p => p.Countries)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK_Country_Region");
        });

        modelBuilder.Entity<Creatore>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.OtpCreatedOn).HasColumnName("OTP_CreatedOn");
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(150)
                .HasColumnName("profilePicture");

            entity.HasOne(d => d.Role).WithMany(p => p.Creatores)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Creatores_Role");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ProfilePicture).HasMaxLength(150);

            entity.HasOne(d => d.Country).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_Countries");
        });

        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.Property(e => e.Subject).HasMaxLength(100);
            entity.Property(e => e.TemplateName).HasMaxLength(50);
        });

        modelBuilder.Entity<Enum>(entity =>
        {
            entity.Property(e => e.EnumValue).HasMaxLength(100);

            entity.HasOne(d => d.EnumType).WithMany(p => p.Enums)
                .HasForeignKey(d => d.EnumTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enums_EnumTypes");

            entity.HasOne(d => d.Parant).WithMany(p => p.InverseParant)
                .HasForeignKey(d => d.ParantId)
                .HasConstraintName("FK_Enums_Enums");
        });

        modelBuilder.Entity<EnumType>(entity =>
        {
            entity.Property(e => e.EnumTypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<EventManagement>(entity =>
        {
            entity.ToTable("EventManagement");

            entity.Property(e => e.RecordingUrl)
                .HasMaxLength(500)
                .HasColumnName("RecordingURL");
            entity.Property(e => e.Title).HasMaxLength(500);

            entity.HasOne(d => d.Role).WithMany(p => p.EventManagements)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_EventManagement");

            entity.HasOne(d => d.User).WithMany(p => p.EventManagements)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FrontUser_EventManagement");
        });

        modelBuilder.Entity<FollowersManagement>(entity =>
        {
            entity.ToTable("FollowersManagement");

            entity.HasOne(d => d.FollowIng).WithMany(p => p.FollowersManagementFollowIngs)
                .HasForeignKey(d => d.FollowIngId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FollowersManagement_FollowingId");

            entity.HasOne(d => d.User).WithMany(p => p.FollowersManagementUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FollowersManagement_FrontUser");
        });

        modelBuilder.Entity<FrontUser>(entity =>
        {
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Designation).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(200);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Mobile).HasMaxLength(15);
            entity.Property(e => e.Otp).HasColumnName("OTP");
            entity.Property(e => e.OtpCreatedOn).HasColumnName("OTP_CreatedOn");
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.FrontUsers)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_FrontUsers_Role");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genres__3214EC07EE04A2FE");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.GenreName).HasMaxLength(255);
            entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Country).WithMany(p => p.Genres)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Genres__CountryI__2EDAF651");

            entity.HasOne(d => d.Region).WithMany(p => p.Genres)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Genres__RegionId__2FCF1A8A");
        });

        modelBuilder.Entity<InfluencerCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Influenc__3214EC077447C174");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Criteria).HasMaxLength(100);
            entity.Property(e => e.EstimatedNumber).HasMaxLength(100);
            entity.Property(e => e.InfluencerTypes).HasMaxLength(100);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<LikeManagement>(entity =>
        {
            entity.ToTable("LikeManagement");

            entity.HasOne(d => d.Event).WithMany(p => p.LikeManagements)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_LikeManagement_EventManagement");

            entity.HasOne(d => d.Post).WithMany(p => p.LikeManagements)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_LikeManagement_PostManagement");

            entity.HasOne(d => d.User).WithMany(p => p.LikeManagements)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikeManagement_FrontUser");
        });

        modelBuilder.Entity<MarketingUser>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Mobile).HasMaxLength(15);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasOne(d => d.Country).WithMany(p => p.MarketingUsers)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MarketingUsers_Countries");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.Property(e => e.ImageName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Image_Name");
            entity.Property(e => e.MenuItemController)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuItemName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuItemView)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_MenuItems_MenuItems");
        });

        modelBuilder.Entity<MusicProducer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MusicPro__3214EC075BC367F0");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<PaymentHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentH__3214EC076CAC82AF");

            entity.ToTable("PaymentHistory");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaidAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StripeSessionId).HasMaxLength(255);
        });

        modelBuilder.Entity<PodcastGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PodcastG__3214EC071526AD97");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<PostManagement>(entity =>
        {
            entity.ToTable("PostManagement");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MediaFile).HasMaxLength(150);
            entity.Property(e => e.Tags).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.Url)
                .HasMaxLength(250)
                .HasColumnName("URL");

            entity.HasOne(d => d.Category).WithMany(p => p.PostManagementCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Category_PostManagement");

            entity.HasOne(d => d.ContentType).WithMany(p => p.PostManagementContentTypes)
                .HasForeignKey(d => d.ContentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enums_PostManagement");

            entity.HasOne(d => d.Country).WithMany(p => p.PostManagements)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_Country_PostManagement");

            entity.HasOne(d => d.Genre).WithMany(p => p.PostManagements)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK_Genres_PostManagement");

            entity.HasOne(d => d.Role).WithMany(p => p.PostManagementRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_PostManagement");

            entity.HasOne(d => d.Type).WithMany(p => p.PostManagementTypes)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_Type_PostManagement");

            entity.HasOne(d => d.User).WithMany(p => p.PostManagements)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FrontUser_PostManagement");
        });

        modelBuilder.Entity<ProducerExpertise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producer__3214EC07C8B4438F");

            entity.ToTable("ProducerExpertise");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Recipient>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.Template).WithMany(p => p.Recipients)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recipients_EmailTemplates");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Region__3214EC075ABEEB06");

            entity.ToTable("Region");

            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RolePrivilege>(entity =>
        {
            entity.HasOne(d => d.MenuItem).WithMany(p => p.RolePrivileges)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePrivileges_MenuItems");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePrivileges)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePrivileges_Roles");
        });

        modelBuilder.Entity<StripeWebhookLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StripeWe__3214EC07E2144939");

            entity.ToTable("StripeWebhookLog");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EventId).HasMaxLength(255);
            entity.Property(e => e.EventType).HasMaxLength(100);
        });

        modelBuilder.Entity<TokenPlan>(entity =>
        {
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TokenQuantity).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.TokenType).WithMany(p => p.TokenPlans)
                .HasForeignKey(d => d.TokenTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_enum_token");
        });

        modelBuilder.Entity<UserPrefrence>(entity =>
        {
            entity.ToTable("UserPrefrence");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.FrontUser).WithMany(p => p.UserPrefrences)
                .HasForeignKey(d => d.FrontUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPrefrence_FrontUser");

            entity.HasOne(d => d.Prefrence).WithMany(p => p.UserPrefrences)
                .HasForeignKey(d => d.PrefrenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPrefrence_Prefrence");
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserToke__3214EC075C918ECA");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ViewManagement>(entity =>
        {
            entity.ToTable("ViewManagement");

            entity.HasOne(d => d.Event).WithMany(p => p.ViewManagements)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_ViewManagement_EventManagement");

            entity.HasOne(d => d.Post).WithMany(p => p.ViewManagements)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_ViewManagement_PostManagement");

            entity.HasOne(d => d.User).WithMany(p => p.ViewManagements)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ViewManagement_FrontUser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
