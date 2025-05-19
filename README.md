
# 📱 Taller IDWM - API REST con .NET

Este proyecto fue desarrollado como parte del taller de la asignatura **Introducción al Desarrollo Web Móvil (IDWM)**. El objetivo principal fue construir una **API RESTful** utilizando .NET 9, aplicando buenas prácticas de arquitectura y acceso a datos, así como el uso de SQLite como base de datos local.

---

## 📦 Estructura del Proyecto

```plaintext
API/
│
├── src/                    # Código fuente principal
│   ├── Controllers/        # Endpoints HTTP de la API
│   ├── Data/               # DbContext y configuración de la base de datos
│   ├── DTOs/               # Objetos de Transferencia de Datos
│   ├── Interface/          # Interfaces para servicios/repositorios
│   └── Repository/         # Implementación de los repositorios
│
├── app.db                  # Base de datos SQLite
├── appsettings.json        # Configuración principal
├── appsettings.Development.json # Configuración para entorno de desarrollo
├── Program.cs              # Punto de entrada de la aplicación
├── API.csproj              # Archivo del proyecto .NET
├── IDWM_API.sln            # Archivo de solución
└── README.md               # Documentación del proyecto
```

---

## ✅ Requisitos Previos

Asegúrate de tener instalado:

- [.NET SDK 9.0](https://dotnet.microsoft.com/download)
- Visual Studio o Visual Studio Code

---

## 🚀 Cómo Ejecutar el Proyecto

Sigue estos pasos desde la terminal dentro del directorio del proyecto:

### 1. Restaurar dependencias

```bash
dotnet restore
```

### 2. Compilar el proyecto

```bash
dotnet build
```

### 3. Ejecutar la API

```bash
dotnet run
```

Por defecto, la API estará disponible en:
```
http://localhost:5000
https://localhost:5001
```

---

## 🧪 Probar la API

Puedes probar la API usando herramientas como **Postman**, **Insomnia** o directamente desde Visual Studio Code usando el archivo `API.http`.

### Ejemplos de endpoints (según implementación):

- `GET /api/usuarios`
- `POST /api/usuarios`
- `PUT /api/usuarios/{id}`
- `DELETE /api/usuarios/{id}`

---

## 🗃 Base de Datos - SQLite

Este proyecto utiliza **SQLite** como sistema de gestión de base de datos local. El archivo `app.db` ya está incluido y configurado.

Si realizas cambios en el modelo de datos, puedes usar las siguientes instrucciones para actualizar la base de datos:

### Crear una nueva migración

```bash
dotnet ef migrations add NombreDeLaMigracion
```

### Aplicar la migración a la base de datos

```bash
dotnet ef database update
```

> Asegúrate de tener instalada la herramienta Entity Framework CLI. Si no la tienes, instálala con:

```bash
dotnet tool install --global dotnet-ef
```

---

## 🧹 Comandos Útiles

- **Limpiar archivos compilados**  
  ```bash
  dotnet clean
  ```

- **Recompilar después de cambios**  
  ```bash
  dotnet build
  ```

- **Ejecutar con rastreo detallado de errores**  
  ```bash
  dotnet run --verbosity detailed
  ```

---

## 🤝 Créditos

- **Asignatura**: Introducción al Desarrollo Web Móvil (IDWM)  
- **Docente**: *[Jorge Rivera]*
- **Ayudante**: *[Ferdando Chavez]*  
- **Estudiantes**:  
  - *[Sergio Parada]*  


---

## 📌 Notas Finales

Este proyecto fue creado con fines educativos. Puedes expandirlo fácilmente para incluir autenticación (JWT), validación de datos, relaciones entre entidades y despliegue en la nube.

> Desarrollado con ❤️ por estudiantes de IDWM usando .NET 9.
