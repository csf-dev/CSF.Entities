//
// Entity`2.cs
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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace CSF.Entities
{
  /// <summary>
  /// Generic base type for an entity object which may be stored in a persistent data source.
  /// </summary>
  /// <typeparam name="TIdentity">The identity type for the current instance.</typeparam>
  [Serializable]
  public abstract class Entity<TIdentity> : IEntity
  {
    #region constants

    private const string NO_IDENTITY = "(no identity)";

    #endregion

    #region fields
    
    private int? _cachedHashCode;
    
    #endregion
    
    #region properties

    /// <summary>
    /// Gets or sets the identity value for the current instance.
    /// </summary>
    /// <value>The identity value.</value>
    protected virtual TIdentity IdentityValue { get; set;  }

    /// <summary>
    /// Gets a value indicating whether this instance has an identity.
    /// </summary>
    /// <value><c>true</c> if this instance has an identity; otherwise, <c>false</c>.</value>
    protected virtual bool HasIdentity
    {
      get {
        return !Object.Equals(IdentityValue, default(TIdentity));
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="T:CSF.Entities.Entity{TIdentity}"/>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="T:CSF.Entities.Entity{TIdentity}"/>.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="T:CSF.Entities.Entity{TIdentity}"/>; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
      return Object.ReferenceEquals(this, obj);
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="T:CSF.Entities.Entity{TIdentity}"/> object.
    /// </summary>
    /// <returns>
    /// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
    /// hash table.
    /// </returns>
    public override int GetHashCode()
    {
      if(!_cachedHashCode.HasValue)
      {
        if(this.HasIdentity)
        {
          _cachedHashCode = IdentityValue.GetHashCode();
        }
        else
        {
          _cachedHashCode = base.GetHashCode();
        }
      }

      return _cachedHashCode.Value;
    }
    
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="T:CSF.Entities.Entity{TIdentity}"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents the current <see cref="T:CSF.Entities.Entity{TIdentity}"/>.
    /// </returns>
    public override string ToString()
    {
      string identityPart = HasIdentity? IdentityValue.ToString() : NO_IDENTITY;
      return String.Format(Identity.IdentityFormat, this.GetType().Name, identityPart);
    }

    #endregion

    #region explicit interface implementations

    bool IEntity.HasIdentity { get { return HasIdentity; } }

    object IEntity.IdentityValue { get { return IdentityValue; } }

    Type IEntity.GetIdentityType() { return typeof(TIdentity); }

    #endregion
  }
}
