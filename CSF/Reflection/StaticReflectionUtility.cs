//  
//  ReflectionHelper.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 Craig Fowler
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CSF.Reflection
{
  /// <summary>
  /// Helper class for reflection-related tasks.
  /// </summary>
  public class StaticReflectionUtility
  {
    #region methods
    
    /// <summary>
    /// Gets a <see cref="MemberInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The member information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <typeparam name='TReturn'>
    /// The return/output type of the member.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static MemberInfo GetMember<TObject, TReturn>(Expression<Func<TObject, TReturn>> expression)
    {
      return GetMember(expression.Body);
    }
      
    /// <summary>
    /// Gets a <see cref="MemberInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The member information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static MemberInfo GetMember<TObject>(Expression<Func<TObject, object>> expression)
    {
      return GetMember(expression.Body);
    }
    
    /// <summary>
    /// Gets a <see cref="PropertyInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The property information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static PropertyInfo GetProperty<TObject>(Expression<Func<TObject, object>> expression)
    {
      return GetMember<TObject>(expression) as PropertyInfo;
    }
    
    /// <summary>
    /// Gets a <see cref="FieldInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The field information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static FieldInfo GetField<TObject>(Expression<Func<TObject, object>> expression)
    {
      return GetMember<TObject>(expression) as FieldInfo;
    }
    
    /// <summary>
    /// Gets a <see cref="MethodInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The method information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static MethodInfo GetMethod<TObject>(Expression<Func<TObject, object>> expression)
    {
      return GetMember<TObject>(expression) as MethodInfo;
    }

    /// <summary>
    /// Similar to <c>Type.GetType</c> but searches every assembly within an <see cref="AppDomain"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="Type"/>.
    /// </returns>
    /// <param name='typeName'>
    /// The full name of the type to find and return, does not need to be assembly-qualified.
    /// </param>
    public static Type GetTypeFromAppDomain(string typeName)
    {
      return GetTypeFromAppDomain(AppDomain.CurrentDomain, typeName);
    }

    /// <summary>
    /// Similar to <c>Type.GetType</c> but searches every assembly within an <see cref="AppDomain"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="Type"/>.
    /// </returns>
    /// <param name='domain'>
    /// The AppDomain to search.
    /// </param>
    /// <param name='typeName'>
    /// The full name of the type to find and return, does not need to be assembly-qualified.
    /// </param>
    public static Type GetTypeFromAppDomain(AppDomain domain, string typeName)
    {
      if(domain == null)
      {
        throw new ArgumentNullException("domain");
      }
      else if(String.IsNullOrEmpty(typeName))
      {
        throw new ArgumentException("Type name may not be null or empty");
      }

      var output = domain.GetAssemblies()
                   .SelectMany(s => s.GetTypes())
                   .Where(x => x.FullName == typeName);

      if(output.Count() == 0)
      {
        string message = String.Format("Could not find type '{0}' in the specified AppDomain", typeName);
        throw new InvalidOperationException(message);
      }
      else if(output.Count() > 1)
      {
        string message = String.Format("Error: Multiple types of name '{0}' found in the specified AppDomain",
                                       typeName);
        throw new InvalidOperationException(message);
      }

      return output.First();
    }

    /// <summary>
    /// Determines whether the application is executing using the Mono framework.  This uses the supported manner of
    /// detecting mono.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the application is executing on the mono framework; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsUsingMonoFramework()
    {
      return (Type.GetType("Mono.Runtime") != null);
    }

    #endregion
    
    #region private methods
    
    /// <summary>
    /// Gets a <see cref="MemberInfo"/> from a LINQ expression.
    /// </summary>
    /// <returns>
    /// The member that the expression refers to.
    /// </returns>
    /// <param name='expression'>
    /// The expression, which must be a <see cref="MemberExpression"/>.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    private static MemberInfo GetMember(Expression expression)
    {
      MemberExpression memberExpression = null;
      
      if(expression == null)
      {
        throw new ArgumentNullException ("expression");
      }
      
      if(expression.NodeType == ExpressionType.Convert)
      {
        UnaryExpression unary = (UnaryExpression) expression;
        memberExpression = unary.Operand as MemberExpression;
      }
      else if(expression.NodeType == ExpressionType.MemberAccess)
      {
        memberExpression = (MemberExpression) expression;
      }
      
      if(memberExpression == null)
      {
        throw new ArgumentException("The expression is not a MemberExpression");
      }
      
      return memberExpression.Member;
    }
    
    #endregion
  }
}

