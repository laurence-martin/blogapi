using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(X => X.Title)
                .IsRequired()
                .HasColumnName("Title")
                .HasColumnType("VARCHAR")
                .HasMaxLength(160);

            builder.Property(X => X.Summary)
                .IsRequired()
                .HasColumnName("Summary")
                .HasColumnType("VARCHAR")
                .HasMaxLength(255);

            builder.Property(x => x.Body)
                .IsRequired()
                .HasColumnName("Body")
                .HasColumnType("Text");

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.CreateDate)
                .HasColumnName("CreateDate")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()"); //  -- UTILIZA A FUNÇÃO GETDATE do SQL SERVER
                                                  //.HasDefaultValue(DateTime.Now.ToUniversalTime()); //--UTILIZA A HORA DO C#

            builder.Property(x => x.LastUpdateDate)
                .HasColumnName("LastUpdateDate")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()");
            //.HasDefaultValue(DateTime.Now.ToUniversalTime());

            builder.HasIndex(x => x.Slug, "IX_Post_Slug")
                .IsUnique();

            //RELACIONAMENTOS
            builder.HasOne(x => x.Author)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Author")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Category")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Tags)
                .WithMany(x => x.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    post => post.HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostTag_PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                    tag => tag.HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_PostTag_TagId")
                        .OnDelete(DeleteBehavior.Cascade));
                


        }
    }
}