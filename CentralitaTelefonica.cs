// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;

// Clase base abstracta
abstract class Llamada
{
    protected string Origen { get; }
    protected string Destino { get; }
    protected int Duracion { get; } // en segundos

    public Llamada(string origen, string destino, int duracion)
    {
        Origen = origen;
        Destino = destino;
        Duracion = duracion;
    }

    public abstract double CalcularCosto();

    public override string ToString()
    {
        return $"Llamada de {Origen} a {Destino} - Duración: {Duracion}s - Costo: {CalcularCosto():0.00} céntimos";
    }
}

// Subclase para llamadas locales
class LlamadaLocal : Llamada
{
    private const double COSTO_POR_SEGUNDO = 0.15;

    public LlamadaLocal(string origen, string destino, int duracion)
        : base(origen, destino, duracion) { }

    public override double CalcularCosto()
    {
        return Duracion * COSTO_POR_SEGUNDO;
    }
}

// Subclase para llamadas provinciales
class LlamadaProvincial : Llamada
{
    private int FranjaHoraria { get; }

    public LlamadaProvincial(string origen, string destino, int duracion, int franjaHoraria)
        : base(origen, destino, duracion)
    {
        if (franjaHoraria < 1 || franjaHoraria > 3)
            throw new ArgumentException("Franja horaria no válida.");
        FranjaHoraria = franjaHoraria;
    }

    public override double CalcularCosto()
    {
        double tarifa = FranjaHoraria switch
        {
            1 => 0.20,
            2 => 0.25,
            3 => 0.30,
            _ => throw new InvalidOperationException()
        };

        return Duracion * tarifa;
    }
}

// Clase que gestiona la centralita
class Centralita
{
    private List<Llamada> llamadas = new List<Llamada>();

    public void RegistrarLlamada(Llamada llamada)
    {
        llamadas.Add(llamada);
    }

    public void MostrarLlamadas()
    {
        foreach (var llamada in llamadas)
        {
            Console.WriteLine(llamada);
        }
    }

    public void GenerarInforme()
    {
        int totalLlamadas = llamadas.Count;
        double totalFacturacion = llamadas.Sum(llamada => llamada.CalcularCosto());

        Console.WriteLine("\n--- INFORME ---");
        Console.WriteLine($"Total de llamadas: {totalLlamadas}");
        Console.WriteLine($"Facturación total: {totalFacturacion:0.00} céntimos");
    }
}

// Clase principal con el método Main
class Practica2
{
    static void Main()
    {
        Centralita centralita = new Centralita();

        // Registrar llamadas
        centralita.RegistrarLlamada(new LlamadaLocal("123456789", "987654321", 120));
        centralita.RegistrarLlamada(new LlamadaProvincial("123456789", "654987321", 100, 1));
        centralita.RegistrarLlamada(new LlamadaProvincial("987654321", "321654987", 80, 3));

        // Mostrar todas las llamadas
        centralita.MostrarLlamadas();

        // Generar informe de facturación
        centralita.GenerarInforme();
    }
}