using Gybs.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Gybs.Results
{
    /// <summary>
    /// Represents a dictionary-like structure for the errors which can be converted to result's errors.
    /// </summary>
    public sealed class ResultErrorsDictionary : IReadOnlyDictionary<string, IReadOnlyCollection<string>>
    {
        private static readonly Regex ExpressionRegex = new Regex("([a-zA-Z0-9_]+)(?=\\.|$)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        public IReadOnlyCollection<string> this[string key] => _errors[key];

        /// <summary>
        /// Gets a collection containing the keys in the dictionary.
        /// </summary>
        public IEnumerable<string> Keys => _errors.Keys;

        /// <summary>
        /// Gets a collection containing the values in the dictionary.
        /// </summary>
        public IEnumerable<IReadOnlyCollection<string>> Values => _errors.Values;

        /// <summary>
        /// Gets the number of key/value pairs contained in the dictionary.
        /// </summary>
        public int Count => _errors.Count;

        /// <summary>
        /// Adds a new error.
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
        /// Adds a new error with the key generated from the expression (with pattern Type.PropertyA.PropertyB).
        /// </summary>
        /// <typeparam name="TType">Type used to build the key.</typeparam>
        /// <param name="propertyExpression">Expression used to generate the key.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>The dictionary itself.</returns>
        public ResultErrorsDictionary Add<TType>(Expression<Func<TType, object>> propertyExpression, params string[] messages)
        {
            var memberExpression = propertyExpression.Body as MemberExpression
                                   ?? (propertyExpression.Body as UnaryExpression)?.Operand as MemberExpression;

            if (memberExpression is null) throw new ArgumentNullException(nameof(propertyExpression));

            var keyParts = new[] { typeof(TType).Name }
                .Concat(ExpressionRegex
                    .Matches(memberExpression.ToString())
                    .OfType<Match>()
                    .Select(m => m.Value)
                    .Skip(1)
            );
            var key = string.Join(".", keyParts);

            return Add(key, messages);
        }

        /// <summary>
        /// Converts stored data to a readonly dictionary.
        /// </summary>
        /// <returns>The dictionary with the errors.</returns>
        [Obsolete("Use ResultErrorsDictionary directly.")]
        public IReadOnlyDictionary<string, IReadOnlyCollection<string>> ToDictionary()
        {
            return _errors.ToDictionary(e => e.Key, e => e.Value.CastToReadOnly());
        }

        /// <summary>
        /// Returns whether this dictionary contains a particular key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>True if dictionary contains the key; otherwise, false.</returns>
        public bool ContainsKey(string key) => _errors.ContainsKey(key);

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>True if the object contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(string key, out IReadOnlyCollection<string> value)
        {
            if (!_errors.TryGetValue(key, out var innerValue))
            {
                value = new string[0];
                return false;
            }

            value = innerValue.CastToReadOnly();
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, IReadOnlyCollection<string>>> GetEnumerator() => new Enumerator(_errors);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(_errors);

        private struct Enumerator : IEnumerator<KeyValuePair<string, IReadOnlyCollection<string>>>
        {
            private readonly IEnumerator<KeyValuePair<string, List<string>>> _enumerator;
            private KeyValuePair<string, IReadOnlyCollection<string>> _current;

            public KeyValuePair<string, IReadOnlyCollection<string>> Current => _current;
            object IEnumerator.Current => _current;

            public Enumerator(Dictionary<string, List<string>> dictionary)
            {
                _enumerator = dictionary.GetEnumerator();
            }

            public void Dispose() => _enumerator.Dispose();

            public bool MoveNext()
            {
                if (!_enumerator.MoveNext())
                {
                    return false;
                }

                var current = _enumerator.Current;
                _current = new KeyValuePair<string, IReadOnlyCollection<string>>(current.Key, current.Value);
                return true;
            }

            public void Reset()
            {
                _current = default;
                _enumerator.Reset();
            }
        }
    }
}
