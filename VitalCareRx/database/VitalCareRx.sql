USE tempdb
go

--DROP DATABASE VitalCareRxDB
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

CREATE SCHEMA Historial
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
	idPaciente INT NOT NULL IDENTITY,
	numeroIdentidad VARCHAR(16) NOT NULL,
	primerNombre VARCHAR(25) NOT NULL,
	segundoNombre VARCHAR(25),
	primerApellido VARCHAR(25) NOT NULL,
	segundoApellido VARCHAR(25),
	direccion VARCHAR(200) NOT NULL,
	celular VARCHAR(8),
	fechaNacimiento DATE NOT NULL,
	peso FLOAT NOT NULL,
	estatura FLOAT NOT NULL,
	estado BIT NOT NULL,
	idTipoSangre INT NOT NULL,
	idSexo INT NOT NULL,
	

	CONSTRAINT PK_Paciente_idIdentidad
		PRIMARY KEY CLUSTERED (idPaciente),
	CONSTRAINT FK_Paciente$Existe$TipoSangre
		FOREIGN KEY (idTipoSangre) REFERENCES Personas.TipoSangre(idTipoSangre),
	CONSTRAINT FK_Paciente$Existe$Sexo
		FOREIGN KEY (idSexo) REFERENCES Personas.Sexo(idSexo)
)

CREATE TABLE Personas.Puesto
(

	idPuesto INT NOT NULL IDENTITY,
	puesto VARCHAR(25) NOT NULL

	CONSTRAINT PK_Puesto_idPuesto
		PRIMARY KEY CLUSTERED (idPuesto)

)

CREATE TABLE Personas.Empleado
(
	idEmpleado INT NOT NULL IDENTITY,
	primerNombre VARCHAR(25) NOT NULL, 
	segundoNombre VARCHAR(25),
	primerApellido VARCHAR(25) NOT NULL,
	segundoApellido VARCHAR(25),
	celular VARCHAR(8) NOT NULL,
	correo VARCHAR(30),
	fechaNacimiento DATE NOT NULL,
	idSexo INT NOT NULL,
	idPuesto INT NOT NULL,
	nombreUsuario VARCHAR(30) NOT NULL,
	contrasenia VARBINARY(MAX) NOT NULL,
	estado BIT NOT NULL
	

	CONSTRAINT PK_Empleado_idEmpleado
		PRIMARY KEY CLUSTERED (idEmpleado),

	CONSTRAINT FK_Empleado$Existe$Puesto
		FOREIGN KEY (idPuesto) REFERENCES Personas.Puesto(idPuesto),

	CONSTRAINT FK_Empleado$Existe$Sexo
		FOREIGN KEY (idSexo) REFERENCES Personas.Sexo(idSexo)
)

CREATE TABLE Historial.Bitacora
(
	idBitacora INT NOT NULL IDENTITY,
	idEmpleado INT NOT NULL,
	pcUsuario varchar NOT NULL,
	accion NVARCHAR(50) NOT NULL,
	fecha DATETIME NOT NULL

	CONSTRAINT PK_Bitacora_idBitacora
		PRIMARY KEY CLUSTERED (idBitacora),

	CONSTRAINT FK_Bitacora$Existe$Empleado
		FOREIGN KEY (idEmpleado) REFERENCES Personas.Empleado(idEmpleado),

)

CREATE TABLE Historial.ControlEmpleado
(
	idControlEmpleado INT NOT NULL IDENTITY,
	idEmpleado INT NOT NULL,
	fechaEntrada DATETIME NOT NULL,
	fechaSalida DATETIME NULL

	CONSTRAINT PK_ControlEmpleado_idControlEmpleado
		PRIMARY KEY CLUSTERED (idControlEmpleado),

	CONSTRAINT FK_ControlEmpleado$Existe$Empleado
		FOREIGN KEY (idEmpleado) REFERENCES Personas.Empleado(idEmpleado),
)

