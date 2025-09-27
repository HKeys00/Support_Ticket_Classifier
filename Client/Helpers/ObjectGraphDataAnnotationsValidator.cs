using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class ObjectGraphDataAnnotationsValidator : ComponentBase
{
    [CascadingParameter] private EditContext CurrentEditContext { get; set; } = default!;
    private ValidationMessageStore _messages = default!;

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
            throw new InvalidOperationException($"{nameof(ObjectGraphDataAnnotationsValidator)} requires a cascading " +
                                                $"parameter of type {nameof(EditContext)}.");

        _messages = new ValidationMessageStore(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += (s, e) => ValidateModel();
    }

    private void ValidateModel()
    {
        _messages.Clear();
        ValidateObjectRecursive(CurrentEditContext.Model, null);
        CurrentEditContext.NotifyValidationStateChanged();
    }

    private void ValidateField(FieldIdentifier fieldIdentifier)
    {
        _messages.Clear(fieldIdentifier);
        ValidateObjectRecursive(CurrentEditContext.Model, null);
        CurrentEditContext.NotifyValidationStateChanged();
    }

    private void ValidateObjectRecursive(object instance, object? parent)
    {
        if (instance == null) return;

        var validationContext = new ValidationContext(instance);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, validationContext, results, validateAllProperties: true);

        foreach (var validationResult in results)
        {
            foreach (var memberName in validationResult.MemberNames)
            {
                // Build a FieldIdentifier for this property
                var propInfo = instance.GetType().GetProperty(memberName);
                if (propInfo != null)
                {
                    var field = new FieldIdentifier(instance, memberName);
                    _messages.Add(field, validationResult.ErrorMessage!);
                }
            }
        }

        // Recurse into nested complex properties
        var complexProps = instance.GetType()
                                   .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(p => p.CanRead && p.PropertyType.IsClass && p.PropertyType != typeof(string));

        foreach (var prop in complexProps)
        {
            var value = prop.GetValue(instance);
            if (value != null)
            {
                ValidateObjectRecursive(value, instance);
            }
        }
    }
}
