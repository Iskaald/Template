using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    [CreateAssetMenu(fileName = "Sprite Provider", menuName = "UI Module/Sprite Provider", order = 0)]
    public class SpriteProviderSO : ScriptableObject
    {
        public Sprite falloutSprite = null;
        public List<IconData> sprites = new();
        public List<ColorData> colors = new();
        public List<IconData> avatars = new();
    }
}