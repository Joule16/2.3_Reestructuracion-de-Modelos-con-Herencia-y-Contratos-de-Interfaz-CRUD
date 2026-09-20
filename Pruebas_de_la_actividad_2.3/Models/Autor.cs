//Suñiga Maciel Joule Alexander
//Villa Olivares Ariel
//Nuñes Martinez Marco Antonio
//Equipo 2
//=============================
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaBiblioteca1.Models
{
    public class Autor : EntidadBase, IAlmacenamientoCRUD
    {
        // ---------- Almacenamiento en memoria ----------
        private static List<Autor> listaAutores = new List<Autor>();

        // ---------- Campos ----------
        private string nombre;
        private string apellido;
        private string nacionalidad;
        private DateTime fechaNacimiento;
        private string imagen;

        // ---------- Propiedades ----------

        // Puente hacia EntidadBase.Id para no romper el código que ya usa IdAutor
        public int IdAutor
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
                    throw new ArgumentException("El nombre no puede estar vacío.");
                nombre = value.Trim();
            }
        }

        public string Apellido
        {
            get { return apellido; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El apellido no puede estar vacío.");
                apellido = value.Trim();
            }
        }

        public string Nacionalidad
        {
            get { return nacionalidad; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La nacionalidad no puede estar vacía.");
                nacionalidad = value.Trim();
            }
        }

        public DateTime FechaNacimiento
        {
            get { return fechaNacimiento; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("La fecha de nacimiento no puede ser una fecha futura.");
                fechaNacimiento = value;
            }
        }

        public string Imagen
        {
            get { return imagen; }
            set { imagen = value; }
        }

        // Puente hacia EntidadBase.EsActivo para no romper el código que ya usa Estado
        public bool Estado
        {
            get { return EsActivo; }
            set { EsActivo = value; }
        }

        // ---------- Constructores ----------
        public Autor() : base()
        {
            nombre = string.Empty;
            apellido = string.Empty;
            nacionalidad = string.Empty;
            imagen = string.Empty;
            fechaNacimiento = DateTime.Now;
            EsActivo = false; // igual que antes: un autor vacío nace inactivo
        }

        public Autor(int idAutor, string nombre, string apellido, string nacionalidad,
                     DateTime fechaNacimiento, bool estado) : base(idAutor)
        {
            Nombre = nombre;
            Apellido = apellido;
            Nacionalidad = nacionalidad;
            FechaNacimiento = fechaNacimiento;
            EsActivo = estado;
        }

        // ---------- Lógica de negocio ----------
        public int CalcularEdad()
        {
            return CalcularEdad(DateTime.Now);
        }

        public int CalcularEdad(DateTime fechaReferencia)
        {
            int edad = fechaReferencia.Year - fechaNacimiento.Year;
            if (fechaReferencia < fechaNacimiento.AddYears(edad))
                edad--;
            return edad;
        }

        // ---------- Implementación de IAlmacenamientoCRUD ----------
        public void InsertarRegistro(object objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto), "El objeto a insertar no puede ser nulo.");

            if (!(objeto is Autor autor))
                throw new ArgumentException("El objeto a insertar no es de tipo Autor.");

            if (listaAutores.Any(a => a.Id == autor.Id))
                throw new InvalidOperationException($"Ya existe un autor con el id {autor.Id}.");

            listaAutores.Add(autor);
        }

        public object ConsultarRegistro(string id)
        {
            int idBuscado = ConvertirId(id);
            return listaAutores.FirstOrDefault(a => a.Id == idBuscado); // null si no existe
        }

        public void ActualizarRegistro(object objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto), "El objeto a actualizar no puede ser nulo.");

            if (!(objeto is Autor autor))
                throw new ArgumentException("El objeto a actualizar no es de tipo Autor.");

            int indice = listaAutores.FindIndex(a => a.Id == autor.Id);
            if (indice == -1)
                throw new InvalidOperationException($"No existe un autor con el id {autor.Id}.");

            listaAutores[indice] = autor;
        }

        public void EliminarRegistro(string id)
        {
            int idBuscado = ConvertirId(id);

            int indice = listaAutores.FindIndex(a => a.Id == idBuscado);
            if (indice == -1)
                throw new InvalidOperationException($"No existe un autor con el id {idBuscado}.");

            listaAutores.RemoveAt(indice);
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
            return $"Autor #{Id}: {nombre} {apellido} | Nacionalidad: {nacionalidad} | " +
                   $"Edad: {CalcularEdad()} años | Estado: {(EsActivo ? "Activo" : "Inactivo")}";
        }
    }
}