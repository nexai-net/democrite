// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace System
{
    using Democrite.Toolbox.Abstractions.Attributes;
    using Democrite.Toolbox.Abstractions.Models;
    using Democrite.Toolbox.Models;

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extensions method used to create <see cref="AbstractType"/> and <see cref="AbstractMethod"/>
    /// </summary>
    public static class AbstractTypeExtensions
    {
        #region Fields

        private static readonly Dictionary<Type, AbstractType> s_abstractTypeCache;
        private static readonly ReaderWriterLockSlim s_abstractTypeCachedLocker;

        private static readonly Dictionary<MethodBase, AbstractMethod> s_abstractMethodCache;
        private static readonly ReaderWriterLockSlim s_abstractMethodCachedLocker;

        private static readonly Dictionary<string, MethodBase> s_methodInfoFromAbstractCache;
        private static readonly ReaderWriterLockSlim s_methodInfoFromAbstractCacheLocker;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes the <see cref="AbstractTypeExtensions"/> class.
        /// </summary>
        static AbstractTypeExtensions()
        {
            s_abstractTypeCache = new Dictionary<Type, AbstractType>();
            s_abstractTypeCachedLocker = new ReaderWriterLockSlim();

            s_abstractMethodCache = new Dictionary<MethodBase, AbstractMethod>();
            s_abstractMethodCachedLocker = new ReaderWriterLockSlim();

            s_methodInfoFromAbstractCache = new Dictionary<string, MethodBase>();
            s_methodInfoFromAbstractCacheLocker = new ReaderWriterLockSlim();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Use parameter type to infer generic method types
        /// </summary>
        public static MethodInfo MakeGenericMethodFromParameters(this MethodInfo method, IReadOnlyList<Type> parameterTypes)
        {
            if (method.IsGenericMethod == false)
                throw new InvalidDataException(method + " must be generic");

            var parameters = method.GetParameters();
            var genericArgs = method.GetGenericArguments();

            var solvedGeneric = new List<Type>[genericArgs.Length];

            foreach (var p in parameters)
            {
                var genericParamUsed = p.ParameterType.SearchInTree<Type>(t => t.IsGenericType ? t.GetGenericArguments() : EnumerableHelper<Type>.ReadOnlyArray,
                                                                          t => t.IsGenericMethodParameter);
                if (genericParamUsed.Any())
                {
                    var paramType = parameterTypes[p.Position];

                    var implemenationCompleted = paramType;

                    // Fullname is null only if the generic type does have a concret base
                    if (p.ParameterType.IsGenericType)
                    {
                        var genericRoot = p.ParameterType.GetGenericTypeDefinition();
                        var newImplemenationCompleted = paramType.GetTypeInfoExtension()
                                                                 .GetAllCompatibleTypes()
                                                                 .FirstOrDefault(p => p.IsGenericType && p.GetGenericTypeDefinition() == genericRoot);

                        if (newImplemenationCompleted is not null)
                            implemenationCompleted = newImplemenationCompleted;
                    }

                    foreach (var g in genericParamUsed)
                    {
                        var typeCollection = solvedGeneric[g.Node.GenericParameterPosition];
                        if (typeCollection is null)
                        {
                            typeCollection = new List<Type>();
                            solvedGeneric[g.Node.GenericParameterPosition] = typeCollection;
                        }

                        var typePart = g.Path.FoundNodeByPath(implemenationCompleted,
                                                              t => t.IsGenericType ? t.GetGenericArguments() : EnumerableHelper<Type>.ReadOnlyArray,
                                                              (source, current_indx, current, tester) => tester == current_indx);

                        typeCollection.Add(typePart);
                    }
                }
            }

            //TO-DO from Opus 5.5 Unused generic parameter crashes resolution : When a generic method parameter appears only in the return type, its solvedGeneric slot stays null and FoundCommonType(null) throws ArgumentNullException. For an empty set FoundCommonType returns default (null) and MakeGenericMethod fails. Detect unresolved slots and throw a clear exception, or fall back to the constraint type.
            var genericResolution = solvedGeneric.Select(FoundCommonType).ToArray();

            var solveMethod = method.MakeGenericMethod(genericResolution);

            return solveMethod;
        }

        /// <summary>
        /// Found the common type between all the types
        /// </summary>
        public static Type FoundCommonType(this IEnumerable<Type> types)
        {
            var counts = types.Count();

            if (counts == 1)
                return types.First();

            var commons = types.SelectMany(g => g.GetTypeInfoExtension().GetAllCompatibleTypes().Distinct())
                             .GroupBy(g => g)
                             .Select(grp => (Type: grp.Key, Count: grp.Count()))
                             .OrderByDescending(d => d.Count).ThenByDescending(t => t.Type.IsGenericType ? t.Type.GetGenericArguments().Length : 0)
                                                             .ThenByDescending(t => t.Type.Name?.Length ?? 42)
                             .ToArray();

            var common = commons.FirstOrDefault();

            if (common.Count < counts)
                throw new InvalidOperationException("Couldn't found a commont type for : " + string.Join(", ", types));

            return common.Type;
        }

        /// <summary>
        /// Converts to method.
        /// </summary>
        public static MethodBase? ToMethod(this AbstractMethod method, Type type)
        {
            MethodBase? mthd = null;

            s_methodInfoFromAbstractCacheLocker.EnterReadLock();
            try
            {
                //TO-DO from Opus 5.5 ToMethod cache ignores the target type : The cache key is only MethodUniqueId, so ToMethod(method, typeB) returns the MethodBase resolved earlier for typeA, for example an interface method resolved on different implementations. The full reflection scan also runs under the global write lock. Key the cache by (type, MethodUniqueId) and resolve outside the write lock.
                if (s_methodInfoFromAbstractCache.TryGetValue(method.MethodUniqueId, out var cacheMthd))
                    return cacheMthd;
            }
            finally
            {
                s_methodInfoFromAbstractCacheLocker.ExitReadLock();
            }

            s_methodInfoFromAbstractCacheLocker.EnterWriteLock();
            try
            {
                if (s_methodInfoFromAbstractCache.TryGetValue(method.MethodUniqueId, out var cacheMthd))
                    return cacheMthd;

                mthd = type.GetAllMethodInfos(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                           .FirstOrDefault(m => method.IsEqualTo(m, true));

                if (mthd is null)
                    mthd = type.GetConstructors().FirstOrDefault(c => method.IsEqualTo(c, true));

                if (mthd != null)
                {
                    if (mthd is MethodInfo info && mthd.IsGenericMethodDefinition && method.HasGenericArguments && method.IsIncomplet == false)
                    {
                        mthd = info.MakeGenericMethod(method.GenericArguments
                                                            .Select(g => g.ToType())
                                                            .ToArray());
                    }

                    if (!s_methodInfoFromAbstractCache.ContainsKey(method.MethodUniqueId))
                        s_methodInfoFromAbstractCache.Add(method.MethodUniqueId, mthd);
                }

                return mthd;
            }
            finally
            {
                s_methodInfoFromAbstractCacheLocker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Convert a <see cref="MethodBase"/> (Constructor or method classic) into serializable structure <see cref="AbstractMethod"/>
        /// </summary>
        public static AbstractMethod GetAbstractMethod(this MethodBase methodInfo, bool useCache = true)
        {
            ArgumentNullException.ThrowIfNull(methodInfo);

            if (useCache)
            {
                s_abstractMethodCachedLocker.EnterReadLock();
                try
                {
                    if (s_abstractMethodCache.TryGetValue(methodInfo, out var abstractType))
                        return abstractType;
                }
                finally
                {
                    s_abstractMethodCachedLocker.ExitReadLock();
                }
            }

            var buildedMethodInfo = BuildAbstractMethod(methodInfo);

            if (useCache)
            {
                s_abstractMethodCachedLocker.EnterWriteLock();
                try
                {
                    if (s_abstractMethodCache.TryGetValue(methodInfo, out var abstractType))
                        return abstractType;

                    s_abstractMethodCache.Add(methodInfo, buildedMethodInfo);
                }
                finally
                {
                    s_abstractMethodCachedLocker.ExitWriteLock();
                }
            }

            return buildedMethodInfo!;
        }

        /// <summary>
        /// Gets the type of the abstract.
        /// </summary>
        public static AbstractType GetAbstractType(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            s_abstractTypeCachedLocker.EnterReadLock();
            try
            {
                if (s_abstractTypeCache.TryGetValue(type, out var abstractType))
                    return abstractType;
            }
            finally
            {
                s_abstractTypeCachedLocker.ExitReadLock();
            }

            s_abstractTypeCachedLocker.EnterWriteLock();
            try
            {
                var buildedType = GetThreadSafeAbstractType(type);
                return buildedType!;
            }
            finally
            {
                s_abstractTypeCachedLocker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets all compatible abstract types (Parent, interfaces, ...)
        /// </summary>
        public static IReadOnlyCollection<AbstractType> GetAllCompatibleAbstractTypes(this Type type)
        {
            var resultAbstractTypes = new List<AbstractType>();
            var parentTypes = type.GetTypeInfoExtension().GetAllCompatibleTypes()
                                  .ToList();

            s_abstractTypeCachedLocker.EnterReadLock();
            try
            {
                for (var i = 0; i < parentTypes.Count; i++)
                {
                    var ptype = parentTypes[i];

                    if (s_abstractTypeCache.TryGetValue(ptype, out var abstractType))
                    {
                        resultAbstractTypes.Add(abstractType);
                        parentTypes.RemoveAt(i);
                        i--;
                    }
                }
            }
            finally
            {
                s_abstractTypeCachedLocker.ExitReadLock();
            }

            // Test if remain parent to be build
            if (parentTypes.Count > 0)
            {
                s_abstractTypeCachedLocker.EnterWriteLock();
                try
                {
                    // Test again to get from cache if some have been added since
                    for (var i = 0; i < parentTypes.Count; i++)
                    {
                        var ptype = parentTypes[i];

                        if (s_abstractTypeCache.TryGetValue(ptype, out var abstractType))
                        {
                            resultAbstractTypes.Add(abstractType);
                            parentTypes.RemoveAt(i);
                            i--;
                        }
                    }

                    foreach (var ptype in parentTypes)
                    {
                        var abstractType = GetThreadSafeAbstractType(ptype);
                        resultAbstractTypes.Add(abstractType);
                    }
                }
                finally
                {
                    s_abstractTypeCachedLocker.ExitWriteLock();
                }
            }

            return resultAbstractTypes;
        }

        /// <summary>
        /// Determines whether if <see cref="AbstractType"/> is equal to <see cref="Type"/>
        /// </summary>
        public static bool IsEqualTo(this AbstractType abstractType, Type type, bool checkhierachy = false)
        {
            var other = type.GetAbstractType();
            var match = abstractType?.Equals(other) ?? abstractType is null;

            if (!match && checkhierachy && abstractType is not null)
            {
                return type.GetAllCompatibleAbstractTypes()
                           .Any(m => abstractType.IsEqualTo(m!, checkhierachy: false));
            }

            return match;
        }

        /// <summary>
        /// Determines whether if <see cref="AbstractMethod"/> is equal to <see cref="MethodInfo"/>
        /// </summary>
        public static bool IsEqualTo(this AbstractMethod abstractMethod, MethodBase method, bool trySpecialization = false)
        {
            ArgumentNullException.ThrowIfNull(abstractMethod);
            ArgumentNullException.ThrowIfNull(method);

            var other = method.GetAbstractMethod();
            var result = abstractMethod?.Equals(other) ?? abstractMethod is null;

            if (method is MethodInfo mth &&
                result == false &&
                trySpecialization &&
                method.IsGenericMethodDefinition &&
                method.GetGenericArguments().Length == abstractMethod!.GenericArguments.Count)
            {
                var types = EnumerableHelper<Type>.ReadOnlyArray;
                try
                {
                    types = abstractMethod!.GenericArguments.Select(g => g.ToType()).ToArray();
                }
                catch
                {
                    // generate types failed
                    return false;
                }

                try
                {
                    var specializedMethod = mth.MakeGenericMethod(types);
                    result = IsEqualTo(abstractMethod, specializedMethod, false);
                }
                catch
                {
                    // generate types failed
                    return false;
                }
            }

            return result;
        }

        #region Tools

        /// <summary>
        /// Builds the type of the abstract.
        /// </summary>
        [ThreadSafe]
        private static AbstractType BuildAbstractType(Type type)
        {
            var extendInfo = type.GetTypeInfoExtension();
            if (type.IsGenericType && type.IsGenericTypeDefinition)
            {
                return new IncompleteType(extendInfo.FullShortName,
                                          type.Namespace,
                                          type.AssemblyQualifiedName!,
                                          type.IsInterface,
                                          type.GetGenericArguments()
                                              .Select(x => GetThreadSafeAbstractType(x)));
            }

            // Is generic part of the type definition or method definition
            if (type.IsGenericTypeParameter || type.IsGenericMethodParameter)
            {
                return new GenericType(extendInfo.FullShortName,
                                       type.ContainsGenericParameters && type.IsGenericTypeParameter
                                            ? type.GetGenericParameterConstraints().Select(c => GetThreadSafeAbstractType(c))
                                            : Array.Empty<AbstractType>());
            }

            if (extendInfo.IsCollection && (type.IsArray || extendInfo.Trait.GetGenericArguments().Length > 0))
            {
                return new CollectionType(extendInfo.FullShortName,
                                          type.Namespace,
                                          type.AssemblyQualifiedName!,
                                          type.IsInterface,
                                          extendInfo.CollectionItemType!.GetThreadSafeAbstractType());
            }

            return new ConcretType(extendInfo.FullShortName,
                                    type.Namespace,
                                    type.AssemblyQualifiedName!,
                                    type.IsInterface,
                                    type.GetGenericArguments()
                                        .Select(x => GetThreadSafeAbstractType(x)));
        }

        private static AbstractMethod BuildAbstractMethod(MethodBase methodInfo, bool useCache = true)
        {
            ArgumentNullException.ThrowIfNull(methodInfo);

            AbstractType? returnType = null;

            if (methodInfo is MethodInfo mInfo)
                returnType = mInfo.ReturnType?.GetAbstractType() ?? typeof(void).GetAbstractType();

            var arguments = methodInfo.GetParameters()
                                      .Select(p => p.ParameterType.GetAbstractType())
                                      .ToArray();

            var genericArgs = Array.Empty<AbstractType>();

            if (methodInfo.IsGenericMethod)
            {
                genericArgs = methodInfo.GetGenericArguments()
                                        .Select(g => g.GetAbstractType())
                                        .ToArray();
            }

            var methodUniqueId = ReflectionExtensions.GetUniqueId(methodInfo, useCache: useCache);
            var methodDisplayName = ReflectionExtensions.GetDisplayName(methodInfo);

            return new AbstractMethod(methodDisplayName,
                                      methodInfo.Name,
                                      methodUniqueId,
                                      methodInfo is ConstructorInfo,
                                      returnType,
                                      arguments,
                                      genericArgs);
        }

        /// <summary>
        /// Gets the type of the thread safe abstract.
        /// </summary>
        [ThreadSafe]
        private static AbstractType GetThreadSafeAbstractType(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (s_abstractTypeCache.TryGetValue(type, out var abstractType))
                return abstractType;

            var buildedType = BuildAbstractType(type);

            //TO-DO from Opus 5.5 AbstractType cache is never populated : GetThreadSafeAbstractType checks s_abstractTypeCache again after the build but never calls Add, and nothing else writes to the cache. Every GetAbstractType call rebuilds the whole type graph while holding the global write lock, which serializes all callers. Add the built type to s_abstractTypeCache (for non-Generic/GenericRef categories) instead of doing a second TryGetValue.
            if (buildedType is not null &&
                buildedType.Category != AbstractTypeCategoryEnum.Generic &&
                buildedType.Category != AbstractTypeCategoryEnum.GenericRef)
            {
                if (s_abstractTypeCache.TryGetValue(type, out var newBuildedAbstractType))
                    return newBuildedAbstractType;
            }

            return buildedType!;
        }

        #endregion

        #endregion
    }
}
