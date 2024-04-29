-- Active: 1714107313666@@bryj5ozcxiqqmbgjmswg-mysql.services.clever-cloud.com@3306@bryj5ozcxiqqmbgjmswg
CREATE TABLE Usuarios(
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nombres VARCHAR(50),
    Apellidos VARCHAR(50),
    Correo VARCHAR(50),
    IdTipoDocumento INT,
    Documento VARCHAR(45),
    IdAtencionPrioritaria INT
);

CREATE TABLE Modulos(
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Modulo VARCHAR(35)
);

CREATE TABLE Asesores(
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Documento VARCHAR(45),
    Correo VARCHAR(45),
    Nombres VARCHAR(45),
    Apellidos VARCHAR(45),
    IdServicio INT,
    IdModulo INT
);

CREATE TABLE Servicios(
    Id INT PRIMARY KEY AUTO_INCREMENT,
    TipoServicio VARCHAR(45),
    Siglas VARCHAR(10),
    CantidadTurnos INT
);

CREATE TABLE TiposDocumento(
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Tipos VARCHAR(45)
);

CREATE TABLE Turnos(
    Id INT PRIMARY KEY AUTO_INCREMENT,
    IdUsuario INT,
    IdServicio INT,
    DigitoTurno VARCHAR(10),
    FechaHoraEntrada DATETIME,
    FechaHoraSalida DATETIME,
    Estado VARCHAR(45)
);

CREATE TABLE AtencionesPrioritarias(
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Condiciones VARCHAR(45)
);

DROP TABLE `Usuarios`;

ALTER TABLE `Usuarios`
DROP COLUMN IdServicio;

ALTER TABLE `Modulos` CHANGE Modulo NumeroModulo VARCHAR(35);

ALTER TABLE `Asesores` ADD IdServicio INT AFTER Apellidos;

ALTER TABLE `Turnos` ADD IdModulo INT AFTER IdServicio;
ALTER TABLE `Turnos` ADD Llamado INT AFTER FechaHoraSalida;

INSERT INTO `Asesores`(`Documento`, `Correo`, `Nombres`, `Apellidos`, `IdServicio`, `IdModulo`) VALUES
('123456789', 'asesor1@example.com', 'Carlos', 'González Pérez', 1, 1),
('987654321', 'asesor2@example.com', 'Laura', 'Sánchez Gómez', 2, 2),
('456789123', 'asesor3@example.com', 'Roberto', 'López Martínez', 3, 3),
('789123456', 'asesor4@example.com', 'Isabel', 'Fernández López', 4, 4);

INSERT INTO Usuarios (Nombres, Apellidos, Correo, IdTipoDocumento, Documento, IdAtencionPrioritaria) VALUES
('Juan','Pérez','juan@example.com', 1, '123456789', 5),
('María','Gómez','maria@example.com', 2, '456789123', NULL),
('Luis','Rodríguez','luis@example.com', 3, '456789123', 3),
('Elena','Sánchez','elena@example.com', 4, 'AB123456', NULL);

INSERT INTO `Modulos`(Modulo) VALUES
("Caja1"),
("Caja2"),
("Caja3"),
("Caja4");

INSERT INTO `Servicios`(`TipoServicio`, `Siglas`, `Contador`) VALUES
("Solicitud de citas", "SC", 0),
("Pago de facturas", "PF", 0),
("Autorización de medicamentos", "AM", 0),
("Información en general", "IG", 0);

INSERT INTO TiposDocumento (`Tipos`) VALUES
("Registro civil"),
("Tarjeta de identidad"),
("Cédula de ciudadanía"),
("Pasaporte"),
("Cédula extranjera");

INSERT INTO AtencionesPrioritarias(`Condiciones`) VALUES
("Embarazada"),
("Discapacitad@"),
("Tercera edad"),
("Lactantes"),
("Otros");


/* LLAVES FORANEAS */
    /* USUARIOS */
ALTER TABLE `Usuarios` ADD FOREIGN KEY (IdTipoDocumento) REFERENCES `TiposDocumento`(Id);
ALTER TABLE `Usuarios` ADD FOREIGN KEY (IdAtencionPrioritaria) REFERENCES `AtencionesPrioritarias`(Id);

    /* ASESORES */
ALTER TABLE `Asesores` ADD FOREIGN KEY (IdServicio) REFERENCES `Servicios`(Id);
ALTER TABLE `Asesores` ADD FOREIGN KEY (IdModulo) REFERENCES `Modulos`(Id);

    /* TURNOS */
ALTER TABLE `Turnos` ADD FOREIGN KEY (IdUsuario) REFERENCES `Usuarios`(Id);
ALTER TABLE `Turnos` ADD FOREIGN KEY (IdServicio) REFERENCES `Servicios`(Id);
ALTER TABLE `Turnos` ADD FOREIGN KEY (IdModulo) REFERENCES `Modulos`(Id);




UPDATE `Servicios` SET `Contador` = 000 WHERE `Id` = 1;

ALTER TABLE `Turnos` CHANGE Llamado Llamado VARCHAR(10);

ALTER TABLE Turnos
DROP COLUMN Llamado;
