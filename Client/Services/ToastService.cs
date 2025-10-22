using Client.Model;

namespace Client.Services
{
    /// <summary>
    /// The service class that manages showing toasts.
    /// </summary>
    public class ToastService
    {
        #region Properties

        /// <summary>
        /// Gets or sets the on show event callback.
        /// </summary>
        public event Func<ToastMessage, Task>? OnShow;

        #endregion

        #region Methods

        /// <summary>
        /// Triggers the OnShow function.
        /// </summary>
        /// <param name="message">The new toast data.</param>
        public async void ShowToast(ToastMessage message)
        {
            if (OnShow == null) return;
            await OnShow.Invoke(message);
        }

        #endregion
    }
}
