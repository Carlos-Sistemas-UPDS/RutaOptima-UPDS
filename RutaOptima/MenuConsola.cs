using System.Globalization;

namespace RutaOptima;

public class MenuConsola
{
    private GrafoTransporte grafo;

    public MenuConsola(GrafoTransporte grafo)
    {
        this.grafo = grafo;
    }

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

                case 0:
                    Console.WriteLine("Gracias por utilizar RutaOptima.");
                    break;

                default:
                    Console.WriteLine("Opcion no valida.");
                    break;
            }

        } while (opcion != 0);
    }

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
}
