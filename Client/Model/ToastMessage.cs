using Client.Enums;

namespace Client.Model
{
    /// <summary>
    /// Data class for presenting a toast.
    /// </summary>
    public class ToastMessage
    {
        #region Properties

        /// <summary>
        /// Gets or sets the title the toast will display.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the message the toast will display.
        /// </summary>
        public required string Message { get; set; }

        /// <summary>
        /// Gets or sets the level of the toast.
        /// </summary>
        public ToastLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the duration in ms the toast will be displayed.
        /// </summary>
        public int DurationMs { get; set; }

        #endregion
    }
}
