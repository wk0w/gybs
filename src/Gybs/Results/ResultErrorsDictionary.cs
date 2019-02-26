using Gybs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Gybs.Results
{
    /// <summary>
    /// Represents a dictionary-like structure for the errors which can be converted to result's errors.
    /// </summary>
    public sealed class ResultErrorsDictionary
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        /// <summary>
        /// Adds new error.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>The dictionary itself.</returns>
        public ResultErrorsDictionary Add(string key, params string[] messages)
        {
            if (!_errors.TryGetValue(key, out var currentMessages))
            {
                currentMessages = new List<string>();
                _errors.Add(key, currentMessages);
            }

            currentMessages.AddRange(messages);
            return this;
        }

        /// <summary>
        /// Adds new error with the key generated from the expression (with pattern Type.PropertyA.PropertyB).
        /// </summary>
        /// <typeparam name="TType">Type used to build the key.</typeparam>
        /// <param name="propertyExpression">Expression used to generate the key.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>The dictionary itself.</returns>
        public ResultErrorsDictionary Add<TType>(Expression<Func<TType, object>> propertyExpression, params string[] messages)
        {
            var memberExpression = propertyExpression.Body as MemberExpression
                                   ?? (propertyExpression.Body as UnaryExpression)?.Operand as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("Provided expression is not valid.", nameof(propertyExpression));
            }

            var names = new Stack<string>();

            while (memberExpression != null)
            {
                names.Push(memberExpression.Member.Name);
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            var key = $"{typeof(TType).Name}.{string.Join(".", names)}";
            return Add(key, messages);
        }

        /// <summary>
        /// Converts stored data to a readonly dictionary.
        /// </summary>
        /// <returns>The dictionary with the errors.</returns>
        public IReadOnlyDictionary<string, IReadOnlyCollection<string>> ToDictionary()
        {
            return _errors.ToDictionary(e => e.Key, e => e.Value.CastToReadOnly());
        }
    }
}
