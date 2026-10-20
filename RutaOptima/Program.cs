using RutaOptima;

Console.WriteLine("==================================");
Console.WriteLine("         RUTAOPTIMA");
Console.WriteLine(" Transporte Publico de La Paz");
Console.WriteLine("==================================");

Parada[] paradas = new Parada[3];

paradas[0] = new Parada(1, "Parada A", "Bus");
paradas[1] = new Parada(2, "Parada B", "PumaKatari");
paradas[2] = new Parada(3, "Estacion C", "Teleferico");

Console.WriteLine("\nPARADAS REGISTRADAS:");

for (int i = 0; i < paradas.Length; i++)
{
    Console.WriteLine(
        $"{paradas[i].Id}. {paradas[i].Nombre} - {paradas[i].Tipo}");
}

//clase nodo
Console.WriteLine("\nPRUEBA DE NODOS:");

Nodo<Parada> nodo1 = new Nodo<Parada>(paradas[0]);
Nodo<Parada> nodo2 = new Nodo<Parada>(paradas[1]);

nodo1.Siguiente = nodo2;

Console.WriteLine(
    $"{nodo1.Dato.Nombre} -> {nodo1.Siguiente.Dato.Nombre}");

//clase ListaSimple
Console.WriteLine("\nPRUEBA DE LISTA ENLAZADA:");

ListaSimple<string> lista = new ListaSimple<string>();

lista.AgregarAlFinal("Parada A");
lista.AgregarAlFinal("Parada B");
lista.AgregarAlFinal("Parada C");

lista.Mostrar();

Console.WriteLine($"Total de paradas: {lista.Contar()}");

//clase conexion

Console.WriteLine("\nPRUEBA DE CONEXION:");

Conexion conexion1 = new Conexion(paradas[1], 2.5, "Linea de prueba");

Console.WriteLine($"Origen: {paradas[0].Nombre}");
Console.WriteLine($"Destino: {conexion1.Destino.Nombre}");
Console.WriteLine($"Distancia: {conexion1.DistanciaKm} km");
Console.WriteLine($"Linea: {conexion1.Linea}");

//clase Grafo Transporte

Console.WriteLine("\nPRUEBA DEL GRAFO:");

GrafoTransporte grafo = new GrafoTransporte(paradas);

grafo.AgregarConexion(1, 2, 2.5, "Linea de prueba");
grafo.AgregarConexion(2, 3, 1.2, "Linea de prueba");

grafo.MostrarConexiones();

//prueba de algoritmo BFS

Console.WriteLine("\nPRUEBA DEL ALGORITMO BFS:");

Parada[] rutaBFS = grafo.BuscarRutaBFS(1, 3);

if (rutaBFS.Length == 0)
{
    Console.WriteLine("No se encontro una ruta.");
}
else
{
    Console.WriteLine("Ruta encontrada:");

    for (int i = 0; i < rutaBFS.Length; i++)
    {
        Console.Write(rutaBFS[i].Nombre);

        if (i < rutaBFS.Length - 1)
        {
            Console.Write(" -> ");
        }
    }

    Console.WriteLine();
    Console.WriteLine($"Conexiones recorridas: {rutaBFS.Length - 1}");
}


//clase ColaConNodos

Console.WriteLine("\nPRUEBA DE COLA CON NODOS:");

ColaConNodos<Parada> cola = new ColaConNodos<Parada>();

cola.Encolar(paradas[0]);
cola.Encolar(paradas[1]);
cola.Encolar(paradas[2]);

Console.WriteLine($"Frente: {cola.VerFrente().Nombre}");

while (!cola.EstaVacia())
{
    Parada actual = cola.Desencolar();
    Console.WriteLine($"Sale: {actual.Nombre}");
}

Console.WriteLine($"Elementos restantes: {cola.Contar()}");


Console.WriteLine("\nPRUEBAS ADICIONALES BFS:");

// Prueba 1: mismo origen y destino
Console.WriteLine($"Mismo origen: {grafo.BuscarRutaBFS(2, 2).Length} parada");

// Prueba 2: una parada inexistente
Console.WriteLine($"Parada inexistente: {grafo.BuscarRutaBFS(1, 99).Length} paradas");

// Prueba 3: paradas sin conexiones
GrafoTransporte sinConexiones = new GrafoTransporte(paradas);
Console.WriteLine($"Sin conexiones: {sinConexiones.BuscarRutaBFS(1, 3).Length} paradas");

// Prueba 4: dos caminos alternativos
GrafoTransporte alternativo = new GrafoTransporte(paradas);

alternativo.AgregarConexion(1, 2, 2.5, "Prueba");
alternativo.AgregarConexion(2, 3, 1.2, "Prueba");
alternativo.AgregarConexion(1, 3, 9.0, "Prueba");

Parada[] rutaAlternativa = alternativo.BuscarRutaBFS(1, 3);

Console.WriteLine($"Conexiones de la ruta elegida: {rutaAlternativa.Length - 1}");

//prueba DFS

Console.WriteLine("\nPRUEBA DEL ALGORITMO DFS:");

GrafoTransporte grafoDFS = new GrafoTransporte(paradas);

grafoDFS.AgregarConexion(1, 2, 2.5, "Linea de prueba");
grafoDFS.AgregarConexion(2, 3, 1.2, "Linea de prueba");
grafoDFS.AgregarConexion(1, 3, 9.0, "Otra linea");

ListaSimple<Parada[]> rutasDFS = grafoDFS.BuscarRutasDFS(1, 3);

Console.WriteLine($"Rutas encontradas: {rutasDFS.Contar()}");

int numero = 1;

foreach (Parada[] ruta in rutasDFS.ObtenerElementos())
{
    Console.Write($"Ruta {numero}: ");

    for (int i = 0; i < ruta.Length; i++)
    {
        Console.Write(ruta[i].Nombre);

        if (i < ruta.Length - 1)
        {
            Console.Write(" -> ");
        }
    }

    Console.WriteLine();
    numero++;
}

Console.WriteLine("\nPRUEBAS ADICIONALES DFS:");

Console.WriteLine($"Mismo origen: {grafoDFS.BuscarRutasDFS(2, 2).Contar()} ruta");

Console.WriteLine($"Destino inexistente: {grafoDFS.BuscarRutasDFS(1, 99).Contar()} rutas");

GrafoTransporte grafoVacio = new GrafoTransporte(paradas);

Console.WriteLine($"Sin conexiones: {grafoVacio.BuscarRutasDFS(1, 3).Contar()} rutas");

//clase pilas con nodos

Console.WriteLine("\nPRUEBA DE PILA CON NODOS:");

PilaConNodos<Parada> pilaPrueba = new PilaConNodos<Parada>();

pilaPrueba.Apilar(paradas[0]);
pilaPrueba.Apilar(paradas[1]);
pilaPrueba.Apilar(paradas[2]);

Console.WriteLine($"Cima: {pilaPrueba.VerCima().Nombre}");

while (!pilaPrueba.EstaVacia())
{
    Parada actualPila = pilaPrueba.Desapilar();
    Console.WriteLine($"Sale: {actualPila.Nombre}");
}

Console.WriteLine($"Elementos restantes: {pilaPrueba.Contar()}");
