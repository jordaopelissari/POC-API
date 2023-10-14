CREATE TABLE Endereco (
    Codigo INT PRIMARY KEY,
    Rua VARCHAR(100),
    Numero INT,
    Complemento VARCHAR(50),
    Bairro VARCHAR(50),
    Cidade VARCHAR(50),
    CEP VARCHAR(9)
);

-- Tabela Telefone
CREATE TABLE Telefone (
    Codigo INT PRIMARY KEY,
	Tipo VARCHAR(30),
    DDD VARCHAR(2),
    NumeroTelefone VARCHAR(9)
);

-- Tabela Cliente
CREATE TABLE Cliente (
    Codigo INT PRIMARY KEY,
    Nome VARCHAR(100),
    CPF VARCHAR(14) UNIQUE,
    CodigoEndereco INT,
    CodigoTelefone INT,
    FOREIGN KEY (CodigoEndereco) REFERENCES Endereco (Codigo),
    FOREIGN KEY (CodigoTelefone) REFERENCES Telefone (Codigo)
);


INSERT INTO Endereco (Codigo, Rua, Numero, Complemento, Bairro, Cidade, CEP)
VALUES
    (1, 'Rua A', 123, 'Apt 101', 'Bairro 1', 'Cidade A', '12345-678'),
    (2, 'Rua B', 456, NULL, 'Bairro 2', 'Cidade B', '98765-432'),
    (3, 'Rua C', 789, 'Casa 3', 'Bairro 3', 'Cidade C', '54321-876');

-- Inserir dados na tabela Telefone
INSERT INTO Telefone (Codigo, Tipo, DDD, NumeroTelefone)
VALUES
    (1, 'Residencial', '11', '123456789'),
    (2, 'Comercial', '21', '987654321'),
    (3, 'Celular', '31', '555555555');

-- Inserir dados na tabela Cliente
INSERT INTO Cliente (Codigo, Nome, CPF, CodigoEndereco, CodigoTelefone)
VALUES
    (1, 'Cliente 1', '123.456.789-01', 1, 1),
    (2, 'Cliente 2', '987.654.321-02', 2, 2),
    (3, 'Cliente 3', '111.222.333-03', 3, 3);


--Procedures
CREATE PROCEDURE AtualizarNomeCliente
    @Codigo INT,
    @NovoNome VARCHAR(100)
AS
BEGIN
    UPDATE Cliente
    SET Nome = @NovoNome
    WHERE Codigo = @Codigo;
END;


CREATE PROCEDURE SelecionarTodosClientes
AS
BEGIN
    SELECT * FROM Cliente;
END;

CREATE PROCEDURE SelecionarClientePorCodigo
    @CodigoCliente INT
AS
BEGIN
    SELECT *
    FROM Cliente
    WHERE Codigo = @CodigoCliente;
END;


CREATE PROCEDURE ObterDadosClienteTelefoneEndereco
    @CodigoCliente INT
AS
BEGIN
    SELECT
        C.Codigo,
        C.Nome,
        C.CPF,
        T.Tipo AS TipoTelefone,
        T.DDD,
        T.NumeroTelefone,
        E.Rua,
        E.Numero,
        E.Complemento,
        E.Bairro,
        E.Cidade,
        E.CEP
    FROM Cliente AS C
    LEFT JOIN Telefone AS T ON C.CodigoTelefone = T.Codigo
    LEFT JOIN Endereco AS E ON C.CodigoEndereco = E.Codigo
    WHERE C.Codigo = @CodigoCliente;
END;


CREATE PROCEDURE ObterTodosDadosClienteTelefoneEndereco
AS
BEGIN
    SELECT
        C.Codigo,
        C.Nome,
        C.CPF,
        T.Tipo AS TipoTelefone,
        T.DDD,
        T.NumeroTelefone,
        E.Rua,
        E.Numero,
        E.Complemento,
        E.Bairro,
        E.Cidade,
        E.CEP
    FROM Cliente AS C
    LEFT JOIN Telefone AS T ON C.CodigoTelefone = T.Codigo
    LEFT JOIN Endereco AS E ON C.CodigoEndereco = E.Codigo
END;