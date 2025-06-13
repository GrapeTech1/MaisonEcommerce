-- Criação do banco de dados 
create database dbMaisonDeBeaute;

-- Usando o banco de dados
use dbMaisonDeBeaute;

-- Criando as tabelas
create table tb_Usuario (
    IdUsuario int primary key auto_increment,
    Nome varchar(50) not null,
    Email varchar(50) not null unique,
    Senha varchar(50) not null
);

-- Tabela do Cliente
create table tb_Cliente (
    IdCliente int primary key auto_increment,
    CPF varchar(14) unique not null,
    Nome varchar (50) not null,
    telefone varchar(14) not null,
    Idade int check (Idade >=18),
    Sexo varchar(10) check (Sexo in ('Masculino', 'Feminino', 'Outro')),
    DataCadastro timestamp default current_timestamp not null,
    DataAtualizacao timestamp default current_timestamp on update current_timestamp
);

-- Tabela do Funcionario
create table tb_Funcionario (
    IdFuncionario int primary key auto_increment,
    CPF varchar (14) unique not null,
    Nome varchar(50) not null,
    Cargo varchar(50) not null,
    Sexo varchar(10) check (Sexo in ('Masculino', 'Feminino', 'Outro')),
    DataCadastro timestamp default current_timestamp not null,
    DataAtualizacao timestamp default current_timestamp on update current_timestamp
);

-- Tabala do Produto
create table tb_Produto(
    IdProduto int primary key auto_increment,
    Foto longblob,
    TipoFoto varchar(100),
    Nome varchar(50) not null,
	Descricao varchar (200) not null,
    Quantidade int not null,
    Preco decimal(10,2) not null,
    DataCadastro timestamp default current_timestamp not null,
    DataAtualizacao timestamp default current_timestamp on update current_timestamp
    );

-- Tabela de Serviços 
create table tb_Servico(
    IdServico int primary key auto_increment,
    Nome varchar (50) not null,
    Descricao varchar (200) not null,
    Preco decimal(10,2) not null,
    DataCadastro timestamp default current_timestamp not null,
    DataAtualizacao timestamp default current_timestamp on update current_timestamp
    );

-- Tabela de Planos
create table tb_Plano (
    IdPlano int primary key auto_increment,
    Nome varchar(50) not null,
    Descricao varchar(200) not null,
    Duracao enum ( '30', '365') not null,
    Preco decimal(10,2) not null,
    DataCadastro timestamp default current_timestamp not null,
    DataAtualizacao timestamp default current_timestamp on update current_timestamp
    );

-- Tabela de Pacote 
create table tb_Pacote(
    IdPacote int primary key auto_increment,
    Nome varchar(50) not null,
    Descricao varchar(200) not null,
    Preco decimal (10,2) not null,
    DataCadastro timestamp default current_timestamp not null,
    DataAtualizacao timestamp default current_timestamp on update current_timestamp
    );

-- Tabela de Agendamentos
create table tb_Agendamento(
    IdAgendamento int primary key auto_increment,
    IdCliente_Agen int,
    IdServico_Agen int,
    DataHora datetime not null,
    DataCadastro timestamp default current_timestamp not null,
    DataAtualizacao timestamp default current_timestamp on update current_timestamp,
    foreign key (IdCliente_Agen) references tb_Cliente(IdCliente),
    foreign key (IdServico_Agen) references tb_Servico(IdServico)
);

-- Tabela de Serviços e Pacotes (tabela intermediaria)
create table tb_Servico_Pacote (
    IdServicoPacote int primary key auto_increment,
    IdServico int,
    IdPacote int,
    foreign key (IdServico) references tb_Servico(IdServico),
    foreign key (IdPacote) references tb_Pacote(IdPacote)
);

-- Tabela de Serviços e Planos (tabela intermediaria)
create table tb_Servico_Plano (
    IdServicoPlano int primary key auto_increment,
    IdServico int,
    IdPlano int,
    foreign key (IdServico) references tb_Servico(IdServico),
    foreign key (IdPlano) references tb_Plano(IdPlano)
);

select * from tb_Agendamento;
select * from tb_Produto;
select * from tb_Funcionario;

-- Procedure do Serviço
DELIMITER $$
create PROCEDURE insertServico 
(vNome varchar(50), vDesc varchar(200), vPreco decimal(10,2))

