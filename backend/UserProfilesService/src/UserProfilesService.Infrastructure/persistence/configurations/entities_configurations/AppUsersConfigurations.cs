using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.common.app_users;
using UserProfilesService.Domain.app_user_aggregate;
using UserProfilesService.Infrastructure.persistence.configurations.extensions;
using UserProfilesService.Infrastructure.persistence.configurations.value_converters;

namespace UserProfilesService.Infrastructure.persistence.configurations.entities_configurations;

internal class AppUsersConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.UniqueName)
            .HasConversion<UserUniqueNameConverter>()
            .HasColumnName("UniqueName")
            .HasColumnType($"varchar({UserUniqueName.MaxLength + 10})")
            .IsRequired();

        builder
            .Property(x => x.DisplayName)
            .HasConversion<UserDisplayNameConverter>()
            .HasColumnName("DisplayName")
            .HasColumnType($"varchar({UserDisplayName.MaxLength + 10})")
            .IsRequired();

        builder.Property<string>("SearchableName")
            .HasColumnName("searchable_name")
            .HasColumnType("text")
            .HasComputedColumnSql("lower(\"UniqueName\" || ' ' || \"DisplayName\")", stored: true)
            .ValueGeneratedOnAddOrUpdate()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.ComplexProperty(x => x.ProfilePic, b => {
            b.Property(p => p.Key)
                .HasConversion<AppUserProfilePicKeyConverter>()
                .HasColumnName("ProfilePicKey");
            b.Property(p => p.Shape)
                .HasConversion<string>()
                .HasColumnName("ProfilePicShape");
        });

        builder.ComplexProperty(x => x.FavoriteTagsSetting, b => {
            b.Property(p => p.ShowOnProfile).HasColumnName("favorite_tags_ShowOnProfile");
            b.Property(p => p.Tags)
                .HasTagIdImmutableHashSetConversion()
                .HasColumnName("favorite_tags_Tags");
        });

        builder.ComplexProperty<UserFeaturedAuthorsSetting>(x => x.FeaturedAuthorsSetting, b => {
            b.Property(p => p.ShowOnProfile).HasColumnName("featured_authors_ShowOnProfile");
            b.Property(p => p.UserIds)
                .HasGuidBasedIdsImmutableHashSetConversion()
                .HasColumnName("featured_authors_UserIds");
        });

        builder
            .Property(x => x.FrontendSettings)
            .HasConversion<UserFrontendSettingsConverter>();

        builder.ComplexProperty(x => x.LanguageSettings, b => {
            b.Property(p => p.ShowOnProfile).HasColumnName("language_settings_ShowOnProfile");
            b.Property<HashSet<Language>>("_knownLanguages")
                .HasLanguagesHashSetConversion()
                .HasColumnName("language_settings_KnownLanguages");

            b.ComplexProperty(p => p.UnknownLanguages, ub => {
                ub.Property(u => u.Value)
                    .HasConversion<string>()
                    .HasColumnName("language_settings_UnknownLanguages_Value");
                ub.Property<HashSet<Language>>("_blacklist")
                    .HasLanguagesHashSetConversion()
                    .HasColumnName("language_settings_UnknownLanguages_Blacklist");
            });
        });

        builder.ComplexProperty<UserProfileSettings>(x => x.ProfileSettings, b => {
            b.Property(p => p.Banner)
                .HasConversion<UserBannerConverter>()
                .HasColumnName("profile_settings_Banner");

            b.ComplexProperty(p => p.Status, cb => {
                cb.Property(s => s.ShowOnProfile).HasColumnName("profile_settings_Status_ShowOnProfile");
                cb.Property(s => s.Value).HasColumnName("profile_settings_Status_Value");
            });

            b.ComplexProperty(p => p.Pronouns, cb => {
                cb.Property(s => s.ShowOnProfile).HasColumnName("profile_settings_Pronouns_ShowOnProfile");
                cb.Property(s => s.Value).HasColumnName("profile_settings_Pronouns_Value");
            });

            b.ComplexProperty(p => p.AboutMe, cb => {
                cb.Property(s => s.ShowOnProfile).HasColumnName("profile_settings_AboutMe_ShowOnProfile");
                cb.Property(s => s.Value).HasColumnName("profile_settings_AboutMe_Value");
            });

            b.ComplexProperty(p => p.Links, cb => {
                cb.Property(s => s.ShowOnProfile).HasColumnName("profile_settings_Links_ShowOnProfile");
                cb.Property(s => s.Links)
                    .HasLinksArrayConversion()
                    .HasColumnName("profile_settings_Links_Values");
            });
        });

        builder.ComplexProperty<UserSocialInteractionSettings>("SocialInteractionSettings", b => {
            b
                .Property(d => d.AllowCoAuthorInvites)
                .HasConversion<string>()
                .HasColumnName("social_settings_AllowCoAuthorInvites");
        });
    }
}