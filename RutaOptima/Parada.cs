using System;
using System.Collections.Generic;
using System.Text;

namespace RutaOptima;

public class Parada
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Tipo { get; set; }

    public Parada(int id, string nombre, string tipo)
    {
        Id = id;
        Nombre = nombre;
        Tipo = tipo;
    }
}