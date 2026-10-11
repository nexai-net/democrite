// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Serializations
{
    using Democrite.Toolbox.Converters;
    using Democrite.Toolbox.Serializations.ConverterParts;

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class JsonSerializerTypeResolverBuilder<TType> : IJsonSerializerTypeResolverBuilder<TType>, IJsonSerializerSettingConfigBuilderInternal
    {
        #region Fields

        private readonly Dictionary<Type, IDemocriteJsonObjectConverterConditions> _resolverTypeCondtioned;
        private long _buildFlag;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializerTypeResolverBuilder{TType}"/> class.
        /// </summary>
        public JsonSerializerTypeResolverBuilder(IJsonSerializerConverterBuilder converterBuilder)
        {
            this._resolverTypeCondtioned = new Dictionary<Type, IDemocriteJsonObjectConverterConditions>();
            this.Done = converterBuilder;
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public IJsonSerializerConverterBuilder Done { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IJsonSerializerTypeResolverBuilder<TType, TPropValue> Where<TPropValue>(System.Linq.Expressions.Expression<Func<TType, TPropValue>> prop, TPropValue value, IEqualityComparer<TPropValue>? comparer = null)
        {
            var cond = DemocriteJsonObjectPropertyMatchConverterConditions<TType, TPropValue>.Build(prop, value, comparer);
            return new JsonSerializerTypeResolverBuilder<TType, TPropValue>(this, cond);
        }

        /// <summary>
        /// Adds the specified type.
        /// </summary>
        public void Add(Type type, IDemocriteJsonObjectConverterConditions converterConditions)
        {
            this._resolverTypeCondtioned.Add(type, converterConditions);
        }

        /// <inheritdoc />
        public void Build()
        {
            if (Interlocked.Increment(ref this._buildFlag) > 1)
                return;

            this.Done.Add(new TypeSolverDemocriteJsonConverter<TType>(this._resolverTypeCondtioned));
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal sealed class JsonSerializerTypeResolverBuilder<TType, TPropValue> : IJsonSerializerTypeResolverBuilder<TType, TPropValue>
    {
        #region Fields

        private readonly JsonSerializerTypeResolverBuilder<TType> _parent;
        private readonly IDemocriteJsonObjectConverterConditions _converterConditions;

        #endregion

        #region 

        /// <summary>
        /// Intializes a new instance of the type <see cref="JsonSerializerTypeResolverBuilder{TType, TPropValue}"/>
        /// </summary>
        public JsonSerializerTypeResolverBuilder(JsonSerializerTypeResolverBuilder<TType> parent, IDemocriteJsonObjectConverterConditions converterConditions)
        {
            this._parent = parent;
            this._converterConditions = converterConditions;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IJsonSerializerTypeResolverBuilder<TType> ApplyType<TSubType>()
            where TSubType : TType
        {
            this._parent.Add(typeof(TSubType), this._converterConditions);
            return this._parent;
        }

        #endregion
    }
}
