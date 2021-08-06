using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Forms;

namespace DNTPersianComponents.Blazor
{
    /// <summary>
    /// Uniquely identifies a single field that can be edited.
    /// This may correspond to a property on a model object, or can be any other named value.
    /// </summary>
    public class BlazorFieldId<T>
    {
        private readonly Expression<Func<T>>? _valueExpression;

        /// <summary>
        /// Uniquely identifies a single field that can be edited.
        /// This may correspond to a property on a model object, or can be any other named value.
        /// </summary>
        public FieldIdentifier FieldIdentifier =>
                _valueExpression is null ? default : FieldIdentifier.Create(_valueExpression);

        /// <summary>
        /// Gets the name of the editable field.
        /// </summary>
        public string FieldName
        {
            get
            {
                var fieldName = FieldIdentifier.FieldName;
                return string.IsNullOrWhiteSpace(fieldName) ? Guid.NewGuid().ToString("N") : fieldName;
            }
        }

        /// <summary>
        /// Uniquely identifies a single field that can be edited.
        /// This may correspond to a property on a model object, or can be any other named value.
        /// </summary>
        public BlazorFieldId(Expression<Func<T>>? valueExpression)
        {
            _valueExpression = valueExpression;
        }

        /// <summary>
        /// Signals that the value for the specified field has changed.
        /// Signals that some aspect of validation state has changed.
        /// </summary>
        public void NotifyFieldChanged(EditContext? editContext)
        {
            if (editContext is null)
            {
                return;
            }

            var fi = FieldIdentifier;
            if (string.IsNullOrWhiteSpace(fi.FieldName))
            {
                return;
            }

            editContext.NotifyFieldChanged(fi);
            editContext.NotifyValidationStateChanged();
        }
    }
}