using System;
using System.Collections.Generic;
using System.Text;


namespace RutaOptima;

public class Conexion
{
    public Parada Destino { get; }
    public double DistanciaKm { get; }
    public string Linea { get; }

    public Conexion(Parada destino, double distanciaKm, string linea)
    {
        if (distanciaKm <= 0)
        {
            throw new ArgumentException("La distancia debe ser mayor que cero.");
        }

        Destino = destino;
        DistanciaKm = distanciaKm;
        Linea = linea;
    }
}