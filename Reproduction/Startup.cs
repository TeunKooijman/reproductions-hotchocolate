using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Data.Filters.Expressions;
using HotChocolate.Data.Projections.Expressions;
using HotChocolate.Data.Sorting.Expressions;
using HotChocolate.Execution.Configuration;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reproduction
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddPooledDbContextFactory<MyDatabase>((sp, options) =>
                {
                    string connectionString = "Server=localhost;Database=reproduction;Trusted_Connection=True;";

                    options.EnableSensitiveDataLogging(true);
                    options.UseSqlServer(connectionString);
                });

            IRequestExecutorBuilder graphql = services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddFiltering()
                .AddSorting()
                .AddProjections()
                .AddInterfaceType<MyBaseEntity>()
                .AddInterfaceType<MyEffect>()
                .AddType<MyTrait>()
                .AddType<MyTraitEffect>()
                .AddType<MyAbility>()
                .AddType<MyAbilityEffect>()
                .AddType<MyBackground>()
                .AddType<MyBackgroundEffectBinding>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }

    public partial class Query
    {
        [UseDbContext(typeof(MyDatabase))]
        [UseOffsetPaging(typeof(ObjectType<MyBackground>))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<MyBackground> Backgrounds([ScopedService] MyDatabase database, IResolverContext context)
        {
            return database.Backgrounds;
        }
    }

    public abstract class MyBaseEntity
    {
        public Guid Id { get; set; }
    }

    public abstract class MyEffect : MyBaseEntity
    {
        public bool IsOptional { get; set; }
    }

    public class MyBackground : MyBaseEntity
    {
        public string Name { get; set; } = default!;

        public ICollection<MyBackgroundEffectBinding> Effects { get; set; } = new List<MyBackgroundEffectBinding>();
    }

    public class MyTraitEffect : MyEffect
    {
        public Guid TraitId { get; set; }
        public MyTrait Trait { get; set; } = default!;
    }

    public class  MyAbilityEffect : MyEffect
    {
        public Guid AbilityId { get; set; }
        public MyAbility Ability { get; set; } = default!;
    }

    public class MyBackgroundEffectBinding : MyBaseEntity
    {
        public Guid BackgroundId { get; set; }
        public MyBackground Background { get; set; } = default!;

        public Guid EffectId { get; set; }
        public MyEffect Effect { get; set; } = default!;
    }

    public class MyTrait : MyBaseEntity
    {
        public string Name { get; set; } = default!;
    }

    public class MyAbility : MyBaseEntity
    {
        public string Name { get; set; } = default!;
    }

    public class MyDatabase : DbContext
    {
        public MyDatabase(DbContextOptions<MyDatabase> options)
            : base(options)
        {

        }

        public DbSet<MyTrait> Traits { get; set; } = default!;
        public DbSet<MyAbility> Abilities { get; set; } = default!;
        public DbSet<MyBackground> Backgrounds { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            EntityTypeBuilder<MyBaseEntity> baseEntity = builder.Entity<MyBaseEntity>();
            baseEntity.HasKey(e => e.Id);
            baseEntity.Property(e => e.Id).ValueGeneratedOnAdd();

            EntityTypeBuilder<MyBackgroundEffectBinding> binding = builder.Entity<MyBackgroundEffectBinding>();
            binding
                .HasOne(e => e.Background)
                .WithMany(e => e.Effects)
                .HasForeignKey(nameof(MyBackgroundEffectBinding.BackgroundId));

            Guid traitEffectBindingId = Guid.Parse("b43c3275-070c-4685-b496-bedf171c38b8");
            Guid abilityEffectBindingId = Guid.Parse("638d2005-d651-4939-af03-04bc7717e9bd");
            Guid abilityId = Guid.Parse("f02c944c-9cd5-44b1-9567-eff6e051ada4");
            Guid traitId = Guid.Parse("d421e715-73e7-413a-907a-bff501a53f58");
            Guid backgroundId = Guid.Parse("8c316892-8b7d-47f8-9b96-b4da3418739c");
            Guid traitEffectId = Guid.Parse("f7eb835e-4c39-4655-9545-61ced8bce356");
            Guid abilityEffectId = Guid.Parse("288e3587-7d6b-44c4-809d-872167d1940c");

            builder.Entity<MyEffect>();
            builder.Entity<MyBackground>().HasData(new MyBackground { Id = backgroundId, Name = "SomeBackground"});
            builder.Entity<MyTrait>().HasData(new MyTrait { Id = traitId, Name = "SomeTrait" });
            builder.Entity<MyAbility>().HasData(new MyAbility{ Id = abilityId, Name = "SomeAbility" });
            builder.Entity<MyTraitEffect>().HasData(new MyTraitEffect { Id = traitEffectId, TraitId = traitId });
            builder.Entity<MyAbilityEffect>().HasData(new MyAbilityEffect{ Id = abilityEffectId, AbilityId = abilityId});
            builder.Entity<MyBackgroundEffectBinding>().HasData(new MyBackgroundEffectBinding { Id = traitEffectBindingId, BackgroundId = backgroundId, EffectId = traitEffectId });
            builder.Entity<MyBackgroundEffectBinding>().HasData(new MyBackgroundEffectBinding { Id = abilityEffectBindingId, BackgroundId = backgroundId, EffectId = abilityEffectId });

            foreach (IMutableForeignKey relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
        }
    }
}
