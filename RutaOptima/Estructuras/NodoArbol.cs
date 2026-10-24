using System;
using System.Collections.Generic;
using System.Text;


namespace RutaOptima;

public class NodoArbol<T>
{
    public T Dato { get; }
    public ListaSimple<NodoArbol<T>> Hijos { get; }

    public NodoArbol(T dato)
    {
        Dato = dato;
        Hijos = new ListaSimple<NodoArbol<T>>();
    }
}