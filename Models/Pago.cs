//Suñiga Maciel Joule Alexander
//Villa Olivares Ariel
//Nuñes Martinez Marco Antonio
//Equipo 2
//=============================
using SistemaBiblioteca1.Models;
using System;
using System.Collections.Generic; 
using System.Text;

namespace SistemaBiblioteca1.Models
{
    public class Pago : EntidadBase, IAlmacenamientoCRUD
    {
        // ---------- Almacenamiento en memoria ----------
        private static List<Pago> listaPagos = new List<Pago>();

        // ---------- Campos ----------
        private int idSancion;
        private double monto;
        private DateTime fechaPago;
        private string metodoPago;

        // ---------- Propiedades ----------

        // Puente hacia EntidadBase.Id para no romper el código que ya usa IdPago
        public int IdPago
        {
            get { return Id; }
            set { Id = value; }
        }

        public int IdSancion
        {
            get { return idSancion; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Debe asignarse una sanción válida.");
                idSancion = value;
            }
        }

        public double Monto
        {
            get { return monto; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El monto del pago debe ser mayor a cero.");
                monto = value;
            }
        }

        public DateTime FechaPago
        {
            get { return fechaPago; }
            set { fechaPago = value; }
        }

        public string MetodoPago
        {
            get { return metodoPago; }
            set
            {
                string v = (value ?? string.Empty).Trim();
                if (v != "Efectivo" && v != "Tarjeta" && v != "Transferencia")
                    throw new ArgumentException("El método de pago debe ser: Efectivo, Tarjeta o Transferencia.");
                metodoPago = v;
            }
        }

        // Puente hacia EntidadBase.EsActivo para no romper el código que ya usa Estado
        public bool Estado
        {
            get { return EsActivo; }
            set { EsActivo = value; }
        }

        // ---------- Constructores ----------
        public Pago() : base()
        {
            idSancion = 0;
            monto = 0.0;
            fechaPago = DateTime.Now;
            metodoPago = "Efectivo";
            EsActivo = false; // igual que antes: un pago vacío nace inactivo
        }

        public Pago(int idPago, int idSancion, double monto, string metodoPago, bool estado) : base(idPago)
        {
            IdSancion = idSancion;
            Monto = monto;
            FechaPago = DateTime.Now;
            MetodoPago = metodoPago;
            EsActivo = estado;
        }

        // ---------- Lógica de negocio ----------
        public bool CubreSancion(double montoSancion)
        {
            return CubreSancion(montoSancion, 0.0);
        }

        public bool CubreSancion(double montoSancion, double descuento)
        {
            double montoConDescuento = montoSancion - descuento;
            if (montoConDescuento < 0)
                montoConDescuento = 0;
            return monto >= montoConDescuento;
        }

        // ---------- Implementación de IAlmacenamientoCRUD ----------
        public void InsertarRegistro(object objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto), "El objeto a insertar no puede ser nulo.");

            if (!(objeto is Pago pago))
                throw new ArgumentException("El objeto a insertar no es de tipo Pago.");

            if (listaPagos.Any(p => p.Id == pago.Id))
                throw new InvalidOperationException($"Ya existe un pago con el id {pago.Id}.");

            listaPagos.Add(pago);
        }

        public object ConsultarRegistro(string id)
        {
            int idBuscado = ConvertirId(id);
            return listaPagos.FirstOrDefault(p => p.Id == idBuscado); // null si no existe
        }

        public void ActualizarRegistro(object objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto), "El objeto a actualizar no puede ser nulo.");

            if (!(objeto is Pago pago))
                throw new ArgumentException("El objeto a actualizar no es de tipo Pago.");

            int indice = listaPagos.FindIndex(p => p.Id == pago.Id);
            if (indice == -1)
                throw new InvalidOperationException($"No existe un pago con el id {pago.Id}.");

            listaPagos[indice] = pago;
        }

        public void EliminarRegistro(string id)
        {
            int idBuscado = ConvertirId(id);

            int indice = listaPagos.FindIndex(p => p.Id == idBuscado);
            if (indice == -1)
                throw new InvalidOperationException($"No existe un pago con el id {idBuscado}.");

            listaPagos.RemoveAt(indice);
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
            return $"Pago #{Id} | Sanción #{idSancion} | Monto: ${monto:0.00} | " +
                   $"Método: {metodoPago} | Fecha: {fechaPago:d} | Estado: {(EsActivo ? "Activo" : "Inactivo")}";
        }
    }
}

