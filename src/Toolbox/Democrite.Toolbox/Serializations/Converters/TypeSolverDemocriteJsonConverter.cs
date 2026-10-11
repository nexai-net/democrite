// Copyright (c) Amexio.

namespace Democrite.Toolbox.Converters
{
    using Democrite.Toolbox.Serializations;

    using System;
    using System.Collections.Frozen;
    using System.Collections.Generic;

    /// <summary>
    /// Apply multiple conditions to founde the correct final type to apply
    /// </summary>
    internal sealed class TypeSolverDemocriteJsonConverter<TType> : IDemocriteJsonObjectConverter, IEquatable<TypeSolverDemocriteJsonConverter<TType>>
    {
        #region Fields

        private static readonly Type s_traits = typeof(TType);

        private readonly IReadOnlyDictionary<Type, IDemocriteJsonObjectConverterConditions> _props;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSolverDemocriteJsonConverter{TType}"/> class.
        /// </summary>
        public TypeSolverDemocriteJsonConverter(IReadOnlyDictionary<Type, IDemocriteJsonObjectConverterConditions> props)
        {
            this._props = props.ToFrozenDictionary();
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public bool CanWrite
        {
            get { return false; }
        }

        /// <inheritdoc />
        public bool CanRead
        {
            get { return true; }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool CanConvert(Type source)
        {
            return s_traits == source;
        }

        /// <inheritdoc />
        public bool ReadJson(Func<string, Tuple<bool, object?>> getJsonValue, ref Type objectType, ref object? existingValue)
        {
            var localObjectType = objectType;
            var localExistingValue = existingValue;

            var propCond = this._props.FirstOrDefault(p => p.Value.Validate(getJsonValue, localObjectType, localExistingValue));

            if (propCond.Key is not null)
                objectType = propCond.Key;

            // Always return false to force use of the new type in classic deserialization
            return false;
        }

        /// <inheritdoc />
        public bool Equals(TypeSolverDemocriteJsonConverter<TType>? other)
        {
            if (other is null)
                return false;

            if (object.ReferenceEquals(this, other))
                return true;

            return other._props.SequenceEqual(this._props); 
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is TypeSolverDemocriteJsonConverter<TType> other)
                return Equals(other);
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(s_traits, this._props);
        }

        #endregion
    }
}
