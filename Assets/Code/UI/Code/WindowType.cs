using System;

namespace Core.UI
{
    /// <summary>
    /// Defines the type of a UI window.
    /// </summary>
    [Serializable]
    public enum WindowType
    {
        /// <summary>
        /// A regular window tracked in window history.
        /// </summary>
        Standard,
        /// <summary>
        /// An independent popup-style window, not part of the main navigation flow.
        /// </summary>
        Modal,
        /// <summary>
        /// A heads-up display window, typically always visible during gameplay.
        /// </summary>
        HUD
    }
}