begin
    if not EXISTS 
    (select IdServico from tb_Servico where Nome = vNome)
    then
        insert into tb_Servico (Nome,Descricao,Preco) 
        values (vNome,vDesc,vPreco);
    end if;
    
end;
$$

-- Procedure de Plano
DELIMITER $$
create PROCEDURE insertPlano 
(vNome varchar(50), vDesc varchar(200), vDuracao enum('30','365'),vPreco decimal(10,2))

begin
	if not EXISTS
	(select IdPlano from tb_Plano where Nome = vNome) 
	then
		insert into tb_Plano (Nome,Descricao, Duracao,Preco) 
        values (vNome,vDesc, vDuracao,vPreco);
    end if;
    
end;
$$

-- Procedure de Pacote 
DELIMITER $$
create procedure insertPacote 
(vNome varchar (50), vDesc varchar(200), vPreco decimal(10,2))

begin
	if not exists 
	(select IdPacote from tb_Pacote where Nome = vNome) 
    then 
		insert into tb_Pacote (Nome, Descricao, Preco)
		values (vNome, vDesc, vPreco);
	end if;

end 
$$

-- Porcedure de Produtos 
delimiter $$
create procedure insertProduto 
(in vFoto longblob, in vTipoFoto varchar(100), in vNome varchar (50), in vDesc varchar(200), in vQuant int, in vPreco decimal (10,2))

begin 
	if not exists 
	(select IdProduto from tb_Produto where Nome = vNome )
	 then 
		insert into tb_Produto (Foto, TipoFoto, Nome, Descricao, Quantidade, Preco)
		values (vFoto, vTipoFoto, vNome, vDesc, vQuant, vPreco);
        
        select 'Produto cadastrado com sucesso!' as Mensagem;
	end if;

end 
$$

/*
delimiter $$
create procedure todosProdutos ()
begin

	if exists (select 1 from information_schema.tables where table_schema = database() and table_name = 'tb_Produto') then
		select * from tb_Produto;
    
    else
		select 'A tabela não existe' as Erro;
    end if;

end;
$$
*/

-- Procedure de Cliente 
delimiter $$
create procedure insertCliente
(vCPF varchar(14), vNome varchar(50), vTel varchar(14), vIdade int, vSexo varchar(10))
begin

	 if not exists 
     (select IdCliente from tb_Cliente where CPF = vCPF)
     then
		insert into tb_Cliente (CPF, Nome, Telefone, Idade, Sexo)
        values (vCPF, vNome, vTel, vIdade, vSexo);
	end if;
    
end 
$$


-- Procedure de Funcionario 
delimiter $$
create procedure insertFuncionario
(vCPF varchar(14), vNome varchar(50), vSexo varchar(10), vCargo varchar(50))

begin
	 if not exists 
     (select IdFuncionario from tb_Funcionario where CPF = vCPF)
     then
		insert into tb_Funcionario (CPF, Nome, Cargo, Sexo)
        values (vCPF, vNome,vCargo, vSexo);
	end if;
    
end 
$$


-- Procedure de insert do agendamento
delimiter $$
create procedure insertAgen
(cliente varchar(50), servico varchar(50), vDataHora datetime)
begin

declare clienteId int;

set clienteId = (select IdCliente from tb_Cliente where Nome = cliente);

	if not exists(select IdAgendamento from tb_Agendamento where DataHora = vDataHora and IdServico_Agen = servico) then
		insert into tb_Agendamento (IdCliente_Agen, IdServico_Agen, DataHora)
        values (clienteId, servico, vDataHora);
    end if;
end
$$


-- Procedure de editar do agendamento
delimiter $$
create procedure editarAgen
(idAgen int, cliente varchar(50), servico varchar(50), vDataHora datetime)
begin

declare clienteId int;
set clienteId = (select IdCliente from tb_Cliente where CPF = cliente);
 
	Update tb_Agendamento set IdCliente_Agen = clienteId, IdServico_Agen = servico, DataHora = vDataHora where IdAgendamento = idAgen;
end
$$

-- Procedure do Serviço Pacote
delimiter $$
create procedure insertServPacote (servico int, pacote int)
begin
	insert into tb_Servico_Pacote (IdServico, IdPacote) values (servico, pacote);

end;
$$
select * from tb_Pacote;
select * from tb_Servico_Pacote;
-- Procedure do Serviço Plano
delimiter $$
create procedure insertServPlano (servico int, plano int)
begin
	insert into tb_Servico_Plano (IdServico, IdPlano) values (servico, plano);
end;
$$