using System;
using System.IO;

namespace Mesa_De_Partes_Sistema
{
    internal class Program
    {
        static int max_documentos = 500;
        static int total_documentos = 0;
        static string[] codigos = new string[500];
        static string[] fechas = new string[500];
        static string[] tipos = new string[500];
        static string[] descripciones = new string[500];
        static string[] nombres_usuarios = new string[500];
        static string[] dni_usuarios = new string[500];
        static string[] telefonos_usuarios = new string[500];
        static string[] nombres_oficiales = new string[500];
        static string[] correos_oficiales = new string[500];
        static string[] telefonos_oficiales = new string[500];
        static string[] fechas_ingreso_oficial = new string[500];
        static string nombre_oficial = "";
        static string correo_oficial = "";
        static string telefono_oficial = "";
        static string fecha_ingreso = "";

        static string carpeta = "DatosMesaPartes";

        static void Main(string[] args)
        {
            Console.WriteLine("MESA DE PARTES");
            string contrasena = "";
            int intentos = 0;

            while (contrasena != "PNP2026" && intentos < 3)
            {
                Console.Write("Ingrese la contraseña: ");
                contrasena = Console.ReadLine();
                intentos++;

                if (contrasena != "PNP2026")
                {
                    Console.WriteLine("Contraseña incorrecta.");
                }
            }

            if (contrasena != "PNP2026")
            {
                Console.WriteLine("Se acabaron los intentos.");
                return;
            }
            Directory.CreateDirectory(carpeta);
            Directory.CreateDirectory(carpeta + "/SOLICITUD");
            Directory.CreateDirectory(carpeta + "/OFICIO");
            Directory.CreateDirectory(carpeta + "/CARTA");
            CargarDocumentos();
            Console.WriteLine("\nDatos del oficial encargado");
            Console.Write("Nombre: ");
            nombre_oficial = Console.ReadLine();

            Console.Write("Correo electrónico: ");
            correo_oficial = Console.ReadLine();

            telefono_oficial = LeerTelefono("Teléfono (9 dígitos): ");
            fecha_ingreso = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            int opcion = 0;

            while (opcion != 9)
            {
                Console.WriteLine("\n--- MESA DE PARTES ---");
                Console.WriteLine("1. Registrar documento");
                Console.WriteLine("2. Buscar documento");
                Console.WriteLine("3. Ver documentos de un DNI");
                Console.WriteLine("4. Eliminar documento");
                Console.WriteLine("5. Modificar documento o usuario");
                Console.WriteLine("6. Modificar datos del oficial");
                Console.WriteLine("7. Ver seguimiento");
                Console.WriteLine("8. Ayuda");
                Console.WriteLine("9. Salir");
                Console.WriteLine("10. Clean (limpiar pantalla)");

                opcion = LeerNumero("Elija una opción: ", 1, 10);

                switch (opcion)
                {
                    case 1:
                        RegistrarDocumento();
                        break;
                    case 2:
                        BuscarDocumento();
                        break;
                    case 3:
                        VerDocumentosDni();
                        break;
                    case 4:
                        EliminarDocumento();
                        break;
                    case 5:
                        ModificarDocumento();
                        break;
                    case 6:
                        ModificarOficial();
                        break;
                    case 7:
                        VerSeguimiento();
                        break;
                    case 8:
                        MostrarAyuda();
                        break;
                    case 9:
                        GuardarSeguimiento();
                        Console.WriteLine("Programa cerrado.");
                        break;
                    case 10:
                        Console.Clear();
                        break;
                }
            }
            static void RegistrarDocumento()
        {
            if (total_documentos >= max_documentos)
            {
                Console.WriteLine("Se alcanzó el máximo de documentos.");
                return;
            }

            Console.WriteLine("Tipo de documento:");
            Console.WriteLine("1. Solicitud");
            Console.WriteLine("2. Oficio");
            Console.WriteLine("3. Carta");
            int opcion_tipo = LeerNumero("Elija el tipo: ", 1, 3);

            if (opcion_tipo == 1)
            {
                tipos[total_documentos] = "SOLICITUD";
            }
            else if (opcion_tipo == 2)
            {
                tipos[total_documentos] = "OFICIO";
            }
            else
            {
                tipos[total_documentos] = "CARTA";
            }

            fechas[total_documentos] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            codigos[total_documentos] = "DOC-" + DateTime.Now.ToString("yyyyMMddHHmmss") + total_documentos;

            Console.Write("Nombre del usuario: ");
            nombres_usuarios[total_documentos] = Console.ReadLine();

            dni_usuarios[total_documentos] = LeerDni("DNI (8 dígitos): ");
            telefonos_usuarios[total_documentos] = LeerTelefono("Teléfono del usuario (9 dígitos): ");

            Console.Write("Descripción del documento: ");
            descripciones[total_documentos] = Console.ReadLine();

            nombres_oficiales[total_documentos] = nombre_oficial;
            correos_oficiales[total_documentos] = correo_oficial;
            telefonos_oficiales[total_documentos] = telefono_oficial;
            fechas_ingreso_oficial[total_documentos] = fecha_ingreso;

            Console.Write("¿Desea guardar el documento? (S/N): ");
            string respuesta = Console.ReadLine().ToUpper();

            if (respuesta == "S")
            {
                total_documentos++;
                GuardarDocumentos();
                Console.WriteLine("Documento guardado.");
                Console.WriteLine("Código: " + codigos[total_documentos - 1]);
            }
            else
            {
                Console.WriteLine("El documento no se guardó.");
            }
        }
          static void BuscarDocumento()
        {
            Console.Write("Ingrese código, DNI o tipo de documento: ");
            string busqueda = Console.ReadLine().ToUpper();
            int encontrados = 0;
            for (int i = 0; i < total_documentos; i++)
            {
                if (codigos[i].ToUpper() == busqueda ||
                    dni_usuarios[i] == busqueda ||
                    tipos[i] == busqueda)
                {
                    MostrarDocumento(i);
                    encontrados++;
                }
            }

            if (encontrados == 0)
            {
                Console.WriteLine("No se encontraron documentos.");
            }
        }

        static void VerDocumentosDni()
        {
            string dni = LeerDni("Ingrese el DNI: ");
            int cantidad = 0;

            for (int i = 0; i < total_documentos; i++)
            {
                if (dni_usuarios[i] == dni)
                {
                    MostrarDocumento(i);
                    cantidad++;
                }
            }

            Console.WriteLine("Esta persona tiene " + cantidad + " documento(s).");
        }

        static void EliminarDocumento()
        {
            string dni = LeerDni("Ingrese el DNI del usuario: ");
            int cantidad = 0;

            for (int i = 0; i < total_documentos; i++)
            {
                if (dni_usuarios[i] == dni)
                {
                    MostrarDocumento(i);
                    cantidad++;
                }
            }

            if (cantidad == 0)
            {
                Console.WriteLine("No hay documentos de ese DNI.");
                return;
            }

            Console.Write("Escriba el código del documento que desea eliminar: ");
            string codigo = Console.ReadLine();
            int posicion = -1;

            for (int i = 0; i < total_documentos; i++)
            {
                if (dni_usuarios[i] == dni && codigos[i] == codigo)
                {
                    posicion = i;
                }
            }

            if (posicion == -1)
            {
                Console.WriteLine("No se encontró ese código para el DNI indicado.");
                return;
            }

            for (int i = posicion; i < total_documentos - 1; i++)
            {
                CopiarDocumento(i + 1, i);
            }

            total_documentos--;
            GuardarDocumentos();
            Console.WriteLine("Documento eliminado.");
        }
    }
}
