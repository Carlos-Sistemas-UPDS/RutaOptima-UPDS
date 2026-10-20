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
