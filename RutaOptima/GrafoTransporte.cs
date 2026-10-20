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

    public Parada[] BuscarRutaDFSIterativo(int idOrigen, int idDestino)
    {
        int origen = BuscarIndice(idOrigen);
        int destino = BuscarIndice(idDestino);

        if (origen == -1 || destino == -1)
        {
            return Array.Empty<Parada>();
        }

        bool[] visitados = new bool[paradas.Length];
        int[] padres = new int[paradas.Length];

        for (int i = 0; i < padres.Length; i++)
        {
            padres[i] = -1;
        }

        PilaConNodos<int> pila = new PilaConNodos<int>();

        pila.Apilar(origen);
        visitados[origen] = true;

        while (!pila.EstaVacia())
        {
            int actual = pila.Desapilar();

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
                    pila.Apilar(vecino);
                }
            }
        }

        if (!visitados[destino])
        {
            return Array.Empty<Parada>();
        }

        int cantidad = 0;

        for (int i = destino; i != -1; i = padres[i])
        {
            cantidad++;
        }

        Parada[] ruta = new Parada[cantidad];
        int posicion = cantidad - 1;

        for (int i = destino; i != -1; i = padres[i])
        {
            ruta[posicion] = paradas[i];
            posicion--;
        }

        return ruta;
    }

    public (Parada[] Ruta, double DistanciaKm) BuscarRutaDijkstra(
        int idOrigen, int idDestino)
    {
        int origen = BuscarIndice(idOrigen);
        int destino = BuscarIndice(idDestino);

        if (origen == -1 || destino == -1)
        {
            return (Array.Empty<Parada>(), -1);
        }

        double[] distancias = new double[paradas.Length];
        int[] padres = new int[paradas.Length];
        bool[] procesados = new bool[paradas.Length];

        for (int i = 0; i < paradas.Length; i++)
        {
            distancias[i] = double.PositiveInfinity;
            padres[i] = -1;
        }

        PriorityQueue<int, double> cola =
            new PriorityQueue<int, double>();

        distancias[origen] = 0;
        cola.Enqueue(origen, 0);

        while (cola.Count > 0)
        {
            int actual = cola.Dequeue();

            if (procesados[actual])
            {
                continue;
            }

            procesados[actual] = true;

            if (actual == destino)
            {
                break;
            }

            foreach (Conexion conexion in adyacencias[actual].ObtenerElementos())
            {
                int vecino = BuscarIndice(conexion.Destino.Id);

                if (vecino == -1 || procesados[vecino])
                {
                    continue;
                }

                double nuevaDistancia =
                    distancias[actual] + conexion.DistanciaKm;

                if (nuevaDistancia < distancias[vecino])
                {
                    distancias[vecino] = nuevaDistancia;
                    padres[vecino] = actual;

                    cola.Enqueue(vecino, nuevaDistancia);
                }
            }
        }

        if (!procesados[destino])
        {
            return (Array.Empty<Parada>(), -1);
        }

        int cantidad = 0;

        for (int i = destino; i != -1; i = padres[i])
        {
            cantidad++;
        }

        Parada[] ruta = new Parada[cantidad];
        int posicion = cantidad - 1;

        for (int i = destino; i != -1; i = padres[i])
        {
            ruta[posicion] = paradas[i];
            posicion--;
        }

        return (ruta, distancias[destino]);
    }
    //arbol expansivo

    public NodoArbol<Parada>? CrearArbolExpansionBFS(int idOrigen)
    {
        int origen = BuscarIndice(idOrigen);

        if (origen == -1)
        {
            return null;
        }

        bool[] visitados = new bool[paradas.Length];

        NodoArbol<Parada> raiz =
            new NodoArbol<Parada>(paradas[origen]);

        ColaConNodos<NodoArbol<Parada>> cola =
            new ColaConNodos<NodoArbol<Parada>>();

        cola.Encolar(raiz);
        visitados[origen] = true;

        while (!cola.EstaVacia())
        {
            NodoArbol<Parada> actual = cola.Desencolar();

            int indiceActual = BuscarIndice(actual.Dato.Id);

            foreach (Conexion conexion in
                adyacencias[indiceActual].ObtenerElementos())
            {
                int vecino = BuscarIndice(conexion.Destino.Id);

                if (vecino != -1 && !visitados[vecino])
                {
                    visitados[vecino] = true;

                    NodoArbol<Parada> hijo =
                        new NodoArbol<Parada>(paradas[vecino]);

                    actual.Hijos.AgregarAlFinal(hijo);

                    cola.Encolar(hijo);
                }
            }
        }

        return raiz;
    }

    public void MostrarArbol(NodoArbol<Parada>? raiz)
    {
        if (raiz == null)
        {
            Console.WriteLine("No se encontro la parada de origen.");
            return;
        }

        MostrarNodoArbol(raiz, 0);
    }

    private void MostrarNodoArbol(NodoArbol<Parada> actual, int nivel)
    {
        Console.WriteLine(
            new string(' ', nivel * 4) + "- " + actual.Dato.Nombre);

        foreach (NodoArbol<Parada> hijo in actual.Hijos.ObtenerElementos())
        {
            MostrarNodoArbol(hijo, nivel + 1);
        }
    }

    public void MostrarReporteConectividad()
    {
        int total = paradas.Length;

        int[] cantidadConexiones = new int[total];
        bool[,] contadas = new bool[total, total];

        // Recorrer las listas de adyacencia
        for (int i = 0; i < total; i++)
        {
            foreach (Conexion conexion in adyacencias[i].ObtenerElementos())
            {
                int vecino = BuscarIndice(conexion.Destino.Id);

                if (vecino == -1 || vecino == i || contadas[i, vecino])
                {
                    continue;
                }

                contadas[i, vecino] = true;
                contadas[vecino, i] = true;

                cantidadConexiones[i]++;
                cantidadConexiones[vecino]++;
            }
        }

        int totalAisladas = 0;
        int totalPocas = 0;

        Console.WriteLine("\nREPORTE DE CONECTIVIDAD:");

        for (int i = 0; i < total; i++)
        {
            string estado;

            if (cantidadConexiones[i] == 0)
            {
                estado = "AISLADA";
                totalAisladas++;
            }
            else if (cantidadConexiones[i] == 1)
            {
                estado = "POCAS CONEXIONES";
                totalPocas++;
            }
            else
            {
                estado = "CONECTADA";
            }

            Console.WriteLine(
                $"{paradas[i].Nombre}: {cantidadConexiones[i]} conexion(es) - {estado}");
        }

        Console.WriteLine($"Total de paradas aisladas: {totalAisladas}");
        Console.WriteLine($"Total de paradas con pocas conexiones: {totalPocas}");
    }
}