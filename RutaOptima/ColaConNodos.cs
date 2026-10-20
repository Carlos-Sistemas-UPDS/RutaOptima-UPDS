using System;
using System.Collections.Generic;
using System.Text;


namespace RutaOptima;

public class ColaConNodos<T>
{
    private Nodo<T>? frente;
    private Nodo<T>? fin;
    private int contador;

    public bool EstaVacia()
    {
        return frente == null;
    }

    public int Contar()
    {
        return contador;
    }

    public void Encolar(T dato)
    {
        Nodo<T> nuevo = new Nodo<T>(dato);

        if (fin == null)
        {
            frente = nuevo;
            fin = nuevo;
        }
        else
        {
            fin.Siguiente = nuevo;
            fin = nuevo;
        }

        contador++;
    }

    public T Desencolar()
    {
        if (frente == null)
        {
            throw new InvalidOperationException("La cola esta vacia.");
        }

        T dato = frente.Dato;
        frente = frente.Siguiente;

        if (frente == null)
        {
            fin = null;
        }

        contador--;
        return dato;
    }

    public T VerFrente()
    {
        if (frente == null)
        {
            throw new InvalidOperationException("La cola esta vacia.");
        }

        return frente.Dato;
    }
}