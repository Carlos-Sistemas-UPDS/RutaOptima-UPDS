
using System.Globalization;

namespace RutaOptima;

public class MenuConsola
{
    private readonly GrafoTransporte grafo;

    public MenuConsola(GrafoTransporte grafo)
    {
        this.grafo = grafo;
    }

    // MENU PRINCIPAL

    public void Iniciar()
    {
        int opcion;

        do
        {
            Console.WriteLine("\n===================================");
            Console.WriteLine("            RUTAOPTIMA");
            Console.WriteLine("  Transporte Publico de La Paz");
            Console.WriteLine("===================================");

            Console.WriteLine("1. Registrar nueva parada");
            Console.WriteLine("2. Registrar conexion");
            Console.WriteLine("3. Mostrar paradas y conexiones");
            Console.WriteLine("4. Buscar ruta con BFS");
            Console.WriteLine("5. Explorar rutas con DFS");
            Console.WriteLine("6. Calcular ruta minima con Dijkstra");
            Console.WriteLine("7. Mostrar arbol de expansion");
            Console.WriteLine("8. Reporte de conectividad");
            Console.WriteLine("0. Salir");

            opcion = LeerEntero("Seleccione una opcion: ");

            switch (opcion)
            {
                case 1:
                    RegistrarParada();
                    break;

                case 2:
                    RegistrarConexion();
                    break;

                case 3:
                    grafo.MostrarConexiones();
                    break;

                case 4:
                    BuscarBFS();
                    break;

                case 5:
                    BuscarDFS();
                    break;

                case 6:
                    BuscarDijkstra();
                    break;

                case 7:
                    MostrarArbolExpansion();
                    break;

                case 8:
                    grafo.MostrarReporteConectividad();
                    break;

                case 0:
                    Console.WriteLine("Gracias por utilizar RutaOptima.");
                    break;

                default:
                    Console.WriteLine("Opcion no valida.");
                    break;
            }

        } while (opcion != 0);
    }

    // REGISTRAR UNA NUEVA PARADA
    private void RegistrarParada()
    {
        Console.WriteLine("\nREGISTRAR PARADA");

        int id = LeerEntero("ID de la parada: ");

        Console.Write("Nombre: ");
        string nombre = Console.ReadLine() ?? "";

        Console.WriteLine("Tipo de transporte:");
        Console.WriteLine("1. Bus");
        Console.WriteLine("2. PumaKatari");
        Console.WriteLine("3. Teleferico");

        int opcionTipo = LeerEntero("Seleccione el tipo: ");

        string tipo = opcionTipo switch
        {
            1 => "Bus",
            2 => "PumaKatari",
            3 => "Teleferico",
            _ => ""
        };

        bool resultado = grafo.RegistrarParada(id, nombre, tipo);

        Console.WriteLine(resultado
            ? "Parada registrada correctamente."
            : "No se pudo registrar. Revise los datos o el ID.");
    }

    // REGISTRAR UNA CONEXION
    private void RegistrarConexion()
    {
        Console.WriteLine("\nREGISTRAR CONEXION");

        int origen = LeerEntero("ID de origen: ");
        int destino = LeerEntero("ID de destino: ");

        double distancia = LeerDistancia("Distancia en km: ");

        Console.Write("Linea de transporte: ");
        string linea = Console.ReadLine() ?? "";

        bool resultado = grafo.AgregarConexion(
            origen, destino, distancia, linea);

        Console.WriteLine(resultado
            ? "Conexion registrada correctamente."
            : "No se pudo registrar la conexion. Revise los datos.");
    }

    // BUSCAR RUTA CON BFS
    private void BuscarBFS()
    {
        Console.WriteLine("\nBUSQUEDA DE RUTA CON BFS");

        int origen = LeerEntero("ID de origen: ");
        int destino = LeerEntero("ID de destino: ");

        Parada[] ruta = grafo.BuscarRutaBFS(origen, destino);

        if (ruta.Length == 0)
        {
            Console.WriteLine("No se encontro una ruta.");
            return;
        }

        Console.Write("Ruta BFS: ");
        MostrarRuta(ruta);

        Console.WriteLine(
            $"Conexiones recorridas: {ruta.Length - 1}");
    }

    // EXPLORAR RUTAS CON DFS
    private void BuscarDFS()
    {
        Console.WriteLine("\nEXPLORAR RUTAS CON DFS");

        int origen = LeerEntero("ID de origen: ");
        int destino = LeerEntero("ID de destino: ");

        ListaSimple<Parada[]> rutas =
            grafo.BuscarRutasDFS(origen, destino);

        if (rutas.EstaVacia())
        {
            Console.WriteLine("No se encontraron rutas.");
            return;
        }

        int numero = 1;

        foreach (Parada[] ruta in rutas.ObtenerElementos())
        {
            Console.Write($"Ruta {numero}: ");

            MostrarRuta(ruta);

            numero++;
        }

        Console.WriteLine(
            $"Total de rutas encontradas: {rutas.Contar()}");
    }

    // BUSCAR RUTA MINIMA CON DIJKSTRA
    private void BuscarDijkstra()
    {
        Console.WriteLine("\nRUTA MINIMA CON DIJKSTRA");

        int origen = LeerEntero("ID de origen: ");
        int destino = LeerEntero("ID de destino: ");

        var resultado =
            grafo.BuscarRutaDijkstra(origen, destino);

        if (resultado.Ruta.Length == 0)
        {
            Console.WriteLine("No se encontro una ruta.");
            return;
        }

        Console.Write("Ruta mas corta: ");

        MostrarRuta(resultado.Ruta);

        Console.WriteLine(
            $"Distancia total: {resultado.DistanciaKm:0.##} km");
    }

    // LEER UN NUMERO ENTERO
    private int LeerEntero(string mensaje)
    {
        Console.Write(mensaje);

        string? entrada = Console.ReadLine();

        if (int.TryParse(entrada, out int numero))
        {
            return numero;
        }

        return -1;
    }

    // LEER UNA DISTANCIA DECIMAL
    private double LeerDistancia(string mensaje)
    {
        Console.Write(mensaje);

        string? entrada = Console.ReadLine();

        if (double.TryParse(
            entrada?.Replace(',', '.'),
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double numero))
        {
            return numero;
        }

        return -1;
    }

    // MOSTRAR LAS PARADAS DE UNA RUTA
    private void MostrarRuta(Parada[] ruta)
    {
        for (int i = 0; i < ruta.Length; i++)
        {
            Console.Write(ruta[i].Nombre);

            if (i < ruta.Length - 1)
            {
                Console.Write(" -> ");
            }
        }

        Console.WriteLine();
    }

    private void MostrarArbolExpansion()
    {
        Console.WriteLine("\nARBOL DE EXPANSION");

        int origen = LeerEntero("ID de la parada de origen: ");

        NodoArbol<Parada>? raiz =
            grafo.CrearArbolExpansionBFS(origen);

        grafo.MostrarArbol(raiz);
    }
}