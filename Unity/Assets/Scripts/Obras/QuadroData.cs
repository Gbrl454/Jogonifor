using UnityEngine;

namespace Game.Obras
{
    [CreateAssetMenu(menuName = "Assets/Quadros/Quadro")]
    public class QuadroData : ScriptableObject
    {
        [SerializeField] int hash;
        [SerializeField] Sprite sprite;

        public int Hash => hash;
        public Sprite Sprite => sprite;
        public Texture Texture => sprite.texture;


        private void OnValidate() => hash = name.GetHashCode();
    }
}