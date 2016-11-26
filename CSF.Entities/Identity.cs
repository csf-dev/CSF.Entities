﻿//
// Identity.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace CSF.Entities
{
  /// <summary>
  /// Static functionality supporting the <see cref="T:Identity{TIdentity}"/> type.
  /// </summary>
  public static class Identity
  {
    #region constants

    private static readonly Type OpenGenericIdentity = typeof(Identity<,>);
    internal const string IdentityFormat = "[{0}#{1}]";

    #endregion

    #region methods

    /// <summary>
    /// Creates an <see cref="IIdentity"/> for the given entity type, identity type and identity value.
    /// </summary>
    /// <param name="entityType">Entity type.</param>
    /// <param name="identityType">Identity type.</param>
    /// <param name="identityValue">Identity value.</param>
    public static IIdentity Create(Type entityType, Type identityType, object identityValue)
    {
      if(entityType == null)
      {
        throw new ArgumentNullException(nameof(entityType));
      }
      if(identityType == null)
      {
        throw new ArgumentNullException(nameof(identityType));
      }

      var closedIdentityType = OpenGenericIdentity.MakeGenericType(identityType, entityType);
      return (IIdentity) Activator.CreateInstance(closedIdentityType, new [] { identityValue });
    }

    /// <summary>
    /// Create an entity-typed identity instance from a given identity value.
    /// </summary>
    /// <param name="identityValue">Identity value.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IIdentity<TEntity> Create<TEntity>(object identityValue)
      where TEntity : IEntity
    {
      return (IIdentity<TEntity>) Create(typeof(TEntity), GetIdentityType<TEntity>(), identityValue);
    }

    /// <summary>
    /// Determines whether the two given entity instances are identity-equal or not.
    /// </summary>
    /// <param name="first">The first entity.</param>
    /// <param name="second">The second entity.</param>
    /// <typeparam name="TFirstEntity">The first entity type.</typeparam>
    /// <typeparam name="TSecondEntity">The second entity type.</typeparam>
    public static bool Equals<TFirstEntity,TSecondEntity>(TFirstEntity first, TSecondEntity second)
      where TFirstEntity : IEntity
      where TSecondEntity : IEntity
    {
      var firstIdentity = first.GetIdentity();
      var secondIdentity = second.GetIdentity();

      return firstIdentity.Equals(secondIdentity);
    }

    /// <summary>
    /// Gets the identity type for a given entity type.
    /// </summary>
    /// <returns>The identity type.</returns>
    /// <param name="entityType">Entity type.</param>
    public static Type GetIdentityType(Type entityType)
    {
      var genericEntityType = Entity.GetGenericEntityType(entityType);

      if(genericEntityType == null)
      {
        string message = String.Format(Resources.ExceptionMessages.TypeMustBeGenericEntityTypeFormat,
                                       entityType.FullName);
        throw new ArgumentException(message, nameof(entityType));
      }

      return genericEntityType.GenericTypeArguments[0];
    }

    /// <summary>
    /// Gets the identity type for a given entity type.
    /// </summary>
    /// <returns>The identity type.</returns>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static Type GetIdentityType<TEntity>() where TEntity : IEntity
    {
      return GetIdentityType(typeof(TEntity));
    }

    /// <summary>
    /// Casts an identity instance to a more specific entity type.
    /// </summary>
    /// <param name="identity">The input identity.</param>
    /// <typeparam name="TCast">The entity type for which to cast the identity.</typeparam>
    public static IIdentity<TCast> Cast<TCast>(this IIdentity identity) where TCast : IEntity
    {
      if(identity == null)
      {
        throw new ArgumentNullException(nameof(identity));
      }

      if(!identity.EntityType.IsAssignableFrom(typeof(TCast)))
      {
        string message = String.Format(Resources.ExceptionMessages.CastTypeMustImplementEntityTypeFormat,
                                       identity.EntityType.Name,
                                       typeof(TCast).Name);
        throw new ArgumentException(message, nameof(identity));
      }

      return Identity.Create<TCast>(identity.Value);
    }

    /// <summary>
    /// Raises an exception if the <paramref name="value"/> is the default for its data-type.
    /// </summary>
    /// <param name="value">The identity value.</param>
    /// <typeparam name="TIdentity">The data type for the potential identity value.</typeparam>
    internal static void RequireNotDefaultValue<TIdentity>(TIdentity value)
    {
      if(Object.Equals(value, default(TIdentity)))
      {
        string message = String.Format(Resources.ExceptionMessages.MustNotBeDefaultForDataTypeFormat,
                                       typeof(TIdentity).Name);
        throw new ArgumentException(message, nameof(value));
      }
    }

    #endregion
  }
}

