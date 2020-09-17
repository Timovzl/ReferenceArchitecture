using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// TODO Enhancement: The DomainModeling types should be moved into a global NuGet package.
namespace ReferenceArchitecture.Tools.DomainModeling
{
	/// <summary>
	/// A value object representing an identity value for an entity of type <typeparamref name="TEntity"/>, with underlying value type <typeparamref name="TValue"/>.
	/// </summary>
	public readonly struct Identity<TEntity, TValue> : IEquatable<Identity<TEntity, TValue>>, IComparable<Identity<TEntity, TValue>>
		where TValue : IEquatable<TValue>, IComparable<TValue>
		where TEntity : class
	{
		public override string ToString() => this.Value?.ToString() ?? "";
		public int CompareTo(Identity<TEntity, TValue> other) => Comparer<TValue>.Default.Compare(this, other);
		public bool Equals(Identity<TEntity, TValue> obj) => obj.HasValue(this.Value);
		public override bool Equals(object? obj) => obj is Identity<TEntity, TValue> other && other.Equals(this);
		public override int GetHashCode() => this.Value is null ? 0 : EqualityComparer<TValue>.Default.GetHashCode(this.Value);

		public TValue Value { get; } // Can technically be null, because an empty struct can always be created

		public Identity([DisallowNull] TValue value)
		{
			if (value is null) ThrowValueArgumentNull();

			this.Value = value;
		}

		private bool HasValue(TValue value) => EqualityComparer<TValue>.Default.Equals(this.Value, value);
		public bool HasDefaultValue() => this.HasValue(default!);

		public static implicit operator TValue(Identity<TEntity, TValue> id) => id.Value;
		public static implicit operator Identity<TEntity, TValue>([DisallowNull] TValue value) => new Identity<TEntity, TValue>(value); // Leave throwing to the ctor

		private static Identity<TEntity, TValue> ThrowValueArgumentNull() => throw new ArgumentNullException("value");

		public static bool operator ==(Identity<TEntity, TValue> left, Identity<TEntity, TValue> right) => left.Equals(right);
		public static bool operator !=(Identity<TEntity, TValue> left, Identity<TEntity, TValue> right) => !(left == right);
		public static bool operator <(Identity<TEntity, TValue> left, Identity<TEntity, TValue> right) => left.CompareTo(right) < 0;
		public static bool operator <=(Identity<TEntity, TValue> left, Identity<TEntity, TValue> right) => left.CompareTo(right) <= 0;
		public static bool operator >(Identity<TEntity, TValue> left, Identity<TEntity, TValue> right) => left.CompareTo(right) > 0;
		public static bool operator >=(Identity<TEntity, TValue> left, Identity<TEntity, TValue> right) => left.CompareTo(right) >= 0;
	}
}
