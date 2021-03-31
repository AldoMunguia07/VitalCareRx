USE tempdb
go


-- Crear la base de datos
CREATE DATABASE VitalCareRxDB
GO

USE VitalCareRxDB
GO
-- ESQUEMAS
CREATE SCHEMA Personas
GO

CREATE SCHEMA Consultas
GO

--TABLAS
CREATE TABLE Personas.Sexo 
(
	idSexo INT NOT NULL IDENTITY,
	descripcionSexo varchar(10) NOT NULL,

	CONSTRAINT PK_Sexo_idSexo
		PRIMARY KEY CLUSTERED (idSexo)
)


CREATE TABLE Personas.TipoSangre
(
	idTipoSangre INT NOT NULL IDENTITY,
	descripcionTipoSangre varchar(15) NOT NULL,

	CONSTRAINT PK_TipoSangre_idTipoSangre
		PRIMARY KEY CLUSTERED (idTipoSangre)
)

CREATE TABLE Personas.Paciente 
(
	numeroIdentidad varchar(16) NOT NULL,
	primerNombre varchar(25) NOT NULL,
	segundoNombre varchar(25) NOT NULL,
	primerApellido varchar(25) NOT NULL,
	segundoApellido varchar(25) NOT NULL,
	direccion varchar(200) NOT NULL,
	celular VARCHAR(8),
	fechaNacimiento date NOT NULL,
	peso float NOT NULL,
	estatura float NOT NULL,
	estado BIT NOT NULL,
	idTipoSangre int NOT NULL,
	idSexo int NOT NULL,
	

	CONSTRAINT PK_Paciente_numeroIdentidad
		PRIMARY KEY CLUSTERED (numeroIdentidad),
	CONSTRAINT FK_Paciente$Existe$TipoSangre
		FOREIGN KEY (idTipoSangre) REFERENCES Personas.TipoSangre(idTipoSangre),
	CONSTRAINT FK_Paciente$Existe$Sexo
		FOREIGN KEY (idSexo) REFERENCES Personas.Sexo(idSexo)
)



CREATE TABLE Personas.Empleado
(
	idEmpleado INT NOT NULL IDENTITY,
	primerNombre VARCHAR(25) NOT NULL,
	segundoNombre VARCHAR(25) NOT NULL,
	primerApellido VARCHAR(25) NOT NULL,
	segundoApellido VARCHAR(25) NOT NULL,
	celular VARCHAR(8),
	idSexo INT NOT NULL,
	nombreUsuario VARCHAR(30) NOT NULL,
	contrasenia VARCHAR(30) NOT NULL,
	

	CONSTRAINT PK_Empleado_idEmpleado
		PRIMARY KEY CLUSTERED (idEmpleado),

	CONSTRAINT FK_Empleado$Existe$Sexo
		FOREIGN KEY (idSexo) REFERENCES Personas.Sexo(idSexo)
)

CREATE TABLE Consultas.Cita
(
	idCita INT NOT NULL IDENTITY,
	fechaCita DATE NOT NULL,
	notas varchar(200) NULL,
	numeroIdentidad varchar(16) NOT NULL, 

	CONSTRAINT PK_Cita_idCita
		PRIMARY KEY CLUSTERED (idCita),

	CONSTRAINT FK_Cita$Existe$Paciente
		FOREIGN KEY (numeroIdentidad) REFERENCES Personas.Paciente(numeroIdentidad)
)	

CREATE TABLE Consultas.Consulta
(
	idConsulta INT NOT NULL IDENTITY,
	motivoConsulta VARCHAR(300) NOT NULL,
	diagnosticoConsulta VARCHAR(300) NOT NULL,
	temperatura FLOAT NOT NULL,
	presionArterial VARCHAR(9) NOT NULL,
	idEmpleado INT NOT NULL,
	idCita INT NOT NULL,

	CONSTRAINT PK_Consulta_idConsulta
		PRIMARY KEY CLUSTERED (idConsulta),

	CONSTRAINT FK_Consulta$Existe$Empleado
		FOREIGN KEY (idEmpleado) REFERENCES Personas.Empleado(idEmpleado),

	CONSTRAINT FK_Consulta$Existe$Cita
		FOREIGN KEY (idCita) REFERENCES Consultas.Cita(idCita)	 
)



