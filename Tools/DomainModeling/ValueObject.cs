using System;

// TODO Enhancement: The DomainModeling types should be moved into a global NuGet package.
namespace ReferenceArchitecture.Tools.DomainModeling
{
	/// <summary>
	/// <para>
	/// A value object is the simplest and best-preferred type in the domain model.
	/// </para>
	/// <para>
	/// Value objects are immutable objects, whose equality is determined entirely by their contents.
	/// </para>
	/// <para>
	/// Outside of the domain, in .NET itself, <see cref="string"/> and <see cref="DateTime"/> are great examples of value objects that developers are familiar with.
	/// They contain one or more values. Instances cannot be changed, but their methods allow new instances to be created based on them, e.g. <see cref="DateTime.Add(TimeSpan)"/>.
	/// Their equality is based solely on their contents.
	/// </para>
	/// </summary>
	public abstract class ValueObject
	{
		public abstract override string ToString();
		/// <summary>
		/// Determines whether the specified object is structurally equal to the current <see cref="ValueObject"/>.
		/// </summary>
		public override bool Equals(object? obj) => throw new NotImplementedException($"Value equality is not implemented for {this.GetType().Name}.");
		public override int GetHashCode() => throw new NotImplementedException($"Value equality is not implemented for {this.GetType().Name}.");

		[Obsolete("Value objects only support structural equality. Use Equals() instead.", error: true)]
		public static bool operator ==(ValueObject left, ValueObject right) => throw new NotSupportedException("Value objects only support structural equality. Use Equals() instead.");
		[Obsolete("Value objects only support structural equality. Use !Equals() instead.", error: true)]
		public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);
	}
}
