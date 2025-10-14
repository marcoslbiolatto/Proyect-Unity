using UnityEngine;

[CreateAssetMenu(menuName = "Objeto Generable")]
public class ObjetoGenerable : ScriptableObject
{
    public GameObject prefab;
    public float tiempoEntreInstancias = 5f;
    public int cantidadMaxima = 7;
}
