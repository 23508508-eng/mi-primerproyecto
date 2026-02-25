using System;
using System.Collections.Generic;
using NanoGuardian.Clean; // Importamos nuestro nuevo código limpio

namespace NanoGuardian
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Simulamos una caída creando nuestra Entidad Pura
            var alerta = new Alerta 
            { 
                Paciente = "Juan Pérez", 
                FuerzaImpacto = 3.5, 
                Fecha = DateTime.Now 
            };

            // 2. Preparamos la Inyección de Dependencias
            // Creamos una lista con los notificadores que queremos usar hoy
            var notificadores = new List<INotificador>
            {
                new NotificadorPushApp(),
                new NotificadorLogLocal()
            };

            // 3. Instanciamos el manager pasándole las dependencias por el constructor
            var procesador = new ProcesadorAlertas(notificadores);
            
            // 4. Ejecutamos
            procesador.Ejecutar(alerta);
        }
    }
}