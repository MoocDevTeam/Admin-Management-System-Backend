using System.ComponentModel;

namespace Mooc.Shared.Enum
{
    public enum MenuType
    {
        /// <summary>
        /// Directory
        /// </summary>
        [Description("Directory")]
        Dir = 1,
        /// <summary>
        /// Menu
        /// </summary>
        [Description("Menu")]
        Menu = 2,
        /// <summary>
        /// Button
        /// </summary>
        [Description("Button")]
        Btn = 3
    }
}
