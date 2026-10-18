using System;
using System.Collections.Generic;
using System.Text;

namespace RutaOptima;

public class ListaSimple<T>
{
    private Nodo<T>? cabeza;
    private int contador;

    public bool EstaVacia()
    {
        return cabeza == null;
    }

    public int Contar()
    {
        return contador;
    }

    public void AgregarAlFinal(T dato)
    {
        Nodo<T> nuevo = new Nodo<T>(dato);

        if (cabeza == null)
        {
            cabeza = nuevo;
        }
        else
        {
            Nodo<T> actual = cabeza;

            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }

            actual.Siguiente = nuevo;
        }

        contador++;
    }

    public void Mostrar()
    {
        Nodo<T>? actual = cabeza;

        while (actual != null)
        {
            Console.Write($"{actual.Dato} -> ");
            actual = actual.Siguiente;
        }

        Console.WriteLine("null");
    }
}