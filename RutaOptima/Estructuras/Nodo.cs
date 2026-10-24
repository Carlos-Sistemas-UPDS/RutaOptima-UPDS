using System;
using System.Collections.Generic;
using System.Text;


namespace RutaOptima;

public class Nodo<T>
{
    public T Dato;
    public Nodo<T>? Siguiente;

    public Nodo(T dato)
    {
        Dato = dato;
        Siguiente = null;
    }
}