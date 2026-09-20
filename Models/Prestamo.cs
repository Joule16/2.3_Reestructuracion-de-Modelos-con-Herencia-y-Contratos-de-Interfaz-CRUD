//Suñiga Maciel Joule Alexander
//Villa Olivarez Ariel
//Nuñes Martinez Marco Antonio
//Equipo 2
//=============================
using SistemaBiblioteca1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaBiblioteca1.Models
{
    public class Prestamo : EntidadBase, IAlmacenamientoCRUD
    {
        // ---------- Almacenamiento en memoria ----------
        private static List<Prestamo> listaPrestamos = new List<Prestamo>();

        // ---------- Campos ----------
        private int idUsuario;
        private int idEjemplar;
        private DateTime fechaPrestamo;
        private DateTime fechaLimite;
        private DateTime? fechaDevolucionReal;
        private bool devuelto;

        // ---------- Propiedades ----------

        // Puente hacia EntidadBase.Id para no romper el código que ya usa IdPrestamo
        public int IdPrestamo
        {
            get { return Id; }
            set { Id = value; }
        }

        public int IdUsuario
        {
            get { return idUsuario; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Debe asignarse un usuario válido.");
                idUsuario = value;
            }
        }

        public int IdEjemplar
        {
            get { return idEjemplar; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Debe asignarse un ejemplar válido.");
                idEjemplar = value;
            }
        }

        public DateTime FechaPrestamo
        {
            get { return fechaPrestamo; }
            set { fechaPrestamo = value; }
        }

        public DateTime FechaLimite
        {
            get { return fechaLimite; }
            set
            {
                if (value.Date < fechaPrestamo.Date)
                    throw new ArgumentException("La fecha límite no puede ser anterior a la fecha de préstamo.");
                fechaLimite = value;
            }
        }

        public DateTime? FechaDevolucionReal
        {
            get { return fechaDevolucionReal; }
            set { fechaDevolucionReal = value; }
        }

        public bool Devuelto
        {
            get { return devuelto; }
            set
            {
                devuelto = value;
                if (devuelto && !fechaDevolucionReal.HasValue)
                {
                    fechaDevolucionReal = DateTime.Now;
                }
                else if (!devuelto)
                {
                    fechaDevolucionReal = null;
                }
            }
        }

        // Puente hacia EntidadBase.EsActivo para no romper el código que ya usa Estado
        public bool Estado
        {
            get { return EsActivo; }
            set { EsActivo = value; }
        }

        // ---------- Constructores ----------
        public Prestamo() : base()
        {
            idUsuario = 0;
            idEjemplar = 0;
            fechaPrestamo = DateTime.Now;
            fechaLimite = DateTime.Now;
            fechaDevolucionReal = null;
            devuelto = false;
            EsActivo = false; // igual que antes: un préstamo vacío nace inactivo
        }

        public Prestamo(int idPrestamo, int idUsuario, int idEjemplar, DateTime fechaPrestamo,
                        DateTime fechaLimite, bool estado) : base(idPrestamo)
        {
            IdUsuario = idUsuario;
            IdEjemplar = idEjemplar;
            FechaPrestamo = fechaPrestamo;
            FechaLimite = fechaLimite;
            fechaDevolucionReal = null;
            devuelto = false;
            EsActivo = estado;
        }

        // ---------- Lógica de negocio ----------
        public int CalcularDiasRetraso()
        {
            DateTime fechaComparacion = fechaDevolucionReal ?? DateTime.Now;
            return CalcularDiasRetraso(fechaComparacion);
        }

        public int CalcularDiasRetraso(DateTime fechaDevolucion)
        {
            TimeSpan diferencia = fechaDevolucion.Date - fechaLimite.Date;
            return diferencia.Days > 0 ? diferencia.Days : 0;
        }

        public void RegistrarDevolucion(DateTime fecha)
        {
            if (fecha.Date < fechaPrestamo.Date)
                throw new ArgumentException("La fecha de devolución no puede ser anterior al préstamo.");
            fechaDevolucionReal = fecha;
            devuelto = true;
        }

        // ---------- Implementación de IAlmacenamientoCRUD ----------
        public void InsertarRegistro(object objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto), "El objeto a insertar no puede ser nulo.");

            if (!(objeto is Prestamo prestamo))
                throw new ArgumentException("El objeto a insertar no es de tipo Prestamo.");

            if (listaPrestamos.Any(p => p.Id == prestamo.Id))
                throw new InvalidOperationException($"Ya existe un préstamo con el id {prestamo.Id}.");

            listaPrestamos.Add(prestamo);
        }

        public object ConsultarRegistro(string id)
        {
            int idBuscado = ConvertirId(id);
            return listaPrestamos.FirstOrDefault(p => p.Id == idBuscado); // null si no existe
        }

        public void ActualizarRegistro(object objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto), "El objeto a actualizar no puede ser nulo.");

            if (!(objeto is Prestamo prestamo))
                throw new ArgumentException("El objeto a actualizar no es de tipo Prestamo.");

            int indice = listaPrestamos.FindIndex(p => p.Id == prestamo.Id);
            if (indice == -1)
                throw new InvalidOperationException($"No existe un préstamo con el id {prestamo.Id}.");

            listaPrestamos[indice] = prestamo;
        }

        public void EliminarRegistro(string id)
        {
            int idBuscado = ConvertirId(id);

            int indice = listaPrestamos.FindIndex(p => p.Id == idBuscado);
            if (indice == -1)
                throw new InvalidOperationException($"No existe un préstamo con el id {idBuscado}.");

            listaPrestamos.RemoveAt(indice);
        }

        // Convierte el id (string) de la interfaz al int de EntidadBase
        private static int ConvertirId(string id)
        {
            if (!int.TryParse(id, out int resultado) || resultado < 0)
                throw new ArgumentException("El id debe ser un número entero no negativo.");
            return resultado;
        }

        // ---------- ToString ----------
        public override string ToString()
        {
            string devolucionTexto = fechaDevolucionReal.HasValue
                ? fechaDevolucionReal.Value.ToShortDateString()
                : "Pendiente";
            return $"Préstamo #{Id} | Usuario #{idUsuario} | Ejemplar #{idEjemplar} | " +
                   $"Límite: {fechaLimite:d} | Devolución: {devolucionTexto} | Retraso: {CalcularDiasRetraso()} días";
        }
    }
}