CREATE TABLE Consultas.RecetaMedica
(
	idRecetaMedica INT NOT NULL IDENTITY,
	idConsulta INT NOT NULL,

	CONSTRAINT PK_RecetaMedica_idRecetaMedica
		PRIMARY KEY CLUSTERED (idRecetaMedica),
	CONSTRAINT FK_RecetaMedica$Existe$Consulta
		FOREIGN KEY (idConsulta) REFERENCES Consultas.Consulta(idConsulta)
)

CREATE TABLE Consultas.Farmaco
(
	idFarmaco INT NOT NULL IDENTITY,
	descripcionFarmaco varchar(60) NOT NULL,
	informacionPrecaucion varchar(200) NOT NULL,

	CONSTRAINT PK_Farmaco_idFarmaco
		PRIMARY KEY CLUSTERED (idFarmaco),

)


CREATE TABLE Consultas.DetalleRecetaMedica
(
	idRecetaMedica INT NOT NULL,
	idFarmaco INT NOT NULL,
	duracionTratamiento varchar(100) NOT NULL,
	indicaciones varchar(200) NOT NULL,

	CONSTRAINT PK_DetellaRecetaMedica_idRecetaMedica_idFarmaco
		PRIMARY KEY CLUSTERED (idRecetaMedica,idFarmaco),
	CONSTRAINT FK_DetellaRecetaMedica$Existe$RecetaMedica
		FOREIGN KEY (idRecetaMedica) REFERENCES Consultas.RecetaMedica(idRecetaMedica),
	CONSTRAINT FK_DetellaRecetaMedica$Existe$idFarmaco
		FOREIGN KEY (idFarmaco) REFERENCES Consultas.Farmaco(idFarmaco)

)





-- RESTRICCIONES

-- Tabla Personas.Paciente

ALTER TABLE Personas.Paciente WITH CHECK
		ADD CONSTRAINT CHK_Personas_Paciente$VerificarLongitudDNI
		CHECK (len(numeroIdentidad) = 13)
GO


ALTER TABLE Personas.Paciente WITH CHECK
		ADD CONSTRAINT CHK_Personas_Paciente$VerificarLongitudCelular 
		CHECK (len(celular) = 8)
GO

-- Tabla Personas.Sexo
ALTER TABLE Personas.Sexo
		ADD CONSTRAINT AK_Personas_Sexo_descripcionSexo
		UNIQUE NONCLUSTERED (descripcionSexo)
GO
-- TABLA Personas.TipoSangre
ALTER TABLE Personas.TipoSangre
		ADD CONSTRAINT AK_Personas_TipoSexo_descripcionTipoSangre
		UNIQUE NONCLUSTERED (descripcionTipoSangre)
GO
-- TABLA Personas.Empleado

ALTER TABLE Personas.Empleado WITH CHECK
		ADD CONSTRAINT CHK_Personas_Empleado$VerificarLongitudCelular 
		CHECK (len(celular) = 8)
GO

ALTER TABLE Personas.Empleado
		ADD CONSTRAINT AK_Personas_Empleado_nombreUsuario
		UNIQUE NONCLUSTERED (nombreUsuario)
GO

ALTER TABLE Personas.Empleado WITH CHECK
		ADD CONSTRAINT CHK_Personas_Empleado$VerificarLongitudContrasenia
		CHECK (len(contrasenia) >= 8)
GO

-- Tabla Consultas.Cita
ALTER TABLE Consultas.Cita WITH CHECK
		ADD CONSTRAINT CHK_Consultas_Cita$VerificarFechaCita
		CHECK (fechaCita >= GETDATE())
GO

ALTER TABLE Consultas.Cita WITH CHECK
		ADD CONSTRAINT CHK_Consultas_Citas$VerificarLongitudDNI
		CHECK (len(numeroIdentidad) = 13)
GO


-- Tabla Consultas.Consulta
ALTER TABLE Consultas.Consulta
		ADD CONSTRAINT AK_Consultas_Consulta_idCita
		UNIQUE NONCLUSTERED (idCita)
GO