CREATE TABLE Consultas.Consulta
(
	idConsulta INT NOT NULL IDENTITY,
	fechaConsulta DATE NOT NULL,
	motivoConsulta VARCHAR(300) NOT NULL,
	diagnosticoConsulta VARCHAR(300) NOT NULL,
	temperatura FLOAT NOT NULL,
	presionArterial VARCHAR(9) NOT NULL,
	idEmpleado INT NOT NULL,
	idPaciente INT NOT NULL
	

	CONSTRAINT PK_Consulta_idConsulta
		PRIMARY KEY CLUSTERED (idConsulta),

	CONSTRAINT FK_Consulta$Existe$Empleado
		FOREIGN KEY (idEmpleado) REFERENCES Personas.Empleado(idEmpleado),

	CONSTRAINT FK_Consulta$Existe$Paciente
		FOREIGN KEY (idPaciente) REFERENCES Personas.Paciente(idPaciente)	 
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

CREATE TABLE Consultas.DetalleFarmaco
(
	idFarmaco INT NOT NULL,
	fechaVencimiento DATE NOT NULL,
	cantidad INT NOT NULL

	CONSTRAINT PK_DetalleFarmaco_idFarmaco_fechaVencimiento
		PRIMARY KEY CLUSTERED (idFarmaco, fechaVencimiento),
	CONSTRAINT FK_DetalleFarmaco$Existe$Farmaco
		FOREIGN KEY (idFarmaco) REFERENCES Consultas.Farmaco (idFarmaco)

)





CREATE TABLE Consultas.DetalleRecetaMedica
(
	idRecetaMedica INT NOT NULL,
	idFarmaco INT NOT NULL,
	cantidad INT NOT NULL,
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

ALTER TABLE Personas.Paciente WITH CHECK
		ADD CONSTRAINT CHK_Personas_Paciente$VerificarFechaNacimiento
		CHECK (fechaNacimiento <= CONVERT (date, GETDATE()))
GO

ALTER TABLE Personas.Paciente WITH CHECK
		ADD CONSTRAINT CHK_Personas_Paciente$VerificarPeso
		CHECK (peso > 0)
GO

ALTER TABLE Personas.Paciente WITH CHECK
		ADD CONSTRAINT CHK_Personas_Paciente$VerificarEstatura
		CHECK (estatura > 0)
GO

ALTER TABLE Personas.Paciente
	ADD CONSTRAINT CHK_Personas_Paciente$EstadoPaciente
	CHECK (estado IN(0, 1))
GO

ALTER TABLE Personas.Paciente
		ADD CONSTRAINT AK_Personas_numeroIdentidad
		UNIQUE NONCLUSTERED (numeroIdentidad)
GO


-- Tabla Personas.Sexo
ALTER TABLE Personas.Sexo
		ADD CONSTRAINT AK_Personas_Sexo_descripcionSexo
		UNIQUE NONCLUSTERED (descripcionSexo)
GO

ALTER TABLE Personas.Sexo
	ADD CONSTRAINT CHK_Personas_Sexo$SexoPersonas
	CHECK (descripcionSexo IN('Masculino', 'Femenino'))
GO
-- TABLA Personas.TipoSangre
ALTER TABLE Personas.TipoSangre
		ADD CONSTRAINT AK_Personas_TipoSexo_descripcionTipoSangre
		UNIQUE NONCLUSTERED (descripcionTipoSangre)
GO

ALTER TABLE Personas.TipoSangre
	ADD CONSTRAINT CHK_Personas_TipoSangre$TipoSangrePersonas
	CHECK (descripcionTipoSangre IN('A+', 'A-', 'AB-', 'AB+','B-','B+', 'O-', 'O+'))
GO

--TABLA Personas.Puesto
ALTER TABLE Personas.Puesto
	ADD CONSTRAINT CHK_Personas_Puesto$PuestoEmpleado
	CHECK (puesto IN('Administrador', 'Doctor', 'Enfermero')) -- 'Vigilante','Conserje'




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

ALTER TABLE Personas.Empleado WITH CHECK
		ADD CONSTRAINT CHK_Personas_Empleado$VerificarFechaNacimiento
		CHECK (fechaNacimiento <= CONVERT (date, GETDATE()))
GO

ALTER TABLE Personas.Empleado 
	ADD CONSTRAINT AK_Personas_Empleado_correo
	UNIQUE NONCLUSTERED (correo)
GO



-- Tabla Consultas.Consulta
ALTER TABLE Consultas.Consulta
		ADD CONSTRAINT AK_Consultas_Consulta_idPaciente
		UNIQUE NONCLUSTERED (idPaciente)
GO


ALTER TABLE Consultas.Consulta WITH CHECK
		ADD CONSTRAINT CHK_Consultas_Consulta$VerificarTemperatura
		CHECK (temperatura > 0)
GO

ALTER TABLE Consultas.Consulta WITH CHECK
		ADD CONSTRAINT CHK_Consultas_Consulta$VerificarFechaConsulta
		CHECK (fechaConsulta = CONVERT (date, GETDATE()))
GO


-- Tabla Consultas.DetalleRecetaMedica
ALTER TABLE Consultas.DetalleRecetaMedica WITH CHECK
		ADD CONSTRAINT CHK_Consultas_DetalleRecetaMedica$VerificarCantidadFarmacos
		CHECK (cantidad > 0)
GO

-- Tabla Consultas.DetalleFarmaco
ALTER TABLE Consultas.DetalleFarmaco WITH CHECK
		ADD CONSTRAINT CHK_Consultas_DetalleFarmaco$VerificarCantidadFarmacos
		CHECK (cantidad >= 0)
GO

ALTER TABLE Consultas.DetalleFarmaco WITH CHECK
		ADD CONSTRAINT CHK_Consultas_DetalleFarmaco$VerificarFechaVencimiento
		CHECK (fechaVencimiento > CONVERT (date, DATEADD(day, 7, GETDATE())))
GO


-- Datos tabla sexo
INSERT [Personas].[Sexo]  VALUES ('Femenino')

INSERT [Personas].[Sexo]  VALUES ('Masculino')


-- Datos tabla TipoSangre
INSERT [Personas].[TipoSangre]  VALUES ( 'A+')

INSERT [Personas].[TipoSangre] VALUES ( 'A-')

INSERT [Personas].[TipoSangre]  VALUES ( 'AB-')

INSERT [Personas].[TipoSangre]  VALUES ( 'AB+')

INSERT [Personas].[TipoSangre]  VALUES ( 'B-')

INSERT [Personas].[TipoSangre]  VALUES ( 'B+')

INSERT [Personas].[TipoSangre]  VALUES ( 'O-')

INSERT [Personas].[TipoSangre]  VALUES ( 'O+')


-- Datos tabla Farmaco

INSERT [Consultas].[Farmaco]  VALUES ('Simvastatina', 'Para controlar el colesterol')INSERT [Consultas].[Farmaco] VALUES ('Aspirina', ' Casi para todo')INSERT [Consultas].[Farmaco]  VALUES ('Omeprazol', 'Para la acidez de est�mago')INSERT [Consultas].[Farmaco]  VALUES ('Lexotiroxina s�dica', 'Para reemplazar la tiroxina')INSERT [Consultas].[Farmaco]  VALUES ('Ramipril', 'Para la hipertensi�n')INSERT [Consultas].[Farmaco] VALUES ('Amlodipina', 'Para la hipertensi�n y la angina')INSERT [Consultas].[Farmaco] VALUES ('Paracetamol ', 'Para aliviar el dolor')INSERT [Consultas].[Farmaco]  VALUES ('Salbutamol', 'Para el asma')

-- Datos tabla puesto
INSERT INTO [Personas].[Puesto] VALUES ('Administrador');

INSERT INTO [Personas].[Puesto] VALUES ('Doctor');

INSERT INTO [Personas].[Puesto] VALUES ('Enfermero');