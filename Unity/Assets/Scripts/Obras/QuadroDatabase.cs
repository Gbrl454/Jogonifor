using System.Collections;
using UnityEngine;

namespace Game.Obras
{
    [CreateAssetMenu(menuName = "Assets/Obras/Quadro Database")]
    public class QuadroDatabase : ScriptableObject
    {
        [SerializeReference] QuadroData[] quadros;


        public QuadroData GetQuadro(int hashCode)
        {
            for (int i = 0; i < quadros.Length; i++)
            {
                if (quadros[i].Hash == hashCode)
                    return quadros[i];
            }
            return null;
        }
    }
}