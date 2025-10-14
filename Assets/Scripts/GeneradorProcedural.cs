using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorProcedural : MonoBehaviour
{
    public ObjetoGenerable config; // Asignár el ScriptableObject desde el Inspector

    private int objetosInstanciados = 0;

    void Start()
    {
        StartCoroutine(GenerarObjetos());
    }

    IEnumerator GenerarObjetos()
    {
        while (objetosInstanciados < config.cantidadMaxima)
        {
            InstanciarObjeto();
            objetosInstanciados++;
            yield return new WaitForSeconds(config.tiempoEntreInstancias);
        }
    }

    void InstanciarObjeto()
    {
        Vector3 posicion = transform.position + new Vector3(Random.Range(-5f, 5f), 0, 0);
        Instantiate(config.prefab, posicion, Quaternion.identity);

    }
}
