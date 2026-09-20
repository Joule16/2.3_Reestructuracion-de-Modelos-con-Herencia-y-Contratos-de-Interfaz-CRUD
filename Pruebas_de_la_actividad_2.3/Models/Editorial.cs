//Suñiga Maciel Joule Alexander
//Villa Olivares Ariel
//Nuñes Martinez Marco Antonio
//Equipo 2
//=============================
using SistemaBiblioteca1.Models;
using System;
using System.Text;

namespace SistemaBiblioteca1.Models
{
    public class Editorial : EntidadBase, IAlmacenamientoCRUD
    {
        // ---------- Almacenamiento en memoria ----------
        private static List<Editorial> listaEditoriales = new List<Editorial>();

        // ---------- Campos ----------
        private string nombre;
        private string paisOrigen;
        private double anioFundacion;

        // ---------- Propiedades ----------

        // Puente hacia EntidadBase.Id para no romper el código que ya usa IdEditorial
        public int IdEditorial
        {
            get { return Id; }
            set { Id = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la editorial no puede estar vacío.");
                nombre = value.Trim();
            }
        }

        public string PaisOrigen
        {
            get { return paisOrigen; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El país de origen no puede estar vacío.");
                paisOrigen = value.Trim();
            }
        }

        public double AnioFundacion
        {
            get { return anioFundacion; }
            set
            {
                if (value < 1400 || value > DateTime.Now.Year)
                    throw new ArgumentException("El año de fundación no es válido.");
                anioFundacion = value;
            }
        }

        // Puente hacia EntidadBase.EsActivo para no romper el código que ya usa Estado
        public bool Estado
        {
            get { return EsActivo; }
            set { EsActivo = value; }
        }

        // ---------- Constructores ----------
        public Editorial() : base()
        {
            nombre = string.Empty;
            paisOrigen = string.Empty;
            anioFundacion = DateTime.Now.Year;
            EsActivo = false; // igual que antes: una editorial vacía nace inactiva
        }

        public Editorial(int idEditorial, string nombre, string paisOrigen,
                         int anioFundacion, bool estado) : base(idEditorial)
        {
            Nombre = nombre;
            PaisOrigen = paisOrigen;
            AnioFundacion = anioFundacion;
            EsActivo = estado;
        }

        // ---------- Lógica de negocio ----------
        public double CalcularAntiguedad()
        {
            return CalcularAntiguedad(DateTime.Now.Year);
        }

        public double CalcularAntiguedad(double anioReferencia)
        {
            if (anioReferencia < anioFundacion)
                throw new ArgumentException("El año de referencia no puede ser anterior a la fundación.");
            return anioReferencia - anioFundacion;
        }

        // ---------- Implementación de IAlmacenamientoCRUD ----------
        public void InsertarRegistro(object objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto), "El objeto a insertar no puede ser nulo.");

            if (!(objeto is Editorial editorial))
                throw new ArgumentException("El objeto a insertar no es de tipo Editorial.");

            if (listaEditoriales.Any(e => e.Id == editorial.Id))
                throw new InvalidOperationException($"Ya existe una editorial con el id {editorial.Id}.");

            listaEditoriales.Add(editorial);
        }

        public object ConsultarRegistro(string id)
        {
            int idBuscado = ConvertirId(id);
            return listaEditoriales.FirstOrDefault(e => e.Id == idBuscado); // null si no existe
        }

        public void ActualizarRegistro(object objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto), "El objeto a actualizar no puede ser nulo.");

            if (!(objeto is Editorial editorial))
                throw new ArgumentException("El objeto a actualizar no es de tipo Editorial.");

            int indice = listaEditoriales.FindIndex(e => e.Id == editorial.Id);
            if (indice == -1)
                throw new InvalidOperationException($"No existe una editorial con el id {editorial.Id}.");

            listaEditoriales[indice] = editorial;
        }

        public void EliminarRegistro(string id)
        {
            int idBuscado = ConvertirId(id);

            int indice = listaEditoriales.FindIndex(e => e.Id == idBuscado);
            if (indice == -1)
                throw new InvalidOperationException($"No existe una editorial con el id {idBuscado}.");

            listaEditoriales.RemoveAt(indice);
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
            return $"Editorial #{Id}: {nombre} | País: {paisOrigen} | " +
                   $"Antigüedad: {CalcularAntiguedad()} años | Estado: {(EsActivo ? "Activa" : "Inactiva")}";
        }
    }
}

