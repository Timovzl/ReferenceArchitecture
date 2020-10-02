using System;

// TODO Enhancement: The DomainModeling types should be moved into a global NuGet package.
namespace ReferenceArchitecture.Tools.DomainModeling
{
	/// <summary>
	/// <para>
	/// An entity is a domain object with a lifecycle.
	/// Although value objects are preferred because of their simplicity, objects with a lifecycle tend to occur naturally.
	/// </para>
	/// <para>
	/// Having a lifecycle means that an entity tends to change over time.
	/// </para>
	/// <para>
	/// Because their contents may change, entities are uniquely referenced by ID.
	/// </para>
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity itself.</typeparam>
	/// <typeparam name="TId">The type of the ID property.</typeparam>
	public abstract class Entity<TEntity, TId> : Entity
		where TEntity : Entity
		where TId : IEquatable<TId>, IComparable<TId>
	{
		public override string ToString() => $"{{{this.GetType().Name} Id={this.Id}}}";
		public override bool Equals(object? obj) => obj is Entity<TEntity, TId> other && (this.Id.HasDefaultValue()
			? ReferenceEquals(this, other) // Without an ID, use reference equality
			: this.Id.Equals(other.Id)); // Normally, entities are equal by ID
		public override int GetHashCode() => this.Id.HasDefaultValue()
			? base.GetHashCode() // Without an ID, use reference equality
			: this.Id.GetHashCode(); // Normally, entities are equal by ID

		/// <summary>
		/// <para>
		/// The entity's ID.
		/// </para>
		/// <para>
		/// Implicitly convertible to and from <typeparamref name="TId"/>.
		/// </para>
		/// </summary>
		public abstract Identity<TEntity, TId> Id { get; }

		protected Entity()
		{
		}
	}

	/// <summary>
	/// <para>
	/// The base type for any domain entity.
	/// </para>
	/// <para>
	/// When inheriting, use <see cref="Entity{TEntity, TId}"/> instead of this.
	/// </para>
	/// </summary>
	public abstract class Entity
	{
		/// <summary>
		/// Private protected, because only intended for construction from this project directly.
		/// </summary>
		private protected Entity()
		{
		}
	}
}
