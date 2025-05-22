using System;

namespace Core.UI
{
    [Serializable]
    public enum ButtonSelectType
    {
        SpriteSwap,
        Flip,
        TextColourSwap,
        IconColourSwap,
        BackgroundColourSwap, 
        IconAndBackgroundColorSwap,
        IconAndBackgroundColorSwapReversed
    }
}