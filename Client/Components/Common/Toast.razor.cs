using Client.Enums;
using Client.Model;
using Client.Services;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Common
{
    /// <summary>
    /// The toast component.
    /// </summary>
    public partial class Toast
    {
        #region Fields

        private List<ToastMessage> _toasts;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the injected <see cref="ToastService"/>
        /// </summary>
        [Inject]
        public required ToastService ToastService { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Toast"/> component.
        /// </summary>
        public Toast()
        {
            _toasts = [];
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            _toasts = [];
            ToastService.OnShow += ShowToast;
        }

        /// <summary>
        /// Shows the new toast.
        /// </summary>
        /// <param name="toast">The toast contents.</param>
        private async Task ShowToast(ToastMessage toast)
        {
            _toasts.Add(toast);
            await InvokeAsync(StateHasChanged);

            await Task.Delay(toast.DurationMs);
            RemoveToast(toast);
        }

        /// <summary>
        /// Removes the toast from the UI.
        /// </summary>
        /// <param name="toast">The toast to remove.</param>
        private void RemoveToast(ToastMessage toast)
        {
            if (!_toasts.Contains(toast))
            {
                return;
            }

            _toasts.Remove(toast);
            StateHasChanged();
        }

        /// <summary>
        /// Gets the css class for a given toast based on its level.
        /// </summary>
        /// <param name="level">The level of the toast.</param>
        /// <returns>The css class.</returns>
        private string GetCssClass(ToastLevel level) => level switch
        {
            ToastLevel.Success => "toast-success",
            ToastLevel.Warning => "toast-warning",
            ToastLevel.Error => "toast-error",
            _ => "toast-info"
        };

        #endregion
    }
}
