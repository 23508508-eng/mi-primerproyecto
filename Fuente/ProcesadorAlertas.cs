using System;
using System.Collections.Generic;
using System.IO;

namespace NanoGuardian.Clean
{
    // 1. Entidad pura
    public class Alerta
    {
        public string Paciente { get; set; }
        public double FuerzaImpacto { get; set; }
        public DateTime Fecha { get; set; }
    }

    // 2. Abstracción (Cumple OCP)
    public interface INotificador
    {
        void Enviar(Alerta alerta);
    }

    // 3. Implementación 1 (Push) - Cumple SRP
    public class NotificadorPushApp : INotificador
    {
        public void Enviar(Alerta alerta)
        {
            // Formateo específico para la App
            string mensaje = $"[App] Push enviado: Paciente {alerta.Paciente} cayó con {alerta.FuerzaImpacto}G.";
            Console.WriteLine("Conectando al servidor WiFi...");
            Console.WriteLine(mensaje);
        }
    }

    // 4. Implementación 2 (Log Local) - Cumple SRP
    public class NotificadorLogLocal : INotificador
    {
        public void Enviar(Alerta alerta)
        {
            // Formateo específico para el Log
            string mensaje = $"[ALERTA CRÍTICA] El paciente {alerta.Paciente} ha sufrido una caída de {alerta.FuerzaImpacto}G a las {alerta.Fecha}.";
            Console.WriteLine($"[Disco] Guardando en registro_caidas.txt: Paciente {alerta.Paciente}");

            // Lógica de Persistencia real
            File.AppendAllText("registro_caidas.txt", mensaje + Environment.NewLine);
        }
    }

    // 5. El Manager Limpio (Inyección de Dependencias)
    public class ProcesadorAlertas
    {
        private readonly IEnumerable<INotificador> _notificadores;

        // Recibe cualquier notificador que cumpla el contrato, sin importar cuántos sean.
        public ProcesadorAlertas(IEnumerable<INotificador> notificadores)
        {
            _notificadores = notificadores;
        }

        public void Ejecutar(Alerta alerta)
        {
            foreach (var notificador in _notificadores)
            {
                notificador.Enviar(alerta);
            }
        }
    }
} // Fin del namespace