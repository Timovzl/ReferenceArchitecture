using System;
using System.Diagnostics.CodeAnalysis;
using ReferenceArchitecture.Tools.DomainModeling;

namespace ReferenceArchitecture.Domain.Orders
{
	/// <summary>
	/// A description on an <see cref="Order"/>.
	/// </summary>
	public class OrderDescription : ValueObject
	{
		public override string ToString() => this.Value.ToString();
		public override bool Equals(object? obj) => obj is OrderDescription other && StringComparer.Equals(other.Value, this.Value);
		public override int GetHashCode() => StringComparer.GetHashCode(this.Value);
		private static StringComparer StringComparer => StringComparer.Ordinal;

		public const int MaxLength = 256;

		public string Value { get; }

		public OrderDescription(string value)
		{
			// Note how the domain constraints are enforced on construction

			this.Value = value ?? throw new ArgumentNullException(nameof(value));

			if (this.Value.Length > MaxLength) throw new ArgumentException($"An {nameof(OrderDescription)} must be no more than {MaxLength} characters long.");
			if (String.IsNullOrWhiteSpace(this.Value)) throw new ArgumentException($"An {nameof(OrderDescription)} must not be empty.");
		}

		[return: NotNullIfNotNull("instance")]
		public static implicit operator string?(OrderDescription? instance) => instance?.Value;

		[return: NotNullIfNotNull("value")]
		public static explicit operator OrderDescription?(string? value) => value is null ? null : new OrderDescription(value);
	}
}
