using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class ObjectGraphDataAnnotationsValidator : ComponentBase
{
    [CascadingParameter] private EditContext CurrentEditContext { get; set; } = default!;
    private ValidationMessageStore _messageStore = default!;

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
            throw new InvalidOperationException($"{nameof(ObjectGraphDataAnnotationsValidator)} requires a cascading " +
                                                $"parameter of type {nameof(EditContext)}.");

        _messageStore = new ValidationMessageStore(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += (s, e) => ValidateModel();
        CurrentEditContext.OnFieldChanged += (s, e) => ValidateField(e.FieldIdentifier);
    }

    private void ValidateModel()
    {
        _messageStore.Clear();
        ValidateObjectRecursive(CurrentEditContext.Model, null);
        CurrentEditContext.NotifyValidationStateChanged();
    }

    private void ValidateField(FieldIdentifier fieldIdentifier)
    {
        _messageStore.Clear(fieldIdentifier);
        ValidateObjectRecursive(CurrentEditContext.Model, null);
        CurrentEditContext.NotifyValidationStateChanged();
    }

    private void ValidateObjectRecursive(object obj, string? parentPath)
    {
        if (obj == null) return;

        var context = new ValidationContext(obj);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(obj, context, results, true);

        foreach (var validationResult in results)
        {
            foreach (var memberName in validationResult.MemberNames)
            {
                string fullPath = parentPath != null ? $"{parentPath}.{memberName}" : memberName;
                var field = CurrentEditContext.Field(fullPath);
                _messageStore.Add(field, validationResult.ErrorMessage!);
            }
        }

        // Recurse into nested properties
        var properties = obj.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Where(p => p.CanRead && p.PropertyType.IsClass && p.PropertyType != typeof(string));

        foreach (var prop in properties)
        {
            var value = prop.GetValue(obj);
            string path = parentPath != null ? $"{parentPath}.{prop.Name}" : prop.Name;
            if (value != null)
            {
                ValidateObjectRecursive(value, path);
            }
        }
    }
}
