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
}