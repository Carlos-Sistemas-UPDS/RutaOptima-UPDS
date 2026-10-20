using System;
using System.Collections.Generic;
using System.Text;

namespace RutaOptima;

public class GrafoTransporte
{
    private readonly Parada[] paradas;
    private readonly ListaSimple<Conexion>[] adyacencias;

    public GrafoTransporte(Parada[] paradas)
    {
        this.paradas = paradas;

        adyacencias = new ListaSimple<Conexion>[paradas.Length];

        for (int i = 0; i < paradas.Length; i++)
        {
            adyacencias[i] = new ListaSimple<Conexion>();
        }
    }

    private int BuscarIndice(int id)
    {
        for (int i = 0; i < paradas.Length; i++)
        {
            if (paradas[i].Id == id)
            {
                return i;
            }
        }

        return -1;
    }

    public bool AgregarConexion(int idOrigen, int idDestino,
        double distanciaKm, string linea, bool bidireccional = true)
    {
        int origen = BuscarIndice(idOrigen);
        int destino = BuscarIndice(idDestino);

        if (origen == -1 || destino == -1 || origen == destino)
        {
            return false;
        }

        if (distanciaKm <= 0 || !double.IsFinite(distanciaKm)
            || string.IsNullOrWhiteSpace(linea))
        {
            return false;
        }

        adyacencias[origen].AgregarAlFinal(
            new Conexion(paradas[destino], distanciaKm, linea));

        if (bidireccional)
        {
            adyacencias[destino].AgregarAlFinal(
                new Conexion(paradas[origen], distanciaKm, linea));
        }

        return true;
    }

    public void MostrarConexiones()
    {
        for (int i = 0; i < paradas.Length; i++)
        {
            Console.WriteLine($"\n{paradas[i].Nombre}:");

            if (adyacencias[i].EstaVacia())
            {
                Console.WriteLine("  Sin conexiones.");
            }

            foreach (Conexion conexion in adyacencias[i].ObtenerElementos())
            {
                Console.WriteLine(
                    $"  -> {conexion.Destino.Nombre} " +
                    $"({conexion.DistanciaKm} km, {conexion.Linea})");
            }
        }
    }

    public Parada[] BuscarRutaBFS(int idOrigen, int idDestino)
    {
        int origen = BuscarIndice(idOrigen);
        int destino = BuscarIndice(idDestino);

        // Verificar que existan las dos paradas
        if (origen == -1 || destino == -1)
        {
            return Array.Empty<Parada>();
        }

        // Preparar arreglos para la busqueda
        bool[] visitados = new bool[paradas.Length];
        int[] padres = new int[paradas.Length];

        for (int i = 0; i < padres.Length; i++)
        {
            padres[i] = -1;
        }

        // Crear nuestra cola con nodos
        ColaConNodos<int> cola = new ColaConNodos<int>();

        cola.Encolar(origen);
        visitados[origen] = true;

        // Recorrer el grafo utilizando BFS
        while (!cola.EstaVacia())
        {
            int actual = cola.Desencolar();

            if (actual == destino)
            {
                break;
            }

            foreach (Conexion conexion in adyacencias[actual].ObtenerElementos())
            {
                int vecino = BuscarIndice(conexion.Destino.Id);

                if (vecino != -1 && !visitados[vecino])
                {
                    visitados[vecino] = true;
                    padres[vecino] = actual;
                    cola.Encolar(vecino);
                }
            }
        }

        // Comprobar si existe una ruta
        if (!visitados[destino])
        {
            return Array.Empty<Parada>();
        }

        // Contar cuantas paradas tiene el camino
        int cantidad = 0;

        for (int i = destino; i != -1; i = padres[i])
        {
            cantidad++;
        }

        // Crear el arreglo de la ruta encontrada
        Parada[] ruta = new Parada[cantidad];

        int posicion = cantidad - 1;

        // Reconstruir el camino desde el destino
        for (int i = destino; i != -1; i = padres[i])
        {
            ruta[posicion] = paradas[i];
            posicion--;
        }

        return ruta;
    }
    //Buscar rutas DFS
    public ListaSimple<Parada[]> BuscarRutasDFS(int idOrigen, int idDestino)
    {
        ListaSimple<Parada[]> rutas = new ListaSimple<Parada[]>();

        int origen = BuscarIndice(idOrigen);
        int destino = BuscarIndice(idDestino);

        if (origen == -1 || destino == -1)
        {
            return rutas;
        }

        bool[] visitados = new bool[paradas.Length];
        Parada[] caminoActual = new Parada[paradas.Length];

        ExplorarDFS(origen, destino, visitados, caminoActual, 0, rutas);

        return rutas;
    }

    private void ExplorarDFS(
        int actual,
        int destino,
        bool[] visitados,
        Parada[] caminoActual,
        int nivel,
        ListaSimple<Parada[]> rutas)
    {
        visitados[actual] = true;
        caminoActual[nivel] = paradas[actual];

        if (actual == destino)
        {
            Parada[] ruta = new Parada[nivel + 1];

            for (int i = 0; i <= nivel; i++)
            {
                ruta[i] = caminoActual[i];
            }

            rutas.AgregarAlFinal(ruta);
        }
        else
        {
            foreach (Conexion conexion in adyacencias[actual].ObtenerElementos())
            {
                int vecino = BuscarIndice(conexion.Destino.Id);

                if (vecino != -1 && !visitados[vecino])
                {
                    ExplorarDFS(
                        vecino,
                        destino,
                        visitados,
                        caminoActual,
                        nivel + 1,
                        rutas);
                }
            }
        }

        visitados[actual] = false;
    }
}