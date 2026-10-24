using System;
using System.Collections.Generic;
using System.Text;


namespace RutaOptima;

public class PilaConNodos<T>
{
    private Nodo<T>? cima;
    private int contador;

    public bool EstaVacia()
    {
        return cima == null;
    }

    public int Contar()
    {
        return contador;
    }

    public void Apilar(T dato)
    {
        Nodo<T> nuevo = new Nodo<T>(dato);

        nuevo.Siguiente = cima;
        cima = nuevo;

        contador++;
    }

    public T Desapilar()
    {
        if (cima == null)
        {
            throw new InvalidOperationException("La pila esta vacia.");
        }

        T dato = cima.Dato;
        cima = cima.Siguiente;

        contador--;
        return dato;
    }

    public T VerCima()
    {
        if (cima == null)
        {
            throw new InvalidOperationException("La pila esta vacia.");
        }

        return cima.Dato;
    }
}