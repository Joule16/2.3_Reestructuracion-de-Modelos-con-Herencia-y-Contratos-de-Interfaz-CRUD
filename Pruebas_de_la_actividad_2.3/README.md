# Sistema de Préstamo de Libros - Biblioteca

## Actividad 2.2 - Vistas (MVC)
Equipo 2

## Integrantes
//Suñiga Maciel Joule Alexander
//Villa Olivarez Ariel
//Nuñes Martinez Marco Antonio


## Descripción
Las 10 entidades del sistema (Usuario, Administrador, Autor, Categoria, Editorial, Libro, Ejemplar, Prestamo, Sancion y Pago) se reestructuraron para compartir una clase base y un contrato de almacenamiento:

EntidadBase: clase abstracta de la que heredan todas las entidades. Concentra los atributos comunes: Id (no puede ser negativo), FechaRegistro (se asigna automáticamente) y EsActivo (por defecto verdadero). Sus constructores protegidos permiten que las clases hijas inicialicen el Id con : base(...).
IAlmacenamientoCRUD: interfaz con el contrato de persistencia que cumplen las 10 entidades:
void InsertarRegistro(object objeto)
object ConsultarRegistro(string id)
void ActualizarRegistro(object objeto)
void EliminarRegistro(string id)

## Entidades
cada clase hereda de EntidadBase, implementa IAlmacenamientoCRUD y conserva sus propias validaciones y métodos de negocio

## Simulacion de persistencia
Como todavía no hay base de datos, cada entidad guarda sus registros en una lista estática en memoria por ejemplo, List<Autor>, compartida por todos los objetos de esa clase. Las operaciones buscan por el Id heredado y aplican conversiones de tipo seguras (is) al recibir el objeto

## como ejecutarlo
Abre el .sln en Visual Studio y presiona F5. Primero se muestra una ventana con el reporte de pruebas de los modelos; al cerrarla, se abre FrmPrincipal con todas las secciones disponibles.
